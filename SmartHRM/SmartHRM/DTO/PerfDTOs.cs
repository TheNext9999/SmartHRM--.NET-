using System;

namespace SmartHRM.DTO
{
    [Table("kpi_periods")]
    public class KpiPeriodDTO : BaseDTO
    {
        [Field("Tên kỳ đánh giá", Required = true)] public string Name { get; set; }
        [Field("Loại kỳ", Kind = FieldKind.Combo, Source = Const.PeriodTypes, Required = true)] public string PeriodType { get; set; } = "Tháng";
        [Field("Từ ngày", Kind = FieldKind.Date, Required = true)] public DateTime? StartDate { get; set; }
        [Field("Đến ngày", Kind = FieldKind.Date, Required = true)] public DateTime? EndDate { get; set; }
    }

    [Table("kpi_criteria")]
    public class KpiCriteriaDTO : BaseDTO
    {
        [Field("Vị trí áp dụng", Kind = FieldKind.Lookup, Source = "positions|id|name")] public int? PositionId { get; set; }
        [Field("Tiêu chí KPI / OKR", Required = true)] public string Name { get; set; }
        [Field("Trọng số (%)", Kind = FieldKind.Decimal)] public decimal Weight { get; set; }
        [Field("Mô tả / cách đo", Kind = FieldKind.MultiLine)] public string Description { get; set; }
    }

    [Table("kpi_evaluations")]
    public class KpiEvaluationDTO : BaseDTO
    {
        [Field("Kỳ đánh giá", Kind = FieldKind.Lookup, Source = "kpi_periods|id|name", Required = true)] public int? PeriodId { get; set; }
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Tự đánh giá (0-100)", Kind = FieldKind.Decimal)] public decimal SelfScore { get; set; }
        [Field("Nhận xét của NV", Kind = FieldKind.MultiLine, InGrid = false)] public string SelfComment { get; set; }
        [Field("Cấp trên chấm (0-100)", Kind = FieldKind.Decimal)] public decimal ManagerScore { get; set; }
        [Field("Nhận xét cấp trên", Kind = FieldKind.MultiLine, InGrid = false)] public string ManagerComment { get; set; }
        [Field("Điểm cuối cùng", Kind = FieldKind.Decimal, ReadOnly = true)] public decimal FinalScore { get; set; }
        [Field("Xếp loại", ReadOnly = true)] public string Rating { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.KpiStatuses, ReadOnly = true)] public string Status { get; set; } = "Chờ tự đánh giá";
    }

    [Table("training_courses")]
    public class CourseDTO : BaseDTO
    {
        [Field("Tên khóa học", Required = true)] public string Name { get; set; }
        [Field("Giảng viên")] public string Trainer { get; set; }
        [Field("Bắt đầu", Kind = FieldKind.Date)] public DateTime? StartDate { get; set; }
        [Field("Kết thúc", Kind = FieldKind.Date)] public DateTime? EndDate { get; set; }
        [Field("Địa điểm")] public string Location { get; set; }
        [Field("Kinh phí", Kind = FieldKind.Decimal)] public decimal Budget { get; set; }
        [Field("Trạng thái", Kind = FieldKind.Combo, Source = Const.CourseStatuses)] public string Status { get; set; } = "Kế hoạch";
        [Field("Nội dung", Kind = FieldKind.MultiLine, InGrid = false)] public string Description { get; set; }
    }

    [Table("training_participants")]
    public class TrainingParticipantDTO : BaseDTO
    {
        [Field("Khóa học", Kind = FieldKind.Lookup, Source = "training_courses|id|name", Required = true)] public int? CourseId { get; set; }
        [Field("Nhân viên", Kind = FieldKind.Lookup, Source = "employees|id|CONCAT(code,' - ',full_name)", Required = true)] public int? EmployeeId { get; set; }
        [Field("Kết quả", Kind = FieldKind.Combo, Source = Const.TrainResults)] public string Result { get; set; } = "Đang học";
        [Field("Điểm", Kind = FieldKind.Decimal)] public decimal Score { get; set; }
        [Field("Số chứng chỉ")] public string CertificateNo { get; set; }
        [Field("Ngày cấp", Kind = FieldKind.Date)] public DateTime? CertificateDate { get; set; }
    }
}
