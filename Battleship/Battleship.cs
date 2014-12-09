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
        enum Mode { SettingShips, Playing, PlayerWon, ComputerWon };

        private const int SQUARESIZE = 25;
        private const int GRIDPADDING_LEFT = 50;
        private const int GRIDPADDING_TOP = 70;
        private const int GRIDPADDING_CENTER = 50;
        private const string PLAYERWON = "Grattis! Du vann!";
        private const string COMPUTERWON = "Datorn vann!";

        private BattleshipPanel playerField;
        private BattleshipPanel computerField;
        private Mode gameMode;
        private int shipsSetCount;
        private int shipsLostComputer;
        private int shipsLostPlayer;
        private BackgroundWorker bgWorker;

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
            this.ResetShips();
            this.InitializeGameboard();
        }

        private void ResetShips()
        {
            Ships = new Ship[4];
            Ship temp = new Ship();
            for (int i = 0; i < 4; i++)
            {
                temp.length = i + 2;
                this.Ships[i] = temp;
            }
        }

        public void RestartGame()
        {
            lblSetShip.Visible = true;
            this.Controls.Remove(playerField);
            this.Controls.Remove(computerField);
            this.playerField = null;
            this.computerField = null;
            this.shipsSetCount = 0;
            this.shipsLostComputer = 0;
            this.shipsLostPlayer = 0;
            this.gameMode = Mode.SettingShips;
            lblGameOver.Visible = false;
            this.ResetShips();
            this.InitializeGameboard();
        }

        private void InitializeGameboard()
        {
            int[] shipLength = new int[Ships.Length];
            int i = 0;
            foreach (Ship ship in Ships)
            {
                shipLength[i++] = ship.length;
            }

            this.playerField = new Battleship.BattleshipPanel(rows, cols, true);
            this.playerField.Location = new System.Drawing.Point(GRIDPADDING_LEFT, GRIDPADDING_TOP);
            this.playerField.Name = "playerPanel";
            this.playerField.Size = new System.Drawing.Size(rows * SQUARESIZE, cols * SQUARESIZE);
            this.playerField.TabIndex = 0;
            this.playerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(playerField);

            this.computerField = new Battleship.BattleshipPanel(rows, cols, false);
            this.computerField.Location = new System.Drawing.Point(GRIDPADDING_LEFT + GRIDPADDING_CENTER + cols * SQUARESIZE, GRIDPADDING_TOP);
            this.computerField.Name = "computerPanel";
            this.computerField.Size = new System.Drawing.Size(rows * SQUARESIZE, cols * SQUARESIZE);
            this.computerField.TabIndex = 0;
            this.computerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(computerField);
            computerField.AutoShipPlacing(shipLength);
        }

        private void GameOver()
        {
            lblGameOver.Text = (gameMode == Mode.PlayerWon) ? PLAYERWON : COMPUTERWON;
            lblGameOver.Left = (this.Size.Width / 2) - (lblGameOver.Size.Width / 2);
            lblGameOver.Top = (this.Size.Height / 2) - (lblGameOver.Size.Height / 2);
            this.computerField.ShowShips = true;
            System.Threading.Thread.Sleep(500);
            lblGameOver.Visible = true;
        }

        private void UpdateForm(object sender, MouseEventArgs e)
        {
            int col, row; 
            row = e.Location.Y / computerField.SquareHeight;
            col = e.Location.X / computerField.SquareWidth;

            if (sender.Equals(computerField) && gameMode == Mode.Playing)
            {
                // We are here because the player fired off a torpedo. (S)he shouldn't be able
                // to shoot more while handling the last shot or while the computer is shooting:
                ////computerField.Enabled = false;

                // Excecute torpedo impact and analyze the result.
                Square result = computerField.PlayerShoot(col, row);
                if (SoundOn)
                {
                    AudioEffect player = new AudioEffect(result);
                    player.Play();
                }
                if (result == Square.Sunk)
                {
                    shipsLostComputer++;
                    if (shipsLostComputer == Ships.Length)
                    {
                        gameMode = Mode.PlayerWon;
                        GameOver();
                        return;
                    }
                }
                if (result != Square.Hit && result != Square.Sunk && result != Square.Forbidden)
                {
                    computerField.Enabled = false;

                    // Call bgWorker_DoWork
                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                    bgWorker.RunWorkerAsync();
                }
            }
            else if (sender.Equals(playerField) && gameMode == Mode.SettingShips)
            {
                // We are here because the player tried to set a starting coordinate for a ship
                // or he actually placed the ship.

                // Loop through the ships to find the first one that is not already on the board
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

                // Send the length of that ship, or 0 if none found. In return we'll get the result
                // of the action (ship placed, point placed, ship removed or failure) and the
                // coordinates of the ship or point affected.
                BattleshipPanel.ShipHandleReturn result = playerField.ManualShipHandling(ref col, ref row, length);
                if (result == BattleshipPanel.ShipHandleReturn.ShipSet)
                {
                    // Save the coordinates of this ship and enable the start game-button if all
                    // ships are on the board
                    shipsSetCount++;
                    if (shipsSetCount == Ships.Length)
                    {
                        btnStartGame.Enabled = true;
                        lblSetShip.Visible = false;
                    }
                    Ships[shipArrayIndex].row = row;
                    Ships[shipArrayIndex].col = col;
                }
                else if (result == BattleshipPanel.ShipHandleReturn.ShipRemoved)
                {
                    // Disable start button and null the coordinates of the removed ship
                    shipsSetCount--;
                    btnStartGame.Enabled = false;
                    lblSetShip.Visible = true;
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

            this.Width = (SQUARESIZE * cols * 2 + 168);
            this.Height = (SQUARESIZE * rows + 158);
            this.Padding = new Padding(0, 0, 50, 50);

            computerField.Location = new Point((25 * cols) + 100, 70);

            this.computerField.Size = new System.Drawing.Size(rows * 25, cols * 25);       
            this.playerField.Size = new System.Drawing.Size(rows * 25, cols * 25);
        }

        private void inställningarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options optionForm = new Options(this);
            optionForm.ShowDialog();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            this.gameMode = Mode.Playing;
            btnStartGame.Enabled = false;
            playerField.ClearForbiddenSquares();
        }

        private void nyttSpelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        public struct Ship
        {
            public int length;
            public string name;
            public int? row;
            public int? col;
        }

        private void avslutaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Computers turn as if player missed and as long as computer hits
            int msSleep = 700;
            Square result;
            do
            {
                System.Threading.Thread.Sleep(msSleep);
                result = (Square)this.Invoke((Func<Square>)delegate()
                {
                    return playerField.ComputerShoot();
                });
                msSleep = (result == Square.Sunk) ? 2200 : 700;
                if (SoundOn)
                {
                    AudioEffect player = new AudioEffect(result);
                    player.Play();
                }
                if (result == Square.Sunk)
                {
                    shipsLostPlayer++;
                    if (shipsLostPlayer == Ships.Length)
                    {
                        e.Result = Mode.ComputerWon;
                        return;
                    }
                }
            } while (result == Square.Hit || result == Square.Sunk);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Both player and computer are done shooting for now.
            computerField.Enabled = true;

            // Check if computer won
            if (e.Result != null && (Mode)e.Result == Mode.ComputerWon)
            {
                gameMode = Mode.ComputerWon;
                GameOver();
            }
        }
    }
}