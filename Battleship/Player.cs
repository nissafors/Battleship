//-----------------------------------------------------
// <copyright file="Player.cs" company="none">
//      Copyright (c) Torbjörn Widström & Andreas Andersson 2014
// </copyright>
// <author>Torbjörn Widström & Andreas Andersson</author>
//-----------------------------------------------------

namespace Battleship
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// This class provides methods for shooting at given coordinate on the game board and get
    /// the result of the shot.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Tests if shot is a hit, miss, sunk or forbidden.
        /// </summary>
        /// <param name="grid">Game board.</param>
        /// <param name="row">Hit row.</param>
        /// <param name="col">Hit col.</param>
        /// <returns>Return if shot is a hit, miss, sunk or forbidden.</returns>
        public static Square LaunchAtTarget(Square[,] grid, int row, int col)
        {   
            // Tests if shot is forbidden. 
            if (grid[row, col] == Square.Hit ||
                grid[row, col] == Square.Miss ||
                grid[row, col] == Square.Sunk)
            {
                return Square.Forbidden;
            }
            else if (grid[row, col] == Square.Ship)
            {
                grid[row, col] = Square.Hit;
                
                // Call the method IsSunk.
                IsSunk(grid, row, col);
                return Square.Sunk;
            }
            else
            {
                return Square.Miss;
            }
        }

        /// <summary>
        /// Test if ship that was hit at row, col is sunk.
        /// </summary>
        /// <param name="grid">Game board.</param>
        /// <param name="row">Hit row.</param>
        /// <param name="col">Hit col.</param>
        /// <returns>Returns true if the ship was sunk.</returns>
        private static bool IsSunk(Square[,] grid, int row, int col)
        {
            int newRow, newCol;
            bool[] testDir = { true, true, true, true };
            List<Point> hitList = new List<Point>();

            // Given coordinates should be a hit. Store them.
            hitList.Add(new Point(col, row));

            // Max limit of the loop is the largest dimension of grid[,]
            int arrayLen = (grid.GetLength(0) > grid.GetLength(1)) ? grid.GetLength(0) : grid.GetLength(1);

            // Start at one position away from (row, col) and step further away at each increment of i
            for (int i = 1; i < arrayLen; i++)
            {
                // Loop through four directions: 0 = up, 1 = down, 2 = left and 3 = right
                for (int dir = 0; dir < 4; dir++)
                {
                    // Set row and column to test
                    newRow = (dir == 0 && testDir[0]) ? row - i : row;
                    newRow = (dir == 1 && testDir[1]) ? row + i : newRow;
                    newCol = (dir == 2 && testDir[2]) ? col - i : col;
                    newCol = (dir == 3 && testDir[3]) ? col + i : newCol;

                    if (testDir[dir])
                    {
                        // If we got this far, the Square at (newRow, newCol) is next to a Square.Hit. If
                        // that Square is also a Square.Hit, continue to investigate this direction,
                        // otherwise not. If it is a Square.Ship, obviously not all the squares of that
                        // ship has been hit and it's therefore not sunk: return false.
                        switch (GetSquare(grid, newRow, newCol))
                        {
                            case Square.Hit:
                                testDir[dir] = true;
                                // Save coordinates so we can easily rewrite the grid if the ship is sunk
                                hitList.Add(new Point(newCol, newRow));
                                break;
                            case Square.Ship:
                                return false;
                            default:
                                testDir[dir] = false;
                                break;
                        }
                    }
                }

                // If no directions are to be tested next loop, then break.
                if (testDir[0] == false &&
                    testDir[1] == false &&
                    testDir[2] == false &&
                    testDir[3] == false)
                {
                    break;
                }
            }

            // If no Square.Ship was found next to any Square.Hit on this ship, it was sunk.
            // Rewrite grid and return true
            foreach (Point hit in hitList)
            {
                grid[hit.Y, hit.X] = Square.Sunk;
            }
            return true;
        }

        /// <summary>
        /// Check which Square value is at grid[row, col]. Also test if we're in the range of grid.
        /// </summary>
        /// <param name="grid">Game board.</param>
        /// <param name="row">Grid row.</param>
        /// <param name="col">Grid column.</param>
        /// <returns>Returns the Square value at grid[row, col] or Square.Forbidden if out of bounds.</returns>
        private static Square GetSquare(Square[,] grid, int row, int col)
        {
            if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1))
            {
                // We're outside the grid
                return Square.Forbidden;
            }

            return grid[row, col];
        }
    }
}
