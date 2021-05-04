using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class BusBoss : Boss
    {
        //Fields
        private EnemyState enemyState;
        private Texture2D[] textures;
        private AttackDirection attackDirection;
        private double moveCounter;
        private double moveCD;
        private int phase;
        private Color[] hpBarColors;
        private bool reverseX;
        private bool reverseY;
        private int xVelocity;
        private int yVelocity;

        //Properties
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

        public override int Health
        {
            get { return currentHP; }
            set { currentHP = value; }
        }

        public override int Attack
        {
            get { return Math.Max(attack + AttackMod, 0); }
            set { attack = value; }
        }

        public override int Knockback
        {
            get { return knockback; }
            set { knockback = value; }
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

        public override EnemyState State
        {
            get { return enemyState; }
            set { enemyState = value; }
        }

        public bool ReverseX
        {
            get { return reverseX; }
            set { reverseX = value; }
        }

        public bool ReverseY
        {
            get { return reverseY; }
            set { reverseY = value; }
        }

        public int XVelocity
        {
            get { return xVelocity; }
        }

        public int YVelocity
        {
            get { return yVelocity; }
        }

        protected override int AttackMod
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
        protected override int DefenseMod
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
        protected override int SpeedMod
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
        protected override bool IsStunned
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

        //Constructor
        public BusBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = maxHP;
            this.attack = attack;
            this.knockback = 100;
            moveCD = 2;
            moveCounter = moveCD;
            attackDirection = AttackDirection.left;
            reverseX = false;
            reverseY = false;
            activeEffects = new List<StatusEffect>();
            effectCounter = 0;
            phase = 0;
            hpBarColors = new Color[] { Color.Blue, Color.Yellow, Color.Red, Color.Black };
        }

        //Methods

        /// <summary>
        /// Adjusts the boss's health when it is attacked
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if(!(enemyState == EnemyState.dead))
            {
                currentHP -= Math.Max(damage - DefenseMod, 0);

                if (currentHP <= 0)
                {
                    currentHP = maxHP;
                    phase++;
                    if(phase == 3)
                    {
                        currentHP = 0;
                        enemyState = EnemyState.dead;
                    }
                }

                //Bus should not be knocked back
            }
        }

        /// <summary>
        /// The bus continuously moves and gains speed as it decreases in health.
        /// If the bus collides with a wall, it changes direction, as handled in 
        /// the CollisionManager class
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if(moveCounter >= moveCD)
            {

                if(phase == 0)
                {
                    //if reverseX is true, the bus will move in the negative x direction
                    if (reverseX)
                    {
                        texture = textures[2];
                        xVelocity = -2;
                    }
                    else
                    {
                        texture = textures[3];
                        xVelocity = 2;
                    }
                    //if reverseY is true, the bus will move in the negative y direction
                    if (reverseY)
                    {
                        yVelocity = -2;
                    }
                    else
                    {
                        yVelocity = 2;
                    }
                }
                //The bus is split into 3 stages in which it moves faster based on its health
                else if(phase == 1)
                {
                    if (reverseX)
                    {
                        texture = textures[2];
                        xVelocity = -4;
                    }
                    else
                    {
                        texture = textures[3];
                        xVelocity = 4;
                    }
                    if (reverseY)
                    {
                        yVelocity = -4;
                    }
                    else
                    {
                        yVelocity = 4;
                    }
                }
                else if(phase == 2)
                {
                    if (reverseX)
                    {
                        texture = textures[2];
                        xVelocity = -6;
                    }
                    else
                    {
                        texture = textures[3];
                        xVelocity = 6;
                    }
                    if (reverseY)
                    {
                        yVelocity = -6;
                    }
                    else
                    {
                        yVelocity = 6;
                    }
                }
                positionRect.X += xVelocity;
                positionRect.Y += yVelocity;
            }
        }

        /// <summary>
        /// Adds a status effect to the enemy.
        /// </summary>
        /// <param name="effect"></param>
        public override void AddEffect(StatusEffect effect)
        {
            //The bus can not be stunned
            if (effect.Effect != StatusType.Stun)
            {
                activeEffects.Add(effect);
            }
        }

        /// <summary>
        /// Updates the timer for each effect and activates the
        /// non stat-modifying effects, and removes any finished effects
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateEffects(GameTime gameTime)
        {
            for (int i = 0; i < activeEffects.Count;)
            {
                activeEffects[i].Counter += gameTime.ElapsedGameTime.TotalSeconds;
                activeEffects[i].EffectInterval += gameTime.ElapsedGameTime.TotalSeconds;

                // Check interval for regen/sickness
                if (activeEffects[i].EffectInterval > 0.5)
                {
                    activeEffects[i].EffectInterval -= 0.5;

                    switch (activeEffects[i].Effect)
                    {
                        case StatusType.Regen:
                            Health += activeEffects[i].Power;

                            if (Health > maxHP)
                                Health = maxHP;
                            break;

                        case StatusType.Sick:
                            Health -= activeEffects[i].Power;

                            if (currentHP <= 0)
                                enemyState = EnemyState.dead;
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
        /// The hitbox with which the bus can attack the player is only in the front of the bus 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
            AttackDirection direction = AttackDirection.left;
            Rectangle busFront;

            switch (reverseX)
            {
                case true:
                    direction = AttackDirection.left;
                    busFront = new Rectangle(positionRect.X, positionRect.Y,
                        positionRect.Width / 8, positionRect.Height);
                    break;
                case false:
                    direction = AttackDirection.right;
                    busFront = new Rectangle(positionRect.X + (positionRect.Width / 8 * 7),
                        positionRect.Y, positionRect.Width / 8, positionRect.Height);
                    break;
            }

            if (!IsStunned)
            {
                attacks.Add(new Attack(busFront, Attack, knockback, null, direction, 0.15, false, null));
            }
            return attacks;
        }

        public override void Draw(SpriteBatch sb)
        {
            //Draw sprite
            sb.Draw(texture, positionRect, Color.White);

            // Draw HP bar
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, Width, 3),
                hpBarColors[phase + 1]);
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, (int)(Width * ((double)currentHP / (double)maxHP)), 3),
                hpBarColors[phase]);

            // Draw status effects
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].PositionRect = new Rectangle(X + 6 * i, Y - 17, 5, 6);
                activeEffects[i].Draw(sb);
            }
        }
    }
}
