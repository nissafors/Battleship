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
            const int arrayBuffer = 1, neighbour = 1;
            int rowMin = 1, rowMax = playFieldArray.GetLength(0) + 1, colMin = 1, colMax = playFieldArray.GetLength(1) + 1;
            Random randomizeCoord = new Random();
            int chosenRandom;
            bool foundHit = false;
            List<Tuple<int, int>> allowedCoordinates = new List<Tuple<int, int>>(); // Array or list to hold possible 

            // Add a buffer around the playFieldArray to avoid having to check for edges
            // Do this by making a new array with an extra row and column on each side.
            Square[,] bufferedSquareArray = new Square[rowMax + 2 * arrayBuffer, colMax + 2 * arrayBuffer];
            for (int row = 0; row < rowMax - arrayBuffer; row++)
            {
                for (int col = 0; col < colMax - arrayBuffer; col++)
                {
                    bufferedSquareArray[row + arrayBuffer, col + arrayBuffer] = playFieldArray[row, col];
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

                        // Check for other hits along 1st dimension
                        if (bufferedSquareArray[row + neighbour, col] == Square.Hit)
                        {
                            // Add square to the left unless we are at the edge or there is a miss
                            if (bufferedSquareArray[row - neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - neighbour, col));
                            }

                            // Find end of hits on the boat along the first dimension
                            while (bufferedSquareArray[row, col] == Square.Hit)
                            {
                                row++;
                            }

                            // Add the square right of the last hit unless we are at the edge or there is a miss
                            if (bufferedSquareArray[row, col] == Square.Water ||
                                bufferedSquareArray[row, col] == Square.Ship)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col));
                            }

                            foundHit = true;
                        }

                        // Check for other hits along 2nd dimension
                        else if (bufferedSquareArray[row, col + neighbour] == Square.Hit)
                        {
                            // Add square above the hit unless we are at the edge or there is a miss
                            if (bufferedSquareArray[row, col - neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col - neighbour));
                            }

                            // Find end of hits on the boat along the seond dimension
                            while (bufferedSquareArray[row, col] == Square.Hit)
                            {
                                col++;
                            }

                            // Add the square below the last hit unless we are at the edge or there is a miss
                            if (bufferedSquareArray[row, col] == Square.Water ||
                                bufferedSquareArray[row, col] == Square.Ship)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col));
                            }

                            foundHit = true;
                        }

                        else
                        {
                            // No other hits were found.
                            // Make sure that we haven't tried to fire in each direction but missed
                            if (bufferedSquareArray[row - neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row - neighbour, col));
                            }

                            if (bufferedSquareArray[row, col - neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col - neighbour));
                            }

                            if (bufferedSquareArray[row + neighbour, col] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row + neighbour, col));
                            }

                            if (bufferedSquareArray[row, col + neighbour] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(row, col + neighbour));
                            }
                        }
                    }
                    else if (!foundHit && (bufferedSquareArray[row, col] == Square.Water || bufferedSquareArray[row, col] == Square.Ship))
                    {
                        // We haven't already found a hit and the current square is water or ship.
                        // Check the eight squares around the current square for sunk ships since 
                        // ships can't be next to each other.
                        if (bufferedSquareArray[row - neighbour, col - neighbour] != Square.Sunk &&
                            bufferedSquareArray[row, col - neighbour] != Square.Sunk &&
                            bufferedSquareArray[row + neighbour, col - neighbour] != Square.Sunk &&
                            bufferedSquareArray[row - neighbour, col] != Square.Sunk &&
                            bufferedSquareArray[row + neighbour, col] != Square.Sunk &&
                            bufferedSquareArray[row - neighbour, col + neighbour] != Square.Sunk &&
                            bufferedSquareArray[row, col + neighbour] != Square.Sunk &&
                            bufferedSquareArray[row + neighbour, col + neighbour] != Square.Sunk)
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
                rowCoord = allowedCoordinates[chosenRandom].Item1 - arrayBuffer;
                colCoord = allowedCoordinates[chosenRandom].Item2 - arrayBuffer;
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
