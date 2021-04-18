using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class VendingMachineBoss : Boss
    {
        //fields
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        Texture2D[] textures;
        GameTime gameTime;
        float _timer = 0f;
        int moveDirection;
        int maxHealth;

        public VendingMachineBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            maxHealth = health;
            this.health = maxHealth;
            this.attack = attack;
            this.knockback = 64;
            moveCD = 2;
            moveCounter = moveCD;
            kbCD = 2;
            kbCounter = kbCD;
            moveDirection = Program.rng.Next(0, 4);
        }

        //properties
        public int MaxHealth
        {
            get;
        }

        public override int Health
        {
            get;
            set;
        }

        public override int Attack
        {
            get;
            set;
        }

        public override int Knockback
        {
            get;
            set;
        }

        public override EnemyState State
        {
            get;
            set;
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


        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                moveCounter = 0;

                health -= damage;

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += (int)(knockback);
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += (int)(knockback);
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= (int)(knockback);
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= (int)(knockback);
                }

                if (health <= 0)
                {
                    enemyState = EnemyState.dead;
                }
            }
        }

        public override void EnemyMove(Player player, GameTime gameTime)
        {
            //Update the cooldowns
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if(health > maxHealth /3 * 2)
                    {

                    }
                    else if(health <= maxHealth / 3 * 2 && health > maxHealth/3)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }

        public override Attack BossAttack(Player player)
        {
            return null;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
