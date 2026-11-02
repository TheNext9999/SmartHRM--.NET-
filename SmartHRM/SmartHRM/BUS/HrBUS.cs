using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    public class DepartmentBUS : GenericBUS<DepartmentDTO>
    {
        readonly DepartmentDAL dal;
        public DepartmentBUS() : this(new DepartmentDAL()) { }
        DepartmentBUS(DepartmentDAL d) : base(Const.M_ORG, d) { dal = d; }

        public override List<DepartmentDTO> GetAll() { Session.Require(Module, "view"); return dal.GetAll(); }

        protected override void Validate(DepartmentDTO e)
        {
            base.Validate(e);
            if (dal.GetAll("code=@c AND id<>@i", "id", DbHelper.P("@c", e.Code), DbHelper.P("@i", e.Id)).Count > 0)
                throw new ValidationException("Mã phòng ban đã tồn tại.");
            if (e.ParentId.HasValue && e.Id != 0)
            {
                // chống vòng lặp cây tổ chức
                var all = dal.GetAll().ToDictionary(d => d.Id);
                int? cur = e.ParentId; int guard = 0;
                while (cur.HasValue && guard++ < 100)
                {
                    if (cur.Value == e.Id) throw new ValidationException("Đơn vị cấp trên không hợp lệ (tạo vòng lặp trong sơ đồ tổ chức).");
                    DepartmentDTO p; cur = all.TryGetValue(cur.Value, out p) ? p.ParentId : null;
                }
            }
        }

        protected override void BeforeDelete(DepartmentDTO e)
        {
            if (dal.GetAll("parent_id=@i", "id", DbHelper.P("@i", e.Id)).Count > 0)
                throw new ValidationException("Đơn vị còn các đơn vị trực thuộc, hãy xóa hoặc chuyển chúng trước.");
            if (dal.CountEmployees(e.Id) > 0)
                throw new ValidationException("Đơn vị còn nhân viên đang làm việc, hãy điều chuyển họ trước.");
        }

        public List<EmployeeDTO> GetEmployees(int deptId) { return new EmployeeDAL().GetByDepartment(deptId); }
    }

    public class PositionBUS : GenericBUS<PositionDTO>
    {
        public PositionBUS() : base(Const.M_POSITION) { }
    }

    public class EmployeeBUS : GenericBUS<EmployeeDTO>
    {
        readonly EmployeeDAL dal;
        public EmployeeBUS() : this(new EmployeeDAL()) { }
        EmployeeBUS(EmployeeDAL d) : base(Const.M_EMPLOYEE, d) { dal = d; }

        public override List<EmployeeDTO> GetAll() { Session.Require(Module, "view"); return dal.GetAll(null, "code"); }

        public override EmployeeDTO CreateNew() { return new EmployeeDTO { Code = "(tự động)", JoinDate = DateTime.Today }; }

        protected override void Validate(EmployeeDTO e)
        {
            base.Validate(e);
            if (!string.IsNullOrWhiteSpace(e.Email) && !Regex.IsMatch(e.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException("Email không hợp lệ.");
            if (!string.IsNullOrWhiteSpace(e.IdCard) && !Regex.IsMatch(e.IdCard.Trim(), @"^(\d{9}|\d{12})$"))
                throw new ValidationException("CCCD/CMND phải gồm 9 hoặc 12 chữ số.");
            if (!string.IsNullOrWhiteSpace(e.IdCard) &&
                dal.GetAll("id_card=@c AND id<>@i", "id", DbHelper.P("@c", e.IdCard.Trim()), DbHelper.P("@i", e.Id)).Count > 0)
                throw new ValidationException("CCCD/CMND đã tồn tại ở nhân viên khác.");
            if (e.BirthDate.HasValue && e.BirthDate.Value.AddYears(15) > DateTime.Today)
                throw new ValidationException("Ngày sinh không hợp lệ (nhân viên phải từ 15 tuổi).");
            if (e.JoinDate.HasValue && e.LeaveDate.HasValue && e.LeaveDate < e.JoinDate)
                throw new ValidationException("Ngày nghỉ việc không thể trước ngày vào làm.");
            if (e.BasicSalary < 0) throw new ValidationException("Lương cơ bản không được âm.");
        }

        protected override void BeforeSave(EmployeeDTO e, bool isNew)
        {
            if (isNew || string.IsNullOrWhiteSpace(e.Code) || e.Code.StartsWith("(")) e.Code = dal.NextCode();
            if (isNew && !e.JoinDate.HasValue) e.JoinDate = DateTime.Today;
            if (e.Status == Const.Resigned && !e.LeaveDate.HasValue) e.LeaveDate = DateTime.Today;
            if (e.Status != Const.Resigned) e.LeaveDate = null;
        }

        protected override void BeforeDelete(EmployeeDTO e)
        {
            throw new ValidationException("Không xóa hồ sơ nhân viên để giữ lịch sử. Hãy chuyển trạng thái sang \"Nghỉ việc\".");
        }
    }

    public class ContractBUS : GenericBUS<ContractDTO>
    {
        readonly ContractDAL dal;
        public ContractBUS() : this(new ContractDAL()) { }
        ContractBUS(ContractDAL d) : base(Const.M_CONTRACT, d) { dal = d; }

        public override ContractDTO CreateNew() { return new ContractDTO { ContractNo = dal.NextNo(), StartDate = DateTime.Today }; }

        protected override void Validate(ContractDTO e)
        {
            base.Validate(e);
            if (e.EndDate.HasValue && e.EndDate < e.StartDate) throw new ValidationException("Ngày kết thúc phải sau ngày bắt đầu.");
            if (e.Salary < 0) throw new ValidationException("Lương không được âm.");
        }

        protected override void BeforeSave(ContractDTO e, bool isNew)
        {
            if (e.Type == "Không thời hạn") e.EndDate = null;
            else if (!e.EndDate.HasValue)
            {
                var s = e.StartDate.Value;
                switch (e.Type)
                {
                    case "1 năm": e.EndDate = s.AddYears(1).AddDays(-1); break;
                    case "2 năm": e.EndDate = s.AddYears(2).AddDays(-1); break;
                    case "3 năm": e.EndDate = s.AddYears(3).AddDays(-1); break;
                    default: e.EndDate = s.AddDays(59); break;   // thử việc: 60 ngày
                }
            }
        }

        public DataTable GetExpiring()
        {
            return dal.GetExpiring(AppSettings.GetInt("ContractWarnDays", 30));
        }
    }

    public class RecruitmentBUS : GenericBUS<RecruitmentRequestDTO>
    {
        public RecruitmentBUS() : base(Const.M_RECREQ) { }
        public override RecruitmentRequestDTO CreateNew() { return new RecruitmentRequestDTO { RequestDate = DateTime.Today }; }

        protected override void Validate(RecruitmentRequestDTO e)
        {
            base.Validate(e);
            if (e.Quantity <= 0) throw new ValidationException("Số lượng cần tuyển phải lớn hơn 0.");
        }

        public void SetStatus(RecruitmentRequestDTO e, string status)
        {
            Session.Require(Module, "edit");
            if (e.Status != Const.Pending) throw new ValidationException("Chỉ xử lý được đề xuất đang ở trạng thái \"Chờ duyệt\".");
            e.Status = status;
            Dal.Update(e);
            AuditBUS.Log(status == Const.Approved ? "Duyệt đề xuất" : "Từ chối đề xuất", Module, Describe(e));
        }
    }

    public class CandidateBUS : GenericBUS<CandidateDTO>
    {
        public CandidateBUS() : base(Const.M_CANDIDATE) { }
        public override CandidateDTO CreateNew() { return new CandidateDTO { AppliedDate = DateTime.Today }; }

        protected override void Validate(CandidateDTO e)
        {
            base.Validate(e);
            if (!string.IsNullOrWhiteSpace(e.Email) && !Regex.IsMatch(e.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException("Email không hợp lệ.");
            if (e.Status == Const.CandScheduled && !e.InterviewDate.HasValue)
                throw new ValidationException("Trạng thái \"Lên lịch PV\" cần nhập lịch phỏng vấn.");
        }

        /// <summary>Chuyển ứng viên trúng tuyển thành hồ sơ nhân viên thử việc + hợp đồng thử việc.</summary>
        public EmployeeDTO Onboard(CandidateDTO c)
        {
            Session.Require(Module, "edit");
            Session.Require(Const.M_EMPLOYEE, "add");
            if (c.EmployeeId.HasValue) throw new ValidationException("Ứng viên này đã được chuyển thành nhân viên.");
            if (c.Status != Const.CandPassed) throw new ValidationException("Chỉ ứng viên có trạng thái \"Đạt\" mới được onboard.");

            RecruitmentRequestDTO req = c.RequestId.HasValue ? new RecruitmentDAL().GetById(c.RequestId.Value) : null;
            if (req != null && req.Status != Const.Approved)
                throw new ValidationException("Đề xuất tuyển dụng của ứng viên này chưa được duyệt.");

            var emp = new EmployeeDTO
            {
                FullName = c.FullName, Email = c.Email, Phone = c.Phone, BirthDate = c.BirthDate,
                Status = Const.Probation, JoinDate = DateTime.Today,
                DepartmentId = req == null ? null : req.DepartmentId, PositionId = req == null ? null : req.PositionId
            };
            new EmployeeBUS().Save(emp);

            var cdal = new ContractDAL();
            var contract = new ContractDTO
            {
                EmployeeId = emp.Id, ContractNo = cdal.NextNo(), Type = "Thử việc",
                StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(59), Status = "Hiệu lực", Note = "Tự động tạo khi onboard"
            };
            cdal.Insert(contract);

            c.EmployeeId = emp.Id;
            c.Status = Const.CandOnboarded;
            Dal.Update(c);
            AuditBUS.Log("Onboard", Module, "Ứng viên " + c.FullName + " -> nhân viên " + emp.Code);
            return emp;
        }
    }
}
