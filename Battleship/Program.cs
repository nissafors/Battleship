﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public enum Square { Water, Ship, Hit, Miss, Sunk, Forbidden, ArrowRight, ArrowDown, ArrowLeft, ArrowUp };
    public enum Orientation { None, Vertical, Horizontal }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BattleshipForm());
        }
    }
}
