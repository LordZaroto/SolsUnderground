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
/// NOTES:
/// 
/// > Use constructor to load item list to collisionManager for checking item collisions
/// 
/// > Need to do:
/// > Chests
/// > Item spawning at appropriate times
/// > Equip weapon and armor to player
/// > Drop current weapon/armor?
/// > Finish Armor class
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

        // Constructor
        public ItemManager(Player player, CollisionManager collisionManager, List<Texture2D> itemTextures)
        {
            this.player = player;
            this.itemTextures = itemTextures;

            items = new List<Item>();
            chests = new List<Chest>();
            collisionManager.GetItemList(items);
        }

        // Methods

        // SpawnChests()
        // Need to figure out how to avoid collisions
        // Use marker system in editor to determine valid placement?
        // X% chance for chest to appear in each marked location

        /// <summary>
        /// Creates money items for the player to pickup.
        /// </summary>
        /// <param name="value">Value of the money object</param>
        /// <param name="enemyRect">Rectangle</param>
        public void DropMoney(int value, Rectangle spawnRect)
        {
            // Shirt position of money drop randomly
            spawnRect.X = spawnRect.X - 5 + Program.rng.Next(11);
            spawnRect.Y = spawnRect.Y - 5 + Program.rng.Next(11);

            Item moneyDrop = new Item(ItemType.Money, value, itemTextures[0], spawnRect);
            items.Add(moneyDrop);
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

                        case ItemType.HealthPotion:
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
        /// Removes all items from previous rooms and spawns chests for the next room.
        /// </summary>
        public void NextRoom()
        {
            items.Clear();

            // Spawn chests for next room
        }

        /// <summary>
        /// Draws all items in the current room.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Item i in items)
            {
                i.Draw(sb);
            }
        }
    }
}
