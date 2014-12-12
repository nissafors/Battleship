//-----------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="none">
//      Copyright (c) Andreas Andersson, Henrik Ottehall, Victor Ström Nilsson & Torbjörn Widström 2014
// </copyright>
// <author>Victor Ström Nilsson</author>
//-----------------------------------------------------------------------------------------------------

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
            
            sizeComboBox.SelectedIndex = sizeComboBox.FindString(_BsForm.Rows.ToString());
            soundCheckBox.Checked = _BsForm.SoundOn;
            numericPatrolboats.Value = _BsForm.NumberOfPatrolboats;
            numericCruisers.Value = _BsForm.NumberOfCruisers;
            numericSubmarines.Value = _BsForm.NumberOfSubmarines;
            numericCarriers.Value = _BsForm.NumberOfCarriers;
        }

        private readonly BattleshipForm _BsForm;

        /// <summary>
        /// Displays a message asking the user to either save the settings and start a new game or to discard the changes.
        /// </summary>       
        private void optionsAvbryt_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Vill du spara inställningarna och starta om spelet?", "Är du säker?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                saveSettings();
                
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                this.Close();
            }
        }
     
        private void optionsSpela_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        /// <summary>
        /// Applies the chosen settings and restarts the game.
        /// </summary>
        private void saveSettings()
        {
            this._BsForm.Cols = Convert.ToInt32(sizeComboBox.SelectedItem);
            this._BsForm.Rows = Convert.ToInt32(sizeComboBox.SelectedItem);
            this._BsForm.FormSize();

            if (soundCheckBox.Checked == true)
            {
                this._BsForm.SoundOn = true;
            }
            else if (soundCheckBox.Checked == false)
            {
                this._BsForm.SoundOn = false;
            }

            this._BsForm.NumberOfShips = (Convert.ToInt32(numericPatrolboats.Value) + Convert.ToInt32(numericCruisers.Value) + Convert.ToInt32(numericSubmarines.Value) + Convert.ToInt32(numericCarriers.Value));
            this._BsForm.NumberOfPatrolboats = Convert.ToInt32(numericPatrolboats.Value);
            this._BsForm.NumberOfCruisers = Convert.ToInt32(numericCruisers.Value);
            this._BsForm.NumberOfSubmarines = Convert.ToInt32(numericSubmarines.Value);
            this._BsForm.NumberOfCarriers = Convert.ToInt32(numericCarriers.Value);

            this._BsForm.RestartGame();
            this.Close();
        }
    }
}
