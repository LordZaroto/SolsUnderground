using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines a Chest object that may appear
/// randomly in dungeon rooms after all enemies have been
/// defeated, and contains an item for the player to use
/// or equip.
/// 
/// NOTES:
/// 
/// > Use ItemManager to spawn chests and determine its item
/// 
/// </summary>

namespace SolsUnderground
{
    class Chest : StaticObject
    {
        // Fields
        // Inherits Texture2D texture and Rectangle positionRect
        private bool isOpen;

        // Properties
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
        public Chest(Texture2D texture, Rectangle positionRect)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            this.isOpen = false;
        }

        // Methods

        // Method to give item
        // Determines item to give inside the method

        // Draw Method
        // Draw depending on open state
    }
}
