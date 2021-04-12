using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//Author: Noah Flanders
//Date: 3/29/21
//Notes:
/*
 * The user clicks on the buttons with the desired tile and then clicks on the map
 * to place the tile within the map.
 * 
 * The save event saves each tile as a line of text to be loaded in by the MapManager in Monogame
 * 
 * The load event mimics the loading in within the MapManager class to test functionality
 */
namespace MapEditor
{
    public enum Tiles
    {
        DefaultTile,
        Barrier,
        RedBrick
    }
    public partial class Form1 : Form
    {
        //Fields
        private PictureBox[,] mapLayout;
        private string filename;
        private int mapWidth;
        private int mapHeight;
        private int tileWidth;
        private int tileHeight;

        public Form1()
        {
            InitializeComponent();
            mapLayout = new PictureBox[33, 25];
            mapWidth = 33;
            mapHeight = 25;
            tileWidth = 25;
            tileHeight = 25;
            CreateMapLayout();
        }

        /// <summary>
        /// Changes the selected tile to draw when a tile selection button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTile_Click(object sender, EventArgs e)
        {
            Button tilePicker = (Button)sender;
            pictureBoxCurrentTile.Image = tilePicker.Image;
        }

        /// <summary>
        /// Changes the tile on the map to the current selected tile sprite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxTile_Click(object sender, EventArgs e)
        {
            ((PictureBox)sender).Capture = false;

            if (MouseButtons.Equals(MouseButtons.Left))
            {
                PictureBox tile = (PictureBox)sender;
                tile.BackgroundImage = pictureBoxCurrentTile.Image;
            }
        }

        /// <summary>
        /// Writes each tile within the map as a line of text in a text file including 
        /// information about the tile like its image and if an enemy should spawn there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSaver = new SaveFileDialog();
            //Customizing the SaveFileDialog
            fileSaver.Title = "Save a level file";
            fileSaver.Filter = "Level Files|*.level";

            DialogResult result = fileSaver.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter($"{fileSaver.FileName}");

                writer.WriteLine($"{textBoxEnemyCount.Text}");//First written value is

                //Writing each tile as a line of text that includes the image and 
                //a boolean representing if there is an enemy in that location or not
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        if (mapLayout[i, j].BackgroundImage == buttonTile2.Image)
                        {
                            writer.WriteLine("RedBrick|false");
                        }
                        else if(mapLayout[i, j].BackgroundImage == buttonTile4.Image)
                        {
                            writer.WriteLine("Barrier|true");
                        }
                        else
                        {
                            writer.WriteLine("DefaultTile|false");
                        }
                    }
                }
                writer.Close();
            }
            this.Text = "Level Editor";
            //Confirmation message
            MessageBox.Show("File saved successfully", "File saved", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Prompts the user to find a file to load into the MapEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileFinder = new OpenFileDialog();
            fileFinder.Title = "Open a level file";
            fileFinder.Filter = "Level Files|*.level";
            DialogResult result = fileFinder.ShowDialog();
            if (result == DialogResult.OK)
            {
                MapLoadIn(fileFinder.FileName);
                MessageBox.Show("File loaded successfully", "File loaded", MessageBoxButtons.OK);
            }
            
        }

        /// <summary>
        /// Iterates through each line of the loaded file to read in the "Tile ID"
        /// in order to draw the corresponding tiles
        /// </summary>
        /// <param name="fileName"></param>
        private void MapLoadIn(string fileName)
        {
            StreamReader reader = new StreamReader($"{fileName}");
            int enemyCount = int.Parse(reader.ReadLine());
            textBoxEnemyCount.Text = $"{enemyCount}";
            
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    mapLayout[i, j] = new PictureBox();
                    string[] tileInfo = reader.ReadLine().Split('|');
                    if(tileInfo[0] == "DefaultTile")
                    {
                        mapLayout[i, j].BackgroundImage = buttonTile1.Image;
                    }
                    else if(tileInfo[0] == "Barrier")
                    {
                        mapLayout[i, j].BackgroundImage = buttonTile4.Image;
                    }
                    else if(tileInfo[0] == "RedBrick")
                    {
                        mapLayout[i, j].BackgroundImage = buttonTile2.Image;
                    }
                    mapLayout[i, j].Width = tileWidth;
                    mapLayout[i, j].Height = tileHeight;
                    mapLayout[i, j].Left = groupBoxMap.Location.X + (tileWidth * i);
                    mapLayout[i, j].Top = groupBoxMap.Location.Y + (tileHeight * j);
                    mapLayout[i, j].MouseDown += pictureBoxTile_Click;
                    mapLayout[i, j].MouseEnter += pictureBoxTile_Click;
                    this.Controls.Add(mapLayout[i, j]);
                    mapLayout[i, j].BringToFront();
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Helper method to create the grid of picture boxes that acts as the map.
        /// It also hooks up the pictureBoxTile_Click event to each picture box.
        /// </summary>
        private void CreateMapLayout()
        {
            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    mapLayout[i, j] = new PictureBox();
                    mapLayout[i, j].Width = tileWidth;
                    mapLayout[i, j].Height = tileHeight;
                    mapLayout[i, j].Left = groupBoxMap.Location.X + (tileWidth * i);
                    mapLayout[i, j].Top = groupBoxMap.Location.Y + (tileHeight * j);
                    mapLayout[i, j].MouseDown += pictureBoxTile_Click;
                    mapLayout[i, j].MouseEnter += pictureBoxTile_Click;
                    this.Controls.Add(mapLayout[i, j]);
                }
            }
            groupBoxMap.SendToBack();
        }

       
    }
}
