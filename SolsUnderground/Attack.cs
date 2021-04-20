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
        left
    }
    
    /// <summary>
    /// Holds a hitbox, damage, and knockback value.
    /// </summary>
    class Attack : GameObject
    {
        private int knockback;
        private int damage;
        private AttackDirection attackDirection;
        private double timer;

        public Rectangle Hitbox
        {
            get { return positionRect; }
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

        public Attack(Rectangle hitbox, int damage, int knockback, Texture2D texture, AttackDirection atkDir, double timer)
        {
            this.positionRect = hitbox;
            this.damage = damage;
            this.knockback = knockback;
            this.texture = texture;
            this.timer = timer;
            attackDirection = atkDir;
        }
    }
}
