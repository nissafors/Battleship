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
        /// Returns the coordinates as two integers. Returns -1 if there are no viable 
        /// coordinates to fire at.
        /// </summary>
        /// <param name="playFieldArray">The array containing the playing field.</param>
        /// <param name="firstCoord">The first dimension coordinate that the AI fires at.</param>
        /// <param name="secondCoord">The second dimension coordinate that the AI fires at.</param>
        public static void Shoot(Square[,] playFieldArray, out int firstCoord, out int secondCoord)
        {
            int minFirst = 0, maxFirst = playFieldArray.GetLength(0), minSecond = 0, maxSecond = playFieldArray.GetLength(1);
            Random randomizeCoord = new Random();
            int chosenRandom;
            bool foundHit = false;
            List<Tuple<int, int>> allowedCoordinates = new List<Tuple<int, int>>(); // Array or list to hold possible 

            // Loop through entire array with nested loop
            for (int x = 0; x < maxFirst && !foundHit; x++)
            {
                for (int y = 0; y < maxSecond && !foundHit; y++)
                {
                    if (playFieldArray[x, y] == Square.Hit)
                    {
                        // Found a hit but not sunk ship, clear allowedCoordinates to fire only around that hit
                        foundHit = true;
                        allowedCoordinates.Clear();

                        // Check for other hits along 1st dimension
                        if (x < (maxFirst - 1) &&
                            playFieldArray[x + 1, y] == Square.Hit)
                        {
                            // Add square to the left unless we are at the edge or there is a miss
                            if (x != minFirst &&
                                playFieldArray[x - 1, y] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x - 1, y));
                            }

                            // Find end of hits on the boat along the first dimension
                            while (x < (maxFirst - 1) &&
                                playFieldArray[x, y] == Square.Hit)
                            {
                                x++;
                            }

                            // Add the square right of the last hit unless we are at the edge or there is a miss
                            if (playFieldArray[x, y] == Square.Water ||
                                playFieldArray[x, y] == Square.Ship)
                            {
                                allowedCoordinates.Add(Tuple.Create(x - 1, y));
                            }

                            foundHit = true;
                        }

                        // Check for other hits along 2nd dimension
                        else if (y < (maxSecond - 1) &&
                            playFieldArray[x, y + 1] == Square.Hit)
                        {
                            // Add square above the hit unless we are at the edge or there is a miss
                            if (y != minSecond &&
                                playFieldArray[x, y - 1] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y - 1));
                            }

                            // Find end of hits on the boat along the seond dimension
                            while (y < (maxSecond - 1) &&
                                playFieldArray[x, y] == Square.Hit)
                            {
                                y++;
                            }

                            // Add the square below the last hit unless we are at the edge or there is a miss
                            if (playFieldArray[x, y] == Square.Water ||
                                playFieldArray[x, y] == Square.Ship)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y));
                            }

                            foundHit = true;
                        }

                        else
                        {
                            // No other hits were found.
                            // Make sure we are not at the edge in each direction and
                            // that we haven't tried to fire there but missed
                            if (x != minFirst &&
                                playFieldArray[x - 1, y] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x - 1, y));
                            }

                            if (y != minSecond &&
                                playFieldArray[x, y - 1] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y - 1));
                            }

                            if (x < (maxFirst - 1) &&
                                playFieldArray[x + 1, y] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x + 1, y));
                            }

                            if (y < (maxSecond - 1) &&
                                playFieldArray[x, y + 1] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y + 1));
                            }
                        }
                    }
                    else if (!foundHit && (playFieldArray[x, y] == Square.Water || playFieldArray[x, y] == Square.Ship))
                    {
                        // We haven't already found a hit and the current square is water or ship.
                        // Check the squares around the current square for sunk ships since 
                        // ships can't be next to each other.
                        if (x != minFirst &&
                            y != minSecond &&
                            playFieldArray[x - 1, y - 1] == Square.Sunk)
                        {
                            // There is a sunk ship above and left of the current square
                        }
                        else if (y != minSecond &&
                            playFieldArray[x, y - 1] == Square.Sunk)
                        {
                            // There is a sunk ship above the current square
                        }
                        else if (x < (maxFirst - 1) &&
                            y != minSecond &&
                            playFieldArray[x + 1, y - 1] == Square.Sunk)
                        {
                            // There is a sunk ship above and right of the current square
                        }
                        else if (x != minFirst &&
                            playFieldArray[x - 1, y] == Square.Sunk)
                        {
                            // There is a sunk ship left of the current square
                        }
                        else if (x < (maxFirst - 1) &&
                            playFieldArray[x + 1, y] == Square.Sunk)
                        {
                            // There is a sunk ship right of the current square
                        }
                        else if (x != minFirst &&
                            y < (maxSecond - 1) &&
                            playFieldArray[x - 1, y + 1] == Square.Sunk)
                        {
                            // There is a sunk ship below and left of the current square
                        }
                        else if (y < (maxSecond - 1) &&
                            playFieldArray[x, y + 1] == Square.Sunk)
                        {
                            // There is a sunk ship below the current square
                        }
                        else if (x < (maxFirst - 1) &&
                            y < (maxSecond - 1) &&
                            playFieldArray[x + 1, y + 1] == Square.Sunk)
                        {
                            // There is a sunk ship below and right of the current square
                        }
                        else
                        {
                            // If it is a square with water or ship with no sunk ship next to it
                            // then add it to the allowed coordinates
                            allowedCoordinates.Add(Tuple.Create(x, y));
                        }
                    }
                }
            }

            if (allowedCoordinates.Count != 0)
            {
                // Randomly choose one of the allowed coordinates
                // and return the chosen coordinates
                chosenRandom = randomizeCoord.Next(allowedCoordinates.Count);
                firstCoord = allowedCoordinates[chosenRandom].Item1;
                secondCoord = allowedCoordinates[chosenRandom].Item2;
            }
            else
            {
                // There are no coordinates that we can fire at
                firstCoord = -1;
                secondCoord = -1;
            }
        }
    }
}
