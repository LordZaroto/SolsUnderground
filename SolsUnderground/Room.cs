using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Alex Dale - 3/20/21
// This class defines a Room which contains a set of Tiles
// that make up a single room of the game.

// NOTES:
// > Load method is unfinished - need to figure out how to parse string data
//   into appropriate data types, might need to use an enum?
//
// > Using a List for storing Tiles should work fine, but possibility is open to
//   other structures like 2D arrays if issues arise
//
// > Two constructors - one loads data using file, other takes parameters and List<Tile>
//   I think the first should be used for loading our rooms, and the second can be for testing
//
// > Also plan to write a Draw() method that uses each Tile's properties to draw based on
//   width and height of the room.

namespace SolsUnderground
{
    class Room
    {
        // Fields
        private int width;
        private int height;
        private List<Tile> tiles;

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
        //public Room(string filepath) // Should probably use this one to load rooms
        //{
        //    Load(filepath);
        //}
        public Room(int width, int height, List<Tile> tiles) // Can use this to build and test rooms w/o editor
        {
            this.width = width;
            this.height = height;
            this.tiles = tiles;
        }

        // Methods

        /// <summary>
        /// Reads the given file and loads fields with appropriate data.
        /// </summary>
        /// <param name="filepath">String path of file</param>
        //public void Load(string filepath)
        //{
        //    StreamReader reader = new StreamReader(filepath);
        //
        //    // First line holds width/height
        //    string line = reader.ReadLine();
        //    string[] data = line.Split('|');
        //
        //    width = Int32.Parse(data[0]);
        //    height = Int32.Parse(data[1]);
        //
        //    // Rest of lines hold Tile data
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        data = line.Split('|');
        //        tiles.Add(new Tile(
        //            // Parse strings into Texture2D, Color, and Boolean
        //            ));
        //    }
        //
        //    reader.Close();
        //}

        /// <summary>
        /// Draws tiles using width and height to scale and place them in position
        /// based on their order in list.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
        }
    }
}
