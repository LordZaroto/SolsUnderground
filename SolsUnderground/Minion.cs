﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    //minion class inherits from enemy
    //implements all of the abstract methods and properties
    class Minion : Enemy
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        Texture2D[] textures;
        //consructor: initializes the fields
        public Minion(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.health = health;
            this.attack = attack;
            moveCD = 0.3;
            moveCounter = moveCD;
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

        /// <summary>
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage)
        {
            if(!(enemyState == EnemyState.dead))
            {
                moveCD = 0;
                
                health -= damage;

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += 32;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += 32;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= 32;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= 32;
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
            if(moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.X >= player.X)
                        {
                            texture = textures[2];
                            positionRect.X -= 3;
                            enemyState = EnemyState.moveLeft;
                        }
                        else
                        {
                            texture = textures[3];
                            positionRect.X += 3;
                            enemyState = EnemyState.moveRight;
                        }
                    }
                    else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.Y >= player.Y)
                        {
                            texture = textures[1];
                            positionRect.Y -= 3;
                            enemyState = EnemyState.moveBack;
                        }
                        else
                        {
                            texture = textures[0];
                            positionRect.Y += 3;
                            enemyState = EnemyState.moveForward;
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
