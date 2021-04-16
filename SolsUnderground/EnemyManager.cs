using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
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
            collisionManager.SetEnemyList(enemies);
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
            int enemyChoice;

            for (int i = 0; i < enemyCount; i++)
            {
                spawnPoint = Program.rng.Next(openTiles.Count);
                enemyChoice = Program.rng.Next(2);

                // Need to expand and implement spawning multiple enemy types
                Rectangle skaterRect = new Rectangle(
                    openTiles[spawnPoint].X,
                    openTiles[spawnPoint].Y, 
                    enemyTextures[0][2].Width, 
                    enemyTextures[0][2].Height);
                //second enemy 
                Rectangle fratRect = new Rectangle(
                    openTiles[spawnPoint].X,
                    openTiles[spawnPoint].Y,
                    enemyTextures[1][2].Width,
                    enemyTextures[1][2].Height);
                switch (enemyChoice)
                {
                    case 0:
                        enemies.Add(new Minion(enemyTextures[0], skaterRect, 6, 4));
                        break;
                    case 1:
                        enemies.Add(new Wanderer(enemyTextures[1], fratRect, 12, 8));
                        break;
                }
            }
        }

        /// <summary>
        /// Moves all active enemies towards player.
        /// </summary>
        public void MoveEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] is Wanderer)
                {
                    Wanderer newWander = (Wanderer)enemies[i];
                    newWander.UpdateTimer(gameTime);
                }
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
