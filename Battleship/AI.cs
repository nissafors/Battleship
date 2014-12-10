//-----------------------------------------------------------------------
// <copyright file="AI.cs" company="None">
//     Copyright (c) Henrik Ottehall 2014
// </copyright>
// <author>Henrik Ottehall</author>
//-----------------------------------------------------------------------

namespace Battleship
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class for handling where the AI shoots.
    /// </summary>
    public class AI
    {
        /// <summary>
        /// Chooses which square the AI in the Battleships game will shoot at.
        /// <para>The coordinates will be returned in <paramref name="rowCoord"/> and <paramref name="colCoord"/></para>
        /// Returns true if valid coordinates could be found, otherwise returns false
        /// </summary>
        /// <param name="playFieldArray">The array containing the playing field.</param>
        /// <param name="rowCoord">The row that the AI fires at.</param>
        /// <param name="colCoord">The column that the AI fires at.</param>
        /// <returns>True if valid coordinates could be found otherwise false.</returns>
        public static bool Shoot(Square[,] playFieldArray, out int rowCoord, out int colCoord)
        {
            const int Neighbour = 1;
            int rowMin = 0, rowMax = playFieldArray.GetLength(0) - 1, colMin = 0, colMax = playFieldArray.GetLength(1) - 1;
            Random randomizeCoord = new Random();
            int chosenRandom;
            bool foundHit = false;
            List<Tuple<int, int>> allowedCoordinates = new List<Tuple<int, int>>(); // List to hold possible coordinates

            // Loop through entire array with nested loop
            for (int row = 0; row <= rowMax && !foundHit; row++)
            {
                for (int column = 0; column <= colMax && !foundHit; column++)
                {
                    if (playFieldArray[row, column] == Square.Hit)
                    {
                        // Found a hit but not sunk ship, clear allowedCoordinates to fire only around that hit
                        foundHit = true;
                        allowedCoordinates.Clear();

                        if (row < rowMax &&
                            playFieldArray[row + Neighbour, column] == Square.Hit)
                        {
                            // There is a hit below the the found hit, add the square above the 
                            // found hit unless we are at the edge or there is a miss
                            if (row != rowMin &&
                                playFieldArray[row - Neighbour, column] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - Neighbour, column));
                            }

                            // Find the last of hits on the ship below the found hit
                            while (row < rowMax &&
                                playFieldArray[row, column] == Square.Hit)
                            {
                                row++;
                            }

                            // Add the square below the last hit unless there is a hit or miss already
                            if (playFieldArray[row, column] != Square.Miss &&
                                playFieldArray[row, column] != Square.Hit)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, column));
                            }
                        }
                        else if (column < colMax &&
                        playFieldArray[row, column + Neighbour] == Square.Hit)
                        {
                            // There is a another hit to the right of the found hit, add the square 
                            // left of the hit unless we are at the edge or there is a miss
                            if (column != colMin &&
                                playFieldArray[row, column - Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, column - Neighbour));
                            }

                            // Find the last of the hits to the right of the found hit
                            while (column < colMax &&
                                playFieldArray[row, column] == Square.Hit)
                            {
                                column++;
                            }

                            // Add the square to the right of the last hit unless there is a hit or miss already
                            if (playFieldArray[row, column] != Square.Miss &&
                                playFieldArray[row, column] != Square.Hit)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, column));
                            }
                        }
                        else
                        {
                            // No other hits were found.
                            // Make sure we are not at the edge in each direction and
                            // that we haven't tried to fire there but missed
                            if (row != rowMin &&
                                playFieldArray[row - Neighbour, column] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - Neighbour, column));
                            }

                            if (column != colMin &&
                                playFieldArray[row, column - Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, column - Neighbour));
                            }

                            if (row < rowMax &&
                                playFieldArray[row + Neighbour, column] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row + Neighbour, column));
                            }

                            if (column < colMax &&
                                playFieldArray[row, column + Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, column + Neighbour));
                            }
                        }
                    }
                    else if (playFieldArray[row, column] == Square.Water || 
                            playFieldArray[row, column] == Square.Ship)
                    {
                        // The current square is water or ship.
                        // Check the squares around the current square for sunk ships since
                        // ships can't be next to each other.
                        if (row != rowMin && 
                            column != colMin && 
                            playFieldArray[row - Neighbour, column - Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship above and left of the current square
                        }
                        else if (column != colMin &&
                                playFieldArray[row, column - Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship to the left the current square
                        }
                        else if (row < rowMax && 
                                column != colMin && 
                                playFieldArray[row + Neighbour, column - Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship below and left of the current square
                        }
                        else if (row != rowMin &&
                                playFieldArray[row - Neighbour, column] == Square.Sunk)
                        {
                            // There is a sunk ship above the current square
                        }
                        else if (row < rowMax &&
                                playFieldArray[row + Neighbour, column] == Square.Sunk)
                        {
                            // There is a sunk ship below the current square
                        }
                        else if (row != rowMin &&
                                column < colMax &&
                                playFieldArray[row - Neighbour, column + Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship above and right of the current square
                        }
                        else if (column < colMax &&
                            playFieldArray[row, column + Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship right the current square
                        }
                        else if (row < rowMax &&
                                column < colMax &&
                                playFieldArray[row + Neighbour, column + Neighbour] == Square.Sunk)
                        {
                            // There is a sunk ship below and right of the current square
                        }
                        else
                        {
                            // If it is a square with water or ship with no sunk ship next to it
                            // then add it to the allowed coordinates
                            allowedCoordinates.Add(Tuple.Create(row, column));
                        }
                    }
                }
            }

            if (allowedCoordinates.Count != 0)
            {
                // Randomly choose one of the allowed coordinates
                // and return the chosen coordinates
                chosenRandom = randomizeCoord.Next(allowedCoordinates.Count);
                rowCoord = allowedCoordinates[chosenRandom].Item1;
                colCoord = allowedCoordinates[chosenRandom].Item2;
                return true;
            }
            else
            {
                // There are no coordinates that we can fire at
                rowCoord = -1;
                colCoord = -1;
                return false;
            }
        }
    }
}
