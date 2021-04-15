using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    // Tracks type of item to determine what happens when the player picks it up.
    public enum ItemType
    {
        Money,
        HealthPotion,
        Weapon,
        Armor
    }

    /// <summary>
    /// Alex Dale
    /// 
    /// This class represents all items that can exist during the main gameplay loop.
    /// This includes gold, healing potions, weapons, and armor.
    /// </summary>
    class Item : StaticObject
    {
        // Fields
        protected ItemType type;
        private int itemValue;
        // Inherits texture and positionRect

        // Properties
        public ItemType Type
        {
            get { return type; }
        }
        public int Value
        {
            get { return itemValue; }
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
        public Item(ItemType type, int itemValue, Texture2D texture, Rectangle positionRect)
        {
            this.type = type;
            this.itemValue = itemValue;
            this.texture = texture;
            this.positionRect = positionRect;
        }

        // Methods

        /// <summary>
        /// Draws the current item.
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
