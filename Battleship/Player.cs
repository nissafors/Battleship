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
            // IF rutan har redan blivit skjuten på
            //     return forbidden
            // ELSE IF träff
            //     skriv in träff i spelplan
            //     IF sänkt
            //         return sänkt
            //     ELSE
            //         return träff
            // ELSE
            //     skriv in miss i spelplanen
            //     return miss
            // END IF
        }

        private bool isSunk(Square[,] grid, int row, int col)
        {

        }
    }
}
