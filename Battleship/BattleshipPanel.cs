//-----------------------------------------------------------------------
// <copyright file="BattleshipPanel.cs" company="None">
//     Copyright (c) Henrik Ottehall 2014
// </copyright>
// <author>Henrik Ottehall</author>
//-----------------------------------------------------------------------

namespace Battleship
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Class for drawing the playing field and handling events in it.
    /// </summary>
    public class BattleshipPanel : Panel
    {
        public enum ShipHandleReturn { ShipSet, PointSet, ShipRemoved, Failed };

        /// <summary>
        /// Constants for the images that is drawn. TODO: Change the arrow images to correct ones once they exist.
        /// </summary>
        private const string IMAGEPATH = @"images\", FORBIDDENIMAGE = "forbidden.png", MISSIMAGE = "miss.png", 
            SHIPIMAGE = "ship.png", SUNKIMAGE = "sunk.png", WATERIMAGE = "water.png",  HITIMAGE = "hit.png", 
            RIGHTARROWIMAGE = "right.png", DOWNARROWIMAGE = "down.png", LEFTARROWIMAGE = "left.png", UPARROWIMAGE = "up.png";

        /// <summary>
        /// Number of rows and columns in the play field and the row and column that a ship is being placed at.
        /// </summary>
        private int rows = 10, columns = 10, shipRow, shipCol;

        /// <summary>
        /// The play field.
        /// </summary>
        private Square[,] playField;

        /// <summary>
        /// Field used for placing ships in the playing field
        /// </summary>
        private ShipPositioner shipPlacer = new ShipPositioner();

        /// <summary>
        /// Whether the player is currently in the process of placing a ship.
        /// </summary>
        private bool isPlacing = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipPanel"/> class.
        /// Defaults to a 10x10 grid.
        /// </summary>
        public BattleshipPanel()
        {
            // Force it to redraw the field if the size of the panel changes
            this.ResizeRedraw = true;
            /*
            * Use OptimizedDoubleBuffer to make the panel paint in a buffer before outputting it to screen
            * Use UserPaint to make the panel paint itself instead of having the operating system do it
            * Use AllPaintingInWmPaint to make the control ignore the WM_ERASEBKGND window message
            * This reduces flicker when repainting the panel
            */
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            
            this.playField = new Square[this.rows, this.columns];

            this.SquareHeight = this.ClientSize.Height / this.rows;
            this.SquareWidth = this.ClientSize.Width / this.columns;
            this.IsPlayer = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipPanel"/> class.
        /// This constructor that can be used to load a preset playing field
        /// </summary>
        /// <param name="fieldArray">An array of Squares.</param>
        /// <param name="playerOwner">Whether the player or the computer owns the field</param>
        public BattleshipPanel(Square[,] fieldArray, bool playerOwner)
        {
            // Force it to redraw the field if the size of the panel changes
            this.ResizeRedraw = true;
            /*
            * Use OptimizedDoubleBuffer to make the panel paint in a buffer before outputting it to screen
            * Use UserPaint to make the panel paint itself instead of having the operating system do it
            * Use AllPaintingInWmPaint to make the control ignore the WM_ERASEBKGND window message
            * This reduces flicker when repainting the panel
            */
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.rows = fieldArray.GetLength(0);
            this.columns = fieldArray.GetLength(1);
            this.playField = fieldArray;

            this.SquareHeight = this.ClientSize.Height / this.rows;
            this.SquareWidth = this.ClientSize.Width / this.columns;
            this.IsPlayer = playerOwner;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipPanel"/> class.
        /// This constructor can be used to create a new field with a variable number of columns and rows.
        /// </summary>
        /// <param name="numColumns">Number of columns</param>
        /// <param name="numRows">Number of rows</param>
        /// <param name="playerOwner">Whether the player or the computer owns the field</param>
        public BattleshipPanel(int numColumns, int numRows, bool playerOwner)
        {
            // Force it to redraw the field if the size of the panel changes
            this.ResizeRedraw = true;
            /*
            * Use OptimizedDoubleBuffer to make the panel paint in a buffer before outputting it to screen
            * Use UserPaint to make the panel paint itself instead of having the operating system do it
            * Use AllPaintingInWmPaint to make the control ignore the WM_ERASEBKGND window message
            * This reduces flicker when repainting the panel
            */
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.rows = numColumns;
            this.columns = numRows;
            this.playField = new Square[this.rows, this.columns];

            this.SquareHeight = this.ClientSize.Height / this.rows;
            this.SquareWidth = this.ClientSize.Width / this.columns;
            this.IsPlayer = playerOwner;
        }

        /// <summary>
        /// Gets or sets the SquareHeight variable.
        /// </summary>
        public int SquareHeight { get; set; }

        /// <summary>
        /// Gets or sets the SquareWidth variable.
        /// </summary>
        public int SquareWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is the players field.
        /// </summary>
        public bool IsPlayer { get; set; }

        /// <summary>
        /// Method used when manually choosing where to shoot in the grid. 
        /// Returns the type of square that the shot results in.
        /// </summary>
        /// <param name="col">The column the player fires at.</param>
        /// <param name="row">The row the player fires at.</param>
        /// <returns>The Square type that the shot results in.</returns>
        public Square PlayerShoot(int col, int row)
        {
            Square hitType;
            hitType = Player.LaunchAtTarget(this.playField, row, col);

            // Force the panel to redraw the field to show any changes.
            this.Refresh();
            return hitType;
        }

        /// <summary>
        /// Method used when the computer chooses where to shoot in the grid.
        /// Returns the type of square that the shot results in.
        /// </summary>
        /// <returns>The Square type that the shot results in.</returns>
        public Square ComputerShoot()
        {
            int row, col;
            Square hitType = Square.Forbidden;

            if (AI.Shoot(this.playField, out row, out col))
            {
                // The computer found a square it could fire on.
                hitType = Player.LaunchAtTarget(this.playField, row, col);
            }

            // Force the panel to redraw the field to show any changes.
            this.Refresh();
            return hitType;
        }

        /// <summary>
        /// Method for placing ships on the playing field. Returns whether the 
        /// method successfully placed the ship.
        /// </summary>
        /// <param name="col">The column to place the ship.</param>
        /// <param name="row">The row to place the ship.</param>
        /// <param name="shipLength">The length of the ship</param>
        /// <returns>Whether the method successfully placed a ship.</returns>
        public ShipHandleReturn PlayerHandleShip(ref int col, ref int row, int shipLength = 0)
        {
            bool addingShip = false;
            ShipHandleReturn returnValue = ShipHandleReturn.Failed;

            // Check if the player have started placing a ship.
            if (this.isPlacing)
            {
                // The player has chosen an initial spot and is now choosing the orientation of the boat
                if (row == this.shipRow && col == this.shipCol)
                {
                    // Player pressed the same square again to stop the placing
                    this.RemoveArrows();
                    addingShip = true;
                    returnValue = ShipHandleReturn.Failed;
                }
                else if (this.playField[row, col] == Square.ArrowDown)
                {
                    // Remove the arrows and place the ship vertically below the chosen square
                    this.RemoveArrows();
                    this.shipPlacer.SetShip(this.playField, shipLength, Orientation.Vertical, this.shipRow, this.shipCol);
                    addingShip = true;
                    returnValue = ShipHandleReturn.ShipSet;
                    row = shipRow;
                    col = shipCol;
                }
                else if (this.playField[row, col] == Square.ArrowRight)
                {
                    // Remove the arrows and place the ship horizontally to the right of the chosen square
                    this.RemoveArrows();
                    this.shipPlacer.SetShip(this.playField, shipLength, Orientation.Horizontal, this.shipRow, this.shipCol);
                    addingShip = true;
                    returnValue = ShipHandleReturn.ShipSet;
                    row = shipRow;
                    col = shipCol;
                }
                else if (this.playField[row, col] == Square.ArrowUp)
                {
                    // Remove the arrows and place the ship vertically above the chosen square
                    this.RemoveArrows();

                    // The SetShip method can only place towards right or down so call it with coordinates on the top most side of the ship
                    this.shipRow = this.shipRow - shipLength + 1;
                    this.shipPlacer.SetShip(this.playField, shipLength, Orientation.Vertical, this.shipRow, this.shipCol);
                    addingShip = true;
                    returnValue = ShipHandleReturn.ShipSet;
                    row = shipRow;
                    col = shipCol;
                }
                else if (this.playField[row, col] == Square.ArrowLeft)
                {
                    // Remove the arrows and place the ship horizontally to the left of the chosen square
                    this.RemoveArrows();

                    // The SetShip method can only place towards right or down so call it with coordinates on the left most side of the ship
                    this.shipCol = this.shipCol - shipLength + 1;
                    this.shipPlacer.SetShip(this.playField, shipLength, Orientation.Horizontal, this.shipRow, this.shipCol);
                    addingShip = true;
                    returnValue = ShipHandleReturn.ShipSet;
                    row = shipRow;
                    col = shipCol;

                }
            }
            else if (this.playField[row, col] == Square.Ship)
            {
                // The player clicked an already placed ship
                // Find the top-left most square of the selected ship
                
                while (true)
                {
                    if (row > 0 &&
                        this.playField[row - 1, col] == Square.Ship)
                    {
                        row--;
                    }
                    else if (col > 0 &&
                        this.playField[row, col - 1] == Square.Ship)
                    {
                        col--;
                    }
                    else
                    {
                        // Found the top-left most square of the ship so break out of the loop.
                        break;
                    }
                }

                // Remove the chosen ship
                if(this.shipPlacer.RemoveShip(this.playField, row, col))
                {
                    returnValue = ShipHandleReturn.ShipRemoved;
                }
            }
            else if(shipLength != 0)
            {
                // The player is choosing the initial position for the ship, 
                // check for space to the right and below the chosen square
                if (this.shipPlacer.IsSettable(this.playField, shipLength, Orientation.Horizontal, row, col))
                {
                    // The ship fits to the right of the chosen spot
                    this.playField[row, col + 1] = Square.ArrowRight;
                    addingShip = true;
                    returnValue = ShipHandleReturn.PointSet;
                }

                if (this.shipPlacer.IsSettable(this.playField, shipLength, Orientation.Vertical, row, col))
                {
                    // The ship fits below the chosen spot
                    this.playField[row + 1, col] = Square.ArrowDown;
                    addingShip = true;
                    returnValue = ShipHandleReturn.PointSet;
                }

                // ShipPositioner.IsSettable can only check to the right and down so make sure the ship fits
                // with regards to the size of the field and then check if it's settable from the far side of the ship
                if (row - shipLength >= 0 &&
                    this.shipPlacer.IsSettable(this.playField, shipLength, Orientation.Vertical, row - shipLength + 1, col))
                {
                    // The ship fits to the above of the chosen spot.
                    this.playField[row - 1, col] = Square.ArrowUp;
                    addingShip = true;
                    returnValue = ShipHandleReturn.PointSet;
                }

                // ShipPositioner.IsSettable can only check to the right and down so make sure the ship fits
                // with regards to the size of the field and then check if it's settable from the far side of the ship
                if (col - shipLength >= 0 &&
                    this.shipPlacer.IsSettable(this.playField, shipLength, Orientation.Horizontal, row, col - shipLength + 1))
                {
                    // The ship fits to the left of the chosen spot.
                    this.playField[row, col - 1] = Square.ArrowLeft;
                    addingShip = true;
                    returnValue = ShipHandleReturn.PointSet;
                }
            }
            
            if (addingShip && this.isPlacing)
            {
                // An action has been performed and the class should no longer be in placing mode.
                this.isPlacing = false;
            }
            else if (addingShip && !this.isPlacing)
            {
                // An action has been performed and the class is now in placing mode,
                // save the chosen starting position for the ship.
                this.shipRow = row;
                this.shipCol = col;
                this.isPlacing = true;
            }

            // Update the squares where the player can't place a ship
            this.UpdateForbiddenSquares();

            // Force a redraw of the panel to show the changed squares
            this.Refresh();
            return returnValue;
        }

        /// <summary>
        /// Clears all forbidden squares in preparation of starting the game
        /// </summary>
        public void ClearForbiddenSquares()
        {
            this.shipPlacer.RemoveForbiddenSquares(this.playField);
            this.Refresh();
            return;
        }

        /// <summary>
        /// Automatically place ships on the play field using the numbers in 
        /// <paramref name="shipLengths"/> as lengths for the ships.
        /// </summary>
        /// <param name="shipLengths">An array containing the lengths of all the ships.</param>
        public void ComputerPlaceShip(int[] shipLengths)
        {
            this.shipPlacer.AutoPosition(this.playField, shipLengths);
        }

        /// <summary>
        /// Changes the number of rows and columns in the play field.
        /// Resets all fields and values.
        /// </summary>
        /// <param name="numColumns">Number of columns</param>
        /// <param name="numRows">Number of rows</param>
        public void ChangeSize(int numColumns, int numRows)
        {
            this.playField = new Square[numRows, numColumns];
            this.columns = numColumns;
            this.rows = numRows;
            this.isPlacing = false;
            this.SquareHeight = this.ClientSize.Height / this.columns;
            this.SquareWidth = this.ClientSize.Width / this.rows;
        }

        /// <summary>
        /// An event that fires when the panel is painted.
        /// </summary>
        /// <param name="paintEvent">A System.Windows.Form.PaintEventArgs that contain the event data.</param>
        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            int rowPlace, columnPlace;
            
            for (int row = 0; row < this.rows; row++)
            {
                // Calculate where along the rows the image is placed
                rowPlace = row * this.SquareHeight;
                for (int column = 0; column < this.columns; column++)
                {
                    // Calculate where along the columns the image is placed
                    columnPlace = column * this.SquareWidth;

                    // Paint the correct image depending on what kind of square we have.
                    if (this.playField[row, column] == Square.Hit)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + HITIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.Miss)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + MISSIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.Sunk)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + SUNKIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.ArrowRight)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + RIGHTARROWIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.ArrowDown)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + DOWNARROWIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.ArrowLeft)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + LEFTARROWIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.ArrowUp)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + UPARROWIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.playField[row, column] == Square.Forbidden)
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + FORBIDDENIMAGE), columnPlace, rowPlace);
                    }
                    else if (this.IsPlayer && this.playField[row, column] == Square.Ship)
                    {
                        // Only paint the ships if the player owns the field to avoid showing the player where the computer
                        // has placed its ships.
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + SHIPIMAGE), columnPlace, rowPlace);
                    }
                    else
                    {
                        paintEvent.Graphics.DrawImageUnscaled(Image.FromFile(IMAGEPATH + WATERIMAGE), columnPlace, rowPlace);
                    }
                }
            }

            base.OnPaint(paintEvent);
        }

        /// <summary>
        /// An event that fires when the panel is resized.
        /// </summary>
        /// <param name="e">An System.EventArgs that contain the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            this.SquareHeight = this.ClientSize.Height / this.columns;
            this.SquareWidth = this.ClientSize.Width / this.rows;

            base.OnResize(e);
        }

        /// <summary>
        /// Remove all arrows in the playing field
        /// </summary>
        private void RemoveArrows()
        {
            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns; column++)
                {
                    if (this.playField[row, column] == Square.ArrowDown || 
                        this.playField[row, column] == Square.ArrowRight ||
                        this.playField[row, column] == Square.ArrowUp ||
                        this.playField[row, column] == Square.ArrowLeft)
                    {
                        this.playField[row, column] = Square.Water;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the forbidden squares to show the player where 
        /// he can or can't place ships
        /// </summary>
        private void UpdateForbiddenSquares()
        {
            // First remove all forbidden squares then set the correct ones 
            // to avoid having any incorrect forbidden squares
            this.shipPlacer.RemoveForbiddenSquares(this.playField);
            this.shipPlacer.SetForbiddenSquares(this.playField);
            return;
        }
    }
}
