using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    class InsufficientGridSpaceException : Exception
    {
        public InsufficientGridSpaceException()
        {
        }
        public InsufficientGridSpaceException(string message) : base(message)
        {
        }
        public InsufficientGridSpaceException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    class ShipPostitioner
    {
        // Constructor
        public ShipPostitioner()
        {
            channels = null;
        }

        // Types
        struct Channel
        {
            public Orientation orientation;
            public int StartRow;
            public int StartCol;
            public int Length;
        }

        // Fields
        List<Channel> channels;

        public void AutoPosition(Square[,] grid, Ship[] ships)
        {
            if (GridTooSmall(grid, ships))
            {
                throw new InsufficientGridSpaceException();
            }
            foreach (Ship ship in ships)
            {
            }
        }

        public bool SetShip(Square[,] grid, Ship ship, int row, int col)
        {
            // Make sure we know our channels
            //if (channels == null)
            //{
                ListChannels(grid);
            //}

            // Is there a channel were caller wants to place the ship?
            bool ok = false;
            foreach (Channel c in channels)
            {
                switch (c.orientation)
                {
                    case Orientation.Vertical:
                        if (col == c.StartCol &&
                            row >= c.StartRow &&
                            row + ship.length <= c.StartRow + c.Length &&
                            ship.orientation == Orientation.Vertical)
                        {
                            ok = true;
                        }
                        break;
                    case Orientation.Horizontal:
                        if (row == c.StartRow &&
                            col >= c.StartCol &&
                            col + ship.length <= c.StartCol + c.Length &&
                            ship.orientation == Orientation.Horizontal)
                        {
                            ok = true;
                        }
                        break;
                }
                if (ok) break;
            }
            if (!ok)
            {
                return false;
            }

            for (int i = 0; i < ship.length; i++)
            {
                switch (ship.orientation)
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
            setForbiddenSquares(grid);
            channels = null;
            return true;
        }

        public bool GridTooSmall(Square[,] grid, Ship[] ships)
        {
            return false;
        }

        public void PrintChannelList(Square[,] grid)
        {
            ListChannels(grid);
            foreach (Channel c in channels)
            {
                Console.WriteLine(c.orientation + " | row, col: " + c.StartRow + ", " +
                    c.StartCol + " | Length: " + c.Length);
            }
            channels = null;
        }

        private void setForbiddenSquares(Square[,] grid)
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
            // except where a neighbor is also a Square.Ship
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

            // Write back to grid
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = tempGrid[row + 1, col + 1];
                }
            }

        }

        private void ListChannels(Square[,] grid)
        {
            channels = new List<Channel>();
            Square[] sqArr;
            Orientation or;

            for (int dim = 0; dim < 2; dim++)
            {
                for (int j = 0; j < grid.GetLength(dim); j++)
                {
                    sqArr = ExtractDimension(grid, dim, j);
                    or = (dim == 0) ? Orientation.Horizontal : Orientation.Vertical;
                    channels.AddRange(ChannelsFromArray(sqArr, or, j));
                }
            }
        }

        private Square[] ExtractDimension(Square[,] grid, int dim, int pos)
        {
            Square[] sqArr = new Square[grid.GetLength(Math.Abs(dim - 1))];
            for (int i = 0; i < sqArr.Length; i++)
            {
                sqArr[i] = (dim == 0) ? grid[pos, i] : grid[i, pos];
            }
            return sqArr;
        }

        private List<Channel> ChannelsFromArray(Square[] array, Orientation orientation, int start)
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
                            channel.StartRow = start;
                            channel.StartCol = startPos;
                        }
                        else
                        {
                            channel.StartRow = startPos;
                            channel.StartCol = start;
                        }
                        channel.orientation = orientation;
                        channel.Length = length;
                        channels.Add(channel);
                        length = 0;
                        newFlag = true;
                    }
                }
            }
            return channels;
        }
    }
}
