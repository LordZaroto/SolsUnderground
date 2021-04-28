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
        private int hp;
        private int maxHp;
        private Item weapon;
        private Item armor;
        private Attack currentAttack;
        private double specialCounter;
        private double basicCounter;
        private double damageCounter;
        private double damageCD;
        private double moveCounter;
        private double moveCD;
        private PlayerState playerState;
        private List<StatusEffect> activeEffects;
        private double effectCounter;
        private Texture2D[] textures;
        private int tigerBucks;
        private Keys forward;
        private Keys backward;
        private Keys left;
        private Keys right;
        private Keys equip;

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

        public Keys ForwardKey
        {
            get { return forward; }
            set { forward = (Keys)value; }
        }
        public Keys BackwardKey
        {
            get { return backward; }
            set { backward = (Keys)value; }
        }
        public Keys LeftKey
        {
            get { return left; }
            set { left = (Keys)value; }
        }
        public Keys RightKey
        {
            get { return right; }
            set { right = (Keys)value; }
        }
        public Keys EquipKey
        {
            get { return equip; }
            set { equip = (Keys)value; }
        }

        //------------------------------

        //Player Stats
        //------------------------------
        /// <summary>
        /// Reduces the value of incoming damage
        /// </summary>
        public int Defense
        {
            get { return Math.Max(CurrentArmor.Defense + DefenseMod, 0); }
        }
        /// <summary>
        /// Defines the player's movement speed
        /// </summary>
        public int Speed
        {
            get { return Math.Max(4 + CurrentArmor.Speed + SpeedMod, 0); }
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
            get { return maxHp + CurrentArmor.HP; }
            set { maxHp = value; }
        }
        /// <summary>
        /// The basic attack damage value of the equiped weapon.
        /// </summary>
        public int Damage
        {
            get 
            { return Math.Max(CurrentWeapon.Attack + AttackMod, 0); }
        }
        /// <summary>
        /// Counter for basic attack cooldown.
        /// </summary>
        public double BasicCounter
        {
            get { return basicCounter; }
        }
        /// <summary>
        /// Counter for special attack cooldown.
        /// </summary>
        public double SpecialCounter
        {
            get { return specialCounter; }
        }
        /// <summary>
        /// A multiplier for how far enemies will be knocked back
        /// </summary>
        public int Knockback
        {
            get { return CurrentWeapon.Knockback; }
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
            get { return (Weapon)weapon; }
        }

        public Armor CurrentArmor
        {
            get { return (Armor)armor; }
        }

        private int AttackMod
        {
            get
            {
                int attackMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.AtkUp)
                        attackMod += fx.Power;

                    if (fx.Effect == StatusType.AtkDown)
                        attackMod -= fx.Power;
                }

                return attackMod;
            }
        }
        private int DefenseMod
        {
            get
            {
                int defenseMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.DefUp)
                        defenseMod += fx.Power;

                    if (fx.Effect == StatusType.DefDown)
                        defenseMod -= fx.Power;
                }

                return defenseMod;
            }
        }
        private int SpeedMod
        {
            get
            {
                int speedMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.SpdUp)
                        speedMod += fx.Power;

                    if (fx.Effect == StatusType.SpdDown)
                        speedMod -= fx.Power;
                }

                return speedMod;
            }
        }
        private bool IsStunned
        {
            get
            {
                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.Stun)
                        return true;
                }

                return false;
            }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Constructor
        //----------------------------------------------------------
        public Player(Texture2D[] textures, Rectangle positionRect, 
            Item startWeapon, Item startArmor, Dictionary<string, Animation> animations)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            maxHp = 100;
            hp = maxHp;
            weapon = startWeapon;
            currentAttack = null;
            basicCounter = CurrentWeapon.BasicCooldown;
            specialCounter = CurrentWeapon.SpecialCooldown;
            armor = startArmor;
            damageCD = 0.6;
            damageCounter = damageCD;
            moveCD = 0.1;
            moveCounter = moveCD;
            playerState = PlayerState.faceBack;
            activeEffects = new List<StatusEffect>();
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value, this);
            _timer = 0f;
            forward = Keys.W;
            left = Keys.A;
            backward = Keys.S;
            right = Keys.D;
            equip = Keys.E;
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
                if (moveCounter >= moveCD && !IsStunned)
                {
                    bool test = false;


                    //If the character does not move, change the playerState to the appropriate
                    //idle direction. Then end the method.

                    //changes texture according to direction
                    if (!(kbState.IsKeyDown(forward) || kbState.IsKeyDown(left) || kbState.IsKeyDown(backward) || kbState.IsKeyDown(right)))
                    {

                        if (playerState == PlayerState.moveForward || playerState == PlayerState.attackForward)
                        {

                            texture = textures[1];
                            playerState = PlayerState.faceForward;
                        }
                        else if (playerState == PlayerState.moveLeft || playerState == PlayerState.attackLeft)
                        {

                            texture = textures[2];
                            playerState = PlayerState.faceLeft;
                        }
                        else if (playerState == PlayerState.moveBack || playerState == PlayerState.attackBack)
                        {

                            texture = textures[0];
                            playerState = PlayerState.faceBack;
                        }
                        else if (playerState == PlayerState.moveRight || playerState == PlayerState.attackRight)
                        {

                            texture = textures[3];
                            playerState = PlayerState.faceRight;
                        }

                        return;
                    }

                    if (kbState.IsKeyDown(forward) || kbState.IsKeyDown(backward))
                    {
                        //The values adjustments are lower when traveling diagonally
                        //to accomadate for adjustments to both axis.
                        if (kbState.IsKeyDown(right))
                        {
                            X += Math.Max(Speed - 1, 0);

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
                        if (kbState.IsKeyDown(right))
                        {
                            X -= Math.Max(Speed - 1, 0);

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
                            if (kbState.IsKeyDown(forward) && kbState.IsKeyDown(backward))
                            {
                                Y += 0;

                                //Is there horizontal movement? If so, change player state accordingly.
                                if (kbState.IsKeyDown(left) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveLeft"]);
                                    playerState = PlayerState.moveLeft;

                                }
                                else if (kbState.IsKeyDown(right) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveRight"]);
                                    playerState = PlayerState.moveRight;

                                }
                            }
                            else if (kbState.IsKeyDown(forward))
                            {
                                _animationManager.Play(_animations["playerMoveForward"]);
                                Y -= Math.Max(Speed - 1, 0);
                                playerState = PlayerState.moveForward;

                            }
                            else if (kbState.IsKeyDown(backward))
                            {
                                _animationManager.Play(_animations["playerMoveBack"]);
                                Y += Math.Max(Speed - 1, 0);
                                playerState = PlayerState.moveBack;

                            }
                        }
                        else
                        {
                            if (kbState.IsKeyDown(forward) && kbState.IsKeyDown(backward))
                            {
                                Y += 0;

                                //Is there horizontal movement? If so, change player state accordingly.
                                if (kbState.IsKeyDown(left) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveLeft"]);
                                    playerState = PlayerState.moveLeft;

                                }
                                else if (kbState.IsKeyDown(right) && test == true)
                                {
                                    _animationManager.Play(_animations["playerMoveRight"]);
                                    playerState = PlayerState.moveRight;

                                }
                            }
                            else if (kbState.IsKeyDown(forward))
                            {
                                _animationManager.Play(_animations["playerMoveForward"]);
                                Y -= Speed;
                                playerState = PlayerState.moveForward;

                            }
                            else if (kbState.IsKeyDown(backward))
                            {
                                _animationManager.Play(_animations["playerMoveBack"]);
                                Y += Speed;
                                playerState = PlayerState.moveBack;

                            }
                        }
                    }
                    if (kbState.IsKeyDown(left) && test == false)
                    {
                        _animationManager.Play(_animations["playerMoveLeft"]);
                        X -= Speed;
                        playerState = PlayerState.moveLeft;

                    }
                    if (kbState.IsKeyDown(right) && test == false)
                    {
                        _animationManager.Play(_animations["playerMoveRight"]);
                        X += Speed;
                        playerState = PlayerState.moveRight;

                    }


                }
            }
        }

        public Attack Special(ButtonState rButton, ButtonState previousRightBState)
        {
            if (SingleRButtonPress(rButton, previousRightBState))
            {
                if (specialCounter >= CurrentWeapon.SpecialCooldown && !IsStunned)
                {
                    //Reset the cooldown
                    specialCounter = 0;

                    currentAttack = CurrentWeapon.Special(this);
                    
                    //Return the special attack of the appropriate weapon
                    return currentAttack;
                }
            }

            return null;
        }

        /// <summary>
        /// The character will unleash the basic attack of their weapon.
        /// Has a short cooldown. Returns the hitbox of the attack.
        /// </summary>
        /// <param name="lButton"></param>
        /// <param name="previousLeftBState"></param>
        /// <param name="gameTime"></param>
        public Attack BasicAttack(ButtonState lButton, ButtonState previousLeftBState)
        {
            if(SingleLButtonPress(lButton, previousLeftBState))
            {
                if (basicCounter >= CurrentWeapon.BasicCooldown && !IsStunned)
                {
                    //Reset the cooldown
                    basicCounter = 0;

                    //Create the attack hitbox in the direction the player is facing
                    if (playerState == PlayerState.faceForward || playerState == PlayerState.moveForward)
                    {
                        playerState = PlayerState.attackForward;

                        currentAttack = new Attack(
                            CurrentWeapon.GetHitbox(X, Y, Width, Height, PlayerState.faceForward),
                            Damage,
                            Knockback,
                            CurrentWeapon.Sprite,
                            AttackDirection.up,
                            CurrentWeapon.Timer,
                            true,
                            null);

                        return currentAttack;
                    }
                    else if (playerState == PlayerState.faceLeft || playerState == PlayerState.moveLeft)
                    {
                        playerState = PlayerState.attackLeft;

                        currentAttack = new Attack(
                            CurrentWeapon.GetHitbox(X, Y, Width, Height, PlayerState.faceLeft),
                            Damage,
                            Knockback,
                            CurrentWeapon.Sprite,
                            AttackDirection.left,
                            CurrentWeapon.Timer,
                            true,
                            null);

                        return currentAttack;
                    }
                    else if (playerState == PlayerState.faceBack || playerState == PlayerState.moveBack)
                    {
                        playerState = PlayerState.attackBack;

                        currentAttack = new Attack(
                            CurrentWeapon.GetHitbox(X, Y, Width, Height, PlayerState.faceBack),
                            Damage,
                            Knockback,
                            CurrentWeapon.Sprite,
                            AttackDirection.down,
                            CurrentWeapon.Timer,
                            true,
                            null);

                        return currentAttack;
                    }
                    else if (playerState == PlayerState.faceRight || playerState == PlayerState.moveRight)
                    {
                        playerState = PlayerState.attackRight;

                        currentAttack = new Attack(
                            CurrentWeapon.GetHitbox(X, Y, Width, Height, PlayerState.faceRight),
                            Damage,
                            Knockback,
                            CurrentWeapon.Sprite,
                            AttackDirection.right,
                            CurrentWeapon.Timer,
                            true,
                            null);

                        return currentAttack;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The player will take damage and be knocked back.
        /// </summary>
        public void TakeDamage(int damage, AttackDirection attackDirection, int knockback)
        {
            if(damageCounter >= damageCD)
            {
                moveCounter = 0;
                damageCounter = 0;

                // Prevent player from "healing" from negative damage
                hp -= Math.Max(damage - Defense, 0);


                //Player knockback - Commented out till reworked
                if (attackDirection == AttackDirection.up)
                {
                    Y -= knockback;
                }
                if (attackDirection == AttackDirection.right)
                {
                    X += knockback;
                }
                if (attackDirection == AttackDirection.left)
                {
                    X -= knockback;
                }
                if (attackDirection == AttackDirection.down)
                {
                    Y += knockback;
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
            if (previousKbState.IsKeyUp(key) && kbState.IsKeyDown(key))
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
        /// The player equips the given weapon or armor piece.
        /// </summary>
        public Item Equip(Item equipment)
        {
            Item drop = null;

            if (equipment.Type == ItemType.Weapon)
            {
                drop = weapon;
                weapon = equipment;
            }
            else if (equipment.Type == ItemType.Armor)
            {
                drop = armor;
                armor = equipment;

                // Clamp health if armor lowers maxHP
                if (hp > MaxHp)
                    hp = MaxHp;
            }

            drop.PositionRect = new Rectangle(X, Y, 40, 40);
            return drop;
        }

        /// <summary>
        /// Adds a status effect to the player.
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffect(StatusEffect effect)
        {
            activeEffects.Add(effect);
        }

        /// <summary>
        /// Updates the timer for each effect and activates the
        /// non stat-modifying effects, and removes any finished effects
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateEffects(GameTime gameTime)
        {
            for (int i = 0; i < activeEffects.Count;)
            {
                activeEffects[i].Counter += gameTime.ElapsedGameTime.TotalSeconds;
                activeEffects[i].EffectInterval += gameTime.ElapsedGameTime.TotalSeconds;

                if (activeEffects[i].EffectInterval > 0.5)
                {
                    activeEffects[i].EffectInterval -= 0.5;

                    switch (activeEffects[i].Effect)
                    {
                        case StatusType.Regen:
                            Hp += activeEffects[i].Power;

                            if (Hp > MaxHp)
                                Hp = MaxHp;
                            break;

                        case StatusType.Sick:
                            Hp -= activeEffects[i].Power;
                            break;
                    }
                }

                // Remove if effect reaches end of duration
                if (activeEffects[i].Counter > activeEffects[i].Duration)
                {
                    activeEffects.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }

        /// <summary>
        /// Removes all status effects.
        /// </summary>
        public void ClearEffects()
        {
            activeEffects.Clear();
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
            else if (_animationManager != null) // Player is moving/attacking
            {
                _animationManager.Draw(sb);
            }

            // Draw any effects
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].PositionRect = new Rectangle(362 + 22 * i, 4, 20, 24);
                activeEffects[i].Draw(sb);
            }
        }
    }
}
