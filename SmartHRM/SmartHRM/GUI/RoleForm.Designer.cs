namespace SmartHRM.GUI
{
    partial class RoleForm
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
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.lstRoles = new System.Windows.Forms.ListBox();
            this.lblRoleInfo = new System.Windows.Forms.Label();
            this.tsRoles = new System.Windows.Forms.ToolStrip();
            this.tsbAddRole = new System.Windows.Forms.ToolStripButton();
            this.tsbEditRole = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteRole = new System.Windows.Forms.ToolStripButton();
            this.lblRolesTitle = new System.Windows.Forms.Label();
            this.dgvPerm = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAll = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colView = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAdd = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsPerm = new System.Windows.Forms.ToolStrip();
            this.tsbAll = new System.Windows.Forms.ToolStripButton();
            this.tsbNone = new System.Windows.Forms.ToolStripButton();
            this.tsbViewOnly = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.pnlBanner = new System.Windows.Forms.Panel();
            this.lblDirty = new System.Windows.Forms.Label();
            this.lblBanner = new System.Windows.Forms.Label();
            this.lblRoleTitle = new System.Windows.Forms.Label();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHint = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tsRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerm)).BeginInit();
            this.tsPerm.SuspendLayout();
            this.pnlBanner.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.lstRoles);
            this.splitMain.Panel1.Controls.Add(this.lblRoleInfo);
            this.splitMain.Panel1.Controls.Add(this.tsRoles);
            this.splitMain.Panel1.Controls.Add(this.lblRolesTitle);
            this.splitMain.Panel2.Controls.Add(this.dgvPerm);
            this.splitMain.Panel2.Controls.Add(this.tsPerm);
            this.splitMain.Panel2.Controls.Add(this.pnlBanner);
            this.splitMain.Size = new System.Drawing.Size(1100, 600);
            this.splitMain.SplitterDistance = 270;
            this.splitMain.TabIndex = 0;
            //
            // lblRolesTitle
            //
            this.lblRolesTitle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblRolesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRolesTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRolesTitle.ForeColor = System.Drawing.Color.White;
            this.lblRolesTitle.Name = "lblRolesTitle";
            this.lblRolesTitle.Padding = new System.Windows.Forms.Padding(8, 6, 0, 0);
            this.lblRolesTitle.Size = new System.Drawing.Size(270, 30);
            this.lblRolesTitle.TabIndex = 3;
            this.lblRolesTitle.Text = "Nhóm quyền";
            //
            // tsRoles
            //
            this.tsRoles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsRoles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.tsbAddRole, this.tsbEditRole, this.tsbDeleteRole });
            this.tsRoles.Name = "tsRoles";
            this.tsRoles.TabIndex = 2;
            //
            // tsbAddRole
            //
            this.tsbAddRole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAddRole.Name = "tsbAddRole";
            this.tsbAddRole.Text = "➕ Thêm";
            this.tsbAddRole.Click += new System.EventHandler(this.tsbAddRole_Click);
            //
            // tsbEditRole
            //
            this.tsbEditRole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditRole.Name = "tsbEditRole";
            this.tsbEditRole.Text = "✏️ Sửa";
            this.tsbEditRole.Click += new System.EventHandler(this.tsbEditRole_Click);
            //
            // tsbDeleteRole
            //
            this.tsbDeleteRole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDeleteRole.Name = "tsbDeleteRole";
            this.tsbDeleteRole.Text = "🗑 Xóa";
            this.tsbDeleteRole.Click += new System.EventHandler(this.tsbDeleteRole_Click);
            //
            // lblRoleInfo
            //
            this.lblRoleInfo.BackColor = System.Drawing.Color.FromArgb(236, 242, 249);
            this.lblRoleInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRoleInfo.Name = "lblRoleInfo";
            this.lblRoleInfo.Padding = new System.Windows.Forms.Padding(8, 6, 6, 0);
            this.lblRoleInfo.Size = new System.Drawing.Size(270, 90);
            this.lblRoleInfo.TabIndex = 1;
            //
            // lstRoles
            //
            this.lstRoles.DisplayMember = "Name";
            this.lstRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRoles.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lstRoles.IntegralHeight = false;
            this.lstRoles.ItemHeight = 25;
            this.lstRoles.Name = "lstRoles";
            this.lstRoles.TabIndex = 0;
            this.lstRoles.SelectedIndexChanged += new System.EventHandler(this.lstRoles_SelectedIndexChanged);
            //
            // pnlBanner
            //
            this.pnlBanner.BackColor = System.Drawing.Color.White;
            this.pnlBanner.Controls.Add(this.lblDirty);
            this.pnlBanner.Controls.Add(this.lblBanner);
            this.pnlBanner.Controls.Add(this.lblRoleTitle);
            this.pnlBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new System.Drawing.Size(826, 64);
            this.pnlBanner.TabIndex = 2;
            //
            // lblRoleTitle
            //
            this.lblRoleTitle.AutoSize = true;
            this.lblRoleTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRoleTitle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.lblRoleTitle.Location = new System.Drawing.Point(12, 6);
            this.lblRoleTitle.Name = "lblRoleTitle";
            this.lblRoleTitle.Text = "Phân quyền";
            //
            // lblBanner
            //
            this.lblBanner.AutoSize = true;
            this.lblBanner.ForeColor = System.Drawing.Color.Gray;
            this.lblBanner.Location = new System.Drawing.Point(14, 38);
            this.lblBanner.Name = "lblBanner";
            this.lblBanner.Text = "";
            //
            // lblDirty
            //
            this.lblDirty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDirty.AutoSize = true;
            this.lblDirty.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDirty.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblDirty.Location = new System.Drawing.Point(620, 14);
            this.lblDirty.Name = "lblDirty";
            this.lblDirty.Text = "● Có thay đổi chưa lưu";
            this.lblDirty.Visible = false;
            //
            // tsPerm
            //
            this.tsPerm.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsPerm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbAll, this.tsbNone, this.tsbViewOnly, this.tsbCopy, this.tss1, this.tsbReload, this.tsbSave });
            this.tsPerm.Name = "tsPerm";
            this.tsPerm.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tsPerm.TabIndex = 1;
            //
            // tsbAll
            //
            this.tsbAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAll.Name = "tsbAll";
            this.tsbAll.Text = "☑ Chọn tất cả";
            this.tsbAll.Click += new System.EventHandler(this.tsbAll_Click);
            //
            // tsbNone
            //
            this.tsbNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbNone.Name = "tsbNone";
            this.tsbNone.Text = "☐ Bỏ chọn tất cả";
            this.tsbNone.Click += new System.EventHandler(this.tsbNone_Click);
            //
            // tsbViewOnly
            //
            this.tsbViewOnly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbViewOnly.Name = "tsbViewOnly";
            this.tsbViewOnly.Text = "👁 Chỉ cho xem";
            this.tsbViewOnly.ToolTipText = "Chỉ cấp quyền Xem cho mọi chức năng";
            this.tsbViewOnly.Click += new System.EventHandler(this.tsbViewOnly_Click);
            //
            // tsbCopy
            //
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Text = "📋 Sao chép từ nhóm khác...";
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            //
            // tss1
            //
            this.tss1.Name = "tss1";
            //
            // tsbReload
            //
            this.tsbReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Text = "↩ Hoàn tác";
            this.tsbReload.ToolTipText = "Bỏ các thay đổi chưa lưu";
            this.tsbReload.Click += new System.EventHandler(this.tsbReload_Click);
            //
            // tsbSave
            //
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Text = "💾 Lưu phân quyền";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            //
            // dgvPerm
            //
            this.dgvPerm.AllowUserToAddRows = false;
            this.dgvPerm.AllowUserToDeleteRows = false;
            this.dgvPerm.AllowUserToResizeRows = false;
            this.dgvPerm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPerm.BackgroundColor = System.Drawing.Color.White;
            this.dgvPerm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.dgvPerm.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvPerm.ColumnHeadersHeight = 32;
            this.dgvPerm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPerm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colCode, this.colGroup, this.colTitle, this.colAll, this.colView, this.colAdd, this.colEdit, this.colDelete });
            this.dgvPerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPerm.EnableHeadersVisualStyles = false;
            this.dgvPerm.MultiSelect = false;
            this.dgvPerm.Name = "dgvPerm";
            this.dgvPerm.RowHeadersVisible = false;
            this.dgvPerm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPerm.TabIndex = 0;
            this.dgvPerm.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPerm_CellFormatting);
            this.dgvPerm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPerm_CellValueChanged);
            this.dgvPerm.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPerm_ColumnHeaderMouseClick);
            this.dgvPerm.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvPerm_CurrentCellDirtyStateChanged);
            //
            // colCode
            //
            this.colCode.HeaderText = "Mã";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Visible = false;
            //
            // colGroup
            //
            this.colGroup.FillWeight = 70F;
            this.colGroup.HeaderText = "Nhóm chức năng";
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            //
            // colTitle
            //
            this.colTitle.FillWeight = 100F;
            this.colTitle.HeaderText = "Chức năng";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            //
            // colAll
            //
            this.colAll.FillWeight = 22F;
            this.colAll.HeaderText = "Tất cả";
            this.colAll.Name = "colAll";
            //
            // colView
            //
            this.colView.FillWeight = 22F;
            this.colView.HeaderText = "Xem";
            this.colView.Name = "colView";
            //
            // colAdd
            //
            this.colAdd.FillWeight = 22F;
            this.colAdd.HeaderText = "Thêm";
            this.colAdd.Name = "colAdd";
            //
            // colEdit
            //
            this.colEdit.FillWeight = 22F;
            this.colEdit.HeaderText = "Sửa";
            this.colEdit.Name = "colEdit";
            //
            // colDelete
            //
            this.colDelete.FillWeight = 22F;
            this.colDelete.HeaderText = "Xóa";
            this.colDelete.Name = "colDelete";
            //
            // statusMain
            //
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus, this.lblHint });
            this.statusMain.Name = "statusMain";
            this.statusMain.TabIndex = 1;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "";
            //
            // lblHint
            //
            this.lblHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHint.Name = "lblHint";
            this.lblHint.Spring = true;
            this.lblHint.Text = "Bấm tiêu đề cột để chọn / bỏ chọn cả cột  |  Thêm, Sửa, Xóa tự kéo theo quyền Xem";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // RoleForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 622);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Name = "RoleForm";
            this.Text = "Phân quyền";
            this.Load += new System.EventHandler(this.RoleForm_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tsRoles.ResumeLayout(false);
            this.tsRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerm)).EndInit();
            this.tsPerm.ResumeLayout(false);
            this.tsPerm.PerformLayout();
            this.pnlBanner.ResumeLayout(false);
            this.pnlBanner.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ListBox lstRoles;
        private System.Windows.Forms.Label lblRoleInfo;
        private System.Windows.Forms.ToolStrip tsRoles;
        private System.Windows.Forms.ToolStripButton tsbAddRole;
        private System.Windows.Forms.ToolStripButton tsbEditRole;
        private System.Windows.Forms.ToolStripButton tsbDeleteRole;
        private System.Windows.Forms.Label lblRolesTitle;
        private System.Windows.Forms.DataGridView dgvPerm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEdit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDelete;
        private System.Windows.Forms.ToolStrip tsPerm;
        private System.Windows.Forms.ToolStripButton tsbAll;
        private System.Windows.Forms.ToolStripButton tsbNone;
        private System.Windows.Forms.ToolStripButton tsbViewOnly;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.Label lblDirty;
        private System.Windows.Forms.Label lblBanner;
        private System.Windows.Forms.Label lblRoleTitle;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHint;
    }
}