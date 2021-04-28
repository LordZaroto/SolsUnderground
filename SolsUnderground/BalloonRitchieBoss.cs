using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class BalloonRitchieBoss : Boss
    {
        // Tracks current actions of boss
        private enum AttackState
        {
            None,
            Barrage,
            Wave
        }

        // Fields
        private EnemyState enemyState;
        private AttackState attackState;
        private int phase;
        private Color[] hpBarColors;
        private Texture2D[] textures;

        private double elapsedGameTime;
        private double attackCounter;
        private double attackInterval;

        private double barrageCD;
        private double barrageCounter;
        private double barrageDuration;
        private double barrageInterval;
        private Point barrageShotSize;
        /// Inherits:
        /// Texture2D texture
        /// Rectangle positionRect
        /// int maxHP
        /// int currentHP
        /// int attack
        /// int knockback
        /// List<StatusEffects> activeEffects
        /// double effectCounter

        // Properties
        public int MaxHealth
        {
            get { return maxHP; }
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
        public override EnemyState State
        {
            get { return enemyState; }
            set { enemyState = value; }
        }
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

        // Constructor
        public BalloonRitchieBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = maxHP;
            this.attack = attack;
            this.knockback = 64;
            activeEffects = new List<StatusEffect>();
            elapsedGameTime = 0;

            phase = 0;
            hpBarColors = new Color[]{ Color.Blue, Color.Yellow, Color.Red, Color.Black};

            barrageCD = 12;
            barrageCounter = 6;
            barrageDuration = 3;
            barrageInterval = 0.3;
            barrageShotSize = new Point(20, 15);
        }

        // Methods

        /// <summary>
        /// Reduces health and inflicts knockback in the appropriate direction
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            currentHP -= Math.Max(damage - DefenseMod, 0);

            // Knockback


            // Check health for phase change/death
            if (currentHP <= 0)
            {
                currentHP = maxHP;
                phase++;

                switch (phase)
                {
                    case 1:
                        activeEffects.Add(new StatusEffect(StatusType.SpdUp, 1, double.MaxValue));
                        break;

                    case 2:
                        activeEffects.Add(new StatusEffect(StatusType.AtkUp, 2, double.MaxValue));
                        barrageInterval = 0.15;
                        break;

                    case 3:
                        currentHP = 0;
                        enemyState = EnemyState.dead;
                        break;
                }
            }
        }

        /// <summary>
        /// Moves the enemy according to its behavior patterns.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            elapsedGameTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (!IsStunned)
            {
                // Line up with enemy in Y direction
                if (player.PositionRect.Center.Y < positionRect.Center.Y)
                {
                    Y -= Math.Max(1 + SpeedMod, 0);
                }
                if (player.PositionRect.Center.Y > positionRect.Center.Y)
                {
                    Y += Math.Max(1 + SpeedMod, 0);
                }

                // Flee from enemy in X direction if too close
                if (Math.Abs(player.PositionRect.Center.X - positionRect.Center.X) < 500)
                {
                    if (player.PositionRect.Center.X < positionRect.Center.X)
                    {
                        X += Math.Max(1 + SpeedMod, 0);
                    }
                    if (player.PositionRect.Center.X > positionRect.Center.X)
                    {
                        X -= Math.Max(1 + SpeedMod, 0);
                    }
                }
                else if (Math.Abs(player.PositionRect.Center.X - positionRect.Center.X) > 550)
                {
                    if (player.PositionRect.Center.X < positionRect.Center.X)
                    {
                        X -= Math.Max(1 + SpeedMod, 0);
                    }
                    if (player.PositionRect.Center.X > positionRect.Center.X)
                    {
                        X += Math.Max(1 + SpeedMod, 0);
                    }
                }

                // Face player
                if (Math.Abs(positionRect.Center.X - player.PositionRect.Center.X)
                    > Math.Abs(positionRect.Center.Y - player.PositionRect.Center.Y))
                {
                    if (player.PositionRect.Center.X < positionRect.Center.X)
                    {
                        enemyState = EnemyState.faceLeft;
                        texture = textures[1];
                    }
                    if (player.PositionRect.Center.X > positionRect.Center.X)
                    {
                        enemyState = EnemyState.faceRight;
                        texture = textures[3];
                    }
                }
                else // Y difference is greater than X difference
                {
                    if (player.PositionRect.Center.Y < positionRect.Center.Y)
                    {
                        enemyState = EnemyState.faceForward;
                        texture = textures[0];
                    }
                    if (player.PositionRect.Center.Y > positionRect.Center.Y)
                    {
                        enemyState = EnemyState.faceBack;
                        texture = textures[2];
                    }
                }
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

            if (enemyState != EnemyState.dead)
            {
                if (!IsStunned)
                {
                    // Determine direction for attacks
                    AttackDirection direction = AttackDirection.left;
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

                    // Check for ongoing attacks
                    switch (attackState)
                    {
                        case AttackState.Barrage:
                            attacks.Add(Barrage(direction));
                            break;

                        case AttackState.Wave:
                            break;

                        case AttackState.None:
                            barrageCounter += elapsedGameTime;

                            if (barrageCounter > barrageCD)
                            {
                                barrageCounter -= barrageCD;
                                attackCounter = 0;
                                attackInterval = 0;
                                attackState = AttackState.Barrage;
                            }
                            break;
                    }

                    attacks.Add(new Attack(PositionRect, Attack, knockback, null, direction, 0.15, false, null));
                }
            }
            else
            {
                // Pop on death
            }

            return attacks;
        }

        /// <summary>
        /// Shoots several balloons towards the player over several seconds.
        /// </summary>
        /// <returns>One Projectile object every 0.5 seconds, null in frames between</returns>
        private Projectile Barrage(AttackDirection direction)
        {
            if (attackCounter < barrageDuration)
            {
                // Update timers
                attackCounter += elapsedGameTime;
                attackInterval -= elapsedGameTime;

                // Launch a projectile every 0.3 seconds, faster in later phases
                if (attackInterval <= 0)
                {
                    attackInterval += barrageInterval;
                    Rectangle shotRect;

                    if (direction == AttackDirection.up || direction == AttackDirection.down)
                    {
                        shotRect = new Rectangle(
                            X + Program.rng.Next(Width - barrageShotSize.Y),
                            positionRect.Center.Y,
                            barrageShotSize.Y,
                            barrageShotSize.X);
                    }
                    else
                    {
                        shotRect = new Rectangle(
                            positionRect.Center.X,
                            Y + Program.rng.Next(Height - barrageShotSize.Y),
                            barrageShotSize.X,
                            barrageShotSize.Y);
                    }

                    return new Projectile(shotRect, Attack / 2, 4, Knockback / 4,
                        Program.drawSquare, direction, false, null);
                }
            }
            else
            {
                attackState = AttackState.None;
            }

            return null;
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

        /// <summary>
        /// Draws the BalloonRitchie boss and its local UI.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw indicators for attacks
            if (barrageCounter > barrageCD - 1)
                sb.Draw(texture, positionRect, Color.Green);
            else
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
