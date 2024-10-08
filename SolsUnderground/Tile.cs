﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines a Tile that stores a texture
/// and a field indicating if it can be walked on.
///
/// NOTES:
/// > Tile's positionRect is initialized to default values of 0, this is done since Room objects
///   are responsible for initializing Tile values, and size can only be determined when Room has
///   windowHeight and windowWidth.
///
/// > Currently cannot change isObstacle value, depending on how game goes this might change
///   i.e. Will tiles be able to change walk-ability during game?
///   
/// </summary>

namespace SolsUnderground
{
    // ID list for tile textures
    // IDs used to assign textures to tiles while loading files
    public enum Tiles
    {
        DefaultTile,
        Barrier,
        RedBrick,
        BlueBrick
    }

    class Tile : StaticObject
    {
        // Fields
        private bool isObstacle;
        private bool isChestSpawn;
        // Also has texture and positionRect from parent classes

        // Properties
        public Texture2D Texture
        {
            get { return texture; }
        }
        public bool IsObstacle
        {
            get { return isObstacle; }
        }
        public bool IsChestSpawn
        {
            get { return isChestSpawn; }
        }
        public override int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }
        public override int Y
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }
        public override int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }
        public override int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
        }
        public override Rectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        // Constructor
        public Tile(Texture2D texture, bool isObstacle, bool isChestSpawn)
        {
            this.texture = texture;
            this.isObstacle = isObstacle;
            this.isChestSpawn = isChestSpawn;

            // This gets set immediately by Room methods using properties
            this.positionRect = new Rectangle(0, 0, 0, 0);
        }

        // Methods

        /// <summary>
        /// Draws Tile using a Texture2D and a Rectangle.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
