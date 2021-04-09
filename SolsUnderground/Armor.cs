using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines Armor items which can be equipped by the player
/// to affect their max health, defense, or movement speed.
/// 
/// </summary>

namespace SolsUnderground
{
    class Armor : Item
    {
        // Fields
        private int defense;

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

        // Constructor
        public Armor(Texture2D texture, int defense, Rectangle positionRect)
            : base(ItemType.Armor, defense, texture, positionRect)
        {
            this.defense = defense;
        }
    }
}
