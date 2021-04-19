using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Preston Gilmore

namespace SolsUnderground
{
    /// <summary>
    /// Bosses will be accounted for seperate to standard enemies.
    /// They will have their own damage interactions with the player
    /// in CombatManager.
    /// </summary>
    abstract class Boss : Enemy
    {
        /// <summary>
        /// A boss will unleash different attacks depending on
        /// certain variables, such as distance from player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract Attack BossAttack(Player player);
        //Use helper methods for each attack that the boss
        //can perform for cleaner code. Make those helper methods
        //return an Attack object and return the appropriate 
        //attack within the BossAttack method. EX:
        // public Attack BossAttack(Player player)
        // {
        //      If( Boss is close to Player and MegaSlam is off cooldown)

        //          return MegaSlam(player);

        //      else if( Boss is far from the player and LaundryToss is off cooldown)

        //          return LaundryToss(player);
        // }
    }
}
