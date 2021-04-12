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

        // Constructor
        public CollisionManager(Player player)
        {
            this.player = player;
        }

        // Methods

        /// <summary>
        /// Load enemy list into the collision manager.
        /// </summary>
        /// <param name="enemies">Reference to working enemy list</param>
        public void GetEnemyList(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// Load item list into the collision manager.
        /// </summary>
        /// <param name="items"></param>
        public void GetItemList(List<Item> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Loads current room's barriers into the collision manager.
        /// </summary>
        /// <param name="barriers">List of Rectangles for barrier hitboxes</param>
        public void GetBarrierList(List<Rectangle> barriers)
        {
            this.barriers = barriers;
        }

        /// <summary>
        /// Detects and fixes collisions between active entities and room barriers.
        /// </summary>
        public void CheckCollisions()
        {
            // Player-wall collisions
            FixWallCollisions(player);

            // Put lists in same loop for efficiency
            int loopMax = Math.Max(enemies.Count, items.Count);
            for (int i = 0; i < loopMax; i++)
            {
                if (i < enemies.Count)
                {
                    // Enemy-wall collisions
                    FixWallCollisions(enemies[i]);
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
                            temp.X -= Rectangle.Intersect(temp, barriers[i]).Width;
                        }
                        else
                        {
                            temp.X += Rectangle.Intersect(temp, barriers[i]).Width;
                        }
                    }
                    else
                    {
                        if (barriers[i].Y > temp.Y)
                        {
                            temp.Y -= Rectangle.Intersect(temp, barriers[i]).Height;
                        }
                        else
                        {
                            temp.Y += Rectangle.Intersect(temp, barriers[i]).Height;
                        }
                    }
                }

                gameObject.X = temp.X;
                gameObject.Y = temp.Y;
            }
        }
    }
}
