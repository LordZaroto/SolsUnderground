using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines a MapManager that creates floors
/// from a pool of Rooms and tracks the player's location
/// within the dungeon.
/// 
/// NOTES:
/// > MapManager needs a full list of loaded tile textures
/// 
/// > MapManager is responsible for loading all Rooms
/// 
/// > NextRoom() automatically loads the next floor (and clears the current one)
///   when the currentRoom becomes higher than the number of rooms on the floor.
/// 
/// > Need to account for boss rooms in floor generation and room pools
/// 
/// > Finish adding enemies and chests to rooms
/// 
/// </summary>

namespace SolsUnderground
{
    class MapManager
    {
        // Fields
        private List<Room> roomPool;
        private List<Room> floor;
        private List<Enemy> enemies;
        private int currentFloor;
        private int currentRoom;
        private int windowWidth;
        private int windowHeight;

        //Properties
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }
        public int CurrentFloor
        {
            get { return currentFloor; }
        }
        public int CurrentRoomNum
        {
            get { return currentRoom; }
        }
        public Room CurrentRoom
        {
            get { return floor[currentRoom]; }
        }

        // Constructor
        public MapManager(List<Texture2D> tileTextures, int windowWidth, int windowHeight)
        {
            this.roomPool = new List<Room>();
            this.floor = new List<Room>();
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            currentRoom = 0;
            currentFloor = 0;

            Load(tileTextures);
        }

        // Methods
        
        /// <summary>
        /// Reads and loads all room files.
        /// </summary>
        public void Load(List<Texture2D> tileTextures)
        {
            // Program works from three directories down in project, in bin\debug\net3.1\
            DirectoryInfo d = new DirectoryInfo("..\\..\\..\\Rooms");
            
            // Load in each Room from file
            foreach (FileInfo f in d.GetFiles())
            {
                roomPool.Add(new Room("..\\..\\..\\Rooms\\" + f.Name, 
                    windowWidth, windowHeight, tileTextures));
            }
        }

        /// <summary>
        /// Randomly draws rooms from the room pool to build a floor of five rooms.
        /// </summary>
        public void NewFloor()
        {
            // Clear previous floor
            floor.Clear();

            int nextRoomID;
            int lastRoomID = -1;
            for (int i = 0; i < 5; i++)
            {
                // Adds a copy of a random room from roomPool to the floor
                // Avoids consecutive repeat rooms
                do
                {
                    nextRoomID = Program.rng.Next(0, roomPool.Count);
                }
                while (nextRoomID == lastRoomID);

                lastRoomID = nextRoomID;

                floor.Add(roomPool[nextRoomID]);
            }

            // Maybe subtract one from loop above and do boss-exclusive stuff here?
        }

        /// <summary>
        /// Sets active room to the next room on the floor. If called
        /// while the last room is active, creates a new floor and sets
        /// the first room active.
        /// </summary>
        public void NextRoom()
        {
            currentRoom++;

            // If moving beyond last room, adjust to new floor
            if (currentRoom == floor.Count)
            {
                currentRoom = 0;
                currentFloor++;
                NewFloor();
            }

            // Add a second list and random picker for Boss rooms
        }

        /// <summary>
        /// Resets floor and room tracking and generates new floor.
        /// </summary>
        public void Reset()
        {
            currentFloor = 0;
            currentRoom = 0;
            NewFloor();
        }

        /// <summary>
        /// Draws the current active room.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            floor[currentRoom].Draw(sb);
        }
    }
}
