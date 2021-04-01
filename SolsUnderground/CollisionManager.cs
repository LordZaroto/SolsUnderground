using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Preston Gilmore
/// Braden Flanders
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
        public void GetEnemies(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// Loads current room's barriers into the collision manager.
        /// </summary>
        /// <param name="barriers">List of Rectangles for barrier hitboxes</param>
        public void GetBarriers(List<Rectangle> barriers)
        {
            this.barriers = barriers;
        }

        /// <summary>
        /// Detects and fixes collisions between active entities and room barriers.
        /// </summary>
        public void CheckCollisions()
        {
            PlayerWallCollisions();
            EnemyWallCollisions();
        }

        /// <summary>
        /// Detects any collisions between the player and room barriers and adjusts
        /// the player's location accordingly.
        /// </summary>
        public void PlayerWallCollisions()
        {
            Rectangle temp = player.PositionRect;

            for (int i = 0; i < barriers.Count; i++)
            {
                //checks if the player intersects with a barrier
                if (barriers[i].Intersects(temp))
                {
                    //checks if the x or y needs to be adjusted
                    if (Rectangle.Intersect(temp, barriers[i]).Width <= Rectangle.Intersect(temp, barriers[i]).Height)
                    {
                        //adjusts the position
                        if (barriers[i].X > player.X)
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

                player.X = temp.X;
                player.Y = temp.Y;
            }
        }

        /// <summary>
        /// Detects any collisions between enemies in the room and the room barriers,
        /// adjusting each enemy's location as necessary.
        /// </summary>
        public void EnemyWallCollisions()
        {
            Rectangle temp;

            //loops through all enemies in the room
            for (int j = 0; j < enemies.Count; j++)
            {
                temp = enemies[j].PositionRect;

                for (int i = 0; i < barriers.Count; i++)
                {
                    //checks if the enemies intersect with a barrier
                    if (temp.Intersects(barriers[i]))
                    {
                        //checks if the x or y needs to be adjusted
                        if (Rectangle.Intersect(temp, barriers[i]).Width <= Rectangle.Intersect(temp, barriers[i]).Height)
                        {
                            //adjusts the position
                            if (barriers[i].X >= player.X)
                            {
                                temp.X += Rectangle.Intersect(temp, barriers[i]).Width;

                            }
                            else
                            {
                                temp.X -= Rectangle.Intersect(temp, barriers[i]).Width;
                            }
                        }
                        else
                        {
                            if (barriers[i].Y >= temp.Y)
                            {
                                temp.Y -= Rectangle.Intersect(temp, barriers[i]).Height;

                            }
                            else
                            {
                                temp.Y += Rectangle.Intersect(temp, barriers[i]).Height;
                            }
                        }
                    }

                    enemies[j].X = temp.X;
                    enemies[j].Y = temp.Y;
                }
            }
        }
    }
}
