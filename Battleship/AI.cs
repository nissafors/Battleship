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
            const int ArrayBuffer = 1, Neighbour = 1;
            int rowMin = 1, rowMax = playFieldArray.GetLength(0), colMin = 1, colMax = playFieldArray.GetLength(1);
            Random randomizeCoord = new Random();
            int chosenRandom;
            bool foundHit = false;
            List<Tuple<int, int>> allowedCoordinates = new List<Tuple<int, int>>(); // Array or list to hold possible 

            // Add a buffer around the playFieldArray to avoid having to check for edges
            // Do this by making a new array with an extra row and column on each side.
            Square[,] bufferedSquareArray = new Square[rowMax + (2 * ArrayBuffer), colMax + (2 * ArrayBuffer)];
            for (int row = 0; row < (rowMax - ArrayBuffer); row++)
            {
                for (int col = 0; col < colMax - ArrayBuffer; col++)
                {
                    bufferedSquareArray[row + ArrayBuffer, col + ArrayBuffer] = playFieldArray[row, col];
                }
            }

            // Loop through entire array with nested loop
            for (int row = rowMin; row < rowMax && !foundHit; row++)
            {
                for (int col = colMin; col < colMax && !foundHit; col++)
                {
                    if (bufferedSquareArray[row, col] == Square.Hit)
                    {
                        // Found a hit but not sunk ship, clear allowedCoordinates to fire only around that hit
                        foundHit = true;
                        allowedCoordinates.Clear();

                        // Check for other hits along each dimension
                        if (bufferedSquareArray[row + Neighbour, col] == Square.Hit)
                        {
                            // Found a hit along the rows
                            // Add square to the left unless we are at the edge or there is a miss
                            if (row > rowMin && 
                                bufferedSquareArray[row - Neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - Neighbour, col));
                            }

                            // Find end of hits on the boat along the first dimension
                            while (bufferedSquareArray[row, col] == Square.Hit)
                            {
                                row++;
                            }

                            // Add the square right of the last hit unless we are at the edge or there is a miss
                            if (row < rowMax &&
                                (bufferedSquareArray[row, col] == Square.Water ||
                                bufferedSquareArray[row, col] == Square.Ship))
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col));
                            }

                            foundHit = true;
                        }
                        else if (bufferedSquareArray[row, col + Neighbour] == Square.Hit)
                        {
                            // Found a hit along the columns
                            // Add square above the hit unless we are at the edge or there is a miss
                            if (col > colMin && 
                                bufferedSquareArray[row, col - Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col - Neighbour));
                            }

                            // Find end of hits on the boat along the second dimension
                            while (bufferedSquareArray[row, col] == Square.Hit)
                            {
                                col++;
                            }

                            // Add the square below the last hit unless we are at the edge or there is a miss
                            if (col < colMax && 
                                (bufferedSquareArray[row, col] == Square.Water ||
                                bufferedSquareArray[row, col] == Square.Ship))
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col));
                            }

                            foundHit = true;
                        }
                        else
                        {
                            // No other hits were found.
                            // Make sure that we aren't at the edge and haven't tried 
                            // to fire in each direction but missed
                            if (row > rowMin &&
                                bufferedSquareArray[row - Neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - Neighbour, col));
                            }

                            if (col > colMin &&
                                bufferedSquareArray[row, col - Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col - Neighbour));
                            }

                            if (row < rowMax &&
                                bufferedSquareArray[row + Neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row + Neighbour, col));
                            }

                            if (col < colMax &&
                                bufferedSquareArray[row, col + Neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col + Neighbour));
                            }
                        }
                    }
                    else if (!foundHit && (bufferedSquareArray[row, col] == Square.Water || bufferedSquareArray[row, col] == Square.Ship))
                    {
                        // We haven't already found a hit and the current square is water or ship.
                        // Check the eight squares around the current square for sunk ships since 
                        // ships can't be next to each other.
                        if (bufferedSquareArray[row - Neighbour, col - Neighbour] != Square.Sunk &&
                            bufferedSquareArray[row, col - Neighbour] != Square.Sunk &&
                            bufferedSquareArray[row + Neighbour, col - Neighbour] != Square.Sunk &&
                            bufferedSquareArray[row - Neighbour, col] != Square.Sunk &&
                            bufferedSquareArray[row + Neighbour, col] != Square.Sunk &&
                            bufferedSquareArray[row - Neighbour, col + Neighbour] != Square.Sunk &&
                            bufferedSquareArray[row, col + Neighbour] != Square.Sunk &&
                            bufferedSquareArray[row + Neighbour, col + Neighbour] != Square.Sunk)
                        {
                            // If it is a square with water or ship with no sunk ship next to it
                            // then add it to the allowed coordinates
                            allowedCoordinates.Add(Tuple.Create(row, col));
                        }
                    }
                }
            }

            if (allowedCoordinates.Count != 0)
            {
                // Randomly choose one of the allowed coordinates
                // Remove the buffer from the coordinates so they point correctly on the 
                // non-buffered array
                // Then return the chosen coordinates
                chosenRandom = randomizeCoord.Next(allowedCoordinates.Count);
                rowCoord = allowedCoordinates[chosenRandom].Item1 - ArrayBuffer;
                colCoord = allowedCoordinates[chosenRandom].Item2 - ArrayBuffer;
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
