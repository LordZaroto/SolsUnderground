using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    /// <summary>
    /// User controlled, can move and use weapon attacks and abilities.
    /// </summary>
    class Player : DynamicObject
    {
        //Fields
        //-----------------------------
        private int defense;
        private int hp;
        private int maxHp;
        //-----------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Properties
        //----------------------------------------

        //Player Position - Hitbox
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

        //Player Stats
        //------------------------------
        /// <summary>
        /// Reduces the value of incoming damage
        /// </summary>
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        /// <summary>
        /// Current hit points
        /// </summary>
        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        /// <summary>
        /// Maximum hit points - starting hp value
        /// </summary>
        public int MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Constructor
        //----------------------------------------------------------
        public Player(Texture2D texture, Rectangle positionRect)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            maxHp = 10;
            hp = maxHp;
        }
        //----------------------------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Methods
        //----------------------------------------------------------
        /// <summary>
        /// Moves the player based on user input - W A S D
        /// </summary>
        public void Move(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.W))
            {
                Y -= 2;
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                X -= 2;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                Y += 2;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                X += 2;
            }
        }

        /// <summary>
        /// Returns true if the key was just pressed in the current frame
        /// </summary>
        public bool SingleKeyPress(Keys key, KeyboardState kbState, KeyboardState previousKbState)
        {
            if (previousKbState.IsKeyDown(key) && kbState.IsKeyDown(key))
            {
                return false;
            }
            else if (kbState.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Displays the player on screen.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
