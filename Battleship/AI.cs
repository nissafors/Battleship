using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class AI
    {
        /// <summary>
        /// Chooses which square the AI in the Battleships game will shoot at based on
        /// which squares are eligible targets as well as earlier hits.
        /// </summary>
        /// <param name="playFieldArray">The array containing the playing field.</param>
        /// <param name="xCoord">The 1st dimension coord that the AI fires at.</param>
        /// <param name="yCoord">The 2nd dimension coord that the AI fires at.</param>
        private void AIChooseShot(Square[,] playFieldArray, out int xCoord, out int yCoord)
        {
            int minX = 0, maxX = playFieldArray.GetLength(0), minY = 0, maxY = playFieldArray.GetLength(1);
            Random randomizeCoord = new Random();
            int chosenRandom;
            bool addCoord = true, foundHit = false;
            List<Tuple<int, int>> allowedCoordinates = new List<Tuple<int, int>>(); // Array or list to hold possible 

            // Loop through entire array with nested loop
            for (int y = 0; y < maxY && !foundHit; y++)
            {
                for (int x = 0; x < maxX && !foundHit; x++)
                {
                    addCoord = true;
                    // Check if the current square is already a hit
                    if (playFieldArray[x, y] == Square.Hit)
                    {
                        // Found a hit but not sunk ship, clear allowedCoordinates to fire only around that ship
                        allowedCoordinates.Clear();

                        // Check for other hits along 1st dimension
                        if (x < (maxX - 1))
                        {
                            if (playFieldArray[x + 1, y] == Square.Hit)
                            {
                                // Add square to the left unless we are at the edge
                                if (x != minX)
                                {
                                    allowedCoordinates.Add(Tuple.Create(x - 1, y));
                                }

                                // Find end of hits on the boat
                                while (x < (maxX - 1) && playFieldArray[x, y] == Square.Hit)
                                {
                                    x++;
                                }

                                // Add the square right of the last hit unless we are at the edge or there is a miss
                                if (playFieldArray[x, y] == Square.Water || playFieldArray[x, y] == Square.Ship)
                                {
                                    allowedCoordinates.Add(Tuple.Create(x - 1, y));
                                }
                                foundHit = true;
                            }
                        }

                        // Check for other hits along 2nd dimension
                        if (y < (maxY - 1))
                        {
                            if (playFieldArray[x, y + 1] == Square.Hit)
                            {
                                // Add square above the hit unless we are at the edge
                                if (y != minY)
                                {
                                    allowedCoordinates.Add(Tuple.Create(x, y - 1));
                                }

                                // Find end of hits on the boat
                                while (y < (maxY - 1) && playFieldArray[x, y] == Square.Hit)
                                {
                                    y++;
                                }

                                // Add the square below the last hit unless we are at the edge or there is a miss
                                if (playFieldArray[x, y] == Square.Water || playFieldArray[x, y] == Square.Ship)
                                {
                                    allowedCoordinates.Add(Tuple.Create(x, y));
                                }

                                foundHit = true;
                            }
                        }

                        // No other hits were found
                        if (!foundHit)
                        {
                            // Make sure we are not at the edge in each direction and
                            // that we haven't tried to fire there but missed
                            if (x != minX && playFieldArray[x - 1, y] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x - 1, y));
                            }
                            if (y != minY && playFieldArray[x, y - 1] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y - 1));
                            }
                            if (x < (maxX - 1) && playFieldArray[x + 1, y] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x + 1, y));
                            }
                            if (y < (maxY - 1) && playFieldArray[x, y + 1] != Square.Miss)
                            {
                                allowedCoordinates.Add(Tuple.Create(x, y + 1));
                            }
                            foundHit = true;
                        }
                    }
                    // Check for water or ship to see if we should fire there
                    else if ((playFieldArray[x, y] == Square.Water || playFieldArray[x, y] == Square.Ship) && !foundHit)
                    {
                        // Check above and left of the current element for a sunk ship
                        if (x != minX && y != minY && playFieldArray[x - 1, y - 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check above the current element for a sunk ship
                        else if (y != minY && playFieldArray[x, y - 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check above and right of the current element for a sunk ship
                        else if (x < (maxX - 1) && y != minY && playFieldArray[x + 1, y - 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check to the left of the current element for a sunk ship
                        else if (x != minX && playFieldArray[x - 1, y] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check to the right of the current element for a sunk ship
                        else if (x < (maxX - 1) && playFieldArray[x + 1, y] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check below and left of the current element for a sunk ship
                        else if (x != minX && y < (maxY - 1) && playFieldArray[x - 1, y + 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check below the current element for a sunk ship
                        else if (y < (maxY - 1) && playFieldArray[x, y + 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // Check below and left of the current element for a sunk ship
                        else if (x < (maxX - 1) && y < (maxY - 1) && playFieldArray[x + 1, y + 1] == Square.Sunk)
                        {
                            addCoord = false; // Ships can't be next to each other
                        }

                        // If it is a square with water or ship with no sunk ship next to it
                        // then add it to the allowed coordinates
                        else //(addCoord)
                        {
                            allowedCoordinates.Add(Tuple.Create(x, y));
                        }
                    }
                }
            }

            chosenRandom = randomizeCoord.Next(allowedCoordinates.Count);
            xCoord = allowedCoordinates[chosenRandom].Item1;
            yCoord = allowedCoordinates[chosenRandom].Item2;
        }
    }
}
