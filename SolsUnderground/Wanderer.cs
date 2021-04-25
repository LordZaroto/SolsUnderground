using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class Wanderer : Enemy
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        Texture2D[] textures;
        GameTime gameTime;
        float _timer = 0f;
        int moveDirection;
        //consructor: initializes the fields
        public Wanderer(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = health;
            this.attack = attack;
            this.knockback = 64;
            activeEffects = new List<StatusEffect>();
            effectCounter = 0;
            moveCD = 0.1;
            moveCounter = moveCD;
            kbCD = 2;
            kbCounter = kbCD;
            moveDirection = Program.rng.Next(0, 4);
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

        public override int Knockback
        {
            get { return knockback; }
            set { knockback = value; }
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
                moveCounter = 0;

                currentHP -= Math.Max(damage - DefenseMod, 0);

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y -= (int)(knockback);
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += (int)(knockback);
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y += (int)(knockback);
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= (int)(knockback);
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
            //Update the cooldowns
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead) && !IsStunned)
                {
                    if (Math.Abs(positionRect.X - player.X) <= 80 && Math.Abs(positionRect.Y - player.Y) <= 80)
                    {
                        if (positionRect.X > player.X)
                        {
                            texture = textures[2];
                            positionRect.X -= Math.Max(1 + SpeedMod, 0);
                            enemyState = EnemyState.moveLeft;
                        }
                        if (positionRect.X < player.X)
                        {
                            texture = textures[3];
                            positionRect.X += Math.Max(1 + SpeedMod, 0);
                            enemyState = EnemyState.moveRight;
                        }
                        if (positionRect.Y > player.Y)
                        {
                            texture = textures[1];
                            positionRect.Y -= Math.Max(1 + SpeedMod, 0);
                            enemyState = EnemyState.moveForward;
                        }
                        if (positionRect.Y < player.Y)
                        {
                            texture = textures[0];
                            positionRect.Y += Math.Max(1 + SpeedMod, 0);
                            enemyState = EnemyState.moveBack;
                        }
                    }
                    else
                    {

                        if (_timer >= 3 && _timer < 4)
                        {

                            switch (moveDirection)
                            {
                                case 0:
                                    texture = textures[3];
                                    positionRect.X += Math.Max(1 + SpeedMod, 0);
                                    break;
                                case 1:
                                    texture = textures[2];
                                    positionRect.X -= Math.Max(1 + SpeedMod, 0);
                                    break;
                                case 2:
                                    texture = textures[0];
                                    positionRect.Y += Math.Max(1 + SpeedMod, 0);
                                    break;
                                case 3:
                                    texture = textures[1];
                                    positionRect.Y -= Math.Max(1 + SpeedMod, 0);
                                    break;
                            }
                        }
                        else if (_timer >= 5)
                        {
                            _timer = 0f;
                            moveDirection = Program.rng.Next(0, 4);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides an attack hitbox equal to the wanderer's hitbox.
        /// </summary>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
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

            if (!IsStunned)
            {
                attacks.Add(new Attack(PositionRect, Attack, knockback, null, 
                    direction, 0.15, false, new StatusEffect(StatusType.Stun, 0, 0.5)));
            }

            return attacks;
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
            effectCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Update effects every half-second
            if (effectCounter > 0.5)
            {
                for (int i = 0; i < activeEffects.Count;)
                {
                    activeEffects[i].Duration -= effectCounter;

                    // Apply any active effects
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

                    // Remove if effect reaches end of duration
                    if (activeEffects[i].Duration < 0)
                    {
                        activeEffects.RemoveAt(i);
                        continue;
                    }

                    i++;
                }

                effectCounter -= 0.5;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);

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

        public void UpdateTimer(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
