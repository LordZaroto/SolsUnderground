using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

//Author: Preston Gilmore

namespace SolsUnderground
{
    //Preston Gillmore
    //Braden Flanders
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
        hit,
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
        private double specialCD;
        private double basicCounter;
        private double damageCounter;
        private double damageCD;
        private double moveCounter;
        private double moveCD;
        private PlayerState playerState;
        private Texture2D[] textures;
        private int tigerBucks;

        private AnimationManager _animationManager;
        private Dictionary<string, Animation> _animations;
        float _timer;
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

        public override Rectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }
        
        public PlayerState State
        {
            get { return playerState; }
            set { playerState = value; }
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

        /// <summary>
        /// A multiplyer for how far enemies will be knocked back
        /// </summary>
        public int Knockback
        {
            get { return weapon.Knockback; }
        }
        /// <summary>
        /// Currency gained upon enemy kill.
        /// </summary>
        public int TigerBucks
        {
            get { return tigerBucks; }
            set { tigerBucks = value; }
        }

        public Weapon CurrentWeapon
        {
            get { return weapon; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Constructor
        //----------------------------------------------------------
        public Player(Texture2D[] textures, Rectangle positionRect, Weapon startWeapon, Dictionary<string, Animation> animations)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            maxHp = 100;
            hp = maxHp;
            weapon = startWeapon;
            basicCounter = 0;
            specialCD = 5;
            specialCounter = specialCD;
            damageCD = 0.6;
            damageCounter = damageCD;
            moveCD = 0.1;
            moveCounter = moveCD;
            playerState = PlayerState.faceBack;
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value, this);
            _timer = 0f;
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
        public void PlayerMove(KeyboardState kbState)
        {
            if (hp > 0)
            {
                if (moveCounter >= moveCD)
                {
                    bool test = false;


                    //If the character does not move, change the playerState to the appropriate
                    //idle direction. Then end the method.

                    //changes texture according to direction
                    if (!(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.D)))
                    {

                        if (playerState == PlayerState.moveForward)
                        {

                            texture = textures[1];
                            playerState = PlayerState.faceForward;
                        }
                        else if (playerState == PlayerState.moveLeft)
                        {

                            texture = textures[2];
                            playerState = PlayerState.faceLeft;
                        }
                        else if (playerState == PlayerState.moveBack)
                        {

                            texture = textures[0];
                            playerState = PlayerState.faceBack;
                        }
                        else if (playerState == PlayerState.moveRight)
                        {

                            texture = textures[3];
                            playerState = PlayerState.faceRight;
                        }

                        return;
                    }

                    if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.S))
                    {
                        //The values adjustments are lower when traveling diagonally
                        //to accomadate for adjustments to both axis.
                        if (kbState.IsKeyDown(Keys.D))
                        {
                            X += 3;

                            //Two trues will make a false!
                            if (test == true)
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
                            X -= 3;

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
                            if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.S))
                            {
                                Y += 0;

                                //Is there horizontal movement? If so, change player state accordingly.
                                if (kbState.IsKeyDown(Keys.A) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveLeft"]);
                                    playerState = PlayerState.moveLeft;

                                }
                                else if (kbState.IsKeyDown(Keys.D) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveRight"]);
                                    playerState = PlayerState.moveRight;

                                }
                            }
                            else if (kbState.IsKeyDown(Keys.W))
                            {
                                _animationManager.Play(_animations["playerMoveForward"]);
                                Y -= 3;
                                playerState = PlayerState.moveForward;

                            }
                            else if (kbState.IsKeyDown(Keys.S))
                            {
                                _animationManager.Play(_animations["playerMoveBack"]);
                                Y += 3;
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
                                    _animationManager.Play(_animations["playerMoveLeft"]);
                                    playerState = PlayerState.moveLeft;

                                }
                                else if (kbState.IsKeyDown(Keys.D) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveRight"]);
                                    playerState = PlayerState.moveRight;

                                }
                            }
                            else if (kbState.IsKeyDown(Keys.W))
                            {
                                _animationManager.Play(_animations["playerMoveForward"]);
                                Y -= 4;
                                playerState = PlayerState.moveForward;

                            }
                            else if (kbState.IsKeyDown(Keys.S))
                            {
                                _animationManager.Play(_animations["playerMoveBack"]);
                                Y += 4;
                                playerState = PlayerState.moveBack;

                            }
                        }
                    }
                    if (kbState.IsKeyDown(Keys.A) && test == false)
                    {
                        _animationManager.Play(_animations["playerMoveLeft"]);
                        X -= 4;
                        playerState = PlayerState.moveLeft;

                    }
                    if (kbState.IsKeyDown(Keys.D) && test == false)
                    {
                        _animationManager.Play(_animations["playerMoveRight"]);
                        X += 4;
                        playerState = PlayerState.moveRight;

                    }


                }
            }
        }

        public void Special()
        {

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
                if (basicCounter >= weapon.BasicCooldown)
                {
                    //Reset the cooldown
                    basicCounter = 0;

                    //Create the attack hitbox in the direction the player is facing
                    if(playerState == PlayerState.faceForward || playerState == PlayerState.moveForward)
                    {
                        playerState = PlayerState.attackForward;
                        return weapon.GetHitbox(X, Y, Width, Height, PlayerState.faceForward);
                    }
                    else if (playerState == PlayerState.faceLeft || playerState == PlayerState.moveLeft)
                    {
                        playerState = PlayerState.attackLeft;
                        return weapon.GetHitbox(X, Y, Width, Height, PlayerState.faceLeft);
                    }
                    else if (playerState == PlayerState.faceBack || playerState == PlayerState.moveBack)
                    {
                        playerState = PlayerState.attackBack;
                        return weapon.GetHitbox(X, Y, Width, Height, PlayerState.faceBack);
                    }
                    else if (playerState == PlayerState.faceRight || playerState == PlayerState.moveRight)
                    {
                        playerState = PlayerState.attackRight;
                        return weapon.GetHitbox(X, Y, Width, Height, PlayerState.faceRight);
                    }
                }
            }

            return new Rectangle();
        }

        /// <summary>
        /// The player will take damage an be knocked back.
        /// </summary>
        public void TakeDamage(int damage, EnemyState enemyState, int knockback)
        {
            if(damageCounter >= damageCD)
            {
                moveCounter = 0;
                damageCounter = 0;
                hp -= damage;


                //Player knockback - Commented out till reworked
                if (enemyState == EnemyState.attackBack || enemyState == EnemyState.moveBack || enemyState == EnemyState.faceBack)
                {
                    Y += knockback;
                }
                if (enemyState == EnemyState.attackRight || enemyState == EnemyState.moveRight || enemyState == EnemyState.faceRight)
                {
                    X += knockback;
                }
                if (enemyState == EnemyState.attackLeft || enemyState == EnemyState.moveLeft || enemyState == EnemyState.faceLeft)
                {
                    Y -= knockback;
                }
                if (enemyState == EnemyState.attackForward || enemyState == EnemyState.moveForward || enemyState == EnemyState.faceForward)
                {
                    X -= knockback;
                }
            }
        }

        /// <summary>
        /// The player can press 'P' to pause the game.
        /// </summary>
        /// <param name="kbState"></param>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public GameState MenuInput(KeyboardState kbState, GameState gameState) //Not being used right now
        {
            if(kbState.IsKeyDown(Keys.P))
            {
                gameState = GameState.Pause;
            }

            return gameState;
        }
        
        /// <summary>
        /// Reads the user's input and executes the desired actions.
        /// Should be called every tick.
        /// </summary>
        /// <param name="kbState"></param>
        public void Input(KeyboardState kbState, GameTime gameTime)
        {
            //Update Cooldowns
            basicCounter += gameTime.ElapsedGameTime.TotalSeconds;
            specialCounter += gameTime.ElapsedGameTime.TotalSeconds;
            damageCounter += gameTime.ElapsedGameTime.TotalSeconds;
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            PlayerMove(kbState);

            _animationManager.Update(gameTime);
            if (hp <= 0 && _animationManager.Animation.CurrentFrame == 3)
            {
                _animationManager.Stop();
                if(_timer > 2)
                {
                    playerState = PlayerState.dead;
                }
                
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
        /// The player equips the given weapon.
        /// </summary>
        /// <param name="weapon"></param>
        public void EquipWeapon(Weapon weapon)
        {
            this.weapon = weapon;
        }


        /// <summary>
        /// Player equips the given armor piece.
        /// </summary>
        /// <param name="armor"></param>
        public void EquipArmor(Armor armor)
        {
            
        }
        
        public void Die()
        {
            
            _animationManager.Play(_animations["heroDeath"]);
            
        }

        public void UpdateTimer(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Displays the player on screen.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            //draws the static image if facing a direction or the moving image if walking in a direction
            if ((playerState == PlayerState.faceBack || playerState == PlayerState.faceForward || playerState == PlayerState.faceLeft || playerState == PlayerState.faceRight)&& hp > 0)
            {
                sb.Draw(texture, positionRect, Color.White);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(sb);
            }


        }
    }
}
