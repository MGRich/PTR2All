namespace PTR2All
{
    partial class MainWindow
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.openISOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.fileRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.wp2Tab = new System.Windows.Forms.TabPage();
            this.intTab = new System.Windows.Forms.TabPage();
            this.intList = new System.Windows.Forms.ListBox();
            this.tm2Tab = new System.Windows.Forms.TabPage();
            this.tm2Image = new Rainbow.App.GUI.Controls.TransparentPictureBox();
            this.menuStrip.SuspendLayout();
            this.fileRightClick.SuspendLayout();
            this.tabs.SuspendLayout();
            this.intTab.SuspendLayout();
            this.tm2Tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openISOToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(6, 4, 0, 5);
            this.menuStrip.Size = new System.Drawing.Size(800, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // openISOToolStripMenuItem
            // 
            this.openISOToolStripMenuItem.Name = "openISOToolStripMenuItem";
            this.openISOToolStripMenuItem.Size = new System.Drawing.Size(69, 19);
            this.openISOToolStripMenuItem.Text = "Open ISO";
            this.openISOToolStripMenuItem.Click += new System.EventHandler(this.isoOpen);
            // 
            // fileTree
            // 
            this.fileTree.Location = new System.Drawing.Point(13, 39);
            this.fileTree.Name = "fileTree";
            this.fileTree.Size = new System.Drawing.Size(138, 399);
            this.fileTree.TabIndex = 2;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.moveTab);
            this.fileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeContextMenu);
            // 
            // fileRightClick
            // 
            this.fileRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFileToolStripMenuItem,
            this.replaceFileToolStripMenuItem});
            this.fileRightClick.Name = "fileRightClick";
            this.fileRightClick.Size = new System.Drawing.Size(141, 48);
            // 
            // extractFileToolStripMenuItem
            // 
            this.extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
            this.extractFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.extractFileToolStripMenuItem.Text = "Extract file..";
            this.extractFileToolStripMenuItem.Click += new System.EventHandler(this.extractFile);
            // 
            // replaceFileToolStripMenuItem
            // 
            this.replaceFileToolStripMenuItem.Name = "replaceFileToolStripMenuItem";
            this.replaceFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.replaceFileToolStripMenuItem.Text = "Replace file..";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.wp2Tab);
            this.tabs.Controls.Add(this.intTab);
            this.tabs.Controls.Add(this.tm2Tab);
            this.tabs.Location = new System.Drawing.Point(157, 39);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(631, 399);
            this.tabs.TabIndex = 4;
            // 
            // wp2Tab
            // 
            this.wp2Tab.Location = new System.Drawing.Point(4, 22);
            this.wp2Tab.Name = "wp2Tab";
            this.wp2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.wp2Tab.Size = new System.Drawing.Size(623, 373);
            this.wp2Tab.TabIndex = 0;
            this.wp2Tab.Text = "WP2 Export/Import";
            this.wp2Tab.UseVisualStyleBackColor = true;
            // 
            // intTab
            // 
            this.intTab.Controls.Add(this.intList);
            this.intTab.Location = new System.Drawing.Point(4, 22);
            this.intTab.Name = "intTab";
            this.intTab.Padding = new System.Windows.Forms.Padding(3);
            this.intTab.Size = new System.Drawing.Size(623, 373);
            this.intTab.TabIndex = 1;
            this.intTab.Text = "INT Editing";
            this.intTab.UseVisualStyleBackColor = true;
            // 
            // intList
            // 
            this.intList.FormattingEnabled = true;
            this.intList.Location = new System.Drawing.Point(6, 16);
            this.intList.Name = "intList";
            this.intList.Size = new System.Drawing.Size(108, 342);
            this.intList.TabIndex = 0;
            // 
            // tm2Tab
            // 
            this.tm2Tab.Controls.Add(this.tm2Image);
            this.tm2Tab.Location = new System.Drawing.Point(4, 22);
            this.tm2Tab.Name = "tm2Tab";
            this.tm2Tab.Size = new System.Drawing.Size(623, 373);
            this.tm2Tab.TabIndex = 2;
            this.tm2Tab.Text = "TM2 Export/Import";
            this.tm2Tab.UseVisualStyleBackColor = true;
            // 
            // tm2Image
            // 
            this.tm2Image.Chessboard = true;
            this.tm2Image.Location = new System.Drawing.Point(211, 30);
            this.tm2Image.Name = "tm2Image";
            this.tm2Image.Size = new System.Drawing.Size(200, 200);
            this.tm2Image.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fileTree);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.fileRightClick.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.intTab.ResumeLayout(false);
            this.tm2Tab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem openISOToolStripMenuItem;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.ContextMenuStrip fileRightClick;
        private System.Windows.Forms.ToolStripMenuItem extractFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceFileToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage wp2Tab;
        private System.Windows.Forms.TabPage intTab;
        private System.Windows.Forms.TabPage tm2Tab;
        private System.Windows.Forms.ListBox intList;
        private Rainbow.App.GUI.Controls.TransparentPictureBox tm2Image;
    }
}

