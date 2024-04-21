namespace SwordStudio.NET
{
    partial class SWORD
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SWORD));
            this.Main_TabControl = new System.Windows.Forms.TabControl();
            this.About_TabPage = new System.Windows.Forms.TabPage();
            this.ReadMe_About_Label = new System.Windows.Forms.Label();
            this.World_TabPage = new System.Windows.Forms.TabPage();
            this.World_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.ToolsBarGroup_World_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.New_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.Open_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.Save_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Select_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.Edit_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.Delete_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.NoPassBlock_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.EventBlock_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.LowLayer_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.HighLayer_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.NoPassLayer_ToolsBar_Word_Button = new UitlCtrls.UtilButton();
            this.button4 = new UitlCtrls.UtilButton();
            this.ContentBox_World_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.ActualBox_ContentBox_World_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.BitMapBox_ActualBox_World_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.Status_Word_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.Message_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.XBlock_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value_XBlock_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.YBlock_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value_YBlock_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Half_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value_Half_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.X_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value_X_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Y_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Value_Y_Status_Word_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Main_TabControl.SuspendLayout();
            this.About_TabPage.SuspendLayout();
            this.World_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.World_SplitContainer)).BeginInit();
            this.World_SplitContainer.Panel1.SuspendLayout();
            this.World_SplitContainer.Panel2.SuspendLayout();
            this.World_SplitContainer.SuspendLayout();
            this.ToolsBarGroup_World_FlowLayoutPanel.SuspendLayout();
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.SuspendLayout();
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.SuspendLayout();
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.SuspendLayout();
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContentBox_World_SplitContainer)).BeginInit();
            this.ContentBox_World_SplitContainer.Panel1.SuspendLayout();
            this.ContentBox_World_SplitContainer.Panel2.SuspendLayout();
            this.ContentBox_World_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActualBox_ContentBox_World_SplitContainer)).BeginInit();
            this.ActualBox_ContentBox_World_SplitContainer.Panel1.SuspendLayout();
            this.ActualBox_ContentBox_World_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BitMapBox_ActualBox_World_SplitContainer)).BeginInit();
            this.BitMapBox_ActualBox_World_SplitContainer.SuspendLayout();
            this.Status_Word_StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main_TabControl
            // 
            this.Main_TabControl.Controls.Add(this.About_TabPage);
            this.Main_TabControl.Controls.Add(this.World_TabPage);
            this.Main_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_TabControl.Location = new System.Drawing.Point(0, 0);
            this.Main_TabControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Main_TabControl.Multiline = true;
            this.Main_TabControl.Name = "Main_TabControl";
            this.Main_TabControl.SelectedIndex = 0;
            this.Main_TabControl.Size = new System.Drawing.Size(1262, 754);
            this.Main_TabControl.TabIndex = 2;
            // 
            // About_TabPage
            // 
            this.About_TabPage.Controls.Add(this.ReadMe_About_Label);
            this.About_TabPage.Location = new System.Drawing.Point(4, 25);
            this.About_TabPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.About_TabPage.Name = "About_TabPage";
            this.About_TabPage.Size = new System.Drawing.Size(1254, 725);
            this.About_TabPage.TabIndex = 0;
            this.About_TabPage.Text = "关于";
            this.About_TabPage.UseVisualStyleBackColor = true;
            // 
            // ReadMe_About_Label
            // 
            this.ReadMe_About_Label.AutoSize = true;
            this.ReadMe_About_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReadMe_About_Label.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ReadMe_About_Label.Location = new System.Drawing.Point(458, 327);
            this.ReadMe_About_Label.Name = "ReadMe_About_Label";
            this.ReadMe_About_Label.Size = new System.Drawing.Size(363, 94);
            this.ReadMe_About_Label.TabIndex = 0;
            this.ReadMe_About_Label.Text = "仙剑奇侠传 MOD 制作平台 By 仪拓诗（liuzhier）\r\n本程序使用 C# .NET Framework 3.5 编写\r\n主逻辑借自 SDLPAL\r\n感谢" +
    " SDLPAL TEAM 的帮助\r\n特别感谢 palxex、PalMusicFan";
            this.ReadMe_About_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ReadMe_About_Label.UseCompatibleTextRendering = true;
            // 
            // World_TabPage
            // 
            this.World_TabPage.BackColor = System.Drawing.Color.Transparent;
            this.World_TabPage.Controls.Add(this.World_SplitContainer);
            this.World_TabPage.Location = new System.Drawing.Point(4, 25);
            this.World_TabPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.World_TabPage.Name = "World_TabPage";
            this.World_TabPage.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.World_TabPage.Size = new System.Drawing.Size(1254, 725);
            this.World_TabPage.TabIndex = 1;
            this.World_TabPage.Text = "世界";
            // 
            // World_SplitContainer
            // 
            this.World_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.World_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.World_SplitContainer.IsSplitterFixed = true;
            this.World_SplitContainer.Location = new System.Drawing.Point(2, 3);
            this.World_SplitContainer.Name = "World_SplitContainer";
            this.World_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // World_SplitContainer.Panel1
            // 
            this.World_SplitContainer.Panel1.Controls.Add(this.ToolsBarGroup_World_FlowLayoutPanel);
            // 
            // World_SplitContainer.Panel2
            // 
            this.World_SplitContainer.Panel2.Controls.Add(this.ContentBox_World_SplitContainer);
            this.World_SplitContainer.Size = new System.Drawing.Size(1250, 719);
            this.World_SplitContainer.SplitterDistance = 58;
            this.World_SplitContainer.TabIndex = 0;
            // 
            // ToolsBarGroup_World_FlowLayoutPanel
            // 
            this.ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.MainMode_ToolsBarGroup_World_FlowLayoutPanel);
            this.ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.EditMode_ToolsBarGroup_World_FlowLayoutPanel);
            this.ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel);
            this.ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel);
            this.ToolsBarGroup_World_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsBarGroup_World_FlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsBarGroup_World_FlowLayoutPanel.Name = "ToolsBarGroup_World_FlowLayoutPanel";
            this.ToolsBarGroup_World_FlowLayoutPanel.Size = new System.Drawing.Size(1248, 56);
            this.ToolsBarGroup_World_FlowLayoutPanel.TabIndex = 0;
            // 
            // MainMode_ToolsBarGroup_World_FlowLayoutPanel
            // 
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.AutoSize = true;
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.New_ToolsBar_Word_Button);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.Open_ToolsBar_Word_Button);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.Save_ToolsBar_Word_Button);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Name = "MainMode_ToolsBarGroup_World_FlowLayoutPanel";
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.Size = new System.Drawing.Size(170, 58);
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.TabIndex = 2;
            // 
            // New_ToolsBar_Word_Button
            // 
            this.New_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.New_ToolsBar_Word_Button.ButtonTipText = "新建场景";
            this.New_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.New_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.New_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_New;
            this.New_ToolsBar_Word_Button.Location = new System.Drawing.Point(3, 3);
            this.New_ToolsBar_Word_Button.Name = "New_ToolsBar_Word_Button";
            this.New_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.New_ToolsBar_Word_Button.TabIndex = 1;
            this.New_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // Open_ToolsBar_Word_Button
            // 
            this.Open_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.Open_ToolsBar_Word_Button.ButtonTipText = "载入场景";
            this.Open_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.Open_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Open_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Open;
            this.Open_ToolsBar_Word_Button.Location = new System.Drawing.Point(59, 3);
            this.Open_ToolsBar_Word_Button.Name = "Open_ToolsBar_Word_Button";
            this.Open_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.Open_ToolsBar_Word_Button.TabIndex = 2;
            this.Open_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // Save_ToolsBar_Word_Button
            // 
            this.Save_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.Save_ToolsBar_Word_Button.ButtonTipText = "保存场景";
            this.Save_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.Save_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Save;
            this.Save_ToolsBar_Word_Button.Location = new System.Drawing.Point(115, 3);
            this.Save_ToolsBar_Word_Button.Name = "Save_ToolsBar_Word_Button";
            this.Save_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.Save_ToolsBar_Word_Button.TabIndex = 3;
            this.Save_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // EditMode_ToolsBarGroup_World_FlowLayoutPanel
            // 
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.AutoSize = true;
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.Select_ToolsBar_Word_Button);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.Edit_ToolsBar_Word_Button);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.Delete_ToolsBar_Word_Button);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Location = new System.Drawing.Point(170, 0);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Name = "EditMode_ToolsBarGroup_World_FlowLayoutPanel";
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.Size = new System.Drawing.Size(170, 58);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.TabIndex = 5;
            // 
            // Select_ToolsBar_Word_Button
            // 
            this.Select_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.Select_ToolsBar_Word_Button.ButtonTipText = "选择元素";
            this.Select_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.Select_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.Select_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Select_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Select;
            this.Select_ToolsBar_Word_Button.Location = new System.Drawing.Point(3, 3);
            this.Select_ToolsBar_Word_Button.Name = "Select_ToolsBar_Word_Button";
            this.Select_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.Select_ToolsBar_Word_Button.TabIndex = 4;
            this.Select_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // Edit_ToolsBar_Word_Button
            // 
            this.Edit_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.Edit_ToolsBar_Word_Button.ButtonTipText = "编辑元素";
            this.Edit_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.Edit_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.Edit_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Edit_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Edit;
            this.Edit_ToolsBar_Word_Button.Location = new System.Drawing.Point(59, 3);
            this.Edit_ToolsBar_Word_Button.Name = "Edit_ToolsBar_Word_Button";
            this.Edit_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.Edit_ToolsBar_Word_Button.TabIndex = 5;
            this.Edit_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // Delete_ToolsBar_Word_Button
            // 
            this.Delete_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.Delete_ToolsBar_Word_Button.ButtonTipText = "删除元素";
            this.Delete_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.Delete_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.Delete_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Delete_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Delete;
            this.Delete_ToolsBar_Word_Button.Location = new System.Drawing.Point(115, 3);
            this.Delete_ToolsBar_Word_Button.Name = "Delete_ToolsBar_Word_Button";
            this.Delete_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.Delete_ToolsBar_Word_Button.TabIndex = 6;
            this.Delete_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel
            // 
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.AutoSize = true;
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.NoPassBlock_ToolsBar_Word_Button);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.EventBlock_ToolsBar_Word_Button);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Location = new System.Drawing.Point(340, 0);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Name = "BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel";
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Size = new System.Drawing.Size(114, 58);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.TabIndex = 6;
            // 
            // NoPassBlock_ToolsBar_Word_Button
            // 
            this.NoPassBlock_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.NoPassBlock_ToolsBar_Word_Button.ButtonTipText = "显示障碍块";
            this.NoPassBlock_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.CheckBox;
            this.NoPassBlock_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.NoPassBlock_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoPassBlock_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_NoPass;
            this.NoPassBlock_ToolsBar_Word_Button.Location = new System.Drawing.Point(3, 3);
            this.NoPassBlock_ToolsBar_Word_Button.Name = "NoPassBlock_ToolsBar_Word_Button";
            this.NoPassBlock_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.NoPassBlock_ToolsBar_Word_Button.TabIndex = 7;
            this.NoPassBlock_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // EventBlock_ToolsBar_Word_Button
            // 
            this.EventBlock_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.EventBlock_ToolsBar_Word_Button.ButtonTipText = "显示事件块";
            this.EventBlock_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.CheckBox;
            this.EventBlock_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.EventBlock_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EventBlock_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock;
            this.EventBlock_ToolsBar_Word_Button.Location = new System.Drawing.Point(59, 3);
            this.EventBlock_ToolsBar_Word_Button.Name = "EventBlock_ToolsBar_Word_Button";
            this.EventBlock_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.EventBlock_ToolsBar_Word_Button.TabIndex = 8;
            this.EventBlock_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            this.EventBlock_ToolsBar_Word_Button.Click += new System.EventHandler(this.EventBlock_ToolsBar_Word_Button_Click);
            // 
            // LayerMode_ToolsBarGroup_World_FlowLayoutPanel
            // 
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.AutoSize = true;
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.LowLayer_ToolsBar_Word_Button);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.HighLayer_ToolsBar_Word_Button);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.NoPassLayer_ToolsBar_Word_Button);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Controls.Add(this.button4);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Location = new System.Drawing.Point(454, 0);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Name = "LayerMode_ToolsBarGroup_World_FlowLayoutPanel";
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Size = new System.Drawing.Size(226, 58);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.TabIndex = 7;
            // 
            // LowLayer_ToolsBar_Word_Button
            // 
            this.LowLayer_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.LowLayer_ToolsBar_Word_Button.ButtonTipText = "地面下层";
            this.LowLayer_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.LowLayer_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.LowLayer_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LowLayer_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Low;
            this.LowLayer_ToolsBar_Word_Button.Location = new System.Drawing.Point(3, 3);
            this.LowLayer_ToolsBar_Word_Button.Name = "LowLayer_ToolsBar_Word_Button";
            this.LowLayer_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.LowLayer_ToolsBar_Word_Button.TabIndex = 9;
            this.LowLayer_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // HighLayer_ToolsBar_Word_Button
            // 
            this.HighLayer_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.HighLayer_ToolsBar_Word_Button.ButtonTipText = "地面上层";
            this.HighLayer_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.HighLayer_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.HighLayer_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HighLayer_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_High;
            this.HighLayer_ToolsBar_Word_Button.Location = new System.Drawing.Point(59, 3);
            this.HighLayer_ToolsBar_Word_Button.Name = "HighLayer_ToolsBar_Word_Button";
            this.HighLayer_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.HighLayer_ToolsBar_Word_Button.TabIndex = 10;
            this.HighLayer_ToolsBar_Word_Button.UseVisualStyleBackColor = false;
            // 
            // NoPassLayer_ToolsBar_Word_Button
            // 
            this.NoPassLayer_ToolsBar_Word_Button.BackColor = System.Drawing.Color.Transparent;
            this.NoPassLayer_ToolsBar_Word_Button.ButtonTipText = "障碍层";
            this.NoPassLayer_ToolsBar_Word_Button.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.NoPassLayer_ToolsBar_Word_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.NoPassLayer_ToolsBar_Word_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoPassLayer_ToolsBar_Word_Button.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_NoPassBlock;
            this.NoPassLayer_ToolsBar_Word_Button.Location = new System.Drawing.Point(115, 3);
            this.NoPassLayer_ToolsBar_Word_Button.Name = "NoPassLayer_ToolsBar_Word_Button";
            this.NoPassLayer_ToolsBar_Word_Button.Size = new System.Drawing.Size(50, 50);
            this.NoPassLayer_ToolsBar_Word_Button.TabIndex = 11;
            this.NoPassLayer_ToolsBar_Word_Button.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.ButtonTipText = "事件层";
            this.button4.ButtonType = UitlCtrls.UtilButton.UtilButtonType.Radio;
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = global::SwordStudio.NET.Properties.Resources.ToolBar_Event;
            this.button4.Location = new System.Drawing.Point(171, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 50);
            this.button4.TabIndex = 12;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // ContentBox_World_SplitContainer
            // 
            this.ContentBox_World_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentBox_World_SplitContainer.IsSplitterFixed = true;
            this.ContentBox_World_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ContentBox_World_SplitContainer.Name = "ContentBox_World_SplitContainer";
            this.ContentBox_World_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ContentBox_World_SplitContainer.Panel1
            // 
            this.ContentBox_World_SplitContainer.Panel1.Controls.Add(this.ActualBox_ContentBox_World_SplitContainer);
            // 
            // ContentBox_World_SplitContainer.Panel2
            // 
            this.ContentBox_World_SplitContainer.Panel2.Controls.Add(this.Status_Word_StatusStrip);
            this.ContentBox_World_SplitContainer.Size = new System.Drawing.Size(1248, 655);
            this.ContentBox_World_SplitContainer.SplitterDistance = 620;
            this.ContentBox_World_SplitContainer.TabIndex = 0;
            // 
            // ActualBox_ContentBox_World_SplitContainer
            // 
            this.ActualBox_ContentBox_World_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActualBox_ContentBox_World_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActualBox_ContentBox_World_SplitContainer.IsSplitterFixed = true;
            this.ActualBox_ContentBox_World_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ActualBox_ContentBox_World_SplitContainer.Name = "ActualBox_ContentBox_World_SplitContainer";
            // 
            // ActualBox_ContentBox_World_SplitContainer.Panel1
            // 
            this.ActualBox_ContentBox_World_SplitContainer.Panel1.Controls.Add(this.BitMapBox_ActualBox_World_SplitContainer);
            this.ActualBox_ContentBox_World_SplitContainer.Size = new System.Drawing.Size(1248, 620);
            this.ActualBox_ContentBox_World_SplitContainer.SplitterDistance = 224;
            this.ActualBox_ContentBox_World_SplitContainer.TabIndex = 0;
            // 
            // BitMapBox_ActualBox_World_SplitContainer
            // 
            this.BitMapBox_ActualBox_World_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BitMapBox_ActualBox_World_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BitMapBox_ActualBox_World_SplitContainer.IsSplitterFixed = true;
            this.BitMapBox_ActualBox_World_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.BitMapBox_ActualBox_World_SplitContainer.Name = "BitMapBox_ActualBox_World_SplitContainer";
            this.BitMapBox_ActualBox_World_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BitMapBox_ActualBox_World_SplitContainer.Size = new System.Drawing.Size(222, 618);
            this.BitMapBox_ActualBox_World_SplitContainer.SplitterDistance = 59;
            this.BitMapBox_ActualBox_World_SplitContainer.TabIndex = 0;
            // 
            // Status_Word_StatusStrip
            // 
            this.Status_Word_StatusStrip.AutoSize = false;
            this.Status_Word_StatusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_Word_StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Status_Word_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Message_Status_Word_ToolStripStatusLabel,
            this.XBlock_Status_Word_ToolStripStatusLabel,
            this.Value_XBlock_Status_Word_ToolStripStatusLabel,
            this.YBlock_Status_Word_ToolStripStatusLabel,
            this.Value_YBlock_Status_Word_ToolStripStatusLabel,
            this.Half_Status_Word_ToolStripStatusLabel,
            this.Value_Half_Status_Word_ToolStripStatusLabel,
            this.X_Status_Word_ToolStripStatusLabel,
            this.Value_X_Status_Word_ToolStripStatusLabel,
            this.Y_Status_Word_ToolStripStatusLabel,
            this.Value_Y_Status_Word_ToolStripStatusLabel});
            this.Status_Word_StatusStrip.Location = new System.Drawing.Point(0, 0);
            this.Status_Word_StatusStrip.Name = "Status_Word_StatusStrip";
            this.Status_Word_StatusStrip.Size = new System.Drawing.Size(1248, 31);
            this.Status_Word_StatusStrip.TabIndex = 0;
            this.Status_Word_StatusStrip.Text = "statusStrip1";
            // 
            // Message_Status_Word_ToolStripStatusLabel
            // 
            this.Message_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Message_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.LightGray;
            this.Message_Status_Word_ToolStripStatusLabel.Name = "Message_Status_Word_ToolStripStatusLabel";
            this.Message_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(500, 25);
            this.Message_Status_Word_ToolStripStatusLabel.Text = "请选择欲编辑的场景";
            this.Message_Status_Word_ToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // XBlock_Status_Word_ToolStripStatusLabel
            // 
            this.XBlock_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.XBlock_Status_Word_ToolStripStatusLabel.Name = "XBlock_Status_Word_ToolStripStatusLabel";
            this.XBlock_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(66, 25);
            this.XBlock_Status_Word_ToolStripStatusLabel.Text = "XBlock: ";
            // 
            // Value_XBlock_Status_Word_ToolStripStatusLabel
            // 
            this.Value_XBlock_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Value_XBlock_Status_Word_ToolStripStatusLabel.Name = "Value_XBlock_Status_Word_ToolStripStatusLabel";
            this.Value_XBlock_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(79, 25);
            this.Value_XBlock_Status_Word_ToolStripStatusLabel.Text = "255(0xFF)";
            // 
            // YBlock_Status_Word_ToolStripStatusLabel
            // 
            this.YBlock_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.YBlock_Status_Word_ToolStripStatusLabel.Name = "YBlock_Status_Word_ToolStripStatusLabel";
            this.YBlock_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(65, 25);
            this.YBlock_Status_Word_ToolStripStatusLabel.Text = "YBlock: ";
            // 
            // Value_YBlock_Status_Word_ToolStripStatusLabel
            // 
            this.Value_YBlock_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Value_YBlock_Status_Word_ToolStripStatusLabel.Name = "Value_YBlock_Status_Word_ToolStripStatusLabel";
            this.Value_YBlock_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(79, 25);
            this.Value_YBlock_Status_Word_ToolStripStatusLabel.Text = "255(0xFF)";
            // 
            // Half_Status_Word_ToolStripStatusLabel
            // 
            this.Half_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.Half_Status_Word_ToolStripStatusLabel.Name = "Half_Status_Word_ToolStripStatusLabel";
            this.Half_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(46, 25);
            this.Half_Status_Word_ToolStripStatusLabel.Text = "Half: ";
            // 
            // Value_Half_Status_Word_ToolStripStatusLabel
            // 
            this.Value_Half_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Value_Half_Status_Word_ToolStripStatusLabel.Name = "Value_Half_Status_Word_ToolStripStatusLabel";
            this.Value_Half_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(79, 25);
            this.Value_Half_Status_Word_ToolStripStatusLabel.Text = "0";
            // 
            // X_Status_Word_ToolStripStatusLabel
            // 
            this.X_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.X_Status_Word_ToolStripStatusLabel.Name = "X_Status_Word_ToolStripStatusLabel";
            this.X_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(27, 25);
            this.X_Status_Word_ToolStripStatusLabel.Text = "X: ";
            // 
            // Value_X_Status_Word_ToolStripStatusLabel
            // 
            this.Value_X_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Value_X_Status_Word_ToolStripStatusLabel.Name = "Value_X_Status_Word_ToolStripStatusLabel";
            this.Value_X_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(115, 25);
            this.Value_X_Status_Word_ToolStripStatusLabel.Text = "65535(0XFFFF)";
            // 
            // Y_Status_Word_ToolStripStatusLabel
            // 
            this.Y_Status_Word_ToolStripStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.Y_Status_Word_ToolStripStatusLabel.Name = "Y_Status_Word_ToolStripStatusLabel";
            this.Y_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(26, 25);
            this.Y_Status_Word_ToolStripStatusLabel.Text = "Y: ";
            // 
            // Value_Y_Status_Word_ToolStripStatusLabel
            // 
            this.Value_Y_Status_Word_ToolStripStatusLabel.AutoSize = false;
            this.Value_Y_Status_Word_ToolStripStatusLabel.Name = "Value_Y_Status_Word_ToolStripStatusLabel";
            this.Value_Y_Status_Word_ToolStripStatusLabel.Size = new System.Drawing.Size(115, 25);
            this.Value_Y_Status_Word_ToolStripStatusLabel.Text = "65535(0XFFFF)";
            // 
            // SWORD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 754);
            this.Controls.Add(this.Main_TabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1280, 800);
            this.MinimumSize = new System.Drawing.Size(1280, 800);
            this.Name = "SWORD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SWORD Studio";
            this.Main_TabControl.ResumeLayout(false);
            this.About_TabPage.ResumeLayout(false);
            this.About_TabPage.PerformLayout();
            this.World_TabPage.ResumeLayout(false);
            this.World_SplitContainer.Panel1.ResumeLayout(false);
            this.World_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.World_SplitContainer)).EndInit();
            this.World_SplitContainer.ResumeLayout(false);
            this.ToolsBarGroup_World_FlowLayoutPanel.ResumeLayout(false);
            this.ToolsBarGroup_World_FlowLayoutPanel.PerformLayout();
            this.MainMode_ToolsBarGroup_World_FlowLayoutPanel.ResumeLayout(false);
            this.EditMode_ToolsBarGroup_World_FlowLayoutPanel.ResumeLayout(false);
            this.BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.ResumeLayout(false);
            this.LayerMode_ToolsBarGroup_World_FlowLayoutPanel.ResumeLayout(false);
            this.ContentBox_World_SplitContainer.Panel1.ResumeLayout(false);
            this.ContentBox_World_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContentBox_World_SplitContainer)).EndInit();
            this.ContentBox_World_SplitContainer.ResumeLayout(false);
            this.ActualBox_ContentBox_World_SplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ActualBox_ContentBox_World_SplitContainer)).EndInit();
            this.ActualBox_ContentBox_World_SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BitMapBox_ActualBox_World_SplitContainer)).EndInit();
            this.BitMapBox_ActualBox_World_SplitContainer.ResumeLayout(false);
            this.Status_Word_StatusStrip.ResumeLayout(false);
            this.Status_Word_StatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Main_TabControl;
        private System.Windows.Forms.TabPage About_TabPage;
        private System.Windows.Forms.Label ReadMe_About_Label;
        private System.Windows.Forms.TabPage World_TabPage;
        private System.Windows.Forms.SplitContainer World_SplitContainer;
        private System.Windows.Forms.FlowLayoutPanel ToolsBarGroup_World_FlowLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel MainMode_ToolsBarGroup_World_FlowLayoutPanel;
        private UitlCtrls.UtilButton New_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton Open_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton Save_ToolsBar_Word_Button;
        private System.Windows.Forms.SplitContainer ContentBox_World_SplitContainer;
        private System.Windows.Forms.StatusStrip Status_Word_StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Message_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel XBlock_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Value_XBlock_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YBlock_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Value_YBlock_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Half_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Value_Half_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel X_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Value_X_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Y_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Value_Y_Status_Word_ToolStripStatusLabel;
        private System.Windows.Forms.SplitContainer ActualBox_ContentBox_World_SplitContainer;
        private System.Windows.Forms.FlowLayoutPanel EditMode_ToolsBarGroup_World_FlowLayoutPanel;
        private UitlCtrls.UtilButton Select_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton Edit_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton Delete_ToolsBar_Word_Button;
        private System.Windows.Forms.FlowLayoutPanel BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel;
        private UitlCtrls.UtilButton NoPassBlock_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton EventBlock_ToolsBar_Word_Button;
        private System.Windows.Forms.FlowLayoutPanel LayerMode_ToolsBarGroup_World_FlowLayoutPanel;
        private UitlCtrls.UtilButton LowLayer_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton HighLayer_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton NoPassLayer_ToolsBar_Word_Button;
        private UitlCtrls.UtilButton button4;
        private System.Windows.Forms.SplitContainer BitMapBox_ActualBox_World_SplitContainer;
    }
}

