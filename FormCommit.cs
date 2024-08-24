using System;
using System.Windows.Forms;

namespace CozyGit
{
    internal class FormCommit : Form
    {
        internal FormCommit()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        private Label label1;
        internal Button btnOK;
        internal Button btnShowLog;
        internal Button btnRefresh;
        internal CheckBox chkShowUnversioned;
        internal CheckBox chkShowIgnored;
        internal CheckBox chkShowUnchanged;
        internal LinkLabel lnkCheckDeleted;
        internal Button btnSettings;
        internal Button btnPullLatest;
        internal RichTextBox txtMessage;
        internal ProgressBar Progress;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnClose;
        internal SplitContainer splitMain;
        private Label label2;
        private Label label3;
        internal LinkLabel lnkLocalRepository;
        internal LinkLabel lnkRemoteRepository;
        internal Label lblStatus;
        internal BetterScrollDataGridView gridMain;
        internal LinkLabel lnkCheckAll;
        internal LinkLabel lnkCheckNone;
        internal LinkLabel lnkCheckUnversioned;
        internal LinkLabel lnkCheckVersioned;
        internal LinkLabel lnkCheckModified;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label label4;
        internal LinkLabel lnkBranch;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkShowUnversioned = new System.Windows.Forms.CheckBox();
            this.chkShowIgnored = new System.Windows.Forms.CheckBox();
            this.chkShowUnchanged = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.gridMain = new BetterScrollDataGridView();
            this.lnkCheckAll = new System.Windows.Forms.LinkLabel();
            this.lnkCheckNone = new System.Windows.Forms.LinkLabel();
            this.lnkCheckUnversioned = new System.Windows.Forms.LinkLabel();
            this.lnkCheckVersioned = new System.Windows.Forms.LinkLabel();
            this.lnkCheckDeleted = new System.Windows.Forms.LinkLabel();
            this.lnkCheckModified = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkLocalRepository = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkRemoteRepository = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.lnkBranch = new System.Windows.Forms.LinkLabel();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.btnPullLatest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Location = new System.Drawing.Point(8, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1088, 165);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Message:";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.DetectUrls = false;
            this.txtMessage.Location = new System.Drawing.Point(6, 19);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1077, 140);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.flowLayoutPanel2);
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Controls.Add(this.gridMain);
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1088, 356);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Changes Made:";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.chkShowUnversioned);
            this.flowLayoutPanel2.Controls.Add(this.chkShowIgnored);
            this.flowLayoutPanel2.Controls.Add(this.chkShowUnchanged);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 330);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(456, 23);
            this.flowLayoutPanel2.TabIndex = 2;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // chkShowUnversioned
            // 
            this.chkShowUnversioned.AutoSize = true;
            this.chkShowUnversioned.Checked = true;
            this.chkShowUnversioned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUnversioned.Location = new System.Drawing.Point(3, 3);
            this.chkShowUnversioned.Name = "chkShowUnversioned";
            this.chkShowUnversioned.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.chkShowUnversioned.Size = new System.Drawing.Size(155, 17);
            this.chkShowUnversioned.TabIndex = 0;
            this.chkShowUnversioned.Text = "Show &unversioned files";
            this.chkShowUnversioned.UseVisualStyleBackColor = true;
            // 
            // chkShowIgnored
            // 
            this.chkShowIgnored.AutoSize = true;
            this.chkShowIgnored.Location = new System.Drawing.Point(164, 3);
            this.chkShowIgnored.Name = "chkShowIgnored";
            this.chkShowIgnored.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.chkShowIgnored.Size = new System.Drawing.Size(132, 17);
            this.chkShowIgnored.TabIndex = 1;
            this.chkShowIgnored.Text = "Show &ignored files";
            this.chkShowIgnored.UseVisualStyleBackColor = true;
            // 
            // chkShowUnchanged
            // 
            this.chkShowUnchanged.AutoSize = true;
            this.chkShowUnchanged.Location = new System.Drawing.Point(302, 3);
            this.chkShowUnchanged.Name = "chkShowUnchanged";
            this.chkShowUnchanged.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.chkShowUnchanged.Size = new System.Drawing.Size(151, 17);
            this.chkShowUnchanged.TabIndex = 2;
            this.chkShowUnchanged.Text = "Show un&changed files";
            this.chkShowUnchanged.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckAll);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckNone);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckUnversioned);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckVersioned);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckDeleted);
            this.flowLayoutPanel1.Controls.Add(this.lnkCheckModified);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1075, 19);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Check:";
            // 
            // lnkCheckAll
            // 
            this.lnkCheckAll.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckAll.AutoSize = true;
            this.lnkCheckAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckAll.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckAll.Location = new System.Drawing.Point(55, 0);
            this.lnkCheckAll.Name = "lnkCheckAll";
            this.lnkCheckAll.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckAll.Size = new System.Drawing.Size(26, 13);
            this.lnkCheckAll.TabIndex = 1;
            this.lnkCheckAll.TabStop = true;
            this.lnkCheckAll.Text = "All";
            this.lnkCheckAll.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // lnkCheckNone
            // 
            this.lnkCheckNone.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckNone.AutoSize = true;
            this.lnkCheckNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckNone.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckNone.Location = new System.Drawing.Point(87, 0);
            this.lnkCheckNone.Name = "lnkCheckNone";
            this.lnkCheckNone.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckNone.Size = new System.Drawing.Size(42, 13);
            this.lnkCheckNone.TabIndex = 2;
            this.lnkCheckNone.TabStop = true;
            this.lnkCheckNone.Text = "None";
            this.lnkCheckNone.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // lnkCheckUnversioned
            // 
            this.lnkCheckUnversioned.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckUnversioned.AutoSize = true;
            this.lnkCheckUnversioned.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckUnversioned.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckUnversioned.Location = new System.Drawing.Point(135, 0);
            this.lnkCheckUnversioned.Name = "lnkCheckUnversioned";
            this.lnkCheckUnversioned.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckUnversioned.Size = new System.Drawing.Size(83, 13);
            this.lnkCheckUnversioned.TabIndex = 3;
            this.lnkCheckUnversioned.TabStop = true;
            this.lnkCheckUnversioned.Text = "Unversioned";
            this.lnkCheckUnversioned.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // lnkCheckVersioned
            // 
            this.lnkCheckVersioned.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckVersioned.AutoSize = true;
            this.lnkCheckVersioned.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckVersioned.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckVersioned.Location = new System.Drawing.Point(224, 0);
            this.lnkCheckVersioned.Name = "lnkCheckVersioned";
            this.lnkCheckVersioned.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckVersioned.Size = new System.Drawing.Size(68, 13);
            this.lnkCheckVersioned.TabIndex = 4;
            this.lnkCheckVersioned.TabStop = true;
            this.lnkCheckVersioned.Text = "Versioned";
            this.lnkCheckVersioned.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // lnkCheckDeleted
            // 
            this.lnkCheckDeleted.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckDeleted.AutoSize = true;
            this.lnkCheckDeleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckDeleted.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckDeleted.Location = new System.Drawing.Point(298, 0);
            this.lnkCheckDeleted.Name = "lnkCheckDeleted";
            this.lnkCheckDeleted.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckDeleted.Size = new System.Drawing.Size(56, 13);
            this.lnkCheckDeleted.TabIndex = 5;
            this.lnkCheckDeleted.TabStop = true;
            this.lnkCheckDeleted.Text = "Deleted";
            this.lnkCheckDeleted.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // lnkCheckModified
            // 
            this.lnkCheckModified.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckModified.AutoSize = true;
            this.lnkCheckModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCheckModified.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkCheckModified.Location = new System.Drawing.Point(360, 0);
            this.lnkCheckModified.Name = "lnkCheckModified";
            this.lnkCheckModified.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkCheckModified.Size = new System.Drawing.Size(60, 13);
            this.lnkCheckModified.TabIndex = 6;
            this.lnkCheckModified.TabStop = true;
            this.lnkCheckModified.Text = "Modified";
            this.lnkCheckModified.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // gridMain
            // 
            this.gridMain.AllowUserToAddRows = false;
            this.gridMain.AllowUserToDeleteRows = false;
            this.gridMain.AllowUserToOrderColumns = true;
            this.gridMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.gridMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridMain.BackgroundColor = System.Drawing.Color.White;
            this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridMain.Location = new System.Drawing.Point(7, 45);
            this.gridMain.Name = "gridMain";
            this.gridMain.RowHeadersVisible = false;
            this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMain.ShowCellErrors = false;
            this.gridMain.ShowEditingIcon = false;
            this.gridMain.ShowRowErrors = false;
            this.gridMain.Size = new System.Drawing.Size(1075, 284);
            this.gridMain.StandardTab = true;
            this.gridMain.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(4, 334);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblStatus.Size = new System.Drawing.Size(1083, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(1022, 367);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.flowLayoutPanel3);
            this.splitMain.Panel1.Controls.Add(this.groupBox1);
            this.splitMain.Panel1MinSize = 110;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.Progress);
            this.splitMain.Panel2.Controls.Add(this.groupBox2);
            this.splitMain.Panel2.Controls.Add(this.btnPullLatest);
            this.splitMain.Panel2.Controls.Add(this.btnClose);
            this.splitMain.Panel2.Controls.Add(this.btnOK);
            this.splitMain.Panel2.Controls.Add(this.btnSettings);
            this.splitMain.Panel2.Controls.Add(this.btnShowLog);
            this.splitMain.Panel2.Controls.Add(this.btnRefresh);
            this.splitMain.Panel2MinSize = 150;
            this.splitMain.Size = new System.Drawing.Size(1104, 601);
            this.splitMain.SplitterDistance = 200;
            this.splitMain.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.lnkLocalRepository);
            this.flowLayoutPanel3.Controls.Add(this.label3);
            this.flowLayoutPanel3.Controls.Add(this.lnkRemoteRepository);
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.lnkBranch);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(8, 8);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1088, 21);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Local Repository:";
            // 
            // lnkLocalRepository
            // 
            this.lnkLocalRepository.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkLocalRepository.AutoSize = true;
            this.lnkLocalRepository.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLocalRepository.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkLocalRepository.Location = new System.Drawing.Point(98, 0);
            this.lnkLocalRepository.Name = "lnkLocalRepository";
            this.lnkLocalRepository.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkLocalRepository.Size = new System.Drawing.Size(16, 13);
            this.lnkLocalRepository.TabIndex = 1;
            this.lnkLocalRepository.TabStop = true;
            this.lnkLocalRepository.Text = " ";
            this.lnkLocalRepository.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "   Remote Repository:";
            // 
            // lnkRemoteRepository
            // 
            this.lnkRemoteRepository.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkRemoteRepository.AutoSize = true;
            this.lnkRemoteRepository.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkRemoteRepository.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkRemoteRepository.Location = new System.Drawing.Point(235, 0);
            this.lnkRemoteRepository.Name = "lnkRemoteRepository";
            this.lnkRemoteRepository.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkRemoteRepository.Size = new System.Drawing.Size(16, 13);
            this.lnkRemoteRepository.TabIndex = 3;
            this.lnkRemoteRepository.TabStop = true;
            this.lnkRemoteRepository.Text = " ";
            this.lnkRemoteRepository.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "   Branch:";
            // 
            // lnkBranch
            // 
            this.lnkBranch.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkBranch.AutoSize = true;
            this.lnkBranch.Enabled = false;
            this.lnkBranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkBranch.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lnkBranch.Location = new System.Drawing.Point(316, 0);
            this.lnkBranch.Name = "lnkBranch";
            this.lnkBranch.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lnkBranch.Size = new System.Drawing.Size(16, 13);
            this.lnkBranch.TabIndex = 5;
            this.lnkBranch.TabStop = true;
            this.lnkBranch.Text = " ";
            this.lnkBranch.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.Location = new System.Drawing.Point(8, 368);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(578, 21);
            this.Progress.TabIndex = 1;
            this.Progress.Visible = false;
            // 
            // btnPullLatest
            // 
            this.btnPullLatest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPullLatest.Location = new System.Drawing.Point(754, 367);
            this.btnPullLatest.Name = "btnPullLatest";
            this.btnPullLatest.Size = new System.Drawing.Size(75, 23);
            this.btnPullLatest.TabIndex = 4;
            this.btnPullLatest.Text = "&Pull latest";
            this.btnPullLatest.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(916, 367);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Commit && Push";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(673, 367);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // btnShowLog
            // 
            this.btnShowLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowLog.Location = new System.Drawing.Point(835, 367);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(75, 23);
            this.btnShowLog.TabIndex = 5;
            this.btnShowLog.Text = "Show &log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(592, 367);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // FormCommit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1104, 601);
            this.Controls.Add(this.splitMain);
            this.KeyPreview = true;
            this.Name = "FormCommit";
            this.Text = "CozyGit - File List";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }

    internal class BetterScrollDataGridView : DataGridView
    {
        internal BetterScrollDataGridView()
        {
            this.SetDoubleBuffering();
            AutoGenerateColumns = false;
            #if DEBUG
            DataError += (object sender, DataGridViewDataErrorEventArgs e) => { throw new NotImplementedException(); };
            #endif
        }
        #if DEBUG
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e) { throw new NotImplementedException(); }
        #else
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e) { }
        #endif
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int rh = RowTemplate.Height, scrollRows = (Math.Abs(e.Delta) < rh ? (e.Delta < 0 ? -1 : 1) : (e.Delta / rh)), maxScroll = Math.Max((Height - ColumnHeadersHeight) / rh - 2, 1);
            FirstDisplayedScrollingRowIndex = Math.Min(Math.Max(FirstDisplayedScrollingRowIndex - Math.Min(Math.Max(scrollRows, -maxScroll), maxScroll), 0), Rows.Count - 1);
        }
    }
}