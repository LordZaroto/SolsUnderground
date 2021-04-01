using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Noah Flanders
/// 
/// This class is in charge of initializing enemies for each room
/// as well as their movement and attack patterns.
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
        private List<Enemy> enemies;
        private List<Texture2D[]> enemyTextures;
        private Player player;

        // Constructor
        public EnemyManager(Player player, CollisionManager collisionManager, CombatManager combatManager)
        {
            this.player = player;
            enemies = new List<Enemy>();
            enemyTextures = new List<Texture2D[]>();

            // Hand a reference of enemy list to collision and combat managers
            collisionManager.GetEnemies(enemies);
            combatManager.GetEnemies(enemies);
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
        /// Currently only spawns default SkaterBro enemy.
        /// </summary>
        /// <param name="enemyCount">Number of enemies to add</param>
        public void SpawnEnemies(int enemyCount, List<Rectangle> openTiles)
        {
            // Use to store random index of openTile list
            int spawnPoint;

            for (int i = 0; i < enemyCount; i++)
            {
                spawnPoint = Program.rng.Next(openTiles.Count);

                // Need to expand and implement spawning multiple enemy types
                Rectangle enemyRect = new Rectangle(
                    openTiles[spawnPoint].X,
                    openTiles[spawnPoint].Y, 
                    enemyTextures[0][2].Width, 
                    enemyTextures[0][2].Height);

                enemies.Add(new Minion(enemyTextures[0], enemyRect, 6, 4));
            }
        }

        /// <summary>
        /// Moves all active enemies towards player.
        /// </summary>
        public void MoveEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].EnemyMove(player);
            }
        }

        /// <summary>
        /// Removes all active enemies.
        /// </summary>
        public void ClearEnemies()
        {
            enemies.Clear();
        }

        /// <summary>
        /// Draws all active enemies.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(sb);
            }
        }
    }
}
