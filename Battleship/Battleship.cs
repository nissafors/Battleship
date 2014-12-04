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
        enum Mode { SettingShips, Playing };

        private const int SQUARESIZE = 25;
        private const int GRIDPADDING_LEFT = 50;
        private const int GRIDPADDING_TOP = 70;
        private const int GRIDPADDING_CENTER = 50;

        private BattleshipPanel playerField;
        private BattleshipPanel computerField;
        private Mode gameMode;
        private int shipsSetCount;

        // Public fields
        public static int rows = 10;
        public static int cols = 10;
        
        // Properties
        public Ship[] Ships { private get; set; }
        public bool SoundOn { get; set; }

        public BattleshipForm()
        {
            InitializeComponent();

            // Default settings
            this.SoundOn = true;
            this.gameMode = Mode.SettingShips;
            Ships = new Ship[4];
            Ship temp = new Ship();
            int[] shipLengths = new int[4];
            for (int i = 0; i < 4; i++)
            {
                temp.length = i + 2;
                temp.name = "Test" + i;
                this.Ships[i] = temp;
                shipLengths[i] = temp.length;
            }

            this.playerField = new Battleship.BattleshipPanel(rows, cols, true);

            this.playerField.Location = new System.Drawing.Point(GRIDPADDING_LEFT, GRIDPADDING_TOP);
            this.playerField.Name = "playerPanel";
            this.playerField.Size = new System.Drawing.Size(rows * SQUARESIZE, cols * SQUARESIZE);
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
            //int[] shipLengths = new int[5] { 1, 2, 3, 4, 5 };

            this.computerField = new Battleship.BattleshipPanel(rows, cols, false);

            this.computerField.Location = new System.Drawing.Point(GRIDPADDING_LEFT + GRIDPADDING_CENTER + cols * SQUARESIZE, GRIDPADDING_TOP);
            this.computerField.Name = "computerPanel";
            this.computerField.Size = new System.Drawing.Size(rows * SQUARESIZE, cols * SQUARESIZE);
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

            if (sender.Equals(computerField) && gameMode == Mode.Playing)
            {
                Square result = computerField.PlayerShoot(col, row);
                playerField.ComputerShoot();
                if (SoundOn)
                {
                    AudioEffect player = new AudioEffect(result);
                    player.Play();
                }
            }
            else if (sender.Equals(playerField) && gameMode == Mode.SettingShips)
            {
                BattleshipPanel.ShipHandleReturn result;
                int length = 0;
                int shipArrayIndex;
                for (shipArrayIndex = 0; shipArrayIndex < Ships.Length; shipArrayIndex++)
                {
                    if (Ships[shipArrayIndex].row == null)
                    {
                        length = Ships[shipArrayIndex].length;
                        break;
                    }
                }

                result = playerField.PlayerHandleShip(ref col, ref row, length);
                if (result == BattleshipPanel.ShipHandleReturn.ShipSet)
                {
                    shipsSetCount++;
                    if (shipsSetCount == Ships.Length)
                    {
                        btnStartGame.Enabled = true;
                    }
                    Ships[shipArrayIndex].row = row;
                    Ships[shipArrayIndex].col = col;
                }
                else if (result == BattleshipPanel.ShipHandleReturn.ShipRemoved)
                {
                    shipsSetCount--;
                    btnStartGame.Enabled = false;
                    for (shipArrayIndex = 0; shipArrayIndex < Ships.Length; shipArrayIndex++)
                    {
                        if (Ships[shipArrayIndex].row == row && Ships[shipArrayIndex].col == col)
                        {
                            Ships[shipArrayIndex].row = null;
                            Ships[shipArrayIndex].col = null;
                        }
                    }
                }
            }

        }
       
        
        public void formSize()
        {
            this.playerField.ChangeSize(rows, cols);
            this.computerField.ChangeSize(rows, cols);

            this.AutoSize = true;
            this.Padding = new Padding(0, 0, 50, 50);

            computerField.Location = new Point((25 * cols) + 100, 50);

            this.computerField.Size = new System.Drawing.Size(rows * 25, cols * 25);       
            this.playerField.Size = new System.Drawing.Size(rows * 25, cols * 25);
        }

        private void inställningarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options optionForm = new Options(this);
            optionForm.Show();
        }

        public struct Ship
        {
            public int length;
            public string name;
            public int? row;
            public int? col;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            this.gameMode = Mode.Playing;
            playerField.ClearForbiddenSquares();
        }
    }
}