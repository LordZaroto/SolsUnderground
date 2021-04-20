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
        public void PlayerAttack(Attack attack)
        {
            if(attack == null)
            {
                return;
            }
            
            for (int i = 0; i < enemies.Count; i++)
            {
                if (attack.Hitbox.Intersects(enemies[i].PositionRect))
                {
                    enemies[i].TakeDamage(attack.Damage, attack.Knockback);
                }
            }
        }

        /// <summary>
        /// If an enemy attack connects with the player, execute the resultant consequences.
        /// </summary>
        public void EnemyAttacks(Player player)
        {
            foreach (Enemy e in enemies)
            {
                if(e is Boss)
                {
                    //Cast the enemy as a boss to use specials
                    Boss boss = (Boss)e;
                    Attack special = boss.BossAttack(player);

                    if(!(special == null))
                    {
                        if (special.Hitbox.Intersects(player.PositionRect) && !(boss.State == EnemyState.dead))
                        {
                            player.TakeDamage(special.Damage, boss.State, special.Knockback);
                        }
                    }
                }
                
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
                    // Activate enemy item drops
                    itemManager.EnemyDrops(enemies[i]);

                    enemies.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }
    }
}
