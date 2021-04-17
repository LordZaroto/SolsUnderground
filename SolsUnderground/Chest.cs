using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This class defines a Chest object that may appear
/// randomly in dungeon rooms after all enemies have been
/// defeated, and contains an item for the player to use
/// or equip.
/// 
/// </summary>

namespace SolsUnderground
{
    class Chest : StaticObject
    {
        // Fields
        // Inherits Texture2D texture and Rectangle positionRect
        private List<Texture2D> chestTextures;
        private bool isOpen;

        // Properties
        public override int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }
        public override int Y
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }
        public override int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }
        public override int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
        }
        public Texture2D Texture
        {
            get
            {
                if (isOpen)
                    return chestTextures[1];
                else
                    return chestTextures[0];
            }
        }
        public override Rectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }
        public bool IsOpen
        {
            get { return isOpen; }
        }

        // Constructor
        public Chest(List<Texture2D> chestTextures, Rectangle positionRect)
        {
            this.chestTextures = chestTextures;
            this.positionRect = positionRect;
            this.isOpen = false;
        }

        // Methods

        /// <summary>
        /// Generates a new item for the player to pick up.
        /// </summary>
        /// <param name="itemTextures">List of item textures</param>
        /// <param name="mSpriteCount">Number of money textures</param>
        /// <param name="hSpriteCount">Number of health pickup textures</param>
        /// <param name="wSpriteCount">Number of weapon textures</param>
        /// <param name="aSpriteCount">Number of armor textures</param>
        /// <returns>Newly generated Item</returns>
        public Item Open(List<Texture2D> itemTextures, int mSpriteCount, int hSpriteCount, int wSpriteCount, int aSpriteCount)
        {
            int randomPick = Program.rng.Next(3);
            int textureID = 0;
            Item drop = null;

            switch (randomPick)
            {
                case 0: // Weapon
                    randomPick = Program.rng.Next(wSpriteCount);
                    textureID = mSpriteCount + hSpriteCount + randomPick;

                    switch (randomPick) // Add weapon items to drop here
                    {
                        case 0:
                            drop = new wStick(itemTextures[textureID], positionRect);
                            break;

                        case 1:
                            drop = new wRITchieClaw(itemTextures[textureID], positionRect);
                            break;

                        case 2:
                            drop = new wBrickBreaker(itemTextures[textureID], positionRect);
                            break;

                        case 3:
                            drop = new wHockeyStick(itemTextures[textureID], positionRect);
                            break;

                        case 4:
                            drop = new wHotDog(itemTextures[textureID], positionRect);
                            break;
                    }
                    break;

                case 1: // Armor
                    randomPick = Program.rng.Next(aSpriteCount);
                    textureID = mSpriteCount + hSpriteCount + wSpriteCount + randomPick;

                    switch (randomPick) // Add armor items to drop here
                    {
                        case 0:
                            drop = new aHoodie(itemTextures[textureID], positionRect);
                            break;

                        case 1:
                            drop = new aWinterCoat(itemTextures[textureID], positionRect);
                            break;

                        case 2:
                            drop = new aBandana(itemTextures[textureID], positionRect);
                            break;

                        case 3:
                            drop = new aSkates(itemTextures[textureID], positionRect);
                            break;

                        case 4:
                            drop = new aMask(itemTextures[textureID], positionRect);
                            break;
                    }
                    break;

                case 2: // Health pickup
                    drop = new Item(ItemType.HealthPickup,
                        100, itemTextures[mSpriteCount], PositionRect);
                    break;
            }

            isOpen = true;
            return drop;
        }

        /// <summary>
        /// Draws chest using texture property to determine if it is open or closed.
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Texture, positionRect, Color.White);
        }
    }
}
