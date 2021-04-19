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
        private List<Texture2D[]> bossTextures;
        private Player player;


        // Constructor
        public EnemyManager(Player player, CollisionManager collisionManager, CombatManager combatManager)
        {
            this.player = player;
            enemies = new List<Enemy>();
            enemyTextures = new List<Texture2D[]>();
            bossTextures = new List<Texture2D[]>();

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
        /// Adds a set of sprites for a boss type.
        /// </summary>
        /// <param name="bossSprites">Texture2D array of boss' sprites</param>
        public void AddBossData(Texture2D[] bossSprites)
        {
            bossTextures.Add(bossSprites);
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
                enemyChoice = Program.rng.Next(enemyTextures.Count);

                // Need to expand and implement spawning multiple enemy types
                //The type of enemy spawned should be random
                switch (enemyChoice)
                {
                    case 0:
                        enemies.Add(new Minion(enemyTextures[enemyChoice],
                            new Rectangle(openTiles[spawnPoint].X, 
                            openTiles[spawnPoint].Y, 
                            enemyTextures[enemyChoice][2].Width, 
                            enemyTextures[enemyChoice][2].Height), 
                            6, 4));
                        break;
                    case 1:
                        enemies.Add(new Wanderer(enemyTextures[enemyChoice],
                            new Rectangle(openTiles[spawnPoint].X,
                            openTiles[spawnPoint].Y,
                            enemyTextures[enemyChoice][2].Width,
                            enemyTextures[enemyChoice][2].Height), 
                            12, 8));
                        break;
                    case 2:
                        enemies.Add(new Shooter(enemyTextures[enemyChoice],
                            new Rectangle(openTiles[spawnPoint].X,
                            openTiles[spawnPoint].Y,
                            enemyTextures[enemyChoice][2].Width,
                            enemyTextures[enemyChoice][2].Height),
                            12, 8));
                        break;
                }
            }
        }

        /// <summary>
        /// Spawns a single boss in the center of the current room.
        /// </summary>
        public void SpawnBoss()
        {
            Point spawnPoint = new Point(640, 480);
            int bossChoice = Program.rng.Next(bossTextures.Count);

            // Need to expand and implement spawning multiple enemy types
            //The type of enemy spawned should be random
            switch (bossChoice)
            {
                case 0:
                    enemies.Add(new Weeb(bossTextures[bossChoice],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[bossChoice][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[bossChoice][2].Height) / 2,
                        bossTextures[bossChoice][2].Width,
                        bossTextures[bossChoice][2].Height),
                        75, 8));
                    break;

                case 1:
                    enemies.Add(new VendingMachineBoss(bossTextures[bossChoice],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[bossChoice][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[bossChoice][2].Height) / 2,
                        bossTextures[bossChoice][2].Width,
                        bossTextures[bossChoice][2].Height),
                        75, 8));
                    break;
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
                }else if(enemies[i] is VendingMachineBoss)
                {
                    VendingMachineBoss vm = (VendingMachineBoss)enemies[i];
                    vm.UpdateTimer(gameTime);
                }
                enemies[i].EnemyMove(player, gameTime);
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
