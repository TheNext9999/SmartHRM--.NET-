using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>
    /// Cửa sổ chính: menu, thanh truy cập nhanh, cây chức năng (lọc theo quyền RBAC và theo từ khóa),
    /// các tab chức năng có nút đóng (✕), thanh trạng thái hiển thị người dùng và đồng hồ.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>true nếu người dùng chọn "Đăng xuất" (Program sẽ hiện lại màn hình đăng nhập).</summary>
        public bool LoggedOut { get; private set; }

        readonly Dictionary<string, TabPage> open = new Dictionary<string, TabPage>();
        readonly Dictionary<string, Func<Form>> factories = new Dictionary<string, Func<Form>>();

        public MainForm()
        {
            InitializeComponent();
            RegisterModules();
            foreach (var b in tsQuick.Items.OfType<ToolStripButton>())
                b.Visible = Session.Can(Convert.ToString(b.Tag), "view");        // ẩn nút nếu không có quyền
            lblUser.Text = "👤 " + (Session.User == null ? "" : (Session.User.FullName ?? Session.User.Username)) +
                           "   |   Nhóm quyền: " + (Session.Role == null ? "" : Session.Role.Name);
            lblWelcome.Text = "Xin chào, " + (Session.User == null ? "" : (Session.User.FullName ?? Session.User.Username)) + "!";
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            BuildTree("");
            tmrClock.Start();
            tmrClock_Tick(this, EventArgs.Empty);
        }

        void tmrClock_Tick(object sender, EventArgs e) { lblClock.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss"); }

        void RegisterModules()
        {
            factories[Const.M_USER] = () => CrudForm.For(new UserBUS(), "Tài khoản người dùng");
            factories[Const.M_ROLE] = () => new RoleForm();
            factories[Const.M_AUDIT] = () => new AuditLogForm();
            factories[Const.M_SETTING] = () => CrudForm.For(new SettingBUS(), "Tham số hệ thống");
            factories[Const.M_ORG] = () => new OrgChartForm();
            factories[Const.M_POSITION] = () => CrudForm.For(new PositionBUS(), "Danh mục chức vụ");
            factories[Const.M_EMPLOYEE] = () => CrudForm.For(new EmployeeBUS(), "Hồ sơ nhân viên");
            factories[Const.M_CONTRACT] = () =>
            {
                var bus = new ContractBUS();
                var f = CrudForm.For(bus, "Hợp đồng lao động");
                f.AddToolAction("⚠ HĐ sắp hết hạn", () => ShowExpiringContracts(bus));
                return f;
            };
            factories[Const.M_RECREQ] = () =>
            {
                var bus = new RecruitmentBUS();
                var f = CrudForm.For(bus, "Đề xuất tuyển dụng");
                f.AddAction<RecruitmentRequestDTO>("✔ Duyệt", r => bus.SetStatus(r, Const.Approved));
                f.AddAction<RecruitmentRequestDTO>("✖ Từ chối", r => bus.SetStatus(r, Const.Rejected));
                return f;
            };
            factories[Const.M_CANDIDATE] = () =>
            {
                var bus = new CandidateBUS();
                var f = CrudForm.For(bus, "Ứng viên & phỏng vấn");
                f.AddAction<CandidateDTO>("🚀 Onboard → nhân viên", c =>
                {
                    if (!Ui.Confirm("Chuyển \"" + c.FullName + "\" thành nhân viên thử việc?")) return;
                    var emp = bus.Onboard(c);
                    Ui.Info("Đã tạo hồ sơ nhân viên " + emp.Code + " và hợp đồng thử việc.\nHãy bổ sung lương cơ bản trong Hồ sơ nhân viên.");
                });
                return f;
            };
            factories[Const.M_SHIFT] = () => CrudForm.For(new ShiftBUS(), "Ca làm việc");
            factories[Const.M_SCHEDULE] = () => CrudForm.For(new WorkScheduleBUS(), "Lịch làm việc / phân ca");
            factories[Const.M_ATTENDANCE] = () =>
            {
                var bus = new AttendanceBUS();
                var f = CrudForm.For(bus, "Chấm công");
                f.AddToolAction("📶 Chấm công Wi-Fi", () => Ui.Info(bus.CheckInOut("Wifi")));
                f.AddToolAction("📍 Chấm công GPS", () => Ui.Info(bus.CheckInOut("GPS")));
                f.AddToolAction("🖱 Chấm công (thủ công)", () => Ui.Info(bus.CheckInOut("Thủ công")));
                f.AddToolAction("📥 Nhập từ Excel", () => ImportAttendance(bus), "add");
                return f;
            };
            factories[Const.M_LEAVE] = () =>
            {
                var bus = new LeaveBUS();
                var f = CrudForm.For(bus, "Đơn từ điện tử");
                f.AddAction<LeaveRequestDTO>("✔ Duyệt", l => bus.Decide(l, true));
                f.AddAction<LeaveRequestDTO>("✖ Từ chối", l => bus.Decide(l, false));
                return f;
            };
            factories[Const.M_TIMESHEET] = () => new TimesheetForm();
            factories[Const.M_FORMULA] = () =>
            {
                var f = CrudForm.For(new SalaryFormulaBUS(), "Công thức lương");
                f.AddToolAction("ℹ Biến & cú pháp", () => Ui.Info(
                    "Biến có sẵn: " + string.Join(", ", FormulaEngine.BuiltInVariables) +
                    "\nCó thể dùng mã của khoản đã tính trước đó (theo thứ tự tính).\n" +
                    "Toán tử: + - * / ( )  và  IIF(điều_kiện, giá_trị_đúng, giá_trị_sai)\n" +
                    "Ví dụ: BASIC / STD_DAYS * WORK_DAYS\n" +
                    "Mã bắt đầu bằng THUONG sẽ được tính vào \"Thưởng\" trong báo cáo chi phí."));
                return f;
            };
            factories[Const.M_PAYROLL] = () => new PayrollForm(false);
            factories[Const.M_MYPAYSLIP] = () => new PayrollForm(true);
            factories[Const.M_KPIPERIOD] = () => CrudForm.For(new KpiPeriodBUS(), "Kỳ đánh giá");
            factories[Const.M_KPICRITERIA] = () => CrudForm.For(new KpiCriteriaBUS(), "Tiêu chí KPI");
            factories[Const.M_KPIEVAL] = () => CrudForm.For(new KpiEvaluationBUS(), "Đánh giá nhân viên");
            factories[Const.M_COURSE] = () => CrudForm.For(new CourseBUS(), "Khóa đào tạo");
            factories[Const.M_TRAINING] = () => CrudForm.For(new TrainingParticipantBUS(), "Học viên & chứng chỉ");
            factories[Const.M_DASHBOARD] = () =>
            {
                var d = new DashboardForm();
                d.NavigateRequested += OpenModule;
                return d;
            };
            factories[Const.M_REPORT] = () => new ReportForm();
        }


        // ------------------------------------------------------------ cây chức năng
        void BuildTree(string filter)
        {
            filter = (filter ?? "").Trim();
            tvModules.BeginUpdate();
            tvModules.Nodes.Clear();
            foreach (var g in Modules.All.GroupBy(m => m.Group))
            {
                var visible = g.Where(m => Session.Can(m.Code, "view") && factories.ContainsKey(m.Code) &&
                                           (filter.Length == 0 || m.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                if (visible.Count == 0) continue;
                var gn = new TreeNode(g.Key) { NodeFont = new Font(tvModules.Font, FontStyle.Bold), ForeColor = Color.FromArgb(31, 78, 121) };
                foreach (var m in visible) gn.Nodes.Add(new TreeNode(m.Title) { Tag = m });
                tvModules.Nodes.Add(gn);
            }
            tvModules.ExpandAll();
            tvModules.EndUpdate();
        }

        void tstFind_TextChanged(object sender, EventArgs e) { BuildTree(tstFind.Text); }

        void tvModules_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var m = e.Node.Tag as ModuleInfo;
            if (m != null) OpenModule(m.Code);
        }

        void tvModules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || tvModules.SelectedNode == null) return;
            var m = tvModules.SelectedNode.Tag as ModuleInfo;
            if (m != null) { OpenModule(m.Code); e.Handled = true; }
        }

        void tsbQuick_Click(object sender, EventArgs e)
        {
            var b = sender as ToolStripItem;
            if (b != null && b.Tag != null) OpenModule(Convert.ToString(b.Tag));
        }

        // ------------------------------------------------------------ tab chức năng
        public void OpenModule(string code)
        {
            TabPage page;
            if (open.TryGetValue(code, out page)) { tabMain.SelectedTab = page; return; }
            if (!factories.ContainsKey(code) || !Session.Can(code, "view")) { Ui.Warn("Bạn không có quyền truy cập chức năng này."); return; }
            try
            {
                var form = factories[code]();
                form.TopLevel = false; form.FormBorderStyle = FormBorderStyle.None; form.Dock = DockStyle.Fill;
                page = new TabPage(Modules.TitleOf(code)) { Tag = code };
                page.Controls.Add(form);
                tabMain.TabPages.Add(page);
                open[code] = page;
                tabMain.SelectedTab = page;
                SyncTabVisibility();
                form.Show();
            }
            catch (Exception ex) { Ui.Error(ex); }
        }

        void CloseTab(TabPage p)
        {
            if (p == null) return;
            foreach (var c in p.Controls.Cast<Control>().ToList()) c.Dispose();
            open.Remove((string)p.Tag);
            tabMain.TabPages.Remove(p);
            SyncTabVisibility();
        }

        void SyncTabVisibility()
        {
            bool any = tabMain.TabCount > 0;
            tabMain.Visible = any;
            pnlWelcome.Visible = !any;
        }

        void CloseCurrentTab_Click(object sender, EventArgs e) { CloseTab(tabMain.SelectedTab); }
        void CloseAllTabs_Click(object sender, EventArgs e) { foreach (var t in tabMain.TabPages.Cast<TabPage>().ToList()) CloseTab(t); }
        void CloseOtherTabs_Click(object sender, EventArgs e)
        {
            var keep = tabMain.SelectedTab;
            foreach (var t in tabMain.TabPages.Cast<TabPage>().Where(t => t != keep).ToList()) CloseTab(t);
        }

        void tabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tabMain.TabPages[e.Index];
            var rect = tabMain.GetTabRect(e.Index);
            bool selected = e.Index == tabMain.SelectedIndex;
            using (var back = new SolidBrush(selected ? Color.White : Color.FromArgb(228, 235, 244)))
                e.Graphics.FillRectangle(back, rect);
            var fore = selected ? Color.FromArgb(31, 78, 121) : Color.FromArgb(70, 70, 70);
            var font = selected ? new Font(e.Font, FontStyle.Bold) : e.Font;
            TextRenderer.DrawText(e.Graphics, tab.Text, font, new Rectangle(rect.X + 4, rect.Y + 3, rect.Width - 24, rect.Height - 3), fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            if (selected) font.Dispose();
            TextRenderer.DrawText(e.Graphics, "✕", e.Font, CloseRect(rect), Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        static Rectangle CloseRect(Rectangle tab) { return new Rectangle(tab.Right - 20, tab.Y + 4, 16, 18); }

        void tabMain_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabMain.TabCount; i++)
            {
                var r = tabMain.GetTabRect(i);
                if (!r.Contains(e.Location)) continue;
                if ((e.Button == MouseButtons.Left && CloseRect(r).Contains(e.Location)) || e.Button == MouseButtons.Middle)
                    CloseTab(tabMain.TabPages[i]);
                else if (e.Button == MouseButtons.Right) tabMain.SelectedIndex = i;
                break;
            }
        }

        // ------------------------------------------------------------ khởi động / đóng
        void MainForm_Shown(object sender, EventArgs e)
        {
            if (Session.Can(Const.M_DASHBOARD, "view")) OpenModule(Const.M_DASHBOARD);
            else if (Session.Can(Const.M_MYPAYSLIP, "view")) OpenModule(Const.M_MYPAYSLIP);
            if (Session.Can(Const.M_CONTRACT, "view"))
            {
                try
                {
                    var dt = new ContractBUS().GetExpiring();
                    if (dt.Rows.Count > 0)
                        Ui.Warn("Có " + dt.Rows.Count + " hợp đồng sắp hết hạn trong " + AppSettings.GetInt("ContractWarnDays", 30) +
                                " ngày tới.\nVào \"Hợp đồng lao động\" → \"⚠ HĐ sắp hết hạn\" để xem chi tiết.");
                }
                catch { }
            }
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrClock.Stop();
            if (Session.User != null) new AuthBUS().Logout();      // ghi nhật ký đăng xuất (dù đóng cửa sổ hay chọn Đăng xuất)
        }

        void mnuLogout_Click(object sender, EventArgs e) { LoggedOut = true; Close(); }
        void mnuExit_Click(object sender, EventArgs e) { Close(); }

        void mnuAbout_Click(object sender, EventArgs e)
        {
            Ui.Info("SmartHRM - Phần mềm quản lý nhân sự\nWinForms .NET Framework + MySQL\n\n" +
                    "Nhân sự • Chấm công • Tiền lương • Tuyển dụng • Đánh giá & Đào tạo • Báo cáo");
        }

        // ------------------------------------------------------------ hộp thoại phụ
        void ShowExpiringContracts(ContractBUS bus)
        {
            var dt = bus.GetExpiring();
            dt.Columns["contract_no"].ColumnName = "Số HĐ"; dt.Columns["code"].ColumnName = "Mã NV"; dt.Columns["full_name"].ColumnName = "Họ tên";
            dt.Columns["type"].ColumnName = "Loại HĐ"; dt.Columns["end_date"].ColumnName = "Ngày hết hạn"; dt.Columns["days_left"].ColumnName = "Còn (ngày)";
            using (var f = new Form { Text = "Hợp đồng sắp hết hạn", Size = new Size(760, 420), StartPosition = FormStartPosition.CenterParent, Font = Font })
            {
                var g = Ui.NewGrid(); g.DataSource = dt; Ui.FormatGrid(g);
                f.Controls.Add(g);
                f.ShowDialog(this);
            }
        }

        void ImportAttendance(AttendanceBUS bus)
        {
            Ui.Info("File Excel cần có dòng tiêu đề và các cột:\nA: Mã nhân viên | B: Ngày | C: Giờ vào | D: Giờ ra\n(xuất từ máy chấm công vân tay / khuôn mặt)");
            using (var d = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx" })
            {
                if (d.ShowDialog() != DialogResult.OK) return;
                var r = bus.ImportExcel(d.FileName);
                var sb = new StringBuilder("Nhập thành công " + r.Ok + " dòng.");
                if (r.Errors.Count > 0) sb.Append("\n\nLỗi (" + r.Errors.Count + "):\n" + string.Join("\n", r.Errors.Take(15)));
                Ui.Info(sb.ToString());
            }
        }


        // ------------------------------------------------------------ đổi mật khẩu
        void mnuChangePassword_Click(object sender, EventArgs e)
        {
            using (var f = new Form
            {
                Text = "Đổi mật khẩu",
                ClientSize = new Size(380, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = Font
            })
            {
                var o = new TextBox { UseSystemPasswordChar = true, Left = 170, Top = 22, Width = 190 };
                var n1 = new TextBox { UseSystemPasswordChar = true, Left = 170, Top = 58, Width = 190 };
                var n2 = new TextBox { UseSystemPasswordChar = true, Left = 170, Top = 94, Width = 190 };
                var ok = new Button { Text = "Đổi mật khẩu", Left = 170, Top = 140, Width = 190, Height = 32 };
                f.Controls.AddRange(new Control[]
                {
                    new System.Windows.Forms.Label { Text = "Mật khẩu hiện tại", Left = 15, Top = 26, AutoSize = true }, o,
                    new System.Windows.Forms.Label { Text = "Mật khẩu mới", Left = 15, Top = 62, AutoSize = true }, n1,
                    new System.Windows.Forms.Label { Text = "Nhập lại mật khẩu mới", Left = 15, Top = 98, AutoSize = true }, n2, ok
                });
                f.AcceptButton = ok;
                ok.Click += (s, ev) =>
                {
                    try
                    {
                        if (n1.Text != n2.Text) throw new ValidationException("Mật khẩu nhập lại không khớp.");
                        new AuthBUS().ChangePassword(o.Text, n1.Text);
                        Ui.Info("Đã đổi mật khẩu."); f.Close();
                    }
                    catch (Exception ex) { Ui.Error(ex); }
                };
                f.ShowDialog(this);
            }
        }
    }
}