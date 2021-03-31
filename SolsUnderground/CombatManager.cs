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
        private List<Enemy> enemies;
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
        public CombatManager(Room room, Player player)
        {
            enemies = room.GetEnemies();
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
        /// If an enemy attack connects with the player
        /// </summary>
        public void EnemyAttack(Rectangle hitBox, int damage)
        {
            if(hitBox.Intersects(player.PositionRect))
            {
                player.TakeDamage(damage);
            }
        }
    }
}
