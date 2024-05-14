using System;
using System.Windows.Forms;

namespace CozyGit
{
    internal class FormClone : Form
    {
        internal FormClone()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        internal TextBox txtURL;
        private Button btnCancel;
        private Button btnOK;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label4;
        internal TextBox txtBranch;
        private Label label2;
        internal BetterScrollNumericUpDown numDepth;
        internal RadioButton radDepthCustom;
        internal RadioButton radDepthEverything;
        private Label label1;
        internal RadioButton radBranchCustom;
        internal RadioButton radBranchDefault;
        private Panel panel2;
        private Panel panel1;
        private Label label3;

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numDepth = new CozyGit.BetterScrollNumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radDepthEverything = new System.Windows.Forms.RadioButton();
            this.radDepthCustom = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBranch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radBranchDefault = new System.Windows.Forms.RadioButton();
            this.radBranchCustom = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepth)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtURL);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "URL of repository to clone:";
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Location = new System.Drawing.Point(7, 20);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(547, 20);
            this.txtURL.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(416, 196);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 119);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Clone options:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.numDepth);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.radDepthEverything);
            this.panel2.Controls.Add(this.radDepthCustom);
            this.panel2.Location = new System.Drawing.Point(7, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(548, 42);
            this.panel2.TabIndex = 1;
            // 
            // numDepth
            // 
            this.numDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numDepth.Enabled = false;
            this.numDepth.Location = new System.Drawing.Point(217, 1);
            this.numDepth.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDepth.Name = "numDepth";
            this.numDepth.Size = new System.Drawing.Size(329, 20);
            this.numDepth.TabIndex = 3;
            this.numDepth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Depth:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(0, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(548, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Setting depth clones with the history truncated to the specified number of commit" +
    "s";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // radDepthEverything
            // 
            this.radDepthEverything.AutoSize = true;
            this.radDepthEverything.Checked = true;
            this.radDepthEverything.Location = new System.Drawing.Point(57, 1);
            this.radDepthEverything.Name = "radDepthEverything";
            this.radDepthEverything.Size = new System.Drawing.Size(75, 17);
            this.radDepthEverything.TabIndex = 1;
            this.radDepthEverything.TabStop = true;
            this.radDepthEverything.Text = "Everything";
            this.radDepthEverything.UseVisualStyleBackColor = true;
            // 
            // radDepthCustom
            // 
            this.radDepthCustom.AutoSize = true;
            this.radDepthCustom.Location = new System.Drawing.Point(148, 1);
            this.radDepthCustom.Name = "radDepthCustom";
            this.radDepthCustom.Size = new System.Drawing.Size(63, 17);
            this.radDepthCustom.TabIndex = 2;
            this.radDepthCustom.Text = "Custom:";
            this.radDepthCustom.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBranch);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.radBranchDefault);
            this.panel1.Controls.Add(this.radBranchCustom);
            this.panel1.Location = new System.Drawing.Point(7, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 42);
            this.panel1.TabIndex = 0;
            // 
            // txtBranch
            // 
            this.txtBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBranch.Enabled = false;
            this.txtBranch.Location = new System.Drawing.Point(217, 1);
            this.txtBranch.Name = "txtBranch";
            this.txtBranch.Size = new System.Drawing.Size(329, 20);
            this.txtBranch.TabIndex = 3;
            this.txtBranch.Text = "main";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Branch:";
            // 
            // radBranchDefault
            // 
            this.radBranchDefault.AutoSize = true;
            this.radBranchDefault.Checked = true;
            this.radBranchDefault.Location = new System.Drawing.Point(57, 2);
            this.radBranchDefault.Name = "radBranchDefault";
            this.radBranchDefault.Size = new System.Drawing.Size(59, 17);
            this.radBranchDefault.TabIndex = 1;
            this.radBranchDefault.TabStop = true;
            this.radBranchDefault.Text = "Default";
            this.radBranchDefault.UseVisualStyleBackColor = true;
            // 
            // radBranchCustom
            // 
            this.radBranchCustom.AutoSize = true;
            this.radBranchCustom.Location = new System.Drawing.Point(148, 2);
            this.radBranchCustom.Name = "radBranchCustom";
            this.radBranchCustom.Size = new System.Drawing.Size(63, 17);
            this.radBranchCustom.TabIndex = 2;
            this.radBranchCustom.Text = "Custom:";
            this.radBranchCustom.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(0, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(548, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cloning an empty repository will ask for the branch name on the first commit.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormClone
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 231);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(9999, 270);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(196, 270);
            this.Name = "FormClone";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clone Repository";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepth)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }

    internal class BetterScrollNumericUpDown : NumericUpDown
    {
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Value = Math.Max(Minimum, Math.Min(Maximum, (Value + (e.Delta >= 0 ? Increment : -Increment))));
        }
    }
}