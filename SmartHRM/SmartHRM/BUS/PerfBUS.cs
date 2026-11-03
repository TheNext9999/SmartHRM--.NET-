using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    public class KpiPeriodBUS : GenericBUS<KpiPeriodDTO>
    {
        public KpiPeriodBUS() : base(Const.M_KPIPERIOD) { }
        protected override void Validate(KpiPeriodDTO e)
        {
            base.Validate(e);
            if (e.EndDate < e.StartDate) throw new ValidationException("Ngày kết thúc phải sau ngày bắt đầu.");
        }
    }

    public class KpiCriteriaBUS : GenericBUS<KpiCriteriaDTO>
    {
        public KpiCriteriaBUS() : base(Const.M_KPICRITERIA) { }
        protected override void Validate(KpiCriteriaDTO e)
        {
            base.Validate(e);
            if (e.Weight < 0 || e.Weight > 100) throw new ValidationException("Trọng số phải trong khoảng 0-100%.");
        }
    }

    public class KpiEvaluationBUS : GenericBUS<KpiEvaluationDTO>
    {
        readonly KpiEvaluationDAL dal;
        public KpiEvaluationBUS() : this(new KpiEvaluationDAL()) { }
        KpiEvaluationBUS(KpiEvaluationDAL d) : base(Const.M_KPIEVAL, d) { dal = d; }

        /// <summary>Người quản lý kỳ đánh giá (HR/cấp trên) được chấm điểm; nhân viên chỉ tự đánh giá.</summary>
        static bool IsManager { get { return Session.Can(Const.M_KPIPERIOD, "edit"); } }

        public override List<KpiEvaluationDTO> GetAll()
        {
            Session.Require(Module, "view");
            if (!IsManager && Session.EmployeeId.HasValue) return dal.GetAll("employee_id=@e", "id DESC", DbHelper.P("@e", Session.EmployeeId.Value));
            return dal.GetAll();
        }

        public override KpiEvaluationDTO CreateNew() { return new KpiEvaluationDTO { EmployeeId = IsManager ? null : Session.EmployeeId }; }

        protected override void Validate(KpiEvaluationDTO e)
        {
            if (!IsManager)
            {
                if (!Session.EmployeeId.HasValue) throw new ValidationException("Tài khoản chưa liên kết nhân viên.");
                e.EmployeeId = Session.EmployeeId;
            }
            base.Validate(e);
            if (e.SelfScore < 0 || e.SelfScore > 100 || e.ManagerScore < 0 || e.ManagerScore > 100)
                throw new ValidationException("Điểm phải nằm trong khoảng 0-100.");
            var dup = dal.GetAll("period_id=@p AND employee_id=@e AND id<>@i", "id",
                DbHelper.P("@p", e.PeriodId), DbHelper.P("@e", e.EmployeeId), DbHelper.P("@i", e.Id));
            if (dup.Count > 0) throw new ValidationException("Nhân viên này đã có phiếu đánh giá trong kỳ đã chọn.");
        }

        protected override void BeforeSave(KpiEvaluationDTO e, bool isNew)
        {
            if (!IsManager)
            {   // nhân viên không được tự sửa phần của cấp trên
                var old = isNew ? null : dal.GetById(e.Id);
                e.ManagerScore = old == null ? 0 : old.ManagerScore;
                e.ManagerComment = old == null ? null : old.ManagerComment;
            }
            if (e.ManagerScore > 0)
            {
                e.FinalScore = Math.Round(e.SelfScore * 0.3m + e.ManagerScore * 0.7m, 2);
                e.Status = "Hoàn thành";
            }
            else if (e.SelfScore > 0) { e.FinalScore = e.SelfScore; e.Status = "Chờ duyệt"; }
            else { e.FinalScore = 0; e.Status = "Chờ tự đánh giá"; }

            e.Rating = e.Status != "Hoàn thành" ? null :
                e.FinalScore >= 90 ? "Xuất sắc" : e.FinalScore >= 80 ? "Tốt" : e.FinalScore >= 65 ? "Khá" : e.FinalScore >= 50 ? "Trung bình" : "Yếu";
        }
    }

    public class CourseBUS : GenericBUS<CourseDTO>
    {
        public CourseBUS() : base(Const.M_COURSE) { }
        protected override void Validate(CourseDTO e)
        {
            base.Validate(e);
            if (e.EndDate < e.StartDate) throw new ValidationException("Ngày kết thúc phải sau ngày bắt đầu.");
            if (e.Budget < 0) throw new ValidationException("Kinh phí không được âm.");
        }
    }

    public class TrainingParticipantBUS : GenericBUS<TrainingParticipantDTO>
    {
        readonly TrainingParticipantDAL dal;
        public TrainingParticipantBUS() : this(new TrainingParticipantDAL()) { }
        TrainingParticipantBUS(TrainingParticipantDAL d) : base(Const.M_TRAINING, d) { dal = d; }

        public override List<TrainingParticipantDTO> GetAll()
        {
            Session.Require(Module, "view");
            if (Session.SelfOnly(Module)) return dal.GetAll("employee_id=@e", "id DESC", DbHelper.P("@e", Session.EmployeeId.Value));
            return dal.GetAll();
        }

        protected override void Validate(TrainingParticipantDTO e)
        {
            base.Validate(e);
            if (dal.GetAll("course_id=@c AND employee_id=@e AND id<>@i", "id",
                DbHelper.P("@c", e.CourseId), DbHelper.P("@e", e.EmployeeId), DbHelper.P("@i", e.Id)).Count > 0)
                throw new ValidationException("Nhân viên đã được ghi danh vào khóa học này.");
            if (e.Result == "Hoàn thành" && string.IsNullOrWhiteSpace(e.CertificateNo) == false && !e.CertificateDate.HasValue)
                e.CertificateDate = DateTime.Today;
        }
    }

    public class ReportBUS
    {
        readonly ReportDAL dal = new ReportDAL();
        public DataTable HeadcountByDepartment() { Session.Require(Const.M_REPORT, "view"); return dal.HeadcountByDepartment(); }
        public DataTable GenderRatio() { Session.Require(Const.M_REPORT, "view"); return dal.GenderRatio(); }
        public DataTable AgeGroups() { Session.Require(Const.M_REPORT, "view"); return dal.AgeGroups(); }
        public DataTable Seniority() { Session.Require(Const.M_REPORT, "view"); return dal.Seniority(); }
        public DataTable Turnover(int year) { Session.Require(Const.M_REPORT, "view"); return dal.Turnover(year); }
        public DataTable PayrollCost(int year) { Session.Require(Const.M_REPORT, "view"); return dal.PayrollCost(year); }

        // Dashboard
        public DataTable Headcount() { Session.Require(Const.M_DASHBOARD, "view"); return dal.HeadcountByDepartment(); }
        public DataTable DashGender() { Session.Require(Const.M_DASHBOARD, "view"); return dal.GenderRatio(); }
        public DataTable DashTurnover(int year)
        {
            Session.Require(Const.M_DASHBOARD, "view");
            var dt = dal.Turnover(year);
            dt.Columns.Remove("Tỷ lệ nghỉ việc (%)");     // biểu đồ chỉ so sánh số lượng mới / nghỉ
            return dt;
        }
        public DataTable CostLastMonths() { Session.Require(Const.M_DASHBOARD, "view"); return dal.PayrollCostLastMonths(6); }
        public Dictionary<string, string> Kpis(int expiringContracts)
        {
            Session.Require(Const.M_DASHBOARD, "view");
            return new Dictionary<string, string>
            {
                { "Nhân viên đang làm", dal.ActiveEmployees().ToString() },
                { "Có mặt hôm nay", dal.PresentToday().ToString() },
                { "Tuyển mới tháng này", dal.NewThisMonth().ToString() },
                { "Nghỉ việc tháng này", dal.LeftThisMonth().ToString() },
                { "Đang thử việc", dal.Probation().ToString() },
                { "HĐ sắp hết hạn", expiringContracts.ToString() },
                { "Đơn chờ duyệt", dal.PendingLeave().ToString() },
                { "Ứng viên đang xử lý", dal.OpenCandidates().ToString() },
                { "Thực lĩnh kỳ gần nhất", dal.LastPayrollNet().ToString("N0", new System.Globalization.CultureInfo("vi-VN")) }
            };
        }
    }
}