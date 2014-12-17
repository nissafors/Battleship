//-----------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="none">
//      Copyright (c) Andreas Andersson, Henrik Ottehall, Victor Ström Nilsson & Torbjörn Widström 2014
// </copyright>
// <author>Victor Ström Nilsson</author>
//-----------------------------------------------------------------------------------------------------

namespace Battleship
{
using System;
using System.Windows.Forms;

    /// <summary>
    /// A form where the user can make adjustments such as changing the size of the grid, change the
    /// amount of ships and turn off sound.
    /// </summary>
    public partial class Options : Form
    {
        /// <summary>
        /// This form is called by a BattleshipForm object who provides a reference to itself. This
        /// field holds that reference.
        /// </summary>
        private readonly BattleshipForm battleshipForm;

        /// <summary>
        /// Indicates whether the player have changed settings
        /// </summary>
        private bool settingsChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// /// </summary>
        /// <param name="battleshipForm">A reference to the calling BattleshipForm object.</param>
        public Options(BattleshipForm battleshipForm)
        {
            InitializeComponent();
            this.battleshipForm = battleshipForm;

            sizeComboBox.SelectedIndex = sizeComboBox.FindString(this.battleshipForm.Rows.ToString());
            numericPatrolboats.Value = this.battleshipForm.NumberOfPatrolboats;
            numericCruisers.Value = this.battleshipForm.NumberOfCruisers;
            numericSubmarines.Value = this.battleshipForm.NumberOfSubmarines;
            numericCarriers.Value = this.battleshipForm.NumberOfCarriers;

            this.settingsChanged = false;
        }

        /// <summary>
        /// Used when any setting have been changed.
        /// </summary>
        /// <param name="sender">The calling object.</param>
        /// <param name="e">Parameters provided.</param>
        private void SettingsChanged(object sender, EventArgs e)
        {
            this.settingsChanged = true;
        }

        /// <summary>
        /// Displays a message asking the user to either save the settings and start a new game or to discard the changes.
        /// </summary>       
        /// <param name="sender">The calling object.</param>
        /// <param name="e">Parameters provided.</param>
        private void Cancel(object sender, EventArgs e)
        {
            if (this.settingsChanged)
            {
                var result = MessageBox.Show("Vill du spara inställningarna och starta om spelet?", "Är du säker?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    this.SaveSettings();
                }
            }
            this.Dispose();
            this.Close();
        }
     
        /// <summary>
        /// Applies the chosen settings and restarts the game.
        /// </summary>
        private void SaveSettings(object sender = null, EventArgs e = null)
        {
            this.battleshipForm.Cols = Convert.ToInt32(sizeComboBox.SelectedItem);
            this.battleshipForm.Rows = Convert.ToInt32(sizeComboBox.SelectedItem);
            this.battleshipForm.FormSize();

            this.battleshipForm.NumberOfShips = Convert.ToInt32(numericPatrolboats.Value) + Convert.ToInt32(numericCruisers.Value) + Convert.ToInt32(numericSubmarines.Value) + Convert.ToInt32(numericCarriers.Value);
            this.battleshipForm.NumberOfPatrolboats = Convert.ToInt32(numericPatrolboats.Value);
            this.battleshipForm.NumberOfCruisers = Convert.ToInt32(numericCruisers.Value);
            this.battleshipForm.NumberOfSubmarines = Convert.ToInt32(numericSubmarines.Value);
            this.battleshipForm.NumberOfCarriers = Convert.ToInt32(numericCarriers.Value);

            this.battleshipForm.RestartGame();
            this.Close();
        }
    }
}
