using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale - 3/23/21
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
        private Random rng;

        // Constructor
        public MapManager(List<Room> roomPool, int windowWidth, int windowHeight)
        {
            this.roomPool = roomPool;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            currentRoom = 0;
            currentFloor = 0;
            rng = new Random();
        }

        // Methods

        /// <summary>
        /// Draws rooms from the room pool to build a floor of five rooms.
        /// </summary>
        /// <param name="floorSize">Number of rooms on new floor</param>
        public void LoadNewFloor(int floorSize)
        {
            // Clear previous floor
            floor.Clear();

            for (int i = 0; i < floorSize; i++)
            {
                // Adds a random room from roomPool to the floor
                floor.Add(roomPool[rng.Next(0, roomPool.Count)]);
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
                LoadNewFloor(5);   // CURRENT DEFAULT FLOOR SIZE
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
