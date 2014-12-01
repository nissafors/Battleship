using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class BattleshipForm : Form
    {
        private BattleshipPanel playerField;
        private BattleshipPanel computerField;

        public BattleshipForm()
        {
            InitializeComponent();

            // TODO: Must be accessible for settings form
            int squareSize = 25;
            int rows = 20, cols = 20;

            this.playerField = new Battleship.BattleshipPanel(rows, cols, true);

            this.playerField.Location = new System.Drawing.Point(50, 50);
            this.playerField.Name = "playerPanel";
            this.playerField.Size = new System.Drawing.Size(rows * squareSize, cols * squareSize);
            this.playerField.TabIndex = 0;
            this.playerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(playerField);

            this.computerField = new Battleship.BattleshipPanel(rows, cols, false);

            this.computerField.Location = new System.Drawing.Point(100 + cols * squareSize, 50);
            this.computerField.Name = "computerPanel";
            this.computerField.Size = new System.Drawing.Size(rows * squareSize, cols * squareSize);
            this.computerField.TabIndex = 0;
            this.computerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(computerField);
        }

        private void UpdateForm(object sender, MouseEventArgs e)
        {
            int col, row; 
            if (sender.Equals(computerField))
            {
                col = e.Location.X / computerField.SquareHeight;
                row = e.Location.Y / computerField.SquareWidth;

                AudioEffect player = new AudioEffect();
                player.Sound = computerField.PlayerShoot(col, row);
                player.Play();
            }
        }
    }
}