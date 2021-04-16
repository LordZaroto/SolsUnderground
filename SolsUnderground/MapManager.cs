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
/// </summary>

namespace SolsUnderground
{
    class MapManager
    {
        // Fields
        private Room startRoom;
        private List<Room> roomPool;
        private List<Room> bossRooms;
        private List<Room> floor;
        private int currentFloor;
        private int currentRoom;
        private int windowWidth;
        private int windowHeight;

        //Properties
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
            this.bossRooms = new List<Room>();
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
            DirectoryInfo d = new DirectoryInfo("Content\\Rooms");

            // Load starting room for each floor
            startRoom = new Room("Content\\Rooms\\StartRooms\\basicStartRoom.level",
                windowWidth, windowHeight, tileTextures);

            // Load in each Room from file
            foreach (FileInfo f in d.GetFiles())
            {
                roomPool.Add(new Room("Content\\Rooms\\" + f.Name, 
                    windowWidth, windowHeight, tileTextures));
            }

            // Load in boss rooms separately
            d = new DirectoryInfo("Content\\Rooms\\BossRooms");

            foreach (FileInfo f in d.GetFiles())
            {
                bossRooms.Add(new Room("Content\\Rooms\\BossRooms\\" + f.Name,
                    windowWidth, windowHeight, tileTextures));
            }
        }

        /// <summary>
        /// Randomly draws rooms from the room pool to build a floor of five rooms.
        /// </summary>
        public void NewFloor()
        {
            // Current floor composition:
            // Start Room -> 4 random rooms -> Boss Room

            // Clear previous floor
            floor.Clear();

            // Add starting room - may do floor specific rooms later
            floor.Add(startRoom);

            // Adds a copy of a random room from roomPool to the floor
            // Avoids repeat rooms on the same floor
            Room nextRoom = null;
            for (int i = 0; i < 4; i++)
            {
                do
                {
                    nextRoom = roomPool[Program.rng.Next(roomPool.Count)];
                }
                while (floor.Contains(nextRoom));

                floor.Add(nextRoom);
            }

            // Add a boss room/boss stuff here
            floor.Add(bossRooms[Program.rng.Next(bossRooms.Count)]);
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
