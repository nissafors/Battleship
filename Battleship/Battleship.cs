﻿//-----------------------------------------------------------------------------------------------------
// <copyright file="Battleship.cs" company="None">
//      Copyright (c) Andreas Andersson, Henrik Ottehall, Victor Ström Nilsson & Torbjörn Widström 2014
// </copyright>
// <author>Henrik Ottehall</author>
// <author>Victor Ström Nilsson</author>
// <author>Andreas Andersson</author>
//-----------------------------------------------------------------------------------------------------

namespace Battleship
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// This is the main form including basic UI control and game logic.
    /// </summary>
    public partial class BattleshipForm : Form
    {
        private const string GAMESTATEPATH = "gamestate.xml";

        /// <summary>
        /// The height of the squares making up the game board in pixels
        /// </summary>
        private const int SQUARESIZE = 25;

        /// <summary>
        /// The distance from the left border of the form to the leftmost game board grid in pixels.
        /// </summary>
        private const int GRIDPADDINGLEFT = 50;

        /// <summary>
        /// The distance from the top border of the form to the game board grids in pixels.
        /// </summary>
        private const int GRIDPADDINGTOP = 70;

        /// <summary>
        /// The distance between the two game board grids in pixels.
        /// </summary>
        private const int GRIDPADDINGCENTER = 50;

        /// <summary>
        /// The text to display when the player has won.
        /// </summary>
        private const string PLAYERWON = "Grattis! Du vann!";

        /// <summary>
        /// The text to display when the computer has won.
        /// </summary>
        private const string COMPUTERWON = "Datorn vann!";

        /// <summary>
        /// The game board panel containing the players ships.
        /// </summary>
        private BattleshipPanel playerField;

        /// <summary>
        /// The game board panel containing the computers ships.
        /// </summary>
        private BattleshipPanel computerField;

        /// <summary>
        /// Indicating the current game mode. 
        /// </summary>
        private Mode gameMode;

        /// <summary>
        /// Keeps track of how many of the players ships are currently on the board.
        /// </summary>
        private int shipsSetCount;

        /// <summary>
        /// Keeps track of how many ships the computer has lost.
        /// </summary>
        private int shipsLostComputer;

        /// <summary>
        /// Keeps track of how many ships the player has lost.
        /// </summary>
        private int shipsLostPlayer;

        /// <summary>
        /// Used to DO the computer shooting in a separate thread.
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipForm"/> class
        /// </summary>
        public BattleshipForm()
        {
            this.InitializeComponent();

            // Default settings
            this.Rows = this.Cols = 10;
            numberOfShips = 4;
            this.SoundOn = true;
            this.gameMode = Mode.SettingShips;
            this.ResetShips();
            this.InitializeGameBoard();
        }

        /// <summary>
        /// Values used to indicate the current game mode.
        /// </summary>
        private enum Mode
        {
            /// <summary>
            /// Player is currently placing ships on the game board.
            /// </summary>
            SettingShips,

            /// <summary>
            /// We're in the middle of a game.
            /// </summary>
            Playing,

            /// <summary>
            /// The player has won the game.
            /// </summary>
            PlayerWon,

            /// <summary>
            /// The computer has won the game.
            /// </summary>
            ComputerWon
        }

        /// <summary>
        /// Gets or sets the number of rows on the game board.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Gets or sets the number of columns on the game board.
        /// </summary>
        public int Cols { get; set; }

        /// <summary>
        /// Gets or sets he list of ships used in this game.
        /// </summary>
        public Ship[] Ships { private get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sounds are played or not.
        /// </summary>
        public bool SoundOn { get; set; }

        /// <summary>
        /// Resets all fields and properties needed to start over with a brand new game of glorious battleship.
        /// </summary>
        public void RestartGame()
        {
            lblSetShip.Visible = true;
            this.Controls.Remove(this.playerField);
            this.Controls.Remove(this.computerField);
            this.playerField = null;
            this.computerField = null;
            this.shipsSetCount = 0;
            this.shipsLostComputer = 0;
            this.shipsLostPlayer = 0;
            this.gameMode = Mode.SettingShips;
            lblGameOver.Visible = false;
            this.ResetShips();
            this.InitializeGameBoard();
        }

        public void FormSize()
        {
            this.playerField.ChangeSize(this.Rows, this.Cols);
            this.computerField.ChangeSize(this.Rows, this.Cols);

            this.Width = (SQUARESIZE * this.Cols * 2) + 168;
            this.Height = (SQUARESIZE * this.Rows) + 158;
            this.Padding = new Padding(0, 0, 50, 50);

            this.computerField.Location = new Point((25 * this.Cols) + 100, 70);

            this.computerField.Size = new System.Drawing.Size(this.Rows * 25, this.Cols * 25);
            this.playerField.Size = new System.Drawing.Size(this.Rows * 25, this.Cols * 25);
        }

        public int numberOfShips;
        public int numberOfPatrolboats = 1, numberOfCruisers = 1, numberOfSubmarines = 1, numberOfCarriers = 1;

        private void ResetShips()
        {
            Ships = new Ship[numberOfShips];
            Ship patrolboats = new Ship();
            Ship cruisers = new Ship();
            Ship submarines = new Ship();
            Ship carriers = new Ship();

            patrolboats.Length = 2;
            cruisers.Length = 3;
            submarines.Length = 4;
            carriers.Length = 5;

            int tempShips = 0;

            for (int i = 0; i < numberOfPatrolboats; i++)
            {
                this.Ships[tempShips] = patrolboats;
                tempShips++;
            }
            for (int i = 0; i < numberOfCruisers; i++)
            {
                this.Ships[tempShips] = cruisers;
                tempShips++;
            }
            for (int i = 0; i < numberOfSubmarines; i++)
            {
                this.Ships[tempShips] = submarines;
                tempShips++;
            }
            for (int i = 0; i < numberOfCarriers; i++)
            {
                this.Ships[tempShips] = carriers;
                tempShips++;
            }
        }

        /// <summary>
        /// Creates one game board panel with auto-placed ships for the computer, and one empty game board panel for the player.
        /// </summary>
        private void InitializeGameBoard()
        {
            // From the list of ships, create an array with only the lengths of the ships for the AutoShipPlacing method.
            int[] shipLength = new int[this.Ships.Length];
            int i = 0;
            foreach (Ship ship in this.Ships)
            {
                shipLength[i++] = ship.Length;
            }

            // Create players panel
            this.playerField = new Battleship.BattleshipPanel(this.Rows, this.Cols, true);
            this.playerField.Location = new System.Drawing.Point(GRIDPADDINGLEFT, GRIDPADDINGTOP);
            this.playerField.Name = "playerPanel";
            this.playerField.Size = new System.Drawing.Size(this.Rows * SQUARESIZE, this.Cols * SQUARESIZE);
            this.playerField.TabIndex = 0;
            this.playerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(this.playerField);

            // Create computers panel and autoplace the ships.
            this.computerField = new Battleship.BattleshipPanel(this.Rows, this.Cols, false);
            this.computerField.Location = new System.Drawing.Point(GRIDPADDINGLEFT + GRIDPADDINGCENTER + (this.Cols * SQUARESIZE), GRIDPADDINGTOP);
            this.computerField.Name = "computerPanel";
            this.computerField.Size = new System.Drawing.Size(this.Rows * SQUARESIZE, this.Cols * SQUARESIZE);
            this.computerField.TabIndex = 0;
            this.computerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(this.computerField);
            this.computerField.AutoShipPlacing(shipLength);
        }

        /// <summary>
        /// Display a label telling who won and set gameMode accordingly.
        /// </summary>
        private void GameOver()
        {
            lblGameOver.Text = (this.gameMode == Mode.PlayerWon) ? PLAYERWON : COMPUTERWON;
            lblGameOver.Left = (this.Size.Width / 2) - (lblGameOver.Size.Width / 2);
            lblGameOver.Top = (this.Size.Height / 2) - (lblGameOver.Size.Height / 2);
            this.computerField.ShowShips = true;
            System.Threading.Thread.Sleep(500);
            lblGameOver.Visible = true;
        }

        private void UpdateForm(object sender, MouseEventArgs e)
        {
            int col, row; 
            row = e.Location.Y / this.computerField.SquareHeight;
            col = e.Location.X / this.computerField.SquareWidth;

            if (sender.Equals(this.computerField) && this.gameMode == Mode.Playing)
            {
                // We are here because the player fired off a torpedo.
                // Excecute torpedo launch and analyze the result.
                Square result = this.computerField.PlayerShoot(col, row);
                if (this.SoundOn)
                {
                    AudioEffect player = new AudioEffect(result);
                    player.Play();
                }

                if (result == Square.Sunk)
                {
                    this.shipsLostComputer++;
                    if (this.shipsLostComputer == this.Ships.Length)
                    {
                        // Computer has lost all her ships.
                        this.gameMode = Mode.PlayerWon;
                        this.GameOver();
                        return;
                    }
                }
                else if (result != Square.Hit && result != Square.Forbidden)
                {
                    // Player missed and it's the computers turn.
                    // Disable the control so player can't shoot while computer is shooting. (The control is
                    // enabled again in the bgWorker_RunWorkerCompleted method below.)
                    this.computerField.Enabled = false;

                    // Computer shooting may take some time so it has to be done in a separate thread.
                    this.bgWorker = new BackgroundWorker();
                    this.bgWorker.DoWork += new DoWorkEventHandler(this.bgWorker_DoWork);
                    this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
                    this.bgWorker.RunWorkerAsync();
                }
            }
            else if (sender.Equals(this.playerField) && this.gameMode == Mode.SettingShips)
            {
                // We are here because the player tried to set a starting coordinate for a ship
                // or he actually placed the ship.

                // Loop through the ships to find the first one that is not already on the board
                int length = 0;
                int shipArrayIndex;
                for (shipArrayIndex = 0; shipArrayIndex < this.Ships.Length; shipArrayIndex++)
                {
                    if (this.Ships[shipArrayIndex].Row == null)
                    {
                        length = this.Ships[shipArrayIndex].Length;
                        break;
                    }
                }

                // Send the length of that ship, or 0 if none found. In return we'll get the result
                // of the action (ship placed, point placed, ship removed or failure) and the
                // coordinates of the ship or point affected.
                BattleshipPanel.ShipHandleReturn result = this.playerField.ManualShipHandling(ref col, ref row, length);
                if (result == BattleshipPanel.ShipHandleReturn.ShipSet)
                {
                    // Save the coordinates of this ship and enable the start game-button if all
                    // ships are on the board
                    this.shipsSetCount++;
                    if (this.shipsSetCount == this.Ships.Length)
                    {
                        btnStartGame.Enabled = true;
                        lblSetShip.Visible = false;
                    }

                    this.Ships[shipArrayIndex].Row = row;
                    this.Ships[shipArrayIndex].Col = col;
                }
                else if (result == BattleshipPanel.ShipHandleReturn.ShipRemoved)
                {
                    // Disable start button and null the coordinates of the removed ship
                    this.shipsSetCount--;
                    btnStartGame.Enabled = false;
                    lblSetShip.Visible = true;
                    for (shipArrayIndex = 0; shipArrayIndex < this.Ships.Length; shipArrayIndex++)
                    {
                        if (this.Ships[shipArrayIndex].Row == row && this.Ships[shipArrayIndex].Col == col)
                        {
                            this.Ships[shipArrayIndex].Row = null;
                            this.Ships[shipArrayIndex].Col = null;
                        }
                    }
                }
            }
        }
       
        private void inställningarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options optionForm = new Options(this);
            optionForm.ShowDialog();
        }

        /// <summary>
        /// This button gets enabled when all ships in <see cref="Ships"/> list have been placed.
        /// Clicking it starts the game by changing the <see cref="gameMode"/>gameMode.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Arguments passed.</param>
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            this.gameMode = Mode.Playing;
            btnStartGame.Enabled = false;
            this.playerField.ClearForbiddenSquares();
        }

        /// <summary>
        /// A menu choice that resets and restarts the game.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Arguments passed.</param>
        private void nyttSpelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RestartGame();
        }

        private void avslutaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Executes computers playing round, doing some shooting and resting some. This is done by a
        /// BackgroundWorker so the UI doesn't freeze up.
        /// </summary>
        /// <param name="sender">Calling object.</param>
        /// <param name="e">Arguments passed.</param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Computer shoots as long as it doesn't miss
            int sleepMS = 700;
            Square result;
            do
            {
                // Don't shoot too fast
                Thread.Sleep(sleepMS);

                // We need to call ComputerShoot() from the UI thread as it changes the GUI
                result = (Square)this.Invoke((Func<Square>)delegate
                {
                    return this.playerField.ComputerShoot();
                });

                // Prolong the next sleep if ship was sunk because sinking sound takes longer to play
                sleepMS = (result == Square.Sunk) ? 2200 : 700;
                if (this.SoundOn)
                {
                    AudioEffect player = new AudioEffect(result);
                    player.Play();
                }

                if (result == Square.Sunk)
                {
                    this.shipsLostPlayer++;
                    if (this.shipsLostPlayer == this.Ships.Length)
                    {
                        // Player has lost all her ships. This triggers a GameOver() call in bgWorker_RunWorkerCompleted.
                        e.Result = Mode.ComputerWon;
                        return;
                    }
                }
            }
            while (result == Square.Hit || result == Square.Sunk);
        }

        /// <summary>
        /// Fires when computer is done shooting. Evaluates if computer won and enables computerField so
        /// player can shoot again.
        /// </summary>
        /// <param name="sender">Calling object.</param>
        /// <param name="e">Arguments passed.</param>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Both player and computer are done shooting for now.
            this.computerField.Enabled = true;

            // bgWorker_DoWork sets e.Result if computer won
            if (e.Result != null && (Mode)e.Result == Mode.ComputerWon)
            {
                this.gameMode = Mode.ComputerWon;
                this.GameOver();
            }
        }

        /// <summary>
        /// Used by the <see cref="Ships"/> property to hold data about the ships available to this game.
        /// </summary>
        public struct Ship
        {
            /// <summary>
            /// The length of the ship in game board squares.
            /// </summary>
            public int Length;

            /// <summary>
            /// The name of the ship.
            /// </summary>
            public string Name;

            /// <summary>
            /// The starting row for this ship if set. Null if not yet placed.
            /// </summary>
            public int? Row;

            /// <summary>
            /// The starting column for this ship if set. Null if not yet placed.
            /// </summary>
            public int? Col;
        }

        private void BattleshipForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Write gamestate to file

            // Get game board
            string[] computerGrid = computerField.PlayFieldToStringArray();
            string[] playerGrid = playerField.PlayFieldToStringArray();

            XmlWriterSettings settings = new XmlWriterSettings(){ Indent = true, NewLineOnAttributes = true};

            // Write XML file
            using (XmlWriter writer = XmlWriter.Create(GAMESTATEPATH, settings))
            {
                writer.WriteStartDocument();
                writer.WriteComment("This file was autogenerated by Battleship. Do not change!");
                writer.WriteStartElement("GameState");

                writer.WriteComment("Settings");
                writer.WriteElementString("GameMode", Convert.ToString((int)gameMode));
                writer.WriteElementString("SoundOn", SoundOn.ToString());
                foreach (Ship ship in Ships)
                {
                    writer.WriteElementString("ShipLength", ship.Length.ToString());
                }

                writer.WriteComment("Game board");
                writer.WriteElementString("GridHeight", Convert.ToString(computerGrid.Length));
                writer.WriteElementString("GridWidth", Convert.ToString(computerGrid[0].Length));
                writer.WriteStartElement("ComputerGrid");
                foreach (String row in computerGrid)
                {
                    writer.WriteElementString("Row", row);
                }
                writer.WriteEndElement();
                writer.WriteStartElement("PlayerGrid");
                foreach (String row in playerGrid)
                {
                    writer.WriteElementString("Row", row);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}