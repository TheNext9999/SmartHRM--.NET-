namespace SmartHRM.GUI
{
    partial class OrgChartForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbAddRoot = new System.Windows.Forms.ToolStripButton();
            this.tsbAddChild = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbExpand = new System.Windows.Forms.ToolStripButton();
            this.tsbCollapse = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.txtFind = new System.Windows.Forms.ToolStripTextBox();
            this.lblFind = new System.Windows.Forms.ToolStripLabel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvOrg = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAddChild = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSep = new System.Windows.Forms.ToolStripSeparator();
            this.cmsExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTreeTitle = new System.Windows.Forms.Label();
            this.dgvEmployees = new System.Windows.Forms.DataGridView();
            this.pnlGridHeader = new System.Windows.Forms.Panel();
            this.chkSub = new System.Windows.Forms.CheckBox();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfoDesc = new System.Windows.Forms.Label();
            this.lblInfoCounts = new System.Windows.Forms.Label();
            this.lblInfoManager = new System.Windows.Forms.Label();
            this.lblInfoParent = new System.Windows.Forms.Label();
            this.lblInfoCode = new System.Windows.Forms.Label();
            this.lblInfoName = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDragHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.cmsTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).BeginInit();
            this.pnlGridHeader.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddRoot,
            this.tsbAddChild,
            this.tsbEdit,
            this.tsbDelete,
            this.tss1,
            this.tsbRefresh,
            this.tsbExpand,
            this.tsbCollapse,
            this.tsbExport,
            this.txtFind,
            this.lblFind});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tsMain.Size = new System.Drawing.Size(1100, 31);
            this.tsMain.TabIndex = 0;
            // 
            // tsbAddRoot
            // 
            this.tsbAddRoot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAddRoot.Name = "tsbAddRoot";
            this.tsbAddRoot.Size = new System.Drawing.Size(116, 20);
            this.tsbAddRoot.Text = "🏢 Thêm đơn vị gốc";
            this.tsbAddRoot.Click += new System.EventHandler(this.tsbAddRoot_Click);
            // 
            // tsbAddChild
            // 
            this.tsbAddChild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAddChild.Name = "tsbAddChild";
            this.tsbAddChild.Size = new System.Drawing.Size(116, 20);
            this.tsbAddChild.Text = "➕ Thêm đơn vị con";
            this.tsbAddChild.Click += new System.EventHandler(this.tsbAddChild_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(45, 20);
            this.tsbEdit.Text = "✏️ Sửa";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(46, 20);
            this.tsbDelete.Text = "🗑 Xóa";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tss1
            // 
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(6, 23);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(73, 20);
            this.tsbRefresh.Text = "🔄 Làm mới";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbExpand
            // 
            this.tsbExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExpand.Name = "tsbExpand";
            this.tsbExpand.Size = new System.Drawing.Size(70, 20);
            this.tsbExpand.Text = "▼ Mở rộng";
            this.tsbExpand.Click += new System.EventHandler(this.tsbExpand_Click);
            // 
            // tsbCollapse
            // 
            this.tsbCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCollapse.Name = "tsbCollapse";
            this.tsbCollapse.Size = new System.Drawing.Size(69, 20);
            this.tsbCollapse.Text = "▲ Thu gọn";
            this.tsbCollapse.Click += new System.EventHandler(this.tsbCollapse_Click);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(126, 20);
            this.tsbExport.Text = "📊 Xuất danh sách NV";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // txtFind
            // 
            this.txtFind.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtFind.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(200, 23);
            this.txtFind.ToolTipText = "Gõ tên hoặc mã đơn vị, Enter để nhảy tới kết quả tiếp theo";
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // lblFind
            // 
            this.lblFind.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(82, 20);
            this.lblFind.Text = "🔍 Tìm đơn vị:";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 31);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tvOrg);
            this.splitMain.Panel1.Controls.Add(this.lblTreeTitle);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.dgvEmployees);
            this.splitMain.Panel2.Controls.Add(this.pnlGridHeader);
            this.splitMain.Panel2.Controls.Add(this.pnlInfo);
            this.splitMain.Size = new System.Drawing.Size(1100, 567);
            this.splitMain.SplitterDistance = 380;
            this.splitMain.TabIndex = 1;
            // 
            // tvOrg
            // 
            this.tvOrg.AllowDrop = true;
            this.tvOrg.ContextMenuStrip = this.cmsTree;
            this.tvOrg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrg.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvOrg.FullRowSelect = true;
            this.tvOrg.HideSelection = false;
            this.tvOrg.ItemHeight = 26;
            this.tvOrg.Location = new System.Drawing.Point(0, 30);
            this.tvOrg.Name = "tvOrg";
            this.tvOrg.Size = new System.Drawing.Size(380, 537);
            this.tvOrg.TabIndex = 0;
            this.tvOrg.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvOrg_ItemDrag);
            this.tvOrg.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvOrg_AfterSelect);
            this.tvOrg.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvOrg_NodeMouseClick);
            this.tvOrg.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvOrg_DragDrop);
            this.tvOrg.DragOver += new System.Windows.Forms.DragEventHandler(this.tvOrg_DragOver);
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAddChild,
            this.cmsEdit,
            this.cmsDelete,
            this.cmsSep,
            this.cmsExpand});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(180, 98);
            // 
            // cmsAddChild
            // 
            this.cmsAddChild.Name = "cmsAddChild";
            this.cmsAddChild.Size = new System.Drawing.Size(179, 22);
            this.cmsAddChild.Text = "Thêm đơn vị con";
            this.cmsAddChild.Click += new System.EventHandler(this.tsbAddChild_Click);
            // 
            // cmsEdit
            // 
            this.cmsEdit.Name = "cmsEdit";
            this.cmsEdit.Size = new System.Drawing.Size(179, 22);
            this.cmsEdit.Text = "Sửa";
            this.cmsEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.Size = new System.Drawing.Size(179, 22);
            this.cmsDelete.Text = "Xóa";
            this.cmsDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // cmsSep
            // 
            this.cmsSep.Name = "cmsSep";
            this.cmsSep.Size = new System.Drawing.Size(176, 6);
            // 
            // cmsExpand
            // 
            this.cmsExpand.Name = "cmsExpand";
            this.cmsExpand.Size = new System.Drawing.Size(179, 22);
            this.cmsExpand.Text = "Mở rộng nhánh này";
            this.cmsExpand.Click += new System.EventHandler(this.cmsExpand_Click);
            // 
            // lblTreeTitle
            // 
            this.lblTreeTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblTreeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTreeTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTreeTitle.ForeColor = System.Drawing.Color.White;
            this.lblTreeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTreeTitle.Name = "lblTreeTitle";
            this.lblTreeTitle.Padding = new System.Windows.Forms.Padding(8, 6, 0, 0);
            this.lblTreeTitle.Size = new System.Drawing.Size(380, 30);
            this.lblTreeTitle.TabIndex = 1;
            this.lblTreeTitle.Text = "Cơ cấu tổ chức   (số NV đang làm)";
            // 
            // dgvEmployees
            // 
            this.dgvEmployees.AllowUserToAddRows = false;
            this.dgvEmployees.AllowUserToDeleteRows = false;
            this.dgvEmployees.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.dgvEmployees.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvEmployees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvEmployees.BackgroundColor = System.Drawing.Color.White;
            this.dgvEmployees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.dgvEmployees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvEmployees.ColumnHeadersHeight = 30;
            this.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEmployees.EnableHeadersVisualStyles = false;
            this.dgvEmployees.Location = new System.Drawing.Point(0, 166);
            this.dgvEmployees.MultiSelect = false;
            this.dgvEmployees.Name = "dgvEmployees";
            this.dgvEmployees.ReadOnly = true;
            this.dgvEmployees.RowHeadersVisible = false;
            this.dgvEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmployees.Size = new System.Drawing.Size(716, 401);
            this.dgvEmployees.TabIndex = 0;
            // 
            // pnlGridHeader
            // 
            this.pnlGridHeader.Controls.Add(this.chkSub);
            this.pnlGridHeader.Controls.Add(this.lblGridTitle);
            this.pnlGridHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGridHeader.Location = new System.Drawing.Point(0, 132);
            this.pnlGridHeader.Name = "pnlGridHeader";
            this.pnlGridHeader.Size = new System.Drawing.Size(716, 34);
            this.pnlGridHeader.TabIndex = 1;
            // 
            // chkSub
            // 
            this.chkSub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSub.AutoSize = true;
            this.chkSub.Checked = true;
            this.chkSub.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSub.Location = new System.Drawing.Point(306, 7);
            this.chkSub.Name = "chkSub";
            this.chkSub.Size = new System.Drawing.Size(228, 21);
            this.chkSub.TabIndex = 0;
            this.chkSub.Text = "Bao gồm nhân viên của đơn vị con";
            this.chkSub.UseVisualStyleBackColor = true;
            this.chkSub.CheckedChanged += new System.EventHandler(this.chkSub_CheckedChanged);
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.AutoSize = true;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.Location = new System.Drawing.Point(12, 8);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(137, 17);
            this.lblGridTitle.TabIndex = 1;
            this.lblGridTitle.Text = "Danh sách nhân viên";
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(249)))));
            this.pnlInfo.Controls.Add(this.lblInfoDesc);
            this.pnlInfo.Controls.Add(this.lblInfoCounts);
            this.pnlInfo.Controls.Add(this.lblInfoManager);
            this.pnlInfo.Controls.Add(this.lblInfoParent);
            this.pnlInfo.Controls.Add(this.lblInfoCode);
            this.pnlInfo.Controls.Add(this.lblInfoName);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(716, 132);
            this.pnlInfo.TabIndex = 2;
            // 
            // lblInfoDesc
            // 
            this.lblInfoDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfoDesc.ForeColor = System.Drawing.Color.DimGray;
            this.lblInfoDesc.Location = new System.Drawing.Point(330, 44);
            this.lblInfoDesc.Name = "lblInfoDesc";
            this.lblInfoDesc.Size = new System.Drawing.Size(370, 80);
            this.lblInfoDesc.TabIndex = 0;
            // 
            // lblInfoCounts
            // 
            this.lblInfoCounts.AutoSize = true;
            this.lblInfoCounts.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoCounts.Location = new System.Drawing.Point(14, 106);
            this.lblInfoCounts.Name = "lblInfoCounts";
            this.lblInfoCounts.Size = new System.Drawing.Size(0, 17);
            this.lblInfoCounts.TabIndex = 1;
            // 
            // lblInfoManager
            // 
            this.lblInfoManager.AutoSize = true;
            this.lblInfoManager.Location = new System.Drawing.Point(14, 84);
            this.lblInfoManager.Name = "lblInfoManager";
            this.lblInfoManager.Size = new System.Drawing.Size(0, 17);
            this.lblInfoManager.TabIndex = 2;
            // 
            // lblInfoParent
            // 
            this.lblInfoParent.AutoSize = true;
            this.lblInfoParent.Location = new System.Drawing.Point(14, 64);
            this.lblInfoParent.Name = "lblInfoParent";
            this.lblInfoParent.Size = new System.Drawing.Size(0, 17);
            this.lblInfoParent.TabIndex = 3;
            // 
            // lblInfoCode
            // 
            this.lblInfoCode.AutoSize = true;
            this.lblInfoCode.Location = new System.Drawing.Point(14, 44);
            this.lblInfoCode.Name = "lblInfoCode";
            this.lblInfoCode.Size = new System.Drawing.Size(0, 17);
            this.lblInfoCode.TabIndex = 4;
            // 
            // lblInfoName
            // 
            this.lblInfoName.AutoSize = true;
            this.lblInfoName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblInfoName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.lblInfoName.Location = new System.Drawing.Point(12, 8);
            this.lblInfoName.Name = "lblInfoName";
            this.lblInfoName.Size = new System.Drawing.Size(172, 25);
            this.lblInfoName.TabIndex = 5;
            this.lblInfoName.Text = "(chọn một đơn vị)";
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblDragHint});
            this.statusMain.Location = new System.Drawing.Point(0, 598);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1100, 22);
            this.statusMain.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // lblDragHint
            // 
            this.lblDragHint.ForeColor = System.Drawing.Color.Gray;
            this.lblDragHint.Name = "lblDragHint";
            this.lblDragHint.Size = new System.Drawing.Size(1085, 17);
            this.lblDragHint.Spring = true;
            this.lblDragHint.Text = "Kéo thả một đơn vị lên đơn vị khác để đổi cấp trên; thả ra vùng trống để chuyển t" +
    "hành đơn vị gốc";
            this.lblDragHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrgChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 620);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "OrgChartForm";
            this.Text = "Sơ đồ tổ chức";
            this.Load += new System.EventHandler(this.OrgChartForm_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.cmsTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).EndInit();
            this.pnlGridHeader.ResumeLayout(false);
            this.pnlGridHeader.PerformLayout();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbAddRoot;
        private System.Windows.Forms.ToolStripButton tsbAddChild;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbExpand;
        private System.Windows.Forms.ToolStripButton tsbCollapse;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripTextBox txtFind;
        private System.Windows.Forms.ToolStripLabel lblFind;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvOrg;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem cmsAddChild;
        private System.Windows.Forms.ToolStripMenuItem cmsEdit;
        private System.Windows.Forms.ToolStripMenuItem cmsDelete;
        private System.Windows.Forms.ToolStripSeparator cmsSep;
        private System.Windows.Forms.ToolStripMenuItem cmsExpand;
        private System.Windows.Forms.Label lblTreeTitle;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.Panel pnlGridHeader;
        private System.Windows.Forms.CheckBox chkSub;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblInfoDesc;
        private System.Windows.Forms.Label lblInfoCounts;
        private System.Windows.Forms.Label lblInfoManager;
        private System.Windows.Forms.Label lblInfoParent;
        private System.Windows.Forms.Label lblInfoCode;
        private System.Windows.Forms.Label lblInfoName;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblDragHint;
    }
}