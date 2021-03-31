using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// This class is in charge of initializing enemies for each room
/// as well as their movement and attack patterns.
/// </summary>

namespace SolsUnderground
{
    // 
    public enum Minions
    {
        SkaterBro
    }

    class EnemyManager
    {
        // Fields
        private List<Enemy> currentEnemies;
        private List<Texture2D[]> enemyTextures;

        // Properties


        // Constructor
        public EnemyManager()
        {

        }

        // Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enemySprites"></param>
        public void AddEnemyData(Texture2D[] enemySprites)
        {
            enemyTextures.Add(enemySprites);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SpawnEnemies(int enemyCount)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearEnemies()
        {

        }
    }
}
