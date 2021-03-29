using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//abstract enemy class that inherits from dynamic object
//all types of enemies will inherit from this class
namespace SolsUnderground
{
    abstract class Enemy : DynamicObject
    {
        //fields
        protected int health;
        protected int attack;

        //properties
        public abstract int Health
        {
            get;
            set;
        }

        public abstract int Attack
        {
            get;
            set;
        }

        //method to be overridden
        //changes health when hit by the player
        public abstract void TakeDamage(int damage);

        public abstract void EnemyMove(Player player);
    }
}
