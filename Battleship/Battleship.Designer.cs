namespace Battleship
{
    partial class BattleshipForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arkivToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inställningarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.lblSetShip = new System.Windows.Forms.Label();
            this.nyttSpelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.avslutaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblGameOver = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arkivToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(644, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arkivToolStripMenuItem
            // 
            this.arkivToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nyttSpelToolStripMenuItem,
            this.inställningarToolStripMenuItem,
            this.avslutaToolStripMenuItem});
            this.arkivToolStripMenuItem.Name = "arkivToolStripMenuItem";
            this.arkivToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.arkivToolStripMenuItem.Text = "Arkiv";
            // 
            // inställningarToolStripMenuItem
            // 
            this.inställningarToolStripMenuItem.Name = "inställningarToolStripMenuItem";
            this.inställningarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inställningarToolStripMenuItem.Text = "Inställningar";
            this.inställningarToolStripMenuItem.Click += new System.EventHandler(this.inställningarToolStripMenuItem_Click);
            // 
            // btnStartGame
            // 
            this.btnStartGame.Enabled = false;
            this.btnStartGame.Location = new System.Drawing.Point(13, 28);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(155, 23);
            this.btnStartGame.TabIndex = 1;
            this.btnStartGame.Text = "Starta spelet";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // lblSetShip
            // 
            this.lblSetShip.AutoSize = true;
            this.lblSetShip.Location = new System.Drawing.Point(190, 37);
            this.lblSetShip.Name = "lblSetShip";
            this.lblSetShip.Size = new System.Drawing.Size(75, 13);
            this.lblSetShip.TabIndex = 2;
            this.lblSetShip.Text = "Placera skepp";
            // 
            // nyttSpelToolStripMenuItem
            // 
            this.nyttSpelToolStripMenuItem.Name = "nyttSpelToolStripMenuItem";
            this.nyttSpelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nyttSpelToolStripMenuItem.Text = "Nytt spel";
            this.nyttSpelToolStripMenuItem.Click += new System.EventHandler(this.nyttSpelToolStripMenuItem_Click);
            // 
            // avslutaToolStripMenuItem
            // 
            this.avslutaToolStripMenuItem.Name = "avslutaToolStripMenuItem";
            this.avslutaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.avslutaToolStripMenuItem.Text = "Avsluta";
            // 
            // lblGameOver
            // 
            this.lblGameOver.AutoSize = true;
            this.lblGameOver.BackColor = System.Drawing.Color.Silver;
            this.lblGameOver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGameOver.Font = new System.Drawing.Font("Comic Sans MS", 44.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameOver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblGameOver.Location = new System.Drawing.Point(81, 149);
            this.lblGameOver.Name = "lblGameOver";
            this.lblGameOver.Size = new System.Drawing.Size(360, 84);
            this.lblGameOver.TabIndex = 3;
            this.lblGameOver.Text = "YOU WON!";
            this.lblGameOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGameOver.Visible = false;
            // 
            // BattleshipForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 361);
            this.Controls.Add(this.lblGameOver);
            this.Controls.Add(this.lblSetShip);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BattleshipForm";
            this.Text = "Sänka Skepp";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arkivToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inställningarToolStripMenuItem;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Label lblSetShip;
        private System.Windows.Forms.ToolStripMenuItem nyttSpelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem avslutaToolStripMenuItem;
        private System.Windows.Forms.Label lblGameOver;









    }
}

