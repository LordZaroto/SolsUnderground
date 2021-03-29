using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale - 3/28/21
/// This class defines a MapManager that creates floors
/// from a pool of Rooms and tracks the player's location
/// within the dungeon.
/// 
/// NOTES:
/// > MapManager needs a full list of loaded Rooms before being initialized
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
        private int currentFloor;
        private int currentRoom;
        private int windowWidth;
        private int windowHeight;

        // Constructor
        public MapManager(List<Room> roomPool, int windowWidth, int windowHeight)
        {
            this.roomPool = roomPool;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            currentRoom = 0;
            currentFloor = 0;
        }

        // Methods

        /// <summary>
        /// Draws rooms from the room pool to build a floor of five rooms.
        /// </summary>
        public void LoadNewFloor()
        {
            // Clear previous floor
            floor.Clear();

            for (int i = 0; i < 5; i++)
            {
                // Adds a random room from roomPool to the floor
                floor.Add(roomPool[Program.rng.Next(0, roomPool.Count)]);

                // Add enemies and chest to room if not boss room
                if (i != 4)
                {
                    // Each room randomly gets assigned 1-3 enemies
                    int enemyCount = Program.rng.Next(1, 4);

                    for (int j = 0; j < enemyCount; j++)
                    {
                        //floor[i].Add(new Minion());                    // FILL IN
                    }

                    // Random chance to contain chest: 30%
                    if (Program.rng.Next(100) < 30)
                    {
                        //floor[i].Add(new Chest());                     // FILL IN
                    }
                }
            }
        }

        /// <summary>
        /// Increments the currentRoom. If currentRoom exceeds number of
        /// rooms on floor, reset currentRoom, increment currentFloor,
        /// and loads next floor.
        /// </summary>
        public void NextRoom()
        {
            currentRoom++;

            // If moving beyond last room, adjust to new floor
            if (currentRoom > floor.Count)
            {
                currentRoom = 0;
                currentFloor++;
                LoadNewFloor();
            }

            // Add a second list and random picker for Boss rooms
        }

        /// <summary>
        /// Call's current room's Draw method
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            floor[currentRoom].Draw(sb, windowWidth, windowHeight);
        }
    }
}
