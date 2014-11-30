//-----------------------------------------------------------------------
// <copyright file="BattleshipPanel.cs" company="None">
//     Copyright (c) Henrik Ottehall 2014
// </copyright>
// <author>Henrik Ottehall</author>
//-----------------------------------------------------------------------

namespace Battleship
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Class for drawing the playing field and handling events in it.
    /// </summary>
    class BattleshipPanel : Panel
    {
        private int columns = 10, rows = 10, squareHeight, squareWidth;
        private bool isPlayer = true;
        private Square[,] playField;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipPanel"/> class.
        /// Defaults to a 10x10 grid.
        /// </summary>
        public BattleshipPanel()
        {
            this.ResizeRedraw = true;
            
            playField = new Square[columns, rows];

            squareHeight = this.ClientSize.Height / rows;
            squareWidth = this.ClientSize.Width / columns;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipPanel"/> class.
        /// This constructor that can be used to load a preset playing field
        /// </summary>
        /// <param name="fieldArray">An array of Squares.</param>
        /// <param name="playerOwner">Whether the player or the computer owns the field</param>
        public BattleshipPanel(Square[,] fieldArray, bool playerOwner)
        {
            this.ResizeRedraw = true;

            isPlayer = playerOwner;
            columns = fieldArray.GetLength(0);
            rows = fieldArray.GetLength(1);
            playField = fieldArray;

            squareHeight = this.ClientSize.Height / rows;
            squareWidth = this.ClientSize.Width / columns;
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
            this.ResizeRedraw = true;

            isPlayer = playerOwner;
            columns = numColumns;
            rows = numRows;
            playField = new Square[columns, rows];

            squareHeight = this.ClientSize.Height / rows;
            squareWidth = this.ClientSize.Width / columns;
        }

        /// <summary>
        /// Gets or sets the SquareHeight variable.
        /// </summary>
        public int SquareHeight
        {
            get { return squareHeight; }
            set { squareHeight = value; }
        }

        /// <summary>
        /// Gets or sets the SquareWidth variable.
        /// </summary>
        public int SquareWidth
        {
            get { return squareWidth; }
            set { squareWidth = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the player owns the field.
        /// </summary>
        public bool IsPlayer
        {
            get { return isPlayer; }
            set { isPlayer = value; }
        }

        /// <summary>
        /// An event that fires when the panel is painted.
        /// </summary>
        /// <param name="e">A System.Windows.Form.PaintEventArgs that contain the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.SuspendLayout();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (playField[column, row] == Square.Hit)
                    {
                        e.Graphics.FillRectangle(Brushes.Red, column * squareWidth, row * squareHeight, squareWidth, squareHeight);
                    }
                    else if (playField[column, row] == Square.Miss)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, column * squareWidth, row * squareHeight, squareWidth, squareHeight);
                    }
                    else if (isPlayer && playField[column, row] == Square.Ship)
                    {
                        e.Graphics.FillRectangle(Brushes.White, column * squareWidth, row * squareHeight, squareWidth, squareHeight);
                    }
                    else if (playField[column, row] == Square.Sunk)
                    {
                        e.Graphics.FillRectangle(Brushes.DarkRed, column * squareWidth, row * squareHeight, squareWidth, squareHeight);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Blue, column * squareWidth, row * squareHeight, squareWidth, squareHeight);
                    }
                }
            }

            this.ResumeLayout();
            base.OnPaint(e);
        }

        /// <summary>
        /// An event that fires when the panel is resized.
        /// </summary>
        /// <param name="e">An System.EventArgs that contain the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            squareHeight = this.ClientSize.Height / rows;
            squareWidth = this.ClientSize.Width / columns;

            base.OnResize(e);
        }

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
            hitType = Player.LaunchAtTarget(playField, row, col);
            if (hitType != Square.Forbidden)
            {
                playField[col, row] = hitType;
                this.Refresh();
            }

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

            if (AI.Shoot(playField, out row, out col))
            {
                playField[col, row] = Player.LaunchAtTarget(playField, row, col);
                this.Refresh();
                return playField[col, row];
            }
            else
            {
                // The computer could not find a square to fire at.
                return Square.Forbidden;
            }
        }

        /// <summary>
        /// Method for placing ships on the playing field.
        /// </summary>
        /// <param name="col">The column to place the ship.</param>
        /// <param name="row">The row to place the ship.</param>
        /// <returns>Whether the method succesfully placed a ship.</returns>
        public bool placeShip(int col, int row)
        {
            // TODO: Implement this class.
            return false;
        }
    }
}
