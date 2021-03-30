using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale - 3/29/21
/// This class defines a Room which contains a set of Tiles
/// that make up a single room of the game.
///
/// NOTES:
/// > Using a List for storing Tiles should work fine, but possibility is open to
///   other structures like 2D arrays if issues arise
///
/// > Draw() method assumes there is no offset in the window, but this can be changed easily once
///   visual design is discussed.
///   
/// > NEED TO FINISH METHODS FOR CONTENT LIST: Make sure all necessary data is accessible
///   What goes in contents list? Enemies, chests, anything else?
/// 
/// </summary>

namespace SolsUnderground
{
    class Room
    {
        // Fields
        private const int ROOM_WIDTH = 33;
        private const int ROOM_HEIGHT = 25;
        private List<Tile> tiles;
        private List<GameObject> contents;

        // Properties
        public int Width
        {
            get { return ROOM_WIDTH; }
        }
        public int Height
        {
            get { return ROOM_HEIGHT; }
        }

        // Constructors
        public Room(string filepath, int windowWidth, int windowHeight, List<Texture2D> tileTextures) // Load room archetypes
        {
            tiles = new List<Tile>();
            contents = new List<GameObject>();
            Load(filepath, tileTextures);
            SetTiles(windowWidth, windowHeight);
        }

        // Methods

        /// <summary>
        /// Reads the given file and loads fields with appropriate data.
        /// </summary>
        /// <param name="filepath">String path of file</param>
        public void Load(string filepath, List<Texture2D> tileTextures)
        {
            
            StreamReader reader = new StreamReader(filepath);

            // Defines necessary variables for file reading
            string line;
            string[] data;
        
            // Rest of lines hold Tile data
            while ((line = reader.ReadLine()) != null)
            {
                data = line.Split('|');

                // Add new tile using data from text line
                // First data piece is Tiles enum, use int value as ID for texture
                // Second data piece is boolean for whether tile is barrier
                tiles.Add(new Tile(
                    tileTextures[(int)Enum.Parse<Tiles>(data[0])],
                    Boolean.Parse(data[1])
                    ));
            }

            reader.Close();
        }

        /// <summary>
        /// Initializes each tile's size and position
        /// </summary>
        /// <param name="windowWidth">Pixel width of area to draw Room</param>
        /// <param name="windowHeight">Pixel height of area to draw Room</param>
        public void SetTiles(int windowWidth, int windowHeight)
        {
            // Determine size of tiles using window size
            int tileWidth = windowWidth / ROOM_WIDTH;
            int tileHeight = windowHeight / ROOM_HEIGHT;

            // Set each tile's position and size
            for (int i = 0; i < ROOM_WIDTH * ROOM_HEIGHT; i++)
            {
                tiles[i].X = tileWidth * (i / ROOM_HEIGHT);
                tiles[i].Y = tileHeight * (i % ROOM_HEIGHT);
                tiles[i].Width = tileWidth;
                tiles[i].Height = tileHeight;
            }
        }

        /// <summary>
        /// Adds game objects to room's list of contents.
        /// </summary>
        /// <param name="gameObject"></param>
        public void Add(GameObject gameObject)
        {
            contents.Add(gameObject);
        }

        /// <summary>
        /// Retrieves data on any enemies in the room.
        /// </summary>
        /// <returns>List containing all Enemy objects in room contents</returns>
        public List<Enemy> GetEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();

            foreach (Enemy e in contents)
            {
                enemies.Add(e);
            }

            return enemies;
        }

        /// <summary>
        /// Retrieves hitboxes of barrier tiles for collision detection.
        /// </summary>
        /// <returns>List of Rectangles of all barrier tiles</returns>
        public List<Rectangle> GetBarriers()
        {
            List<Rectangle> barriers = new List<Rectangle>();
            Rectangle rect;

            foreach (Tile t in tiles)
            {
                // If Tile is a barrier, create and store Rectangle for collision
                if (t.IsObstacle)
                {
                    rect = new Rectangle(t.X, t.Y, t.Width, t.Height);
                    barriers.Add(rect);
                }
            }

            return barriers;
        }

        /// <summary>
        /// Draws all tiles in room.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        /// <param name="windowWidth">Pixel width of area to draw Room</param>
        /// <param name="windowHeight">Pixel height of area to draw Room</param>
        public void Draw(SpriteBatch sb)
        {
            // Draw tiles of room
            foreach (Tile t in tiles)
            {
                t.Draw(sb);
            }

            // I dont think the Room should be responsible for drawing all of its content,
            // it could get pretty messy trying to track everything in the contents list.
        }
    }
}
