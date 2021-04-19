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
            barriers.Add(new Rectangle(windowWidth, 0, 50, windowHeight));
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
    }
}
