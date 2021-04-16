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
            this.health = health;
            this.attack = attack;
            this.knockback = 64;
            moveCD = 2;
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
            get { return health; }
            set { health = value; }
        }

        public override int Attack
        {
            get { return attack; }
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

        /// <summary>
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                moveCD = 0;

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

        /// <summary>
        /// movement AI that will chase the player
        /// </summary>
        public override void EnemyMove(Player player)
        {
            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (Math.Abs(positionRect.X - player.X) <= 80 && Math.Abs(positionRect.Y - player.Y) <= 80)
                    {
                        if (positionRect.X >= player.X)
                        {
                            texture = textures[2];
                            positionRect.X -= 1;
                            enemyState = EnemyState.moveLeft;
                        }
                        if (positionRect.X < player.X)
                        {
                            texture = textures[3];
                            positionRect.X += 1;
                            enemyState = EnemyState.moveRight;
                        }
                        if (positionRect.Y >= player.Y)
                        {
                            texture = textures[1];
                            positionRect.Y -= 1;
                            enemyState = EnemyState.moveBack;
                        }
                        if (positionRect.Y < player.Y)
                        {
                            texture = textures[0];
                            positionRect.Y += 1;
                            enemyState = EnemyState.moveForward;
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
                                    positionRect.X += 1;
                                    break;
                                case 1:
                                    texture = textures[2];
                                    positionRect.X -= 1;
                                    break;
                                case 2:
                                    texture = textures[0];
                                    positionRect.Y += 1;
                                    break;
                                case 3:
                                    texture = textures[1];
                                    positionRect.Y -= 1;
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

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }

        public void UpdateTimer(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
