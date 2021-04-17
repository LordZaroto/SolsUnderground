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
        private int moneySpriteCount;
        private int healthSpriteCount;
        private int weaponSpriteCount;
        private int armorSpriteCount;
        private List<Texture2D> chestTextures;

        // Constructor
        public ItemManager(Player player, CollisionManager collisionManager, List<Texture2D> chestTextures)
        {
            this.player = player;
            this.itemTextures = new List<Texture2D>();
            moneySpriteCount = 0;
            healthSpriteCount = 0;
            weaponSpriteCount = 0;
            armorSpriteCount = 0;
            this.chestTextures = chestTextures;

            items = new List<Item>();
            chests = new List<Chest>();
            collisionManager.SetItemList(items, chests);
        }

        // Methods

        /// <summary>
        /// Registers a new money texture.
        /// </summary>
        /// <param name="armorSprite">Texture to register</param>
        public void AddMoneySprite(Texture2D moneySprite)
        {
            itemTextures.Insert(moneySpriteCount, moneySprite);
            moneySpriteCount++;
        }

        /// <summary>
        /// Registers a new health pickup texture.
        /// </summary>
        /// <param name="healthSprite">Texture to register</param>
        public void AddHealthSprite(Texture2D healthTexture)
        {
            itemTextures.Insert(moneySpriteCount + healthSpriteCount, healthTexture);
            healthSpriteCount++;
        }

        /// <summary>
        /// Registers a new weapon texture.
        /// </summary>
        /// <param name="weaponSprite">Texture to register</param>
        public void AddWeaponSprite(Texture2D weaponSprite)
        {
            itemTextures.Insert(moneySpriteCount + healthSpriteCount + weaponSpriteCount, weaponSprite);
            weaponSpriteCount++;
        }

        /// <summary>
        /// Registers a new armor texture.
        /// </summary>
        /// <param name="armorSprite">Texture to register</param>
        public void AddArmorSprite(Texture2D armorSprite)
        {
            itemTextures.Add(armorSprite);
            armorSpriteCount++;
        }

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
                         itemTextures[moneySpriteCount].Width, 
                         itemTextures[moneySpriteCount].Height)));
            }
        }

        /// <summary>
        /// Activates the effects of picking up an item if its colliding with the player.
        /// </summary>
        public void ActivateItems(bool isEKeyPressed)
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
                            if (isEKeyPressed)
                            {
                                player.EquipWeapon((Weapon)items[i]);
                                items.RemoveAt(i);
                                continue;
                            }
                            break;

                        case ItemType.Armor:
                            if (isEKeyPressed)
                            {
                                player.EquipArmor((Armor)items[i]);
                                items.RemoveAt(i);
                                continue;
                            }
                            break;
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
                if (!c.IsOpen)
                {
                    items.Add(c.Open(itemTextures, 
                        moneySpriteCount, 
                        healthSpriteCount, 
                        weaponSpriteCount, 
                        armorSpriteCount));
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
                // Add percentage chance?
                chests.Add(new Chest(chestTextures,
                    new Rectangle(p,
                    new Point(40, 40))));
            }
        }

        /// <summary>
        /// Creates all weapons and armor in the starting room
        /// </summary>
        public void FullAccess()
        {
            if (Program.godMode)
            {
                int equipmentStart = moneySpriteCount + healthSpriteCount;
                Point size = new Point(40, 40);

                for (int i = 0; i < itemTextures.Count - equipmentStart; i++)
                {
                    switch (i)
                    {
                            // Weapons
                        case 0:
                            items.Add(new wStick(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;
                        case 1:
                            items.Add(new wRITchieClaw(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;
                        case 2:
                            items.Add(new wBrickBreaker(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;
                        case 3:
                            items.Add(new wHockeyStick(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;
                        case 4:
                            items.Add(new wHotDog(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;

                            // Armor
                        case 5:
                            items.Add(new aHoodie(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 6:
                            items.Add(new aWinterCoat(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 7:
                            items.Add(new aBandana(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 8:
                            items.Add(new aSkates(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 9:
                            items.Add(new aMask(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;

                    }
                }
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
