using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Battleship : Form
    {
        public enum Square { Water, Ship, Hit, Miss, Sunk, Forbidden };

        public Battleship()
        {
            InitializeComponent();
        }
    }
}
