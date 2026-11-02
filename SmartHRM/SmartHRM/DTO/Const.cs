namespace SmartHRM.DTO
{
    /// <summary>Các hằng số dùng chung (giá trị lưu trong CSDL).</summary>
    public static class Const
    {
        public const string Male = "Nam", Female = "Nữ", Other = "Khác";
        public const string Genders = "Nam|Nữ|Khác";

        public const string Probation = "Thử việc", Official = "Chính thức", Resigned = "Nghỉ việc";
        public const string EmployeeStatuses = "Thử việc|Chính thức|Nghỉ việc";

        public const string ContractTypes = "Thử việc|1 năm|2 năm|3 năm|Không thời hạn";
        public const string ContractStatuses = "Hiệu lực|Hết hạn|Chấm dứt";

        public const string Pending = "Chờ duyệt", Approved = "Đã duyệt", Rejected = "Từ chối";
        public const string ApprovalStatuses = "Chờ duyệt|Đã duyệt|Từ chối";

        public const string CandNew = "Mới", CandScheduled = "Lên lịch PV", CandPassed = "Đạt", CandFailed = "Trượt", CandOnboarded = "Đã onboard";
        public const string CandidateStatuses = "Mới|Lên lịch PV|Đạt|Trượt|Đã onboard";

        public const string LeaveAnnual = "Nghỉ phép", LeaveLate = "Đi muộn/Về sớm", LeaveOt = "Làm thêm giờ (OT)";
        public const string LeaveTypes = "Nghỉ phép|Đi muộn/Về sớm|Làm thêm giờ (OT)";

        public const string AttMethods = "Máy chấm công|Excel|GPS|Wifi|Thủ công";
        public const string Income = "Thu nhập", Deduction = "Khấu trừ";
        public const string FormulaKinds = "Thu nhập|Khấu trừ";
        public const string PeriodTypes = "Tháng|Quý|Năm";
        public const string KpiStatuses = "Chờ tự đánh giá|Chờ duyệt|Hoàn thành";
        public const string CourseStatuses = "Kế hoạch|Đang diễn ra|Hoàn thành|Hủy";
        public const string TrainResults = "Đang học|Hoàn thành|Không đạt";

        // Mã mô-đun dùng cho phân quyền
        public const string M_USER = "SYS_USER", M_ROLE = "SYS_ROLE", M_AUDIT = "SYS_AUDIT", M_SETTING = "SYS_SETTING",
            M_ORG = "ORG", M_POSITION = "POSITION", M_EMPLOYEE = "EMPLOYEE", M_CONTRACT = "CONTRACT",
            M_RECREQ = "REC_REQ", M_CANDIDATE = "CANDIDATE",
            M_SHIFT = "SHIFT", M_SCHEDULE = "SCHEDULE", M_ATTENDANCE = "ATTENDANCE", M_LEAVE = "LEAVE", M_TIMESHEET = "TIMESHEET",
            M_FORMULA = "SALARY_FORMULA", M_PAYROLL = "PAYROLL", M_MYPAYSLIP = "MY_PAYSLIP",
            M_KPIPERIOD = "KPI_PERIOD", M_KPICRITERIA = "KPI_CRITERIA", M_KPIEVAL = "KPI_EVAL", M_COURSE = "COURSE", M_TRAINING = "TRAINING",
            M_DASHBOARD = "DASHBOARD", M_REPORT = "REPORT";
    }
}
