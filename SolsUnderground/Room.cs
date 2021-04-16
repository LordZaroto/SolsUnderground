using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines a Room which contains a set of Tiles
/// that make up a single room of the game.
/// 
/// Noah Flanders - 3/30/21
/// Different enemies spawn in each room
/// (this code moved to EnemyManager)
///
/// NOTES:
/// > FINISH COPY METHOD: Currently doesnt solve issue, only moves object references
///   into a new Room object
/// 
/// > Using a List for storing Tiles should work fine, but possibility is open to
///   other structures like 2D arrays if issues arise
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
        private int windowWidth;
        private int windowHeight;
        private const int ROOM_WIDTH = 33;
        private const int ROOM_HEIGHT = 25;
        private List<Tile> tiles;
        private int enemyCount;

        // Properties
        public static int Width
        {
            get { return ROOM_WIDTH; }
        }
        public static int Height
        {
            get { return ROOM_HEIGHT; }
        }
        public int EnemyCount
        {
            get { return enemyCount; }
        }

        // Constructors
        public Room(string filepath, int windowWidth, int windowHeight, 
            List<Texture2D> tileTextures) // Load room archetypes
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            tiles = new List<Tile>();
            Load(filepath, tileTextures);
            SetTiles(windowWidth, windowHeight);
        }

        // Methods

        /// <summary>
        /// Reads the given file and loads fields with appropriate data.
        /// </summary>
        /// <param name="filepath">String path of file</param>
        private void Load(string filepath, List<Texture2D> tileTextures)
        {
            
            StreamReader reader = new StreamReader(filepath);

            //First line will be the number of enemies in the room
            enemyCount = int.Parse(reader.ReadLine());
            
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
        private void SetTiles(int windowWidth, int windowHeight)
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
        /// Retrieves hitboxes of barrier tiles for collision detection.
        /// </summary>
        /// <returns>List of Rectangles of all barrier tiles</returns>
        public List<Rectangle> GetBarriers()
        {
            List<Rectangle> barriers = new List<Rectangle>();

            foreach (Tile t in tiles)
            {
                // If Tile is a barrier, create and store Rectangle for collision
                if (t.IsObstacle)
                {
                    barriers.Add(new Rectangle(t.X, t.Y, t.Width, t.Height));
                }
            }

            // Add barrier to left-hand side of screen
            barriers.Add(new Rectangle(-50, 0, 50, windowHeight));

            return barriers;
        }

        /// <summary>
        /// Retrieves hitboxes of nonbarrier tiles for enemy spawning.
        /// </summary>
        /// <returns>List of Rectangles of all nonbarrier tiles</returns>
        public List<Rectangle> GetOpenTiles()
        {
            List<Rectangle> openTiles = new List<Rectangle>();

            foreach (Tile t in tiles)
            {
                // If tile is not a barrier, create and store Rectangle for spawning
                if (!t.IsObstacle)
                {
                    openTiles.Add(new Rectangle(t.X, t.Y, t.Width, t.Height));
                }
            }

            return openTiles;
        }

        /// <summary>
        /// Draws all tiles in room.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            // Draw tiles of room
            foreach (Tile t in tiles)
            {
                t.Draw(sb);
            }
        }
    }
}
