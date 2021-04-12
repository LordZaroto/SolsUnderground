using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Author: Preston Gilmore
//Hunter Wells
// Alex Dale

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
        public CombatManager(Player player)
        {
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
        public void PlayerAttack(Rectangle hitBox, int damage, int knockback)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (hitBox.Intersects(enemies[i].PositionRect))
                {
                    enemies[i].TakeDamage(damage, knockback);
                }
            }
        }

        /// <summary>
        /// If an enemy attack connects with the player, execute the resultant consequences.
        /// </summary>
        public void EnemyAttacks()
        {
            foreach (Enemy e in enemies)
            {
                if (e.PositionRect.Intersects(player.PositionRect) && !(e.State == EnemyState.dead))
                {
                    player.TakeDamage(e.Attack, e.State, e.Knockback);
                }
            }
        }

        /// <summary>
        /// Loads enemy list into combat manager.
        /// </summary>
        /// <param name="enemies">Reference to working enemy list</param>
        public void GetEnemies(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// Removes any dead enemies and gives money accordingly.
        /// </summary>
        /// <returns>Int money equal to dead enemies</returns>
        public void CleanUp(ItemManager itemManager)
        {
            for (int i = 0; i < enemies.Count;)
            {
                if (enemies[i].State == EnemyState.dead)
                {
                    // Create gold item in enemy's place
                    itemManager.DropMoney(enemies[i].Attack * 3, enemies[i].PositionRect);

                    enemies.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }
    }
}
