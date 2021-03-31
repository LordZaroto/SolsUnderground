using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// This class is in charge of initializing enemies for each room
/// as well as their movement and attack patterns.
/// 
/// > Currently not in use, fully implement in next milestone
/// 
/// > Need to expand on SpawnEnemies method, currently hardcoded for SkaterBros only
/// 
/// </summary>

namespace SolsUnderground
{
    // ID list for minion types
    public enum Minions
    {
        SkaterBro
    }

    class EnemyManager
    {
        // Fields
        private List<Enemy> currentEnemies;
        private List<Texture2D[]> enemyTextures;
        private int windowWidth;
        private int windowHeight;

        // Properties


        // Constructor
        public EnemyManager(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        // Methods

        /// <summary>
        /// Adds a set of sprites for an enemy type.
        /// </summary>
        /// <param name="enemySprites">Texture2D array of enemy's sprites</param>
        public void AddEnemyData(Texture2D[] enemySprites)
        {
            enemyTextures.Add(enemySprites);
        }

        /// <summary>
        /// Creates a number of enemies and adds them to the current active enemies.
        /// </summary>
        /// <param name="enemyCount">Number of enemies to add</param>
        public void SpawnEnemies(int enemyCount, List<Rectangle> barriers)
        {
            int enemyXRange = windowWidth - enemyTextures[0][0].Width;
            int enemyYRange = windowHeight - enemyTextures[0][0].Height;
            int enemyWidth = enemyTextures[0][0].Width;
            int enemyHeight = enemyTextures[0][0].Height;

            for (int i = 0; i < enemyCount; i++)
            {
                Rectangle enemyRect = new Rectangle(Program.rng.Next(enemyXRange),
                    Program.rng.Next(enemyYRange), enemyWidth, enemyHeight);
                List<Rectangle> barrierSpots = barriers;
                foreach (Rectangle b in barrierSpots)
                {
                    while (enemyRect.Intersects(b))
                    {
                        enemyRect = new Rectangle(Program.rng.Next(enemyXRange),
                        Program.rng.Next(enemyYRange), enemyWidth, enemyHeight);
                    }
                }

                // Adds default SkaterBro - should expand for different enemies
                currentEnemies.Add(new Minion(enemyTextures[0], enemyRect, 10, 2));

            }
        }

        /// <summary>
        /// UNFINISHED - Moves each enemy on the screen.
        /// </summary>
        public void MoveEnemies()
        {
            foreach(Enemy e in currentEnemies)
            {
                // Move each enemy
            }
        }

        /// <summary>
        /// Removes all active enemies.
        /// </summary>
        public void ClearEnemies()
        {
            currentEnemies.Clear();
        }
    }
}
