//-----------------------------------------------------
// <copyright file="ShipPositioner.cs" company="none">
//    Copyright (c) Andreas Andersson 2014
// </copyright>
// <author>Andreas Andersson</author>
//-----------------------------------------------------

namespace Battleship
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Use an instance of this class to 1) check if grid is large enough for ships with
    /// GridTooSmall(), 2) manually add ships to the grid with setShip() and 3) automatically
    /// place ships to the grid with AutoPosition().
    /// </summary>
    public class ShipPositioner
    {
        /// <summary>
        /// Internal store for all free water. Should be null-ed after each use.</summary>
        private List<Channel> channels;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipPositioner" /> class.</summary>
        public ShipPositioner()
        {
            this.channels = null;
        }

        /// <summary>
        /// Automatically add ships to the playing field.
        /// <para>Before attempting to add any ships, AutoPosition() sets all squares in
        /// <paramref name="grid"/> to Square.Water!</para>
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        /// <param name="shipLengths">One length for each ship to add.</param>
        /// <exception cref="Battleship.InsufficientGridSpaceException">This exception should
        /// always be caught, as running GridTooSmall() beforehand is no guarantee for success!
        /// </exception>
        public void AutoPosition(Square[,] grid, int[] shipLengths)
        {
            const int MAXTRIES = 1000;
            List<Channel> channelsLongEnough = new List<Channel>();
            Random rnd = new Random();
            int channel, startPos, row, col, resetCount = 0;

            this.ResetGrid(grid);
            for (int i = 0; i < shipLengths.Length; i++)
            {
                // Find long enough channels and add those to a list
                this.ListChannels(grid);
                channelsLongEnough = this.channels.FindAll(c => c.Length >= shipLengths[i]);

                // If no long enough channels were found: Reset grid and try again. Give
                // up and throw exception after MAXTRIES times.
                if (channelsLongEnough.Count == 0)
                {
                    this.ResetGrid(grid);
                    this.channels = null;
                    i = -1;
                    resetCount++;
                    if (resetCount > MAXTRIES)
                    {
                        throw new InsufficientGridSpaceException();
                    }

                    continue;
                }

                // Randomize which channel to use and how far into it to place the ship.
                channel = rnd.Next(0, channelsLongEnough.Count);
                startPos = rnd.Next(0, channelsLongEnough[channel].Length - shipLengths[i] + 1);

                // Find out the orientation and set start row and start column
                if (channelsLongEnough[channel].Orientation == Orientation.Horizontal)
                {
                    row = channelsLongEnough[channel].StartRow;
                    col = channelsLongEnough[channel].StartCol + startPos;
                }
                else
                {
                    row = channelsLongEnough[channel].StartRow + startPos;
                    col = channelsLongEnough[channel].StartCol;
                }

                // Add the ship to the grid
                this.SetShip(grid, shipLengths[i], channelsLongEnough[channel].Orientation, row, col);
            }
        }

        /// <summary>
        /// Add a ship to the playing field at given coordinates.</summary>
        /// <param name="grid">The playing field grid.</param>
        /// <param name="shipLength">The length of the ship.</param>
        /// <param name="orientation">The orientation of the ship.</param>
        /// <param name="row">Least significant row.</param>
        /// <param name="col">Least significant column.</param>
        /// <returns>Returns false if ship is too close to another ship or off the grid.</returns>
        public bool SetShip(Square[,] grid, int shipLength, Orientation orientation, int row, int col)
        {
            // Make sure we know our channels
            if (this.channels == null)
            {
                this.ListChannels(grid);
            }

            // Is there a channel were caller wants to place the ship?
            bool ok = false;
            foreach (Channel c in this.channels)
            {
                switch (c.Orientation)
                {
                    case Orientation.Vertical:
                        if (col == c.StartCol &&
                            row >= c.StartRow &&
                            row + shipLength <= c.StartRow + c.Length &&
                            orientation == Orientation.Vertical)
                        {
                            ok = true;
                        }

                        break;
                    case Orientation.Horizontal:
                        if (row == c.StartRow &&
                            col >= c.StartCol &&
                            col + shipLength <= c.StartCol + c.Length &&
                            orientation == Orientation.Horizontal)
                        {
                            ok = true;
                        }

                        break;
                }

                if (ok)
                {
                    break;
                }
            }

            // If not: Ship cannot be set!
            if (!ok)
            {
                return false;
            }

            // Write ship to grid.
            for (int i = 0; i < shipLength; i++)
            {
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        grid[row, col + i] = Square.Ship;
                        break;
                    case Orientation.Vertical:
                        grid[row + i, col] = Square.Ship;
                        break;
                    default:
                        return false;
                }
            }

            // Nullify channels
            this.channels = null;
            return true;
        }

        /// <summary>
        /// Remove a ship from the playing field at given coordinates.</summary>
        /// <param name="grid">The playing field grid.</param>
        /// <param name="row">Least significant row.</param>
        /// <param name="col">Least significant column.</param>
        /// <returns>Returns true if a ship was found and removed successfully.</returns>
        public bool RemoveShip(Square[,] grid, int row, int col)
        {
            // If this is not the uppermost, leftmost (row, col) on the ship, 
            // or if this is not a ship at all, return false.
            if ((grid[row, col] != Square.Ship) ||
                ((col - 1 >= 0 && row - 1 >= 0) &&
                (grid[row - 1, col] == Square.Ship || grid[row, col - 1] == Square.Ship)))
            {
                return false;
            }

            // Remove adjacent ship squares.
            if (col + 1 < grid.GetLength(0) && grid[row, col + 1] == Square.Ship)
            {
                // This is a ship with horizontal orientation
                for (int c = col; c < grid.GetLength(1); c++)
                {
                    if (grid[row, c] == Square.Ship)
                    {
                        grid[row, c] = Square.Water;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                // This is a ship with vertical orientation
                for (int r = row; r < grid.GetLength(0); r++)
                {
                    if (grid[r, col] == Square.Ship)
                    {
                        grid[r, col] = Square.Water;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if playing field is large enough for all ships.
        /// <para>
        /// If very few possible ship combinations exists on a large grid, GridTooSmall() may
        /// sometimes report true even if the ships can actually fit in the grid. If GridTooSmall()
        /// reports false, however, rest assured that a valid ship configuration exists.</para>
        /// <para>
        /// GridTooSmall() will set all squares in <paramref name="grid"/> to Square.Water!</para>
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        /// <param name="shipLengths">One length for each ship to add.</param>
        /// <returns>Returns true if the grid is likely to be too small to fit all ships.</returns>
        public bool GridTooSmall(Square[,] grid, int[] shipLengths)
        {
            try
            {
                // If AutoPosition() succeeds, there is definitely at least four ways to
                // place the ships.
                this.AutoPosition(grid, shipLengths);
                this.ResetGrid(grid);
                return false;
            }
            catch (InsufficientGridSpaceException)
            {
                // If AutoPosition() fails, the grid is probably too small.
                this.ResetGrid(grid);
                return true;
            }
        }

        /// <summary>
        /// Set all squares in grid to Square.Water.
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        private void ResetGrid(Square[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = Square.Water;
                }
            }
        }

        /// <summary>
        /// Set squares surrounding ships to Square.Forbidden.
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        private void SetForbiddenSquares(Square[,] grid)
        {
            // Work on a framed grid
            Square[,] tempGrid = new Square[grid.GetLength(0) + 2, grid.GetLength(1) + 2];
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    tempGrid[row + 1, col + 1] = grid[row, col];
                }
            }

            // Write Square.Forbidden on all squares surrounding a Square.Ship,
            // except where the bordering square is also a Square.Ship.
            for (int row = 0; row < tempGrid.GetLength(0); row++)
            {
                for (int col = 0; col < tempGrid.GetLength(1); col++)
                {
                    if (tempGrid[row, col] == Square.Ship)
                    {
                        tempGrid[row - 1, col - 1] = (tempGrid[row - 1, col - 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row - 1, col] = (tempGrid[row - 1, col] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row - 1, col + 1] = (tempGrid[row - 1, col + 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row, col - 1] = (tempGrid[row, col - 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row, col + 1] = (tempGrid[row, col + 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row + 1, col - 1] = (tempGrid[row + 1, col - 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row + 1, col] = (tempGrid[row + 1, col] == Square.Ship) ? Square.Ship : Square.Forbidden;
                        tempGrid[row + 1, col + 1] = (tempGrid[row + 1, col + 1] == Square.Ship) ? Square.Ship : Square.Forbidden;
                    }
                }
            }

            // Write back to original grid
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = tempGrid[row + 1, col + 1];
                }
            }
        }

        /// <summary>
        /// Change all Square.Forbidden in <paramref name="grid"/> to Square.Water.
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        private void RemoveForbiddenSquares(Square[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == Square.Forbidden)
                    {
                        grid[row, col] = Square.Water;
                    }
                }
            }
        }

        /// <summary>
        /// Find all contiguous Square.Water's and write them to the channels field.
        /// </summary>
        /// <param name="grid">The playing field grid.</param>
        private void ListChannels(Square[,] grid)
        {
            this.channels = new List<Channel>();
            Square[] squareArray;
            Orientation or;

            // Find all squares adjacent to a ship and mark them
            this.SetForbiddenSquares(grid);

            for (int dim = 0; dim < 2; dim++)
            {
                for (int j = 0; j < grid.GetLength(dim); j++)
                {
                    squareArray = this.ExtractDimension(grid, dim, j);
                    or = (dim == 0) ? Orientation.Horizontal : Orientation.Vertical;
                    this.channels.AddRange(this.ChannelsFromArray(squareArray, or, j));
                }
            }

            // Remove marks from grid
            this.RemoveForbiddenSquares(grid);
        }

        /// <summary>
        /// Extract a row or a column from a 2-dimensional array.
        /// </summary>
        /// <param name="grid">The 2-dimensional array.</param>
        /// <param name="dim">The dimension to extract: 0 for rows and 1 for columns.</param>
        /// <param name="pos">The number of the row or column to extract.</param>
        /// <returns>Returns a row or a column as a one-dimensional array.</returns>
        private Square[] ExtractDimension(Square[,] grid, int dim, int pos)
        {
            Square[] squareArray = new Square[grid.GetLength(Math.Abs(dim - 1))];
            for (int i = 0; i < squareArray.Length; i++)
            {
                squareArray[i] = (dim == 0) ? grid[pos, i] : grid[i, pos];
            }

            return squareArray;
        }

        /// <summary>
        /// Find and report all contiguous Square.Water's.</summary>
        /// <param name="array">The array to search.</param>
        /// <param name="orientation">
        /// Indicates whether <paramref name="array"/> is a
        /// row or a column.</param>
        /// <param name="pos">Which row or column we are working with.</param>
        /// <returns>Returns a Channel List.</returns>
        private List<Channel> ChannelsFromArray(Square[] array, Orientation orientation, int pos)
        {
            bool newFlag = true;
            int length = 0, startPos = 0;
            Channel channel = new Channel();
            List<Channel> channels = new List<Channel>();

            for (int i = 0; i <= array.Length; i++)
            {
                if (i < array.Length && array[i] == Square.Water)
                {
                    if (newFlag == true)
                    {
                        startPos = i;
                        newFlag = false;
                    }

                    length++;
                }
                else
                {
                    if (newFlag == true)
                    {
                        continue;
                    }
                    else
                    {
                        if (orientation == Orientation.Horizontal)
                        {
                            channel.StartRow = pos;
                            channel.StartCol = startPos;
                        }
                        else
                        {
                            channel.StartRow = startPos;
                            channel.StartCol = pos;
                        }

                        channel.Orientation = orientation;
                        channel.Length = length;
                        channels.Add(channel);
                        length = 0;
                        newFlag = true;
                    }
                }
            }

            return channels;
        }

        /// <summary>
        /// Describes contiguous Square.Water's in the playing field grid.
        /// </summary>
        private struct Channel
        {
            /// <summary>The orientation of the channel.</summary>
            public Orientation Orientation;

            /// <summary>Start row within the grid.</summary>
            public int StartRow;

            /// <summary>Start column within the grid.</summary>
            public int StartCol;

            /// <summary>The length of the channel.</summary>
            public int Length;
        }
    }
}
