//-----------------------------------------------------------------------------------------------------
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
    using System.Collections.Generic;
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
        /// The path to the xml-file containing game state
        /// </summary>
        private const string GAMESTATEPATH = "gamestate.xml";

        /// <summary>
        /// The lengths of the different ships
        /// </summary>
        private const int CARRIERLENGTH = 5, SUBMARINELENGTH = 4, CRUISERLENGTH = 3, PATROLBOATLENGTH = 2;

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
        private BackgroundWorker backgWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipForm"/> class
        /// </summary>
        public BattleshipForm()
        {
            this.InitializeComponent();

            if (this.ReadGameStateFromXML())
            {
                if (this.gameMode != Mode.Playing)
                {
                    // The save did not contain a game in progress. 
                    // Set up for a new game.
                    this.RestartGame();
                }
                else
                {
                    this.FormSize();
                    this.InitializeGameBoard();
                }
            }
            else 
            {
                // Failed to read settings from file
                // Using default settings
                this.Rows = this.Cols = 10;
                this.NumberOfPatrolboats = this.NumberOfCruisers = this.NumberOfSubmarines = this.NumberOfCarriers = 1;
                this.NumberOfShips = this.NumberOfPatrolboats + this.NumberOfCruisers + this.NumberOfSubmarines + this.NumberOfCarriers;
                this.SoundOn = true;

                this.RestartGame();
            }
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
            ComputerWon,

            /// <summary>
            /// Game board not accessible.
            /// </summary>
            Hold
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
        /// Gets or sets the total number of ships used in this game.
        /// </summary>
        public int NumberOfShips { get; set; }

        /// <summary>
        /// Gets or sets the number of patrol-boats used in this game.
        /// </summary>
        public int NumberOfPatrolboats { get; set; }

        /// <summary>
        /// Gets or sets the number of cruisers used in this game.
        /// </summary>
        public int NumberOfCruisers { get; set; }

        /// <summary>
        /// Gets or sets the number of submarines used in this game.
        /// </summary>
        public int NumberOfSubmarines { get; set; }

        /// <summary>
        /// Gets or sets the number of carriers used in this game.
        /// </summary>
        public int NumberOfCarriers { get; set; }
       
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
            this.ResetShips();

            int[] shipLength = new int[this.Ships.Length];
            int i = 0;

            foreach (Ship ship in this.Ships)
            {
                shipLength[i++] = ship.Length;
            }

            this.playerField = new BattleshipPanel(this.Cols, this.Rows, true);
            this.computerField = new BattleshipPanel(this.Cols, this.Rows, false);
            
            this.shipsSetCount = 0;
            this.shipsLostComputer = 0;
            this.shipsLostPlayer = 0;
            this.gameMode = Mode.SettingShips;
            lblGameOver.Visible = false;
            this.InitializeGameBoard();

            try
            {
                this.computerField.AutoShipPlacing(shipLength);
            }
            catch (InsufficientGridSpaceException)
            {
                this.gameMode = Mode.Hold;
                MessageBox.Show(
                    "Datorn kan inte att placerar sina skepp. Prova att minska antalet och/eller storleken under inställningar.",
                    "Fel!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            this.FormSize();
        }

        /// <summary>
        /// Calculates and sets the size of the window.
        /// </summary>
        public void FormSize()
        {
            this.Width = (SQUARESIZE * this.Cols * 2) + 168;
            this.Height = (SQUARESIZE * this.Rows) + 158;
            this.Padding = new Padding(0, 0, 50, 50);
        }

        /// <summary>
        /// Sets the length and amount of each ship in the game.
        /// </summary>
        private void ResetShips()
        {
            this.Ships = new Ship[this.NumberOfShips];
            Ship patrolboats = new Ship();
            Ship cruisers = new Ship();
            Ship submarines = new Ship();
            Ship carriers = new Ship();

            patrolboats.Length = PATROLBOATLENGTH;
            cruisers.Length = CRUISERLENGTH;
            submarines.Length = SUBMARINELENGTH;
            carriers.Length = CARRIERLENGTH;

            int tempShips = 0;

            for (int i = 0; i < this.NumberOfPatrolboats; i++)
            {
                this.Ships[tempShips] = patrolboats;
                tempShips++;
            }

            for (int i = 0; i < this.NumberOfCruisers; i++)
            {
                this.Ships[tempShips] = cruisers;
                tempShips++;
            }

            for (int i = 0; i < this.NumberOfSubmarines; i++)
            {
                this.Ships[tempShips] = submarines;
                tempShips++;
            }

            for (int i = 0; i < this.NumberOfCarriers; i++)
            {
                this.Ships[tempShips] = carriers;
                tempShips++;
            }
        }

        /// <summary>
        /// Places one game board panel for the computer, and one game board panel for the player.
        /// </summary>
        private void InitializeGameBoard()
        {
            // Create players panel
            this.playerField.Location = new System.Drawing.Point(GRIDPADDINGLEFT, GRIDPADDINGTOP);
            this.playerField.Size = new System.Drawing.Size(this.Rows * SQUARESIZE, this.Cols * SQUARESIZE);
            this.playerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(this.playerField);

            // Create computers panel
            this.computerField.Location = new System.Drawing.Point(GRIDPADDINGLEFT + GRIDPADDINGCENTER + (this.Cols * SQUARESIZE), GRIDPADDINGTOP);
            this.computerField.Size = new System.Drawing.Size(this.Rows * SQUARESIZE, this.Cols * SQUARESIZE);
            this.computerField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateForm);
            this.Controls.Add(this.computerField);
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

        /// <summary>
        /// Handles player interaction with the playing fields. Outcome depends on GameMode.
        /// </summary>
        /// <param name="sender">A System.Object containing the sender date</param>
        /// <param name="e">A System.Windows.Forms.MouseEventArgs that contain the event data.</param>
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
                    this.backgWorker = new BackgroundWorker();
                    this.backgWorker.DoWork += new DoWorkEventHandler(this.BackgroundWorker_DoWork);
                    this.backgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
                    this.backgWorker.RunWorkerAsync();
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
       
        /// <summary>
        /// Runs the options form.
        /// </summary>
        /// <param name="sender">A System.Object containing the sender data.</param>
        /// <param name="e">A System.EventArgs that contain the event data.</param>
        private void InställningarToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void BtnStartGame_Click(object sender, EventArgs e)
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
        private void NyttSpelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RestartGame();
        }

        /// <summary>
        /// Closes the program
        /// </summary>
        /// <param name="sender">A System.Object containing the sender data.</param>
        /// <param name="e">An System.EventArgs that contain the event data.</param>
        private void AvslutaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Executes computers playing round, doing some shooting and resting some. This is done by a
        /// BackgroundWorker so the UI doesn't freeze up.
        /// </summary>
        /// <param name="sender">Calling object.</param>
        /// <param name="e">Arguments passed.</param>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
        /// Tries to read the saved game state and settings in <paramref name="GAMESTATEPATH"/>.
        /// <para>Returns true if successful.</para>
        /// </summary>
        /// <returns>Returns true if it successfully reads from the file</returns>
        private bool ReadGameStateFromXML()
        {
            List<Ship> shipList = new List<Ship>();
            this.lblSetShip.Visible = false;
            int shipLength;

            try
            {
                using (XmlReader reader = XmlReader.Create(GAMESTATEPATH))
                {
                    while (reader.Read())
                    {
                        // Handle the contents of the XML-file depending on the name of the tag
                        switch (reader.Name)
                        {
                            case "GameMode":
                                this.gameMode = (Mode)reader.ReadElementContentAsInt();
                                break;

                            case "SoundOn":
                                this.SoundOn = reader.ReadElementContentAsBoolean();
                                break;

                            case "ShipsLostPlayer":
                                this.shipsLostPlayer = reader.ReadElementContentAsInt();
                                break;

                            case "ShipsLostComputer":
                                this.shipsLostComputer = reader.ReadElementContentAsInt();
                                break;

                            case "ShipLength":
                                // Add the ship to the list of ships then count up the correct kind of ship
                                // depending on the length of the added ship.
                                shipLength = reader.ReadElementContentAsInt();
                                shipList.Add(new Ship() { Name = "Ship", Length = shipLength });
                                if (shipLength == CARRIERLENGTH)
                                {
                                    this.NumberOfCarriers++;
                                }
                                else if (shipLength == SUBMARINELENGTH)
                                {
                                    this.NumberOfSubmarines++;
                                }
                                else if (shipLength == CRUISERLENGTH)
                                {
                                    this.NumberOfCruisers++;
                                }
                                else if (shipLength == PATROLBOATLENGTH)
                                {
                                    this.NumberOfPatrolboats++;
                                }

                                break;

                            case "GridHeight":
                                this.Rows = reader.ReadElementContentAsInt();
                                break;

                            case "GridWidth":
                                this.Cols = reader.ReadElementContentAsInt();
                                break;

                            case "ComputerGrid":
                                // Send the computerGrid XML subtree to ReadArrayFromXML to parse the array
                                this.computerField = new BattleshipPanel(this.ReadArrayFromXML(reader.ReadSubtree()), false);
                                break;

                            case "PlayerGrid":
                                // Send the computerGrid XML subtree to ReadArrayFromXML to parse the array
                                this.playerField = new BattleshipPanel(this.ReadArrayFromXML(reader.ReadSubtree()), true);
                                break;
                        }
                    }
                }

                this.Ships = shipList.ToArray();
                this.NumberOfShips = this.NumberOfCarriers + this.NumberOfSubmarines + this.NumberOfCruisers + this.NumberOfPatrolboats;

                return true;
            }
            catch (Exception)
            {
                // An exception occured when trying to read the gamestate file
                return false;
            }
        }

        /// <summary>
        /// Parses the playing field from an XML file to an array.
        /// </summary>
        /// <param name="reader">An XmlReader with the rows of the array</param>
        /// <returns>A two dimensional Square array</returns>
        private Square[,] ReadArrayFromXML(XmlReader reader)
        {
            Square[,] playField = new Square[this.Rows, this.Cols];
            string row;
            int rowCount = 0;

            // Finds the Row XML elements to parse each row
            while (reader.ReadToFollowing("Row") && rowCount < this.Rows)
            {
                row = reader.ReadElementContentAsString();

                // Parse each element in the row to a Square for the array
                for (int i = 0; i < row.Length && i < this.Cols; i++)
                {
                    playField[rowCount, i] = (Square)int.Parse(row[i].ToString());
                }

                rowCount++;
            }

            return playField;
        }

        /// <summary>
        /// Fires when the program is closing. Saves game state in an xml file.
        /// </summary>
        /// <param name="sender">Calling object.</param>
        /// <param name="e">Arguments passed.</param>
        private void BattleshipForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Get game board
            string[] computerGrid = this.computerField.PlayFieldToStringArray();
            string[] playerGrid = this.playerField.PlayFieldToStringArray();

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true };

            // Write XML file
            using (XmlWriter writer = XmlWriter.Create(GAMESTATEPATH, settings))
            {
                writer.WriteStartDocument();
                writer.WriteComment("This file was autogenerated by Battleship. Do not change!");
                writer.WriteStartElement("GameState");

                writer.WriteComment("Settings");
                writer.WriteElementString("GameMode", Convert.ToString((int)this.gameMode));
                writer.WriteElementString("SoundOn", this.SoundOn.ToString().ToLower());
                foreach (Ship ship in this.Ships)
                {
                    writer.WriteElementString("ShipLength", ship.Length.ToString().ToLower());
                }

                writer.WriteComment("Game state");
                writer.WriteElementString("ShipsLostPlayer", Convert.ToString(this.shipsLostPlayer));
                writer.WriteElementString("ShipsLostComputer", Convert.ToString(this.shipsLostComputer));

                writer.WriteComment("Game board");
                writer.WriteElementString("GridHeight", Convert.ToString(computerGrid.Length));
                writer.WriteElementString("GridWidth", Convert.ToString(computerGrid[0].Length));
                writer.WriteStartElement("ComputerGrid");
                foreach (string row in computerGrid)
                {
                    writer.WriteElementString("Row", row);
                }

                writer.WriteEndElement();
                writer.WriteStartElement("PlayerGrid");
                foreach (string row in playerGrid)
                {
                    writer.WriteElementString("Row", row);
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();

                this.Dispose();
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
    }
}