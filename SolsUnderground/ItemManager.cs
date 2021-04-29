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
        public void EnemyDrops(Enemy enemy)
        {
            int moneyValue = enemy.Attack;

            if (enemy is Boss)
            {
                // 40% chance for boss-specific drop
                if (Program.rng.Next(100) < 40)
                {
                    if (enemy is Weeb)
                    {
                        // Drop ThePrecipice
                        items.Add(new wThePrecipice(itemTextures[moneySpriteCount + healthSpriteCount + 5],
                        new Rectangle(
                            enemy.PositionRect.Center.X - 20 + (Program.rng.Next(5) - 2),
                            enemy.PositionRect.Center.Y - 20 + (Program.rng.Next(5) - 2),
                            40, 40)));
                    }
                    if (enemy is VendingMachineBoss)
                    {
                        // Vending Machine drops here
                    }
                    if (enemy is BalloonRitchieBoss)
                    {
                        // Balloon Ritchie drops here
                    }
                }
                else
                {
                    // Spawn increased money item in small randomized area
                    items.Add(new Item(ItemType.Money, moneyValue * 5, itemTextures[0],
                        new Rectangle(
                            enemy.PositionRect.Center.X - itemTextures[0].Width / 2 + (Program.rng.Next(5) - 2),
                            enemy.PositionRect.Center.Y - itemTextures[0].Height / 2 + (Program.rng.Next(5) - 2),
                            itemTextures[0].Width,
                            itemTextures[0].Height)));
                }
            }
            else // Normal enemy drops
            {
                // Spawn money item in small randomized area
                items.Add(new Item(ItemType.Money, moneyValue, itemTextures[0],
                    new Rectangle(
                        enemy.PositionRect.Center.X - itemTextures[0].Width / 2 + (Program.rng.Next(5) - 2),
                        enemy.PositionRect.Center.Y - itemTextures[0].Height / 2 + (Program.rng.Next(5) - 2),
                        itemTextures[0].Width,
                        itemTextures[0].Height)));

                // 30% chance to spawn health pickup in small randomized area
                if (Program.rng.Next(100) < 30)
                {
                    items.Add(new Item(ItemType.HealthPickup, 20, itemTextures[1],
                         new Rectangle(
                             enemy.PositionRect.Center.X - itemTextures[moneySpriteCount].Width / 2 + (Program.rng.Next(5) - 2),
                             enemy.PositionRect.Center.Y - itemTextures[moneySpriteCount].Height / 2 + (Program.rng.Next(5) - 2),
                             itemTextures[moneySpriteCount].Width,
                             itemTextures[moneySpriteCount].Height)));
                }
            }
        }

        /// <summary>
        /// Activates the effects of picking up an item if its colliding with the player.
        /// </summary>
        public void ActivateItems(bool isEKeyPressed)
        {
            List<Item> unequipped = new List<Item>();

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
                                unequipped.Add(player.Equip(items[i]));
                                items.RemoveAt(i);
                                continue;
                            }
                            break;

                        case ItemType.Armor:
                            if (isEKeyPressed)
                            {
                                unequipped.Add(player.Equip(items[i]));
                                items.RemoveAt(i);
                                continue;
                            }
                            break;
                    }
                }

                i++;
            }

            items.AddRange(unequipped);
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
                        case 5:
                            items.Add(new wThePrecipice(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * i, 100), size)));
                            break;

                        // Armor
                        case 6:
                            items.Add(new aHoodie(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 7:
                            items.Add(new aWinterCoat(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 8:
                            items.Add(new aBandana(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 9:
                            items.Add(new aSkates(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;
                        case 10:
                            items.Add(new aMask(itemTextures[i + equipmentStart],
                                new Rectangle(new Point(50 + 50 * (i - weaponSpriteCount), 800), size)));
                            break;

                    }
                }
            }
        }

        /// <summary>
        /// Draws all chests and items in the current room.
        /// Also draws information boxes for moused-over items.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Chest c in chests)
            {
                c.Draw(sb);
            }

            // Draw items over chests
            foreach (Item i in items)
            {
                i.Draw(sb);
            }
        }

        /// <summary>
        /// Draws info boxes for any equipment item that the mouse hovers over.
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="uiText">Info text spritefont</param>
        public void DrawInfoBoxes(SpriteBatch sb, MouseState mouse, SpriteFont uiText)
        {
            foreach (Item i in items)
            {
                // Draw info for any items mouse hovers over
                if (Game1.MouseOver(i.PositionRect, mouse))
                {
                    Rectangle infoRect;

                    switch (i.Type)
                    {
                        case ItemType.Weapon:
                            infoRect = new Rectangle(mouse.X, mouse.Y, 280, 140);

                            sb.Draw(Program.drawSquare, infoRect, Color.DarkRed);

                            sb.DrawString(uiText, ((Weapon)i).Name,
                                new Vector2(infoRect.X + 10, infoRect.Y + 5), Color.LightSalmon);

                            sb.DrawString(uiText, "Damage: " + ((Weapon)i).Attack,
                                new Vector2(infoRect.X + 10, infoRect.Y + 30), Color.White);

                            sb.DrawString(uiText, "Knockback: " + ((Weapon)i).Knockback,
                                new Vector2(infoRect.X + 10, infoRect.Y + 55), Color.White);

                            sb.DrawString(uiText, "Basic Cooldown: " + ((Weapon)i).BasicCooldown,
                                new Vector2(infoRect.X + 10, infoRect.Y + 80), Color.White);

                            sb.DrawString(uiText, "Special Cooldown: " + ((Weapon)i).SpecialCooldown,
                                new Vector2(infoRect.X + 10, infoRect.Y + 105), Color.White);
                            break;


                        case ItemType.Armor:
                            infoRect = new Rectangle(mouse.X, mouse.Y, 160, 115);

                            sb.Draw(Program.drawSquare, infoRect, Color.DarkSlateGray);

                            sb.DrawString(uiText, ((Armor)i).Name,
                                new Vector2(infoRect.X + 10, infoRect.Y + 3), Color.LightBlue);

                            sb.DrawString(uiText, "Defense: " + ((Armor)i).Defense,
                                new Vector2(infoRect.X + 10, infoRect.Y + 30), Color.White);

                            sb.DrawString(uiText, "Speed: " + ((Armor)i).Speed,
                                new Vector2(infoRect.X + 10, infoRect.Y + 55), Color.White);

                            sb.DrawString(uiText, "HP: " + ((Armor)i).HP,
                                new Vector2(infoRect.X + 10, infoRect.Y + 80), Color.White);
                            break;
                    }
                }
            }
        }
    }
}
