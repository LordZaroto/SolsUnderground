using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    /// <summary>
    /// Weapons have different attack stats and unique abilities.
    /// Basic Attacks and ablities will have cooldowns.
    /// </summary>
    class Weapon : Equip
    {
        //Fields
        //-----------------------------
        private int attack;
        private double basicCooldown;
        //-----------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Properties
        //----------------------------------------

        //Weapon Position
        //------------------------------
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
        //------------------------------

        //Weapon Stats
        //------------------------------
        /// <summary>
        /// Reduces the value of incoming damage
        /// </summary>
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        
        /// <summary>
        /// Minimum time that must pass between each Basic Attack
        /// </summary>
        public double BasicCooldown
        {
            get { return basicCooldown; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Constructor
        //----------------------------------------------------------
        public Weapon(Texture2D texture, Rectangle positionRect)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            attack = 3;
            basicCooldown = 0.6;
        }
        //----------------------------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Methods
        //----------------------------------------------------------
    }
}
