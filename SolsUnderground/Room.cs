using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale - 3/28/21
/// This class defines a Room which contains a set of Tiles
/// that make up a single room of the game.
///
/// NOTES:
/// > Load method is unfinished - structure is there, but needs specific textures
///   before it can actually be used
///
/// > Using a List for storing Tiles should work fine, but possibility is open to
///   other structures like 2D arrays if issues arise
///
/// > Two constructors - one loads data using file, other takes parameters and a Tile List
///   I think the first should be used for loading our rooms, and the second can be for testing
///
/// > Draw() method assumes there is no offset in the window, but this can be changed easily once
///   visual design is discussed.
///   
/// > NEED TO FINISH METHODS FOR CONTENT LIST: Make sure all necessary data is accessible
/// 
/// </summary>

namespace SolsUnderground
{
    class Room
    {
        // Fields
        private int width;
        private int height;
        private List<Tile> tiles;
        private List<GameObject> contents;

        // Properties
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        // Constructors
        public Room(string filepath) // Should probably use this one to load rooms
        {
            Load(filepath);
            contents = new List<GameObject>();
        }
        public Room(int width, int height, List<Tile> tiles) // Can use this to build and test rooms w/o editor
        {
            this.width = width;
            this.height = height;
            this.tiles = tiles;
            contents = new List<GameObject>();
        }

        // Methods

        /// <summary>
        /// Reads the given file and loads fields with appropriate data.
        /// </summary>
        /// <param name="filepath">String path of file</param>
        public void Load(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
        
            // First line holds width/height
            string line = reader.ReadLine();
            string[] data = line.Split('|');
            Texture2D texture = null;              // CHANGE ONCE TEXTURES ARE ADDED
            
            // Width of the room (IN TILES)
            width = Int32.Parse(data[0]);
            height = Int32.Parse(data[1]);
        
            // Rest of lines hold Tile data
            while ((line = reader.ReadLine()) != null)
            {
                data = line.Split('|');

                // Use tile ID to assign texture
                switch (Enum.Parse<Tiles>(data[0]))
                {
                    // Fill appropriately once textures are added      <<< ALSO HERE

                    case Tiles.DefaultTile:
                        // texture = defaultTileTexture;
                        break;

                    case Tiles.Barrier:
                        // texture = barrierTexture;
                        break;
                }

                // Add new tile using data from text line
                tiles.Add(new Tile(
                    texture,
                    Boolean.Parse(data[1])
                    ));
            }

            reader.Close();
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
        /// Draws tiles using width and height to scale and place them in position
        /// based on their order in list.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        /// <param name="windowWidth">Pixel width of area to draw Room</param>
        /// <param name="windowHeight">Pixel height of area to draw Room</param>
        public void Draw(SpriteBatch sb, int windowWidth, int windowHeight)
        {
            // Draw tiles of room
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    sb.Draw(
                        tiles[i * width + j].Texture,
                        new Rectangle(width * j, height * i, 
                        windowWidth / width, windowHeight / height),
                        Color.White);
                }
            }

            // Draw enemies and other objects in room
            foreach (GameObject o in contents)
            {
                // Call each object's Draw
            }
        }
    }
}
