using System;
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
        //consructor: initializes the fields
        public Minion(Texture2D texture, Rectangle positionRect, int health, int attack)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            this.health = health;
            this.attack = attack;
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

        /// <summary>
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage)
        {
            health -= damage;
        }

        /// <summary>
        /// movement AI that will chase the player
        /// </summary>
        public override void EnemyMove(Player player) //I got rid of your override for now - see DynamicObject for the reason
        {
            if(Math.Abs(positionRect.X-player.X) > Math.Abs(positionRect.Y - player.Y))
            {
                if(positionRect.X > player.X)
                {
                    positionRect.X--;
                }
                else
                {
                    positionRect.X++;
                }
            }else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
            {
                if (positionRect.Y > player.Y)
                {
                    positionRect.Y--;
                }
                else
                {
                    positionRect.Y++;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
