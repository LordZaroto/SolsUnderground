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


        // Methods

        /// <summary>
        /// Reduces health and inflicts knockback in the appropriate direction
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public override void AddEffect(StatusEffect effect)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateEffects(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            
        }
    }
}
