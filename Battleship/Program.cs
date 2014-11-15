using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    enum Square { Water, Ship, Hit, Miss, Sunk, Forbidden };
    enum Orientation { None, Vertical, Horizontal }
    struct Ship
    {
        int length;
        Orientation orientation;
    }

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
            Application.Run(new Battleship());
        }
    }
}
