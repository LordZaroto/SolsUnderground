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

        private Point shotSize;
        private int shotSpeed;
        private double elapsedGameTime;
        private double attackCounter;
        private double attackInterval;

        private double barrageCD;
        private double barrageCounter;
        private double barrageDuration;
        private double barrageInterval;

        private double waveCD;
        private double waveCounter;
        private double waveDuration;
        private double waveInterval;

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
            shotSize = new Point(20, 15);
            shotSpeed = 4;
            attackCounter = 0;
            attackInterval = 0;

            barrageCD = 9;
            barrageCounter = 0;
            barrageDuration = 3;
            barrageInterval = 0.3;

            waveCD = 5;
            waveCounter = 0;
            waveDuration = 3;
            waveInterval = 1;
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
            // No knockback

            // Check health for phase change/death
            if (currentHP <= 0)
            {
                currentHP = maxHP;
                phase++;

                switch (phase)
                {
                    case 1:
                        // After first phase, speed up and attack intervals are halved
                        activeEffects.Add(new StatusEffect(StatusType.SpdUp, 2, double.MaxValue));
                        barrageInterval /= 2f;
                        waveInterval /= 2f;
                        break;

                    case 2:
                        // After second phase, attack up, shot speed doubled, barrage cooldown shortened, and barrage interval halved again
                        activeEffects.Add(new StatusEffect(StatusType.AtkUp, 2, double.MaxValue));
                        barrageCD /= 3f;
                        barrageInterval /= 2f;
                        shotSpeed *= 2;

                        // Make sure barrage doesnt happen immediately due to shortening
                        barrageCD = 4;
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
                        texture = textures[2];
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
                        texture = textures[1];
                    }
                }
            }
        }

        /// <summary>
        /// Attacks the player using barrages and waves of balloons.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();

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

            if (enemyState != EnemyState.dead)
            {
                if (!IsStunned)
                {
                    // Check for ongoing attacks
                    switch (attackState)
                    {
                        case AttackState.Barrage:
                            attacks.Add(Barrage(direction));
                            break;

                        case AttackState.Wave:
                            attacks.AddRange(Wave(direction));
                            break;

                        case AttackState.None:
                            // Increment timers when not attacking
                            barrageCounter += elapsedGameTime;
                            waveCounter += elapsedGameTime;

                            if (barrageCounter > barrageCD)
                            {
                                barrageCounter -= barrageCD;
                                attackCounter = 0;
                                attackInterval = 0;
                                attackState = AttackState.Barrage;
                            }
                            else if (waveCounter > waveCD)
                            {
                                waveCounter -= waveCD;
                                attackCounter = 0;
                                attackInterval = 0;
                                attackState = AttackState.Wave;
                            }
                            break;
                    }

                    attacks.Add(new Attack(PositionRect, Attack, knockback, null, direction, 0.15, false, null));
                }
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
                    Rectangle shotRect = new Rectangle();

                    switch (direction)
                    {
                        case AttackDirection.up:
                            shotRect = new Rectangle(
                                X + Program.rng.Next(Width - (int)(shotSize.Y * 1.5)),
                                Y,
                                (int)(shotSize.Y * 1.5),
                                (int)(shotSize.X * 1.5));
                            break;

                        case AttackDirection.left:
                            shotRect = new Rectangle(
                            X,
                            Y + Program.rng.Next(Height - (int)(shotSize.Y * 1.5)),
                            (int)(shotSize.X * 1.5),
                            (int)(shotSize.Y * 1.5));
                            break;

                        case AttackDirection.down:
                            shotRect = new Rectangle(
                                X + Program.rng.Next(Width - (int)(shotSize.Y * 1.5)),
                                positionRect.Bottom - (int)(shotSize.X * 1.5),
                                (int)(shotSize.Y * 1.5),
                                (int)(shotSize.X * 1.5));
                            break;

                        case AttackDirection.right:
                            shotRect = new Rectangle(
                            positionRect.Right - (int)(shotSize.X * 1.5),
                            Y + Program.rng.Next(Height - (int)(shotSize.Y * 1.5)),
                            (int)(shotSize.X * 1.5),
                            (int)(shotSize.Y * 1.5));
                            break;
                    }

                    return new Projectile(shotRect, Attack, (int)(shotSpeed * 1.5), Knockback / 4,
                        textures[4], direction, false, null);
                }
            }
            else
            {
                // Ensure break between attacks
                attackState = AttackState.None;
                waveCounter -= 0.5;
            }

            return null;
        }

        /// <summary>
        /// Shoots walls of balloons towards the player at set intervals.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>List of Projectiles once a second, empty List all other frames</returns>
        private List<Projectile> Wave(AttackDirection direction)
        {
            List<Projectile> shots = new List<Projectile>();

            if (attackCounter < waveDuration)
            {
                // Update timers
                attackCounter += elapsedGameTime;
                attackInterval -= elapsedGameTime;

                // Launch a wall of projectiles every second, faster in later phases
                if (attackInterval <= 0)
                {
                    attackInterval += waveInterval;
                    Rectangle shotRect;

                    switch (direction)
                    {
                        case AttackDirection.up:
                            shotRect = new Rectangle(
                                positionRect.Center.X,
                                Y,
                                shotSize.Y,
                                shotSize.X);

                            // Add center shot
                            shots.Add(new Projectile(shotRect, Attack, shotSpeed, Knockback, 
                                textures[4], direction, false, null));

                            // Add shots extending to the size of BalloonRitchie
                            for (int i = shotSize.Y; i < Width / 2; i += shotSize.Y)
                            {
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X + i, shotRect.Y, shotRect.Width, shotRect.Height), 
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X - i, shotRect.Y, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                            }
                            break;

                        case AttackDirection.left:
                            shotRect = new Rectangle(
                                X,
                                positionRect.Center.Y,
                                shotSize.X,
                                shotSize.Y);

                            // Add center shot
                            shots.Add(new Projectile(shotRect, Attack, shotSpeed, Knockback,
                                textures[4], direction, false, null));

                            // Add shots extending to the size of BalloonRitchie
                            for (int i = shotSize.Y; i < Height / 2; i += shotSize.Y)
                            {
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X, shotRect.Y + i, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X, shotRect.Y - i, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                            }
                            break;

                        case AttackDirection.down:
                            shotRect = new Rectangle(
                                positionRect.Center.X,
                                positionRect.Bottom - shotSize.X,
                                shotSize.Y,
                                shotSize.X);

                            // Add center shot
                            shots.Add(new Projectile(shotRect, Attack, shotSpeed, Knockback,
                                textures[4], direction, false, null));

                            // Add shots extending to the size of BalloonRitchie
                            for (int i = shotSize.Y; i < Width / 2; i += shotSize.Y)
                            {
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X + i, shotRect.Y, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X - i, shotRect.Y, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                            }
                            break;

                        case AttackDirection.right:
                            shotRect = new Rectangle(
                                positionRect.Right - shotSize.X,
                                positionRect.Center.Y,
                                shotSize.X,
                                shotSize.Y);

                            // Add center shot
                            shots.Add(new Projectile(shotRect, Attack, shotSpeed, Knockback,
                                textures[4], direction, false, null));

                            // Add shots extending to the size of BalloonRitchie
                            for (int i = shotSize.Y; i < Height / 2; i += shotSize.Y)
                            {
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X, shotRect.Y + i, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                                shots.Add(new Projectile(
                                    new Rectangle(shotRect.X, shotRect.Y - i, shotRect.Width, shotRect.Height),
                                    Attack, shotSpeed, Knockback,
                                    textures[4], direction, false, null));
                            }
                            break;

                    }
                }
            }
            else
            {
                // Ensure break between attacks
                attackState = AttackState.None;
                barrageCounter -= 0.5;
            }

            return shots;
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
            if (barrageCounter > barrageCD - 1 && attackState == AttackState.None)
                sb.Draw(texture, positionRect, Color.Green);
            else if (waveCounter > waveCD - 1 && attackState == AttackState.None)
                sb.Draw(texture, positionRect, Color.Blue);
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
