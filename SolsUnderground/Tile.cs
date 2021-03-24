using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale - 3/23/21
/// This class defines a Tile that stores a texture
/// and a field indicating if it can be walked on.
///
/// NOTES:
/// > Tile has no Draw method, plan to use Room to draw Tiles using their properties
///   This is because width and height of Tile is dependent on Room size
///
/// > Currently cannot change isObstacle value, depending on how game goes this might change
///   i.e. Will tiles be able to change walk-ability during game?
///   
/// > Color is not stored because the Color used by Monogame is different than the Color in
///   Windows Forms, plus determining the right tints may be too tedious.
///   
/// </summary>

namespace SolsUnderground
{
    // ID list for tile textures
    // IDs used to assign textures to tiles while loading files
    public enum Tiles
    {
        DefaultTile,
        Barrier
    }

    class Tile
    {
        // Fields
        private Texture2D texture;
        private bool isObstacle;

        // Properties
        public Texture2D Texture
        {
            get { return texture; }
        }
        public bool IsObstacle
        {
            get { return isObstacle; }
        }

        // Constructor
        public Tile(Texture2D texture, bool isObstacle)
        {
            this.texture = texture;
            this.isObstacle = isObstacle;
        }
    }
}
