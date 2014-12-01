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
            const int squareSize = 25;
            int rows = 15, cols = 15;

            this.playerField = new Battleship.BattleshipPanel(rows, cols, true);

            this.playerField.Location = new System.Drawing.Point(50, 50);
            this.playerField.Name = "playerPanel";
            this.playerField.Size = new System.Drawing.Size(rows * squareSize, cols * squareSize);
            this.playerField.TabIndex = 0;
            this.playerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(playerField);
            /*
            Square[,] playFieldArray = new Square[5, 5]
            {
                {Square.Water, Square.Water, Square.Water, Square.Water, Square.Hit  },
                {Square.Water, Square.Water, Square.Water, Square.Water, Square.Hit  },
                {Square.Water, Square.Miss,  Square.Water, Square.Water, Square.Hit  },
                {Square.Water, Square.Miss,  Square.Water, Square.Water, Square.Ship },
                {Square.Water, Square.Water, Square.Water, Square.Water, Square.Ship },
            };
            */
            int[] shipLengths = new int[5] { 1, 2, 3, 4, 5 };

            this.computerField = new Battleship.BattleshipPanel(rows, cols, false);

            this.computerField.Location = new System.Drawing.Point(100 + cols * squareSize, 50);
            this.computerField.Name = "computerPanel";
            this.computerField.Size = new System.Drawing.Size(rows * squareSize, cols * squareSize);
            this.computerField.TabIndex = 0;
            this.computerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(computerField);
            computerField.ComputerPlaceShip(shipLengths);
        }

        private void UpdateForm(object sender, MouseEventArgs e)
        {
            int col, row; 
            row = e.Location.Y / computerField.SquareHeight;
            col = e.Location.X / computerField.SquareWidth;



            if (sender.Equals(computerField))
            {
                computerField.PlayerShoot(col, row);
                playerField.ComputerShoot();
                //AudioEffect player = new AudioEffect();
                //player.Sound = 
                //player.Play();
            }
            else if (sender.Equals(playerField))
            {
                playerField.PlayerHandleShip(col, row, 3);
            }
        }
    }
}