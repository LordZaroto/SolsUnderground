using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class aMask : Item, Armor
    {
        // Fields
        // Inherits: texture, positionRect, ItemType type, itemValue
        private string name;
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
        public string Name
        {
            get { return name; }
        }

        // Constructor
        public aMask(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Armor, 0, texture, positionRect)
        {
            name = "Mask";
            defense = 0;
            speed = 0;
            hp = 50;
        }

        // Methods

        // Inherits Item.Draw()
    }
}
