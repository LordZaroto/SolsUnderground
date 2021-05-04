using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    public enum AttackDirection
    {
        up,
        down,
        right,
        left,
        allAround
    }
    
    /// <summary>
    /// Holds a hitbox, damage, and knockback value.
    /// </summary>
    class Attack : GameObject
    {
        protected int knockback;
        protected int damage;
        protected AttackDirection attackDirection;
        protected double timer;
        protected bool isPlayerAttack;
        protected StatusEffect effect;

        public Rectangle Hitbox
        {
            get { return positionRect; }
        }

        public StatusEffect Effect
        {
            get { return effect; }
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

        public int Knockback
        {
            get { return knockback; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public AttackDirection AttackDirection
        {
            get { return attackDirection; }
            set { attackDirection = value; }
        }
        public double Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public bool IsPlayerAttack
        {
            get { return isPlayerAttack; }
        }

        public override int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Attack(Rectangle hitbox, int damage, int knockback, Texture2D texture, 
            AttackDirection atkDir, double timer, bool isPlayerAttack, StatusEffect effect)
        {
            this.positionRect = hitbox;
            this.damage = damage;
            this.knockback = knockback;
            this.texture = texture;
            this.timer = timer;
            this.isPlayerAttack = isPlayerAttack;
            this.effect = effect;
            attackDirection = atkDir;
        }

        public Attack(Rectangle hitbox, int damage, int knockback, AttackDirection atkDir)
        {
            this.positionRect = hitbox;
            this.damage = damage;
            this.knockback = knockback;
            attackDirection = atkDir;
        }
    }
}
