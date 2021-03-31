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
        private List<GameObject> contents;
        private int enemyCount;

        // Properties
        public int Width
        {
            get { return ROOM_WIDTH; }
        }
        public int Height
        {
            get { return ROOM_HEIGHT; }
        }
        public int EnemyCount
        {
            get { return enemyCount; }
        }

        // Constructors
        public Room(string filepath, int windowWidth, int windowHeight, 
            List<Texture2D> tileTextures, Texture2D[] enemyTextures) // Load room archetypes
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            tiles = new List<Tile>();
            contents = new List<GameObject>();
            Load(filepath, tileTextures, enemyTextures);
            SetTiles(windowWidth, windowHeight);
            enemyCount = 0;
        }

        // Only to be used for copying Rooms
        private Room(int windowWidth, int windowHeight, List<Tile> tiles, List<GameObject> contents, int enemyCount)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.tiles = tiles;
            this.enemyCount = enemyCount;

            this.contents = contents;

        }

        // Methods

        /// <summary>
        /// Reads the given file and loads fields with appropriate data.
        /// </summary>
        /// <param name="filepath">String path of file</param>
        public void Load(string filepath, List<Texture2D> tileTextures, Texture2D[] enemyTextures)
        {
            
            StreamReader reader = new StreamReader(filepath);

            int enemyWidth = enemyTextures[2].Width;
            int enemyHeight = enemyTextures[2].Height;

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

            //Establishing a set list of valid spawn locations for the enemies
            SetTiles(windowWidth, windowHeight);
            List<Tile> validSpawn = new List<Tile>();
            foreach(Tile t in tiles)
            {
                if(t.IsObstacle == false)
                {
                    validSpawn.Add(t);
                }
            }
            //Creating enemies based on the enemy count at the top of the room file
            for (int i = 0; i < enemyCount; i++)
            {
                Rectangle enemyRect = new Rectangle(validSpawn[Program.rng.Next(validSpawn.Count)].X, 
                    validSpawn[Program.rng.Next(validSpawn.Count)].Y, enemyWidth, enemyHeight);
                contents.Add(new Minion(enemyTextures, enemyRect, 3, 4));

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
        /// Creates a copy of the Room and its contents.
        /// </summary>
        /// <returns></returns>
        public Room Copy()
        {
            // NEED TO FINISH: lists use references, need to create new objects
            return new Room(windowWidth, windowHeight, tiles, contents, enemyCount);
        }

        /// <summary>
        /// Adds game objects to room's list of contents.
        /// </summary>
        /// <param name="gameObject">Game object to initialize in room</param>
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
