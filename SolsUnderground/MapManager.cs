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
        private List<Room> startRooms;
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
        public bool IsBossRoom
        {
            get { return (CurrentRoomNum == floor.Count - 1); }
        }
        public float FloorFactor
        {
            get { return 1 + 0.2f * (currentFloor / 2); }
        }

        // Constructor
        public MapManager(List<Texture2D> tileTextures, int windowWidth, int windowHeight)
        {
            this.roomPool = new List<Room>();
            this.bossRooms = new List<Room>();
            this.floor = new List<Room>();
            this.startRooms = new List<Room>();
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

            //if (currentFloor > 1)
            //{
            //    startRooms.Add(new Room("Content\\Rooms\\StartRooms\\basicStartRoom.level",
            //        windowWidth, windowHeight, tileTextures));
            //}
            //else
            //{
            //    startRoom = new Room("Content\\Rooms\\StartRooms\\basicStartRoom.level",
            //        windowWidth, windowHeight, tileTextures);
            //}
            


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

            //Loading in start rooms
            d = new DirectoryInfo("Content\\Rooms\\StartRooms");
            foreach (FileInfo f in d.GetFiles())
            {
                startRooms.Add(new Room("Content\\Rooms\\StartRooms\\" + f.Name,
                    windowWidth, windowHeight, tileTextures));
            }
        }

        /// <summary>
        /// Randomly draws rooms from the room pool to build a floor of five rooms.
        /// </summary>
        private void NewFloor()
        {
            // Current floor composition:
            // Start Room -> 4 random rooms -> Boss Room

            // Clear previous floor
            floor.Clear();

            // Add starting room - may do floor specific rooms later
            if(currentFloor == 0)
            {
                floor.Add(startRooms[0]);//Basic start room
            }
            else
            {
                //After the first floor, each starting room is a shop
                floor.Add(startRooms[1]);
            }
            

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

            // Add boss-specific rooms
            switch (currentFloor)
            {
                case 0: // Bus
                    floor.Add(bossRooms[4]);
                    break;

                case 1: // Weeb
                    floor.Add(bossRooms[0]);
                    break;

                case 2: // Janitor
                    floor.Add(bossRooms[3]);
                    break;

                case 3: // VM
                    floor.Add(bossRooms[5]);
                    break;

                case 4: // Stalker
                    floor.Add(bossRooms[2]);
                    break;

                case 5: // BR
                    floor.Add(bossRooms[3]);
                    break;

                case 6: // Munson
                    floor.Add(bossRooms[1]);
                    break;
            }
        }

        /// <summary>
        /// Sets active room to the next room on the floor. If called
        /// while the last room is active, creates a new floor and sets
        /// the first room active.
        /// </summary>
        /// <returns>Bool indicating if the floor factor has increased, false otherwise</returns>
        public bool NextRoom()
        {
            currentRoom++;

            // If moving beyond last room, adjust to new floor
            if (currentRoom >= floor.Count)
            {
                currentRoom = 0;
                currentFloor++;
                NewFloor();

                // Returns true once after every two new floors
                if (currentFloor % 2 == 0)
                {
                    return true;
                }
            }

            return false;
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
