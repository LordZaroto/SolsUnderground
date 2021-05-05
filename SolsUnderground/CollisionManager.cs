using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// Braden Flanders
/// Preston Gilmore
/// Noah Flanders
/// This class is responsible for managing collisions between static and dynamic
/// game objects, or any collision that doens't deal damage to an entity.
/// </summary>

namespace SolsUnderground
{
    class CollisionManager
    {
        // Fields
        private List<Rectangle> barriers;
        private Player player;
        private List<Enemy> enemies;
        private List<Item> items;
        private List<Chest> chests;
        private int windowWidth;
        private int windowHeight;

        // Constructor
        public CollisionManager(Player player, int windowWidth, int windowHeight)
        {
            this.player = player;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        // Methods

        /// <summary>
        /// Load enemy list into the collision manager.
        /// </summary>
        /// <param name="enemies">Reference to working enemy list</param>
        public void SetEnemyList(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// Load item list and chest list into the collision manager.
        /// </summary>
        /// <param name="items"></param>
        public void SetItemList(List<Item> items, List<Chest> chests)
        {
            this.items = items;
            this.chests = chests;
        }

        /// <summary>
        /// Loads current room's barriers into the collision manager.
        /// </summary>
        /// <param name="barriers">List of Rectangles for barrier hitboxes</param>
        public void SetBarrierList(List<Rectangle> barriers)
        {
            this.barriers = barriers;

            // Add barrier to right-hand side of screen - to be removed after enemies are cleared
            barriers.Add(new Rectangle(windowWidth, 0, 250, windowHeight));
        }

        /// <summary>
        /// Removes the barrier used to block player from moving into next room.
        /// </summary>
        public void OpenNextRoom()
        {
            barriers.RemoveAt(barriers.Count - 1);
        }

        /// <summary>
        /// Detects and fixes collisions between active entities and room barriers.
        /// </summary>
        public void CheckCollisions()
        {
            // Player collisions
            FixWallCollisions(player);
            FixChestCollisions(player);

            // Put lists in same loop for efficiency
            int loopMax = Math.Max(enemies.Count, items.Count);
            for (int i = 0; i < loopMax; i++)
            {
                if (i < enemies.Count)
                {
                    // Enemy collisions
                    FixWallCollisions(enemies[i]);
                    FixChestCollisions(enemies[i]);
                }
                if (i < items.Count)
                {
                    // Item-wall collisions
                    FixWallCollisions(items[i]);
                }
            }
        }

        /// <summary>
        /// Detects and corrects collisions with current barriers.
        /// </summary>
        /// <param name="gameObject">Object to detect collisions of</param>
        public void FixWallCollisions(GameObject gameObject)
        {
            Rectangle temp = gameObject.PositionRect;

            for (int i = 0; i < barriers.Count; i++)
            {
                //checks if the player intersects with a barrier
                if (barriers[i].Intersects(temp))
                {
                    //checks if the x or y needs to be adjusted
                    if (Rectangle.Intersect(temp, barriers[i]).Width <= Rectangle.Intersect(temp, barriers[i]).Height)
                    {
                        //adjusts the position
                        if (barriers[i].X > temp.X)
                        {
                            //The Bus continues to move and bounce off of walls changing direction
                            temp.X -= Rectangle.Intersect(temp, barriers[i]).Width;
                            if(gameObject is BusBoss)
                            {
                                BusBoss b = (BusBoss)gameObject;
                                b.ReverseX = true;
                                b.State = EnemyState.moveLeft;
                            }
                        
                            if(gameObject is JanitorBoss)
                            {
                                JanitorBoss j = (JanitorBoss)gameObject;
                                j.ReverseX = true;
                            }
                        }
                        else
                        {
                            temp.X += Rectangle.Intersect(temp, barriers[i]).Width;
                            if (gameObject is BusBoss)
                            {
                                BusBoss b = (BusBoss)gameObject;
                                b.ReverseX = false;
                                b.State = EnemyState.moveRight;
                            }

                            if (gameObject is JanitorBoss)
                            {
                                JanitorBoss j = (JanitorBoss)gameObject;
                                j.ReverseX = false;
                            }
                        }
                    }
                    else
                    {
                        if (barriers[i].Y > temp.Y)
                        {
                            temp.Y -= Rectangle.Intersect(temp, barriers[i]).Height;
                            if (gameObject is JanitorBoss)
                            {
                                JanitorBoss j = (JanitorBoss)gameObject;
                                j.ReverseY = true;
                            }
                        
                            if (gameObject is BusBoss)
                            {
                                BusBoss b = (BusBoss)gameObject;
                                b.ReverseY = true;
                                b.State = EnemyState.moveBack;
                            }
                        }
                        else
                        {
                            temp.Y += Rectangle.Intersect(temp, barriers[i]).Height;
                            if (gameObject is JanitorBoss)
                            {
                                JanitorBoss j = (JanitorBoss)gameObject;
                                j.ReverseY = false;
                            }
                            if (gameObject is BusBoss)
                            {
                                BusBoss b = (BusBoss)gameObject;
                                b.ReverseY = false;
                                b.State = EnemyState.moveForward;
                            }
                        }
                    }
                }

                gameObject.X = temp.X;
                gameObject.Y = temp.Y;
            }
        }

        /// <summary>
        /// Detects and corrects collisions with active chests.
        /// </summary>
        /// <param name="gameObject"></param>
        public void FixChestCollisions(GameObject gameObject)
        {
            Rectangle temp = gameObject.PositionRect;
            Rectangle chest;

            for (int i = 0; i < chests.Count; i++)
            {
                chest = chests[i].PositionRect;

                //checks if the player intersects with a barrier
                if (chest.Intersects(temp) && !chests[i].IsOpen)
                {
                    //checks if the x or y needs to be adjusted
                    if (Rectangle.Intersect(temp, chest).Width <= Rectangle.Intersect(temp, chest).Height)
                    {
                        //adjusts the position
                        if (chest.X > temp.X)
                        {
                            temp.X -= Rectangle.Intersect(temp, chest).Width;
                        }
                        else
                        {
                            temp.X += Rectangle.Intersect(temp, chest).Width;
                        }
                    }
                    else
                    {
                        if (chest.Y > temp.Y)
                        {
                            temp.Y -= Rectangle.Intersect(temp, chest).Height;
                        }
                        else
                        {
                            temp.Y += Rectangle.Intersect(temp, chest).Height;
                        }
                    }
                }

                gameObject.X = temp.X;
                gameObject.Y = temp.Y;
            }
        }

        /// <summary>
        /// Corrects attack hitboxes as they collide with walls and deletes colliding projectiles.
        /// </summary>
        /// <param name="attack">Attack to correct hitbox of</param>
        /// <returns>Bool indicating whether attack exists after collision</returns>
        public bool AttackWallCollisions(Attack attack)
        {
            foreach (Rectangle b in barriers)
            {
                if (b.Intersects(attack.Hitbox))
                {
                    if (attack is Projectile)
                    {
                        return false;
                    }

                    // Non-projectiles have hitbox cut based on collision and direction
                    switch (attack.AttackDirection)
                    {
                        case AttackDirection.up:

                            if (b.Right < attack.X + attack.Width / 3)
                            {  // If barrier is to the left, adjust left side of hitbox
                                attack.Width = (attack.X + attack.Width) - (b.X + b.Width);
                                attack.X = b.X + b.Width;
                            }
                            else if (b.Left > attack.X + attack.Width * 2 / 3)
                            {  // If barrier is to the right, adjust right side of hitbox
                                attack.Width = b.X - attack.X;
                            }
                            else
                            {  // Barrier is more centered; adjust hitbox vertically
                                attack.Height = attack.Y + attack.Height - (b.Y + b.Height);
                                attack.Y = b.Y + b.Height;
                            }
                            break;

                        case AttackDirection.left:

                            if (b.Bottom < attack.Y + attack.Height / 3)
                            {  // If barrier is to the top, adjust top side of hitbox
                                attack.Height = (attack.Y + attack.Height) - (b.Y + b.Height);
                                attack.Y = b.Y + b.Height;
                            }
                            else if (b.Top > attack.Y + attack.Height * 2 / 3)
                            {  // If barrier is to the bottom, adjust bottom side of hitbox
                                attack.Height = b.Y - attack.Y;
                            }
                            else
                            {  // Barrier is more centered; adjust hitbox horizontally
                                attack.Width = attack.X + attack.Width - (b.X + b.Width);
                                attack.X = b.X + b.Width;
                            }
                            break;

                        case AttackDirection.down:

                            if (b.Right < attack.X + attack.Width / 3)
                            {  // If barrier is to the left, adjust left side of hitbox
                                attack.Width = (attack.X + attack.Width) - (b.X + b.Width);
                                attack.X = b.X + b.Width;
                            }
                            else if (b.Left > attack.X + attack.Width * 2 / 3)
                            {  // If barrier is to the right, adjust right side of hitbox
                                attack.Width = b.X - attack.X;
                            }
                            else
                            {  // Barrier is more centered; adjust hitbox vertically
                                attack.Height = b.Y - attack.Y;
                            }
                            break;

                        case AttackDirection.right:

                            if (b.Bottom < attack.Y + attack.Height / 3)
                            {  // If barrier is to the top, adjust top side of hitbox
                                attack.Height = (attack.Y + attack.Height) - (b.Y + b.Height);
                                attack.Y = b.Y + b.Height;
                            }
                            else if (b.Top > attack.Y + attack.Height * 2 / 3)
                            {  // If barrier is to the bottom, adjust bottom side of hitbox
                                attack.Height = b.Y - attack.Y;
                            }
                            else
                            {  // Barrier is more centered; adjust hitbox horizontally
                                attack.Width = b.X - attack.X;
                            }
                            break;

                        case AttackDirection.allAround:

                            // Compare and see whether the closest horizonal edge is closer to the attack's center
                            // than the closest vertical edge
                            if (Math.Min(Math.Abs(attack.Hitbox.Center.X - b.Left), Math.Abs(attack.Hitbox.Center.X - b.Right)) 
                                > Math.Min(Math.Abs(attack.Hitbox.Center.Y - b.Top), Math.Abs(attack.Hitbox.Center.Y - b.Bottom)))
                            {
                                // Adjust horizontally
                                if (b.Left > attack.Hitbox.Center.X)
                                { // If barrier is to the right, adjust right side of hitbox
                                    attack.Width = b.X - attack.X;
                                }
                                else
                                { // If barrier is to the left, adjust left side of hitbox
                                    attack.Width = (attack.X + attack.Width) - (b.X + b.Width);
                                    attack.X = b.X + b.Width;
                                }
                            }
                            else // Adjust vertically
                            {
                                if (b.Bottom < attack.Hitbox.Center.Y)
                                { // If barrier is to the top, adjust top side of hitbox
                                    attack.Height = (attack.Y + attack.Height) - (b.Y + b.Height);
                                    attack.Y = b.Y + b.Height;
                                }
                                else
                                {  // If barrier is to the bottom, adjust bottom side of hitbox
                                    attack.Height = b.Y - attack.Y;
                                }
                            }
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Noah Flanders
        /// 
        /// The knockback of an incoming attack is adjusted if there are walls
        /// in between the target's current position and the knocked back position
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Attack AdjustAttackKnockback(Attack a)
        {
            Rectangle noKnockArea = new Rectangle(a.Hitbox.X + a.Knockback, a.Hitbox.Y + a.Knockback,
                    (2*a.Knockback) + a.Hitbox.Width, (2*a.Knockback) + a.Hitbox.Height);
            for (int i = 0; i < barriers.Count; i++)
            {
                //When the player is within knockback range of a barrier
                if (noKnockArea.Intersects(barriers[i]) || noKnockArea.Contains(barriers[i]))
                {
                    int knockback;
                    if (a.AttackDirection == AttackDirection.left && barriers[i].X < player.X)
                    {
                        knockback = player.PositionRect.X - (barriers[i].X + barriers[i].Width);
                    }
                    else if (a.AttackDirection == AttackDirection.up && barriers[i].Y < player.Y)
                    {
                        knockback = player.PositionRect.Y - (barriers[i].Y + barriers[i].Height);
                    }
                    else if (a.AttackDirection == AttackDirection.right && barriers[i].X > player.X)
                    {
                        knockback = barriers[i].X - (player.PositionRect.X + player.PositionRect.Width);
                    }
                    else if (a.AttackDirection == AttackDirection.down && barriers[i].Y > player.Y)
                    {
                        knockback = barriers[i].Y - (player.PositionRect.Y + player.PositionRect.Height);
                    }
                    else
                    {
                        knockback = a.Knockback;
                    }
                    return new Attack(a.Hitbox, a.Damage, knockback, a.Texture, a.AttackDirection,
                        a.Timer, a.IsPlayerAttack, a.Effect);
                }
            }
            return a;
        }
    }
}
