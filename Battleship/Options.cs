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
            sizeComboBox.SelectedIndex = 0;
        }

        private readonly BattleshipForm _BsForm;

        private void optionsAvbryt_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Vill du spara inställningarna och starta om spelet?", "Är du säker?", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                saveSettings();
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                this.Close();
            }
            else if (result == DialogResult.Cancel)
            {
          
            }
        }
     
        private void optionsSpela_Click(object sender, EventArgs e)
        {
            saveSettings();
        }


        private void saveSettings()
        {
            BattleshipForm.cols = Convert.ToInt32(sizeComboBox.SelectedItem);
            BattleshipForm.rows = Convert.ToInt32(sizeComboBox.SelectedItem);
            this._BsForm.formSize();
            this.Close();
        }
    }
}
