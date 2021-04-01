using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Author: Preston Gilmore

namespace SolsUnderground
{
    /// <summary>
    /// Deals with all combat related collisions.
    /// </summary>
    class CombatManager
    {
        //Fields
        //-----------------------------
        public List<Enemy> enemies;
        private Player player;
        //-----------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Properties
        //----------------------------------------
        /// <summary>
        /// The amount of enemies currently in play.
        /// </summary>
        public int EnemyCount
        {
            get { return enemies.Count; }
        }
        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Constructor
        //----------------------------------------------------------
        public CombatManager(List<Enemy> enemies, Player player)
        {
            this.enemies = enemies;
            this.player = player;
        }
        //----------------------------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Methods
        //----------------------------------------------------------
        /// <summary>
        /// If that player's attack connects, execute the resultant consequences.
        /// </summary>
        public void PlayerAttack(Rectangle hitBox, int damage)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(hitBox.Intersects(enemies[i].PositionRect))
                {
                    enemies[i].TakeDamage(damage);
                }
            }
        }

        /// <summary>
        /// If an enemy attack connects with the player, execute the resultant consequences.
        /// </summary>
        public void EnemyAttack(Rectangle hitBox, int damage, EnemyState state)
        {
            if(hitBox.Intersects(player.PositionRect) && !(state == EnemyState.dead))
            {
                player.TakeDamage(damage);
            }
        }

        /// <summary>
        /// Take care of the dead enemies in the room.
        /// </summary>
        public void CleanUp() //Not to be implemented at the moment
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].State == EnemyState.dead)
                {
                    enemies.RemoveAt(i);
                }
            }
        }
    }
}
