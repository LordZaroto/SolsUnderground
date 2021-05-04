using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    
    class JanitorBoss : Boss
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        private double sp1Counter;
        private double sp1CD;
        private double sp2Counter;
        private double sp2CD;
        private double sp1HitTimer;
        private double sp2HitTimer;
        private int prevX;
        private int prevY;
        private int xVelocity;
        private int yVelocity;
        private bool reverseX;
        private bool reverseY;
        Texture2D[] textures;
        Texture2D atkTexture;
        float puddleTimer;

        //consructor: initializes the fields
        public JanitorBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack, Texture2D atkTexture)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.atkTexture = atkTexture;
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = health;
            this.attack = attack;
            this.knockback = 32;
            activeEffects = new List<StatusEffect>();
            effectCounter = 0;
            moveCD = 0.3;
            moveCounter = moveCD;
            kbCD = 0.1;
            kbCounter = kbCD;
            sp1CD = 7;
            sp1Counter = 6;
            //sp2CD = 1;
            //sp2Counter = 0;
            sp1HitTimer = 0.1;
            sp2HitTimer = 0.1;
            prevX = 0;
            prevY = 0;
            xVelocity = 0;
            yVelocity = 0;
            reverseX = false;
            reverseY = false;
            puddleTimer = 0f;
        }

        //properties
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

        /// <summary>
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                //moveCounter = 0; Testing no knockback on bosses

                currentHP -= Math.Max(damage - DefenseMod, 0);

                //Reduces knockback recieved by 50%
                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += knockback / 2;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += knockback / 2;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= knockback / 2;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= knockback / 2;
                }

                if (currentHP <= 0)
                {
                    enemyState = EnemyState.dead;
                }
            }
        }

        /// <summary>
        /// movement AI that will chase the player
        /// </summary>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            if (!IsStunned)
            {
                //Update the cooldowns
                //sp1Counter += gameTime.ElapsedGameTime.TotalSeconds;
                sp2Counter += gameTime.ElapsedGameTime.TotalSeconds;
                moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

                //if (moveCounter >= moveCD)
                //{
                    if (!(enemyState == EnemyState.dead))
                    {
                        if (reverseX && reverseY)
                        {
                        texture = textures[0];
                            if (currentHP > maxHP / 3 * 2)
                            {
                                xVelocity = -2;
                                yVelocity = -1;
                            }
                            else if (currentHP > maxHP / 3 && currentHP <= maxHP / 3 * 2)
                            {
                                xVelocity = -3;
                                yVelocity = -2;
                            }
                            else
                            {
                                xVelocity = -4;
                                yVelocity = -3;
                            }
                        }
                        else if (reverseX)
                        {
                        texture = textures[0];
                        if (currentHP > maxHP / 3 * 2)
                            {
                                xVelocity = -2;
                                yVelocity = 1;
                            }
                            else if (currentHP > maxHP / 3 && currentHP <= maxHP / 3 * 2)
                            {
                                xVelocity = -3;
                                yVelocity = 2;
                            }
                            else
                            {
                                xVelocity = -4;
                                yVelocity = 3;
                            }
                        }
                        else  if (reverseY)
                        {
                        texture = textures[1];
                        if (currentHP > maxHP / 3 * 2)
                            {
                                xVelocity = 2;
                                yVelocity = -1;
                            }
                            else if (currentHP > maxHP / 3 && currentHP <= maxHP / 3 * 2)
                            {
                                xVelocity = 3;
                                yVelocity = -2;
                            }
                            else
                            {
                                xVelocity = 4;
                                yVelocity = -3;
                            }
                        }
                        else
                        {
                        texture = textures[1];
                        if (currentHP > maxHP / 3 * 2)
                            {
                                xVelocity = 2;
                                yVelocity = 1;
                            }
                            else if (currentHP > maxHP / 3 && currentHP <= maxHP / 3 * 2)
                            {
                                xVelocity = 3;
                                yVelocity = 2;
                            }
                            else
                            {
                                xVelocity = 4;
                                yVelocity = 3;
                            }
                        }
                        positionRect.X += xVelocity;
                        positionRect.Y += yVelocity;
                    }
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
            AttackDirection direction = AttackDirection.left;

            if (!IsStunned)
            {
                if (moveCounter >= moveCD)
                {
                    //If close to player
                    if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                    {
                        if(player.PositionRect.Intersects(new Rectangle(positionRect.X - 50, positionRect.Y - 25, 50, 100)))
                        {
                            attacks.Add(MopSwing());
                        }
                        
                    }
                    else if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                    {
                        if (player.PositionRect.Intersects(new Rectangle(positionRect.X + texture.Width, positionRect.Y - 25, 50, 100)))
                        {
                            attacks.Add(MopSwing());
                        }
                        
                    }

                    if(puddleTimer > 1)
                    {
                        puddleTimer = 0;
                        attacks.Add(PuddleDrop());
                    }
                    
                }

                // Determine direction for collision attack
                switch (enemyState)
                {
                    case EnemyState.faceForward:
                    case EnemyState.moveForward:
                        direction = AttackDirection.up;
                        break;

                    case EnemyState.faceLeft:
                    case EnemyState.moveLeft:
                        direction = AttackDirection.left;
                        break;

                    case EnemyState.faceBack:
                    case EnemyState.moveBack:
                        direction = AttackDirection.down;
                        break;

                    case EnemyState.faceRight:
                    case EnemyState.moveRight:
                        direction = AttackDirection.right;
                        break;
                }
                attacks.Add(new Attack(PositionRect, Attack, knockback, null, direction, 0.15, false, null));
            }

            return attacks;
        }

        public Attack PuddleDrop()
        {
            if (sp2Counter >= sp2CD)
            {
                //Reset the cooldown
                sp2Counter = 0;
                moveCounter = 0;

                Attack special = null;
                if (currentHP > maxHP / 3 * 2)
                {
                    if(enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.left, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }else if(enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.right, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }
                    
                }
                else if (currentHP > maxHP / 3 && currentHP <= maxHP / 3 * 2)
                {
                    if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.left, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }
                    else if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.right, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }
                }
                else
                {
                    if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.left, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }
                    else if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                    {
                        special = new Attack(PositionRect, 1, 0, textures[2], AttackDirection.right, 5, false, new StatusEffect(StatusType.Sick, 1, 3));
                    }
                }

                return special;
            }
            return null;
        }

        public Attack MopSwing()
        {
            Attack special = null;
            if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
            {
                special = new Attack(new Rectangle(positionRect.X-50, positionRect.Y-25, 50, 100), 10, 32, textures[2], AttackDirection.left, 5, false, null);
            }
            else if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
            {
                special = new Attack(new Rectangle(positionRect.X + texture.Width, positionRect.Y - 25, 50, 100), 10, 32, textures[2], AttackDirection.right, 5, false, null);
            }
            return special;
        }

        /// <summary>
        /// Adds a status effect to the enemy.
        /// </summary>
        /// <param name="effect"></param>
        public override void AddEffect(StatusEffect effect)
        {
            activeEffects.Add(effect);
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

        public void UpdateTimer(GameTime gameTime)
        {
            puddleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch sb)
        {
            // Use draw colors to give player attack tells
            /*if (sp2Counter > sp2CD - 1)
            {
                sb.Draw(texture, positionRect, Color.Purple);
            }
            else if (sp1Counter > sp1CD - 1)
            {
                sb.Draw(texture, positionRect, Color.Green);
            }
            else
            {*/
                sb.Draw(texture, positionRect, Color.White);
            //}

            // Draw HP bar
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, Width, 3),
                Color.Black);
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, (int)(Width * ((double)currentHP / (double)maxHP)), 3),
                Color.Red);

            // Draw status effects
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].PositionRect = new Rectangle(X + 6 * i, Y - 17, 5, 6);
                activeEffects[i].Draw(sb);
            }
        }
    }
}
