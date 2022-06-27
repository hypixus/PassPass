namespace PassPassForms
{
    partial class ManagerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.collListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewCollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entriesListBox = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // collListBox
            // 
            this.collListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.collListBox.FormattingEnabled = true;
            this.collListBox.ItemHeight = 15;
            this.collListBox.Location = new System.Drawing.Point(0, 27);
            this.collListBox.Name = "collListBox";
            this.collListBox.Size = new System.Drawing.Size(192, 349);
            this.collListBox.TabIndex = 1;
            this.collListBox.SelectedIndexChanged += new System.EventHandler(this.CollListBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.NewGroupToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(712, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.NewToolStripMenuItem.Text = "New";
            this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // NewGroupToolStripMenuItem
            // 
            this.NewGroupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewCollToolStripMenuItem,
            this.NewEntryToolStripMenuItem});
            this.NewGroupToolStripMenuItem.Name = "NewGroupToolStripMenuItem";
            this.NewGroupToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.NewGroupToolStripMenuItem.Text = "New...";
            // 
            // NewCollToolStripMenuItem
            // 
            this.NewCollToolStripMenuItem.Name = "NewCollToolStripMenuItem";
            this.NewCollToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NewCollToolStripMenuItem.Text = "Collection";
            this.NewCollToolStripMenuItem.Click += new System.EventHandler(this.NewCollToolStripMenuItem_Click);
            // 
            // NewEntryToolStripMenuItem
            // 
            this.NewEntryToolStripMenuItem.Name = "NewEntryToolStripMenuItem";
            this.NewEntryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NewEntryToolStripMenuItem.Text = "Entry";
            this.NewEntryToolStripMenuItem.Click += new System.EventHandler(this.NewEntryToolStripMenuItem_Click);
            // 
            // entriesListBox
            // 
            this.entriesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entriesListBox.FormattingEnabled = true;
            this.entriesListBox.ItemHeight = 15;
            this.entriesListBox.Location = new System.Drawing.Point(198, 27);
            this.entriesListBox.Name = "entriesListBox";
            this.entriesListBox.Size = new System.Drawing.Size(514, 349);
            this.entriesListBox.TabIndex = 3;
            this.entriesListBox.SelectedIndexChanged += new System.EventHandler(this.EntriesListBox_SelectedIndexChanged);
            this.entriesListBox.DoubleClick += new System.EventHandler(this.EntriesListBox_DoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 385);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(712, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel.Text = "Ready.";
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 407);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.entriesListBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.collListBox);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ManagerForm";
            this.Text = "PassPassForms";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox collListBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem NewToolStripMenuItem;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private ListBox entriesListBox;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripMenuItem NewGroupToolStripMenuItem;
        private ToolStripMenuItem NewCollToolStripMenuItem;
        private ToolStripMenuItem NewEntryToolStripMenuItem;
    }
}