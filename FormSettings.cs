using System;
using System.Windows.Forms;

namespace CozyGit
{
    internal class FormSettings : Form
    {
        internal System.Func<bool> Check;
        internal string SetBoolOption = null;
        internal bool NeedSave;
        internal FormSettings()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private Label label3;
        private Label label4;
        private GroupBox groupBox3;
        private Label label5;
        private Label label6;
        private GroupBox groupBox4;
        internal Button btnDiffTool;
        private Label label8;
        internal TextBox txtDiffTool;
        internal Button btnLicenses;
        internal Button btnDonate;
        internal Button btnGitHub;
        internal CheckBox chkRemotePassword;
        internal CheckBox chkRemoteUser;
        internal TextBox txtRemotePassword;
        internal TextBox txtRemoteUser;
        internal CheckBox chkAuthorEmail;
        internal CheckBox chkAuthorName;
        internal TextBox txtAuthorEmail;
        internal TextBox txtAuthorName;
        internal CheckBox chkCommitterEmail;
        internal CheckBox chkCommitterName;
        internal TextBox txtCommitterEmail;
        internal TextBox txtCommitterName;
        internal CheckBox chkUseGlobalConfig;
        internal Button btnCancel;
        internal Label lblMessage;
        internal Button btnOK;
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemoteUser = new System.Windows.Forms.TextBox();
            this.chkRemoteUser = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemotePassword = new System.Windows.Forms.TextBox();
            this.chkRemotePassword = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAuthorName = new System.Windows.Forms.TextBox();
            this.chkAuthorName = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAuthorEmail = new System.Windows.Forms.TextBox();
            this.chkAuthorEmail = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCommitterName = new System.Windows.Forms.TextBox();
            this.chkCommitterName = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCommitterEmail = new System.Windows.Forms.TextBox();
            this.chkCommitterEmail = new System.Windows.Forms.CheckBox();
            this.chkUseGlobalConfig = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDiffTool = new System.Windows.Forms.TextBox();
            this.btnDiffTool = new System.Windows.Forms.Button();
            this.btnLicenses = new System.Windows.Forms.Button();
            this.btnDonate = new System.Windows.Forms.Button();
            this.btnGitHub = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtRemoteUser);
            this.groupBox1.Controls.Add(this.chkRemoteUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtRemotePassword);
            this.groupBox1.Controls.Add(this.chkRemotePassword);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 78);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Remote Credentials";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name:";
            // 
            // txtRemoteUser
            // 
            this.txtRemoteUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemoteUser.Location = new System.Drawing.Point(117, 20);
            this.txtRemoteUser.Name = "txtRemoteUser";
            this.txtRemoteUser.Size = new System.Drawing.Size(327, 20);
            this.txtRemoteUser.TabIndex = 1;
            // 
            // chkRemoteUser
            // 
            this.chkRemoteUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRemoteUser.AutoSize = true;
            this.chkRemoteUser.Checked = true;
            this.chkRemoteUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemoteUser.Location = new System.Drawing.Point(459, 22);
            this.chkRemoteUser.Name = "chkRemoteUser";
            this.chkRemoteUser.Size = new System.Drawing.Size(77, 17);
            this.chkRemoteUser.TabIndex = 2;
            this.chkRemoteUser.Text = "Remember";
            this.chkRemoteUser.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password / Token:";
            // 
            // txtRemotePassword
            // 
            this.txtRemotePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemotePassword.Location = new System.Drawing.Point(116, 47);
            this.txtRemotePassword.Name = "txtRemotePassword";
            this.txtRemotePassword.Size = new System.Drawing.Size(327, 20);
            this.txtRemotePassword.TabIndex = 4;
            this.txtRemotePassword.UseSystemPasswordChar = true;
            // 
            // chkRemotePassword
            // 
            this.chkRemotePassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRemotePassword.AutoSize = true;
            this.chkRemotePassword.Checked = true;
            this.chkRemotePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemotePassword.Location = new System.Drawing.Point(459, 49);
            this.chkRemotePassword.Name = "chkRemotePassword";
            this.chkRemotePassword.Size = new System.Drawing.Size(77, 17);
            this.chkRemotePassword.TabIndex = 5;
            this.chkRemotePassword.Text = "Remember";
            this.chkRemotePassword.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtAuthorName);
            this.groupBox2.Controls.Add(this.chkAuthorName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtAuthorEmail);
            this.groupBox2.Controls.Add(this.chkAuthorEmail);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(542, 78);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Commit Author Signature";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Name:";
            // 
            // txtAuthorName
            // 
            this.txtAuthorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthorName.Location = new System.Drawing.Point(117, 20);
            this.txtAuthorName.Name = "txtAuthorName";
            this.txtAuthorName.Size = new System.Drawing.Size(327, 20);
            this.txtAuthorName.TabIndex = 1;
            // 
            // chkAuthorName
            // 
            this.chkAuthorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAuthorName.AutoSize = true;
            this.chkAuthorName.Checked = true;
            this.chkAuthorName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuthorName.Location = new System.Drawing.Point(459, 22);
            this.chkAuthorName.Name = "chkAuthorName";
            this.chkAuthorName.Size = new System.Drawing.Size(77, 17);
            this.chkAuthorName.TabIndex = 2;
            this.chkAuthorName.Text = "Remember";
            this.chkAuthorName.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "E-Mail:";
            // 
            // txtAuthorEmail
            // 
            this.txtAuthorEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthorEmail.Location = new System.Drawing.Point(116, 47);
            this.txtAuthorEmail.Name = "txtAuthorEmail";
            this.txtAuthorEmail.Size = new System.Drawing.Size(327, 20);
            this.txtAuthorEmail.TabIndex = 4;
            // 
            // chkAuthorEmail
            // 
            this.chkAuthorEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAuthorEmail.AutoSize = true;
            this.chkAuthorEmail.Checked = true;
            this.chkAuthorEmail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuthorEmail.Location = new System.Drawing.Point(459, 49);
            this.chkAuthorEmail.Name = "chkAuthorEmail";
            this.chkAuthorEmail.Size = new System.Drawing.Size(77, 17);
            this.chkAuthorEmail.TabIndex = 5;
            this.chkAuthorEmail.Text = "Remember";
            this.chkAuthorEmail.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtCommitterName);
            this.groupBox3.Controls.Add(this.chkCommitterName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtCommitterEmail);
            this.groupBox3.Controls.Add(this.chkCommitterEmail);
            this.groupBox3.Location = new System.Drawing.Point(12, 199);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(542, 78);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Commit Committer Signature";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Name:";
            // 
            // txtCommitterName
            // 
            this.txtCommitterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommitterName.Location = new System.Drawing.Point(117, 20);
            this.txtCommitterName.Name = "txtCommitterName";
            this.txtCommitterName.Size = new System.Drawing.Size(327, 20);
            this.txtCommitterName.TabIndex = 1;
            // 
            // chkCommitterName
            // 
            this.chkCommitterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCommitterName.AutoSize = true;
            this.chkCommitterName.Checked = true;
            this.chkCommitterName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommitterName.Location = new System.Drawing.Point(459, 22);
            this.chkCommitterName.Name = "chkCommitterName";
            this.chkCommitterName.Size = new System.Drawing.Size(77, 17);
            this.chkCommitterName.TabIndex = 2;
            this.chkCommitterName.Text = "Remember";
            this.chkCommitterName.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "E-Mail:";
            // 
            // txtCommitterEmail
            // 
            this.txtCommitterEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommitterEmail.Location = new System.Drawing.Point(116, 47);
            this.txtCommitterEmail.Name = "txtCommitterEmail";
            this.txtCommitterEmail.Size = new System.Drawing.Size(327, 20);
            this.txtCommitterEmail.TabIndex = 4;
            // 
            // chkCommitterEmail
            // 
            this.chkCommitterEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCommitterEmail.AutoSize = true;
            this.chkCommitterEmail.Checked = true;
            this.chkCommitterEmail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommitterEmail.Location = new System.Drawing.Point(459, 49);
            this.chkCommitterEmail.Name = "chkCommitterEmail";
            this.chkCommitterEmail.Size = new System.Drawing.Size(77, 17);
            this.chkCommitterEmail.TabIndex = 5;
            this.chkCommitterEmail.Text = "Remember";
            this.chkCommitterEmail.UseVisualStyleBackColor = true;
            // 
            // chkUseGlobalConfig
            // 
            this.chkUseGlobalConfig.AutoSize = true;
            this.chkUseGlobalConfig.Location = new System.Drawing.Point(12, 283);
            this.chkUseGlobalConfig.Name = "chkUseGlobalConfig";
            this.chkUseGlobalConfig.Size = new System.Drawing.Size(493, 17);
            this.chkUseGlobalConfig.TabIndex = 4;
            this.chkUseGlobalConfig.Text = "Remember credentials in system-wide git config file (otherwise it will be saved t" +
    "o the local repository)";
            this.chkUseGlobalConfig.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(398, 377);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(479, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.txtDiffTool);
            this.groupBox4.Controls.Add(this.btnDiffTool);
            this.groupBox4.Location = new System.Drawing.Point(12, 315);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(542, 50);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Other Settings:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Diff Viewer Tool:";
            // 
            // txtDiffTool
            // 
            this.txtDiffTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiffTool.Location = new System.Drawing.Point(98, 20);
            this.txtDiffTool.Name = "txtDiffTool";
            this.txtDiffTool.Size = new System.Drawing.Size(403, 20);
            this.txtDiffTool.TabIndex = 1;
            // 
            // btnDiffTool
            // 
            this.btnDiffTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiffTool.Location = new System.Drawing.Point(507, 19);
            this.btnDiffTool.Name = "btnDiffTool";
            this.btnDiffTool.Size = new System.Drawing.Size(28, 22);
            this.btnDiffTool.TabIndex = 2;
            this.btnDiffTool.Text = "...";
            this.btnDiffTool.UseVisualStyleBackColor = true;
            // 
            // btnLicenses
            // 
            this.btnLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLicenses.Location = new System.Drawing.Point(144, 377);
            this.btnLicenses.Name = "btnLicenses";
            this.btnLicenses.Size = new System.Drawing.Size(60, 23);
            this.btnLicenses.TabIndex = 9;
            this.btnLicenses.Text = "Licenses";
            this.btnLicenses.UseVisualStyleBackColor = true;
            // 
            // btnDonate
            // 
            this.btnDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDonate.Location = new System.Drawing.Point(12, 377);
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(60, 23);
            this.btnDonate.TabIndex = 7;
            this.btnDonate.Text = "Donate";
            this.btnDonate.UseVisualStyleBackColor = true;
            // 
            // btnGitHub
            // 
            this.btnGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGitHub.Location = new System.Drawing.Point(78, 377);
            this.btnGitHub.Name = "btnGitHub";
            this.btnGitHub.Size = new System.Drawing.Size(60, 23);
            this.btnGitHub.TabIndex = 8;
            this.btnGitHub.Text = "GitHub";
            this.btnGitHub.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(566, 411);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkUseGlobalConfig);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnDonate);
            this.Controls.Add(this.btnGitHub);
            this.Controls.Add(this.btnLicenses);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(8000, 450);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(392, 450);
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}