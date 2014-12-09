//-----------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="none">
//      Copyright (c) Andreas Andersson, Henrik Ottehall, Victor Ström Nilsson & Torbjörn Widström 2014
// </copyright>
// <author>Andreas Andersson</author>
// <author>Henrik Ottehall</author>
//-----------------------------------------------------------------------------------------------------

namespace Battleship
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Values representing the squares that can be seen on the Battleship game board.
    /// </summary>
    public enum Square
    {
        /// <summary>
        /// Represents water.
        /// </summary>
        Water,

        /// <summary>
        /// Represents a part of a ship.
        /// </summary>
        Ship,

        /// <summary>
        /// Represents a part of a ship that has been hit.
        /// </summary>
        Hit,

        /// <summary>
        /// Represents water that has been shot at.
        /// </summary>
        Miss,

        /// <summary>
        /// Represents a part of a sunken ship.
        /// </summary>
        Sunk,

        /// <summary>
        /// Represents water where ships can't be placed.
        /// </summary>
        Forbidden,

        /// <summary>
        /// Represents an arrow pointing right for choosing ship orientation while placing them.
        /// </summary>
        ArrowRight,

        /// <summary>
        /// Represents an arrow pointing down for choosing ship orientation while placing them.
        /// </summary>
        ArrowDown,

        /// <summary>
        /// Represents an arrow pointing left for choosing ship orientation while placing them.
        /// </summary>
        ArrowLeft,

        /// <summary>
        /// Represents an arrow pointing up for choosing ship orientation while placing them.
        /// </summary>
        ArrowUp
    }

    /// <summary>
    /// Contains the program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BattleshipForm());
        }
    }
}
