using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    /// <summary>
    /// Exception to be thrown when the playing field grid was too small to hold all the ships.
    /// </summary>
    public class InsufficientGridSpaceException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="Battleship.InsufficientGridSpaceException"/> class.</summary>
        public InsufficientGridSpaceException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Battleship.InsufficientGridSpaceException"/> class.</summary>
        /// <param name="message">Error message.</param>
        public InsufficientGridSpaceException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Battleship.InsufficientGridSpaceException"/> class.</summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">An exception.</param>
        public InsufficientGridSpaceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
