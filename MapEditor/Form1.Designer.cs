namespace MapEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxMap = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxTileSelect = new System.Windows.Forms.GroupBox();
            this.buttonTile9 = new System.Windows.Forms.Button();
            this.buttonTile8 = new System.Windows.Forms.Button();
            this.buttonTile7 = new System.Windows.Forms.Button();
            this.buttonTile6 = new System.Windows.Forms.Button();
            this.buttonTile5 = new System.Windows.Forms.Button();
            this.buttonTile4 = new System.Windows.Forms.Button();
            this.buttonTile3 = new System.Windows.Forms.Button();
            this.buttonTile2 = new System.Windows.Forms.Button();
            this.buttonTile1 = new System.Windows.Forms.Button();
            this.pictureBoxCurrentTile = new System.Windows.Forms.PictureBox();
            this.textBoxEnemyCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxTileSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrentTile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMap
            // 
            this.groupBoxMap.Location = new System.Drawing.Point(390, 4);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Size = new System.Drawing.Size(825, 625);
            this.groupBoxMap.TabIndex = 0;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Map";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(30, 565);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(150, 72);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(213, 565);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(150, 72);
            this.buttonLoad.TabIndex = 3;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoadFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(165, 384);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Tile";
            // 
            // groupBoxTileSelect
            // 
            this.groupBoxTileSelect.Controls.Add(this.buttonTile9);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile8);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile7);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile6);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile5);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile4);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile3);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile2);
            this.groupBoxTileSelect.Controls.Add(this.buttonTile1);
            this.groupBoxTileSelect.Location = new System.Drawing.Point(12, 4);
            this.groupBoxTileSelect.Name = "groupBoxTileSelect";
            this.groupBoxTileSelect.Size = new System.Drawing.Size(351, 305);
            this.groupBoxTileSelect.TabIndex = 1;
            this.groupBoxTileSelect.TabStop = false;
            this.groupBoxTileSelect.Text = "Tile Select";
            // 
            // buttonTile9
            // 
            this.buttonTile9.Location = new System.Drawing.Point(252, 227);
            this.buttonTile9.Name = "buttonTile9";
            this.buttonTile9.Size = new System.Drawing.Size(50, 50);
            this.buttonTile9.TabIndex = 8;
            this.buttonTile9.UseVisualStyleBackColor = true;
            this.buttonTile9.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile8
            // 
            this.buttonTile8.Image = global::MapEditor.Properties.Resources.ExitSprite;
            this.buttonTile8.Location = new System.Drawing.Point(142, 227);
            this.buttonTile8.Name = "buttonTile8";
            this.buttonTile8.Size = new System.Drawing.Size(50, 50);
            this.buttonTile8.TabIndex = 7;
            this.buttonTile8.UseVisualStyleBackColor = true;
            this.buttonTile8.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile7
            // 
            this.buttonTile7.Image = global::MapEditor.Properties.Resources.EntranceSprite;
            this.buttonTile7.Location = new System.Drawing.Point(39, 227);
            this.buttonTile7.Name = "buttonTile7";
            this.buttonTile7.Size = new System.Drawing.Size(50, 50);
            this.buttonTile7.TabIndex = 6;
            this.buttonTile7.UseVisualStyleBackColor = true;
            this.buttonTile7.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile6
            // 
            this.buttonTile6.Location = new System.Drawing.Point(252, 134);
            this.buttonTile6.Name = "buttonTile6";
            this.buttonTile6.Size = new System.Drawing.Size(50, 50);
            this.buttonTile6.TabIndex = 5;
            this.buttonTile6.UseVisualStyleBackColor = true;
            this.buttonTile6.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile5
            // 
            this.buttonTile5.Image = global::MapEditor.Properties.Resources.EnemySpawnSprite;
            this.buttonTile5.Location = new System.Drawing.Point(142, 134);
            this.buttonTile5.Name = "buttonTile5";
            this.buttonTile5.Size = new System.Drawing.Size(50, 50);
            this.buttonTile5.TabIndex = 4;
            this.buttonTile5.UseVisualStyleBackColor = true;
            this.buttonTile5.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile4
            // 
            this.buttonTile4.Image = global::MapEditor.Properties.Resources.BarrierSprite;
            this.buttonTile4.Location = new System.Drawing.Point(39, 134);
            this.buttonTile4.Name = "buttonTile4";
            this.buttonTile4.Size = new System.Drawing.Size(50, 50);
            this.buttonTile4.TabIndex = 3;
            this.buttonTile4.UseVisualStyleBackColor = true;
            this.buttonTile4.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile3
            // 
            this.buttonTile3.Image = global::MapEditor.Properties.Resources.BlueBrickSprite;
            this.buttonTile3.Location = new System.Drawing.Point(252, 31);
            this.buttonTile3.Name = "buttonTile3";
            this.buttonTile3.Size = new System.Drawing.Size(50, 50);
            this.buttonTile3.TabIndex = 2;
            this.buttonTile3.UseVisualStyleBackColor = true;
            this.buttonTile3.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile2
            // 
            this.buttonTile2.Image = global::MapEditor.Properties.Resources.RedBrickSprite;
            this.buttonTile2.Location = new System.Drawing.Point(142, 31);
            this.buttonTile2.Name = "buttonTile2";
            this.buttonTile2.Size = new System.Drawing.Size(50, 50);
            this.buttonTile2.TabIndex = 1;
            this.buttonTile2.UseVisualStyleBackColor = true;
            this.buttonTile2.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // buttonTile1
            // 
            this.buttonTile1.Image = global::MapEditor.Properties.Resources.BrickSprite;
            this.buttonTile1.Location = new System.Drawing.Point(39, 31);
            this.buttonTile1.Name = "buttonTile1";
            this.buttonTile1.Size = new System.Drawing.Size(50, 50);
            this.buttonTile1.TabIndex = 0;
            this.buttonTile1.UseVisualStyleBackColor = true;
            this.buttonTile1.Click += new System.EventHandler(this.buttonTile_Click);
            // 
            // pictureBoxCurrentTile
            // 
            this.pictureBoxCurrentTile.Location = new System.Drawing.Point(77, 363);
            this.pictureBoxCurrentTile.Name = "pictureBoxCurrentTile";
            this.pictureBoxCurrentTile.Size = new System.Drawing.Size(64, 69);
            this.pictureBoxCurrentTile.TabIndex = 4;
            this.pictureBoxCurrentTile.TabStop = false;
            // 
            // textBoxEnemyCount
            // 
            this.textBoxEnemyCount.Location = new System.Drawing.Point(252, 476);
            this.textBoxEnemyCount.Name = "textBoxEnemyCount";
            this.textBoxEnemyCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxEnemyCount.TabIndex = 6;
            this.textBoxEnemyCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(12, 476);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number of Enemies Desired";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 649);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxEnemyCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxCurrentTile);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxTileSelect);
            this.Controls.Add(this.groupBoxMap);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBoxTileSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrentTile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMap;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.PictureBox pictureBoxCurrentTile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTile1;
        private System.Windows.Forms.Button buttonTile2;
        private System.Windows.Forms.Button buttonTile3;
        private System.Windows.Forms.Button buttonTile4;
        private System.Windows.Forms.Button buttonTile5;
        private System.Windows.Forms.GroupBox groupBoxTileSelect;
        private System.Windows.Forms.Button buttonTile9;
        private System.Windows.Forms.Button buttonTile8;
        private System.Windows.Forms.Button buttonTile7;
        private System.Windows.Forms.Button buttonTile6;
        private System.Windows.Forms.TextBox textBoxEnemyCount;
        private System.Windows.Forms.Label label2;
    }
}

