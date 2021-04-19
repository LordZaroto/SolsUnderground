using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class aBandana : Item, Armor
    {
        // Fields
        // Inherits: texture, positionRect, ItemType type, itemValue

        private int defense;
        private int speed;
        private int hp;

        // Properties

        // Inherits

        public Texture2D Sprite
        {
            get { return texture; }
        }

        public Rectangle Position
        {
            get { return positionRect; }
            set { positionRect = value; }
        }
        public int Defense
        {
            get { return defense; }
        }
        public int Speed
        {
            get { return speed; }
        }
        public int HP
        {
            get { return hp; }
        }

        // Constructor
        public aBandana(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Armor, -1, texture, positionRect)
        {
            defense = -1;
            speed = 2;
            hp = 0;
        }

        // Methods

        // Inherits Item.Draw()
    }
}
