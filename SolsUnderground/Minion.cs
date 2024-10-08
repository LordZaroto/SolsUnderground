﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    //Braden Flanders
    //Preston Gilmore

    //minion class inherits from enemy
    //implements all of the abstract methods and properties
    class Minion : Enemy
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        Texture2D[] textures;
        //consructor: initializes the fields
        public Minion(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
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
            if(!(enemyState == EnemyState.dead))
            {
                moveCounter = 0;
                
                currentHP -= Math.Max(damage - DefenseMod, 0);

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += knockback;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += knockback;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= knockback;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= knockback;
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
                    if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.X >= player.X)
                        {
                            texture = textures[2];
                            positionRect.X -= Math.Max(3 + SpeedMod, 0);
                            enemyState = EnemyState.moveLeft;
                        }
                        else
                        {
                            texture = textures[3];
                            positionRect.X += Math.Max(3 + SpeedMod, 0);
                            enemyState = EnemyState.moveRight;
                        }
                    }
                    else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.Y >= player.Y)
                        {
                            texture = textures[1];
                            positionRect.Y -= Math.Max(3 + SpeedMod, 0);
                            enemyState = EnemyState.moveBack;
                        }
                        else
                        {
                            texture = textures[0];
                            positionRect.Y += Math.Max(3 + SpeedMod, 0);
                            enemyState = EnemyState.moveForward;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides an attack hitbox equal to the skaterBro's hitbox.
        /// </summary>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
            AttackDirection direction = AttackDirection.left;

            Rectangle temp = player.PositionRect;
            if (Rectangle.Intersect(temp, PositionRect).Width <= Rectangle.Intersect(temp, PositionRect).Height)
            {
                if (PositionRect.X > temp.X)
                {
                    direction = AttackDirection.left;
                }
                else
                {
                    direction = AttackDirection.right;
                }
            }
            else
            {
                if (PositionRect.Y > temp.Y)
                {
                    direction = AttackDirection.up;
                }
                else
                {
                    direction = AttackDirection.down;
                }
            }

            if (!IsStunned)
            {
                attacks.Add(new Attack(PositionRect, Attack, knockback, null, direction, 0.15, false, null));
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
    }
}
