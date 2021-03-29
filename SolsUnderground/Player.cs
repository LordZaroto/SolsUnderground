using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    public enum PlayerState
    {
        faceForward,
        faceLeft,
        faceBack,
        faceRight,
        moveForward,
        moveLeft,
        moveBack,
        moveRight,
        attackForward,
        attackLeft,
        attackBack,
        attackRight,
        specialForward,
        specialLeft,
        specialBack,
        specialRight,
        dead
    }
    
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
        private double specialCounter;
        private double basicCounter;
        private PlayerState playerState;
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
        /// <summary>
        /// The basic attack damage value of the equiped weapon.
        /// </summary>
        public int Attack
        {
            get { return weapon.Attack; }
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
            basicCounter = 0;
            specialCounter = 0;
            playerState = PlayerState.faceBack;
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
            
            //If the character does not move, change the playerState to the appropriate
            //idle direction. Then end the method.
            if(!(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.D)))
            {
                if(playerState == PlayerState.moveForward)
                {
                    playerState = PlayerState.faceForward;
                }
                else if (playerState == PlayerState.moveLeft)
                {
                    playerState = PlayerState.faceLeft;
                }
                else if (playerState == PlayerState.moveBack)
                {
                    playerState = PlayerState.faceBack;
                }
                else if (playerState == PlayerState.moveRight)
                {
                    playerState = PlayerState.faceRight;
                }

                return;
            }
            
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

                        //Is there horizontal movement? If so, change player state accordingly.
                        if (kbState.IsKeyDown(Keys.A) && test == true)
                        {
                            playerState = PlayerState.moveLeft;
                        }
                        else if (kbState.IsKeyDown(Keys.D) && test == true)
                        {
                            playerState = PlayerState.moveRight;
                        }
                    }
                    else if(kbState.IsKeyDown(Keys.W))
                    {
                        Y -= 2;
                        playerState = PlayerState.moveForward;
                    }
                    else if(kbState.IsKeyDown(Keys.S))
                    {
                        Y += 2;
                        playerState = PlayerState.moveBack;
                    }
                }
                else
                {
                    if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.S))
                    {
                        Y += 0;

                        //Is there horizontal movement? If so, change player state accordingly.
                        if (kbState.IsKeyDown(Keys.A) && test == true)
                        {
                            playerState = PlayerState.moveLeft;
                        }
                        else if (kbState.IsKeyDown(Keys.D) && test == true)
                        {
                            playerState = PlayerState.moveRight;
                        }
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        Y -= 4;
                        playerState = PlayerState.moveForward;
                    }
                    else if (kbState.IsKeyDown(Keys.S))
                    {
                        Y += 4;
                        playerState = PlayerState.moveBack;
                    }
                }
            }
            if (kbState.IsKeyDown(Keys.A) && test == false)
            {
                X -= 4;
                playerState = PlayerState.moveLeft;
            }
            if (kbState.IsKeyDown(Keys.D) && test == false)
            {
                X += 4;
                playerState = PlayerState.moveRight;
            }
        }

        /// <summary>
        /// The character will unleash the basic attack of their weapon.
        /// Has a short cooldown. Returns the hitbox of the attack.
        /// </summary>
        /// <param name="lButton"></param>
        /// <param name="previousLeftBState"></param>
        /// <param name="gameTime"></param>
        public Rectangle BasicAttack(ButtonState lButton, ButtonState previousLeftBState)
        {
            if(SingleLButtonPress(lButton, previousLeftBState))
            {
                if(basicCounter >= weapon.BasicCooldown)
                {
                    //Reset the cooldown
                    basicCounter = 0;

                    //Create the attack hitbox in the direction the player is facing
                    if(playerState == PlayerState.faceForward || playerState == PlayerState.moveForward)
                    {
                        return new Rectangle(X - Width / 2, Y - Height / 2, Width * 2, Height);
                    }
                    else if (playerState == PlayerState.faceLeft || playerState == PlayerState.moveLeft)
                    {
                        return new Rectangle(X - Width / 2, Y - Height / 2, Width, Height * 2);
                    }
                    else if (playerState == PlayerState.faceBack || playerState == PlayerState.moveBack)
                    {
                        return new Rectangle(X + Width / 2, Y + Height / 2, Width * 2, Height);
                    }
                    else if (playerState == PlayerState.faceRight || playerState == PlayerState.moveRight)
                    {
                        return new Rectangle(X + Width / 2, Y - Height / 2, Width, Height * 2);
                    }
                }
            }

            return new Rectangle();
        }
        
        /// <summary>
        /// Reads the user's input and executes the desired actions.
        /// Should be called every tick.
        /// </summary>
        /// <param name="kbState"></param>
        public void Input(KeyboardState kbState, GameTime gameTime, ButtonState lButton, ButtonState previousLeftBState)
        {
            //Keep track of time passed
            basicCounter += gameTime.ElapsedGameTime.TotalSeconds;
            specialCounter += gameTime.ElapsedGameTime.TotalSeconds;

            Move(kbState);
            BasicAttack(lButton, previousLeftBState);
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
