using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/// <summary>
/// Alex Dale
/// 
/// This class is responsible for handling chest spawns, collectible gold
/// from enemies, and any items that appear on-screen during gameplay.
/// 
/// > Need to do:
/// > Chest spawning
/// 
/// </summary>

namespace SolsUnderground
{
    class ItemManager
    {
        // Fields
        private Player player;
        private List<Item> items;
        private List<Chest> chests;
        private List<Texture2D> itemTextures;
        private List<Texture2D> chestTextures;

        // Constructor
        public ItemManager(Player player, CollisionManager collisionManager, 
            List<Texture2D> itemTextures, List<Texture2D> chestTextures)
        {
            this.player = player;
            this.itemTextures = itemTextures;
            this.chestTextures = chestTextures;

            items = new List<Item>();
            chests = new List<Chest>();
            collisionManager.SetItemList(items, chests);
        }

        // Methods

        /// <summary>
        /// Drops money for the player to pick up, and possibly a health pickup as well.
        /// </summary>
        /// <param name="moneyValue">Value of the money object</param>
        /// <param name="enemyRect">Rectangle</param>
        public void EnemyDrops(int moneyValue, Point spawn)
        {
            // Spawn money item in small randomized area
            items.Add(new Item(ItemType.Money, moneyValue, itemTextures[0],
                new Rectangle(
                    spawn.X + (Program.rng.Next(5) - 2), 
                    spawn.Y + (Program.rng.Next(5) - 2), 
                    itemTextures[0].Width, 
                    itemTextures[0].Height)));

            if (Program.rng.Next(100) < 30)
            {
                // 30% chance to spawn health pickup in small randomized area
                items.Add(new Item(ItemType.HealthPickup, 20, itemTextures[1],
                     new Rectangle(
                         spawn.X + (Program.rng.Next(5) - 2), 
                         spawn.Y + (Program.rng.Next(5) - 2), 
                         itemTextures[1].Width, 
                         itemTextures[1].Height)));
            }
        }

        /// <summary>
        /// Activates the effects of picking up an item if its colliding with the player.
        /// </summary>
        public void ActivateItems()
        {
            for (int i = 0; i < items.Count;)
            {
                if (player.PositionRect.Intersects(items[i].PositionRect))
                {
                    switch (items[i].Type)
                    {
                        case ItemType.Money:
                            player.TigerBucks += items[i].Value;
                            items.RemoveAt(i);
                            continue;

                        case ItemType.HealthPickup:
                            player.Hp += items[i].Value;
                            //
                            // Clamp player's Hp to maxHp
                            if (player.Hp > player.MaxHp)
                                player.Hp = player.MaxHp;
                            items.RemoveAt(i);
                            continue;

                        case ItemType.Weapon:
                            player.EquipWeapon((Weapon)items[i]);
                            items.RemoveAt(i);
                            continue;

                        case ItemType.Armor:
                            player.EquipArmor((Armor)items[i]);
                            items.RemoveAt(i);
                            continue;
                    }
                }

                i++;
            }
        }

        /// <summary>
        /// Opens all chests and registers their dropped items.
        /// </summary>
        public void OpenChests()
        {
            foreach (Chest c in chests)
            {
                if (c.IsOpen)
                {
                    items.Add(c.Open(itemTextures));
                }
            }
        }

        /// <summary>
        /// Removes all chests and items from previous rooms and spawns chests for the next room.
        /// </summary>
        public void NextRoom(List<Point> chestSpawns)
        {
            items.Clear();
            chests.Clear();

            // Spawn chests for next room
            foreach(Point p in chestSpawns)
            {
                chests.Add(new Chest(chestTextures,
                    new Rectangle(p,
                    new Point(chestTextures[0].Width,
                    chestTextures[0].Height))));
            }
        }

        /// <summary>
        /// Draws all chests and items in the current room.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Chest c in chests)
            {
                c.Draw(sb);
            }

            foreach (Item i in items)
            {
                i.Draw(sb);
            }
        }
    }
}
