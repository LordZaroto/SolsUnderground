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
            combatManager.SetEnemyList(enemies);
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
        /// Removes all sprites for all regular enemies from manager.
        /// </summary>
        public void ClearEnemyData()
        {
            enemyTextures.Clear();
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
        /// <param name="openTiles">List of tile positions that enemies can spawn on</param>
        /// <param name="floorFactor">Difficulty multiplier</param>
        public void SpawnEnemies(int enemyCount, List<Rectangle> openTiles, float floorFactor)
        {
            // Use to store random index of openTile list
            int spawnPoint;
            int enemyChoice;

            for (int i = 0; i < enemyCount; i++)
            {
                spawnPoint = Program.rng.Next(openTiles.Count);

                // Prevent enemies from spawning too close to player
                Rectangle noSpawnZone = new Rectangle(
                    0, 360, 160, 280);

                while (noSpawnZone.Contains(openTiles[spawnPoint].Center))
                {
                    spawnPoint = Program.rng.Next(openTiles.Count);
                }

                enemyChoice = Program.rng.Next(enemyTextures.Count);
                Rectangle spawnRect = new Rectangle(openTiles[spawnPoint].X,
                            openTiles[spawnPoint].Y,
                            enemyTextures[enemyChoice][2].Width,
                            enemyTextures[enemyChoice][2].Height);

                // Need to expand and implement spawning multiple enemy types
                //The type of enemy spawned should be random
                switch (enemyChoice)
                {
                    // Tier 0 - Starting enemies
                    case 0:
                        enemies.Add(new Minion(enemyTextures[enemyChoice], spawnRect, 
                            (int)(6 * floorFactor), (int)(4 * floorFactor)));
                        break;
                    case 1:
                        enemies.Add(new Wanderer(enemyTextures[enemyChoice], spawnRect, 
                            (int)(12 * floorFactor), (int)(8 * floorFactor)));
                        break;

                    // Tier 1 - After 2nd floor
                    case 2:
                        enemies.Add(new Shooter(enemyTextures[enemyChoice], spawnRect,
                            (int)(25 * floorFactor), (int)(3 * floorFactor)));
                        break;

                    // Tier 2 - After 4th floor
                    case 3:
                        enemies.Add(new Weeb(enemyTextures[enemyChoice], spawnRect,
                            20, 7, enemyTextures[enemyChoice][4]));
                        break;

                    // Tier 3 - After 6th floor
                    case 4:
                        enemies.Add(new SolsWorker(enemyTextures[enemyChoice], spawnRect,
                            (int)(20 * floorFactor), (int)(4 * floorFactor)));
                        break;
                }
            }
        }

        /// <summary>
        /// Spawns a single boss in the center of the current room.
        /// </summary>
        public void SpawnBoss(int floor)
        {
            // Currently spawns all bosses in center of room
            // When all bosses are added, maybe add specific spawn points for each boss?

            Point spawnPoint = new Point(640, 480);

            // Need to expand and implement spawning multiple enemy types
            //The type of enemy spawned should be random
            switch (floor)
            {
                case 0: // Bus
                    enemies.Add(new BusBoss(bossTextures[floor],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[floor][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[floor][2].Height) / 2,
                        bossTextures[floor][2].Width,
                        bossTextures[floor][2].Height),
                        40, 4));
                    break;

                case 1: // Weeb
                    enemies.Add(new Weeb(bossTextures[floor],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[floor][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[floor][2].Height) / 2,
                        bossTextures[floor][2].Width,
                        bossTextures[floor][2].Height),
                        75, 8,
                        bossTextures[floor][4]));
                    break;

                case 2: // Janitor
                    enemies.Add(new JanitorBoss(bossTextures[floor],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[floor][0].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[floor][0].Height) / 2,
                        bossTextures[floor][0].Width,
                        bossTextures[floor][0].Height),
                        150, 8,
                        bossTextures[floor][2]));
                    break;

                case 3: // VM
                    enemies.Add(new VendingMachineBoss(bossTextures[floor],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[floor][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[floor][2].Height) / 2,
                        bossTextures[floor][2].Width,
                        bossTextures[floor][2].Height),
                        150, 8,
                        bossTextures[floor][4]));
                    break;

                case 4: // Stalker
                    break;

                case 5: // BR
                    enemies.Add(new BalloonRitchieBoss(bossTextures[floor],
                        new Rectangle(spawnPoint.X + (40 - bossTextures[floor][2].Width) / 2,
                        spawnPoint.Y + (40 - bossTextures[floor][2].Height) / 2,
                        bossTextures[floor][2].Width,
                        bossTextures[floor][2].Height),
                        40, 4));
                    break;
                case 6:
                    enemies.Add(new MunsonBoss(bossTextures[floor],
                        new Rectangle(475,
                        20,
                        200,
                        500),
                        300, 9));
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
                }else if(enemies[i] is JanitorBoss)
                {
                    JanitorBoss j = (JanitorBoss)enemies[i];
                    j.UpdateTimer(gameTime);
                }
                enemies[i].EnemyMove(player, gameTime);
            }
        }

        /// <summary>
        /// Updates enemy status effects.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateEnemyEffects(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
                e.UpdateEffects(gameTime);
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
