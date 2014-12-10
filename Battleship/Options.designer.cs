namespace Battleship
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericCarriers = new System.Windows.Forms.NumericUpDown();
            this.numericSubmarines = new System.Windows.Forms.NumericUpDown();
            this.numericCruisers = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericPatrolboats = new System.Windows.Forms.NumericUpDown();
            this.sizeComboBox = new System.Windows.Forms.ComboBox();
            this.optionsAvbryt = new System.Windows.Forms.Button();
            this.optionsSpela = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.soundCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericCarriers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSubmarines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCruisers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPatrolboats)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Storlek:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Hangarfartyg:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Ubåtar:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Kryssare:";
            // 
            // numericCarriers
            // 
            this.numericCarriers.Location = new System.Drawing.Point(149, 152);
            this.numericCarriers.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericCarriers.Name = "numericCarriers";
            this.numericCarriers.Size = new System.Drawing.Size(67, 20);
            this.numericCarriers.TabIndex = 19;
            this.toolTip1.SetToolTip(this.numericCarriers, "Välj antal hangarfartyg som ska vara med i spelet.");
            this.numericCarriers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericSubmarines
            // 
            this.numericSubmarines.Location = new System.Drawing.Point(149, 126);
            this.numericSubmarines.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericSubmarines.Name = "numericSubmarines";
            this.numericSubmarines.Size = new System.Drawing.Size(67, 20);
            this.numericSubmarines.TabIndex = 18;
            this.toolTip1.SetToolTip(this.numericSubmarines, "Välj antal ubåtar som ska vara med i spelet.");
            this.numericSubmarines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericCruisers
            // 
            this.numericCruisers.Location = new System.Drawing.Point(149, 100);
            this.numericCruisers.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericCruisers.Name = "numericCruisers";
            this.numericCruisers.Size = new System.Drawing.Size(67, 20);
            this.numericCruisers.TabIndex = 17;
            this.toolTip1.SetToolTip(this.numericCruisers, "Välj antal kryssare som ska vara med i spelet.");
            this.numericCruisers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Jagare:";
            // 
            // numericPatrolboats
            // 
            this.numericPatrolboats.Location = new System.Drawing.Point(149, 74);
            this.numericPatrolboats.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericPatrolboats.Name = "numericPatrolboats";
            this.numericPatrolboats.Size = new System.Drawing.Size(67, 20);
            this.numericPatrolboats.TabIndex = 15;
            this.toolTip1.SetToolTip(this.numericPatrolboats, "Välj antal jagare som ska vara med i spelet.");
            this.numericPatrolboats.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // sizeComboBox
            // 
            this.sizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeComboBox.FormattingEnabled = true;
            this.sizeComboBox.Items.AddRange(new object[] {
            "10",
            "15",
            "20"});
            this.sizeComboBox.Location = new System.Drawing.Point(95, 27);
            this.sizeComboBox.Name = "sizeComboBox";
            this.sizeComboBox.Size = new System.Drawing.Size(121, 21);
            this.sizeComboBox.TabIndex = 14;
            this.toolTip1.SetToolTip(this.sizeComboBox, "Välj storlek på spelplanen. (x*x)");
            // 
            // optionsAvbryt
            // 
            this.optionsAvbryt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.optionsAvbryt.Location = new System.Drawing.Point(149, 225);
            this.optionsAvbryt.Name = "optionsAvbryt";
            this.optionsAvbryt.Size = new System.Drawing.Size(75, 23);
            this.optionsAvbryt.TabIndex = 13;
            this.optionsAvbryt.Text = "Avbryt";
            this.optionsAvbryt.UseVisualStyleBackColor = true;
            this.optionsAvbryt.Click += new System.EventHandler(this.optionsAvbryt_Click);
            // 
            // optionsSpela
            // 
            this.optionsSpela.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.optionsSpela.Location = new System.Drawing.Point(68, 225);
            this.optionsSpela.Name = "optionsSpela";
            this.optionsSpela.Size = new System.Drawing.Size(75, 23);
            this.optionsSpela.TabIndex = 12;
            this.optionsSpela.Text = "Nytt spel";
            this.optionsSpela.UseVisualStyleBackColor = true;
            this.optionsSpela.Click += new System.EventHandler(this.optionsSpela_Click);
            // 
            // soundCheckBox
            // 
            this.soundCheckBox.AutoSize = true;
            this.soundCheckBox.Checked = true;
            this.soundCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.soundCheckBox.Location = new System.Drawing.Point(102, 191);
            this.soundCheckBox.Name = "soundCheckBox";
            this.soundCheckBox.Size = new System.Drawing.Size(15, 14);
            this.soundCheckBox.TabIndex = 24;
            this.soundCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Ljud:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(99, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "•••••";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(99, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "••••";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(99, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "•••";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(99, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "••";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.optionsAvbryt;
            this.ClientSize = new System.Drawing.Size(232, 256);
            this.ControlBox = false;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.soundCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericCarriers);
            this.Controls.Add(this.numericSubmarines);
            this.Controls.Add(this.numericCruisers);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericPatrolboats);
            this.Controls.Add(this.sizeComboBox);
            this.Controls.Add(this.optionsAvbryt);
            this.Controls.Add(this.optionsSpela);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Inställningar";
            ((System.ComponentModel.ISupportInitialize)(this.numericCarriers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSubmarines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCruisers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPatrolboats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericCarriers;
        private System.Windows.Forms.NumericUpDown numericSubmarines;
        private System.Windows.Forms.NumericUpDown numericCruisers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericPatrolboats;
        private System.Windows.Forms.ComboBox sizeComboBox;
        private System.Windows.Forms.Button optionsAvbryt;
        private System.Windows.Forms.Button optionsSpela;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox soundCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;

    }
}