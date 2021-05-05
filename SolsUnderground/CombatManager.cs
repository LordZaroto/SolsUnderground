using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        private List<Attack> activeAttacks;
        private List<double> attackIntervals;
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
            activeAttacks = new List<Attack>();
            attackIntervals = new List<double>();
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
            if (attack == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (attack.Hitbox.Intersects(enemies[i].PositionRect))
                {
                    attack = collisionManager.AdjustAttackKnockback(attack);
                    enemies[i].TakeDamage(attack.Damage, attack.Knockback);
                }
            }
        }

        /// <summary>
        /// If an enemy attack connects with the player, execute the resultant consequences.
        /// </summary>
        //public void EnemyAttacks(Player player)
        //{
        //    foreach (Enemy e in enemies)
        //    {
        //        if(e is Boss)
        //        {
        //            //Cast the enemy as a boss to use specials
        //            Boss boss = (Boss)e;
        //            Attack special = boss.EnemyAttack(player);
        //            if (special != null)
        //            {
        //                special = collisionManager.AttackWallCollision(special);//Adjusts knockback for walls
        //            }
        //
        //            if(!(special == null))
        //            {
        //                if (special.Hitbox.Intersects(player.PositionRect) && !(boss.State == EnemyState.dead))
        //                {
        //                    player.TakeDamage(special.Damage, special.AttackDirection, special.Knockback);
        //                }
        //            }
        //        }
        //        
        //        if ((e.PositionRect.Intersects(player.PositionRect) || player.PositionRect.Contains(e.PositionRect)
        //            || e.PositionRect.Contains(player.PositionRect)) && !(e.State == EnemyState.dead))
        //        {
        //            Rectangle temp = player.PositionRect;
        //            //A new attack is created when the player intersects with an enemy
        //            Attack basic;
        //            if(Rectangle.Intersect(temp, e.PositionRect).Width <= Rectangle.Intersect(temp, e.PositionRect).Height)
        //            {
        //                if(e.PositionRect.X > temp.X)
        //                {
        //                    basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.left);
        //                }
        //                else
        //                {
        //                    basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.right);
        //                }
        //            }
        //            else
        //            {
        //                if(e.PositionRect.Y > temp.Y)
        //                {
        //                    basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.up);
        //                }
        //                else
        //                {
        //                    basic = new Attack(e.PositionRect, e.Attack, e.Knockback, AttackDirection.down);
        //                }
        //            }
        //            basic = collisionManager.AttackWallCollision(basic);//The knockback of the attack is adjusted for walls
        //            player.TakeDamage(e.Attack, basic.AttackDirection, basic.Knockback);
        //        }
        //        
        //    }
        //}

        /// <summary>
        /// Loads enemy list into combat manager.
        /// </summary>
        /// <param name="enemies">Reference to working enemy list</param>
        public void SetEnemyList(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// Loads all player and enemy attacks into active attack list.
        /// </summary>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        public void LoadAttacks(MouseState mouse, MouseState prevMouse)
        {
            // Load player attacks
            Attack pBasic = player.BasicAttack(mouse.LeftButton, prevMouse.LeftButton);
            Attack pSpecial = player.Special(mouse.RightButton, prevMouse.RightButton);

            if (pBasic != null)
            {
                activeAttacks.Add(pBasic);
                attackIntervals.Add(0.15);
            }
            if (pSpecial != null)
            {
                activeAttacks.Add(pSpecial);
                attackIntervals.Add(0.15);
            }

            // Load enemy attacks
            List<Attack> eAttacks;
            foreach (Enemy e in enemies)
            {
                eAttacks = e.EnemyAttack(player);

                for (int i = 0; i < eAttacks.Count; i++)
                {
                    if (eAttacks[i] != null)
                    {
                        if (!(eAttacks[i] is Projectile))
                        {


                            //Adjusts knockback of each enemy attack based on positions of barriers relative to the atackee
                            eAttacks[i] = collisionManager.AdjustAttackKnockback(eAttacks[i]);
                            
                            
                        }
                        activeAttacks.Add(eAttacks[i]);
                        attackIntervals.Add(0.15);
                    }
                }
            }
        }

        /// <summary>
        /// Checks attack collisions with entities, deals damage/knockback as appropriate,
        /// and clears any attacks that have reached the end of their duration.
        /// </summary>
        /// <param name="gameTime"></param>
        public void ActivateAttacks(GameTime gameTime)
        {
            for (int i = 0; i < activeAttacks.Count;)
            {
                // Filter hitboxes of attacks
                // Remove projectiles colliding with walls
                if (!collisionManager.AttackWallCollisions(activeAttacks[i]))
                {
                    activeAttacks.RemoveAt(i);
                    attackIntervals.RemoveAt(i);
                    continue;
                }

                // If enemy attack, check against player
                if (!activeAttacks[i].IsPlayerAttack)
                {
                    if (activeAttacks[i].Hitbox.Intersects(player.PositionRect) && attackIntervals[i] > 0.15)
                    {
                        if (activeAttacks[i] is Projectile)
                        {
                            activeAttacks.RemoveAt(i);
                            attackIntervals.RemoveAt(i);
                            //continue;
                        }
                        attackIntervals[i] -= 0.15;

                        activeAttacks[i] = collisionManager.AdjustAttackKnockback(activeAttacks[i]);

                        player.TakeDamage(activeAttacks[i].Damage,
                            activeAttacks[i].AttackDirection, activeAttacks[i].Knockback);

                        // Apply effects
                        if (activeAttacks[i].Effect != null)
                            player.AddEffect(activeAttacks[i].Effect);

                        
                    }
                }
                else // If player attack, check against all enemies
                {
                    if (activeAttacks[i] is Projectile)
                    {
                        bool hit = false;

                        foreach (Enemy e in enemies)
                        {
                            if (activeAttacks[i].Hitbox.Intersects(e.PositionRect) && attackIntervals[i] > 0.15)
                            {
                                attackIntervals[i] -= 0.15;

                                e.TakeDamage(activeAttacks[i].Damage, activeAttacks[i].Knockback);

                                // Apply effects
                                if (activeAttacks[i].Effect != null)
                                    e.AddEffect(activeAttacks[i].Effect);

                                hit = true;
                            }

                            if (hit)
                                break;
                        }

                        if (hit)
                        {
                            activeAttacks.RemoveAt(i);
                            attackIntervals.RemoveAt(i);
                            continue;
                        }
                    }
                    else // Non-projectile attacks
                    {
                        foreach (Enemy e in enemies)
                        {
                            if (activeAttacks[i].Hitbox.Intersects(e.PositionRect) && attackIntervals[i] > 0.15)
                            {
                                e.TakeDamage(activeAttacks[i].Damage, activeAttacks[i].Knockback);

                                // Apply effects
                                if (activeAttacks[i].Effect != null)
                                    e.AddEffect(activeAttacks[i].Effect);
                            }
                        }

                        if (attackIntervals[i] > 0.15)
                            attackIntervals[i] -= 0.15;
                    }
                }

                // If timer runs out, remove attack
                if ((activeAttacks[i].Timer -= gameTime.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    activeAttacks.RemoveAt(i);
                    attackIntervals.RemoveAt(i);
                    continue;
                }

                // Increment attack intervals
                attackIntervals[i] += gameTime.ElapsedGameTime.TotalSeconds;

                // Move any active projectiles
                if (activeAttacks[i] is Projectile)
                {
                    Projectile p = (Projectile)activeAttacks[i];
                    p.Move();
                    //((Projectile)activeAttacks[i]).Move();
                }

                i++;
            }
        }

        /// <summary>
        /// Draws all active attacks on screen.
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void DrawAttacks(SpriteBatch sb)
        {
            float rotation = 0f;

            foreach (Attack a in activeAttacks)
            {
                if (a.Texture != null)
                {
                    switch (a.AttackDirection)
                    {
                        case AttackDirection.up:
                            rotation = MathHelper.Pi + MathHelper.PiOver2;
                            break;
                        case AttackDirection.left:
                            rotation = MathHelper.Pi;
                            break;
                        case AttackDirection.down:
                            rotation = MathHelper.PiOver2;
                            break;
                        case AttackDirection.right:
                            rotation = MathHelper.TwoPi;
                            break;
                    }

                    // Figure out how to rotate sprites for attack animations
                    //sb.Draw(a.Texture, a.Hitbox, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 1f);

                    sb.Draw(a.Texture, a.Hitbox, Color.White);
                }
            }
        }

        /// <summary>
        /// Removes all active attacks.
        /// </summary>
        public void ClearAttacks()
        {
            activeAttacks.Clear();
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
