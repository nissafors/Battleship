using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Player
    {
        static Square LaunchAtTarget(Square[,] grid, int row, int col)
        {
        }

        /// <summary>
        /// Test if ship that was hit at row, col is sunk
        /// </summary>
        /// <param name="grid">Gameboard</param>
        /// <param name="row">Hit row</param>
        /// <param name="col">Hit col</param>
        /// <returns>Returns true if the ship was sunk</returns>
        private bool IsSunk(Square[,] grid, int row, int col)
        {
            bool testUp, testDown, testLeft, testRight;

            // In which directions should we test for hits?
            testUp = (row - 1 >= 0 && grid[row - 1, col] == Square.Hit) ? true : false;
            testDown = (row + 1 < grid.GetLength(1) && grid[row + 1, col] == Square.Hit) ? true : false;
            testLeft = (col - 1 >= 0 & grid[row, col - 1] == Square.Hit) ? true : false;
            testRight = (col + 1 < grid.GetLength(0) && grid[row, col + 1] == Square.Hit) ? true : false;
            
            // Loop through array and test for hits?
            int greatestDimension = (grid.GetLength(0) > grid.GetLength(1)) ? grid.GetLength(0) : grid.GetLength(1);
            for (int i = 1; i < greatestDimension; i++)
            {
                if (testUp && row - i >= 0 && grid[row - i, col] == Square.Hit) {
                    testUp = true;
                }
                else
                {
                    if (row - i >= 0 && grid[row - i, col] == Square.Ship)
                    {
                        return false;
                    }
                    testUp = false;
                }

                if (testUp && row + i < grid.GetLength(1) && grid[row + i, col] == Square.Hit)
                {
                    testUp = true;
                }
                else
                {
                    if (row + i < grid.GetLength(1) && grid[row + i, col] == Square.Ship)
                    {
                        return false;
                    }
                    testUp = false;
                }

                if (testUp && col - i >= 0 && grid[row, col - i] == Square.Hit)
                {
                    testUp = true;
                }
                else
                {
                    if (col - i >= 0 && grid[row, col - i] == Square.Ship)
                    {
                        return false;
                    }
                    testUp = false;
                }

                if (testUp && col + i < grid.GetLength(0) && grid[row, col + i] == Square.Hit)
                {
                    testUp = true;
                }
                else
                {
                    if (col + i < grid.GetLength(0) && grid[row, col + i] == Square.Ship)
                    {
                        return false;
                    }
                    testUp = false;
                }

                if (testUp == testDown == testLeft == testRight == false)
                {
                    break;
                }
            }

            return true;
        }
    }
}
