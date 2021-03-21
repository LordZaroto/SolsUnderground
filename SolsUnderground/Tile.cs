using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Alex Dale - 3/20/21
// This class defines a Tile that stores a texture,
// a color, and a field indicating if it can be walked on.

// NOTES:
// > Tile has no Draw method, plan to use Room to draw Tiles using their properties
//   This is because width and height of Tile is dependent on Room size
//
// > Currently cannot change isObstacle value, depending on how game goes this might change
//   i.e. Will tiles be able to change walk-ability during game?

namespace SolsUnderground
{
    class Tile
    {
        // Fields
        private Texture2D texture;
        private Color color;
        private bool isObstacle;

        // Properties
        public Texture2D Texture
        {
            get { return texture; }
        }
        public Color Color
        {
            get { return color; }
        }
        public bool IsObstacle
        {
            get { return isObstacle; }
        }

        // Constructor
        public Tile(Texture2D texture, Color color, bool isObstacle)
        {
            this.texture = texture;
            this.color = color;
            this.isObstacle = isObstacle;
        }
    }
}
