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
        public Minion(Texture2D texture, Vector2 position, int health, int attack)
        {
            this.texture = texture;
            this.position = position;
            this.health = health;
            this.attack = attack;
        }

        //properties
        public override float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public override float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
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
        public void Move() //I got rid of your override for now - see DynamicObject for the reason
        {
            if(Math.Abs(position.X-Player.X) > Math.Abs(position.Y - Player.Y))
            {
                if(position.X > Player.X)
                {
                    position.X--;
                }
                else
                {
                    position.X++;
                }
            }else if (Math.Abs(position.X - Player.X) < Math.Abs(position.Y - Player.Y))
            {
                if (position.Y > Player.Y)
                {
                    position.Y--;
                }
                else
                {
                    position.Y++;
                }
            }
        }
    }
}
