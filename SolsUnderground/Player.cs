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
        private Weapon weapon;
        private float timeCounter;
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
        public Player(Texture2D texture, Rectangle positionRect, Weapon startWeapon)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            maxHp = 10;
            hp = maxHp;
            weapon = startWeapon;
            timeCounter = 0;
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
            bool test = false;
            
            if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.S))
            {
                //The values adjustments are lower when traveling diagonally
                //to accomadate for adjustments to both axis.
                if(kbState.IsKeyDown(Keys.D))
                {
                    X += 2;
                    
                    //Two trues will make a false!
                    if(test == true)
                    {
                        test = false;
                    }
                    else
                    {
                        test = true;
                    }
                }
                if (kbState.IsKeyDown(Keys.A))
                {
                    X -= 2;

                    if (test == true)
                    {
                        test = false;
                    }
                    else
                    {
                        test = true;
                    }
                }
                //Determine the vertical displacement
                if (test == true)
                {
                    if(kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.S))
                    {
                        Y += 0;
                    }
                    else if(kbState.IsKeyDown(Keys.W))
                    {
                        Y -= 2;
                    }
                    else if(kbState.IsKeyDown(Keys.S))
                    {
                        Y += 2;
                    }
                }
                else
                {
                    if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.S))
                    {
                        Y += 0;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        Y -= 4;
                    }
                    else if (kbState.IsKeyDown(Keys.S))
                    {
                        Y += 4;
                    }
                }
            }
            if (kbState.IsKeyDown(Keys.A) && test == false)
            {
                X -= 4;
            }
            if (kbState.IsKeyDown(Keys.D) && test == false)
            {
                X += 4;
            }
        }

        public void BasicAttack(ButtonState lButton, ButtonState previousLeftBState, GameTime gameTime)
        {
            if(SingleLButtonPress(lButton, previousLeftBState))
            {

            }
        }
        
        /// <summary>
        /// Reads the user's input and executes the desired actions.
        /// </summary>
        /// <param name="kbState"></param>
        public void Input(KeyboardState kbState)
        {
            Move(kbState);
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
        /// Returns true if the user just pressed the button
        /// </summary>
        /// <param name="lButton"></param>
        /// <returns></returns>
        public bool SingleLButtonPress(ButtonState lButton, ButtonState previousLeftBState)
        {
            MouseState mouseState = Mouse.GetState();

            return lButton == ButtonState.Pressed && previousLeftBState == ButtonState.Released;
        }
        /// <summary>
        /// Returns true if the user just pressed the button
        /// </summary>
        /// <param name="rButton"></param>
        /// <returns></returns>
        public bool SingleRButtonPress(ButtonState rButton, ButtonState previousRightBState)
        {
            MouseState mouseState = Mouse.GetState();

            return rButton == ButtonState.Pressed && previousRightBState == ButtonState.Released;
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
