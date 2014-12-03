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
    public partial class Options : Form
    {
        public Options(BattleshipForm BattleshipForm )
        {
            InitializeComponent();
            this._BsForm = BattleshipForm;
        }
        private readonly BattleshipForm _BsForm;

        private void optionsAvbryt_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Vill du spara inställningarna?", "Är du säker?", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //spara
                this.Close();
            }

            else if (result == DialogResult.No)
            {
                //spara inte

                this.Close();
            }
            else if (result == DialogResult.Cancel)
            {
                //gå tillbaka

            }
        }

        

        private void optionsSpela_Click(object sender, EventArgs e)
        {
            if (sizeBox.SelectedIndex == 0)
            {
                BattleshipForm.cols = 10;
                BattleshipForm.rows = 10;
                
            }
            
            else if(sizeBox.SelectedIndex == 1)
            {
                BattleshipForm.cols = 15;
                BattleshipForm.rows = 15;
            }
            else if (sizeBox.SelectedIndex == 2)
            {
                BattleshipForm.cols = 20;
                BattleshipForm.rows = 20;
            }
            else if (sizeBox.SelectedIndex == 3)
            {
                BattleshipForm.cols = 25;
                BattleshipForm.rows = 25;
            }
            else if (sizeBox.SelectedIndex == 4)
            {
                BattleshipForm.cols = 30;
                BattleshipForm.rows = 30;
            }
            
            this._BsForm.formSize();
            this.Close();
        }
    }
}
