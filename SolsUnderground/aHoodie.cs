using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class aHoodie : Item, Armor
    {
        // Fields
        // Inherits: texture, positionRect, ItemType type, itemValue

        private int defense;
        private int speed;
        private int hp;

        // Properties

        // Inherits
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
        public aHoodie(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Armor, 0, texture, positionRect)
        {
            defense = 0;
            speed = 0;
            hp = 0;
        }

        // Methods

        // Inherits Item.Draw()
    }
}
