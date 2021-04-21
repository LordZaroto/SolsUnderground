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
        private CollisionManager collisionManager;
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
        public CombatManager(Player player, CollisionManager collisionManager)
        {
            this.player = player;
            this.collisionManager = collisionManager;
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
                    attack = collisionManager.AttackWallCollision(attack);
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
                    if (special != null)
                    {
                        special = collisionManager.AttackWallCollision(special);//Adjusts knockback for walls
                    }

                    if(!(special == null))
                    {
                        if (special.Hitbox.Intersects(player.PositionRect) && !(boss.State == EnemyState.dead))
                        {
                            player.TakeDamage(special.Damage, special.AttackDirection, special.Knockback);
                        }
                    }
                }
                
                if (e.PositionRect.Intersects(player.PositionRect) && !(e.State == EnemyState.dead))
                {
                    Rectangle temp = player.PositionRect;
                    //A new attack is created when the player intersects with an enemy
                    Attack basic;
                    if(Rectangle.Intersect(temp, e.PositionRect).Width <= Rectangle.Intersect(temp, e.PositionRect).Height)
                    {
                        if(e.PositionRect.X > temp.X)
                        {
                            basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.left);
                        }
                        else
                        {
                            basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.right);
                        }
                    }
                    else
                    {
                        if(e.PositionRect.Y > temp.Y)
                        {
                            basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.up);
                        }
                        else
                        {
                            basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.down);
                        }
                    }
                    basic = collisionManager.AttackWallCollision(basic);//The knockback of the attack is adjusted for walls
                    player.TakeDamage(e.Attack, basic.AttackDirection, basic.Knockback);
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
