using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;
using SmartHRM.DTO;

namespace SmartHRM.DAL
{
    public class DepartmentDAL : GenericDAL<DepartmentDTO>
    {
        public override List<DepartmentDTO> GetAll(string where = null, string orderBy = "id DESC", params MySqlParameter[] ps)
        {
            return base.GetAll(where, orderBy == "id DESC" ? "code" : orderBy, ps);
        }
        public int CountEmployees(int deptId)
        {
            return DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE department_id=@d AND status<>'Nghỉ việc'", DbHelper.P("@d", deptId));
        }
    }

    public class EmployeeDAL : GenericDAL<EmployeeDTO>
    {
        public string NextCode()
        {
            int n = DbHelper.ScalarInt("SELECT IFNULL(MAX(CAST(SUBSTRING(code,3) AS UNSIGNED)),0)+1 FROM employees");
            return "NV" + n.ToString("D4");
        }
        public EmployeeDTO GetByCode(string code)
        {
            return GetAll("code=@c", "id", DbHelper.P("@c", code)).FirstOrDefault();
        }
        public void AdjustLeaveBalance(int employeeId, decimal delta)
        {
            DbHelper.Execute("UPDATE employees SET annual_leave_balance = annual_leave_balance + @d WHERE id=@i",
                DbHelper.P("@d", delta), DbHelper.P("@i", employeeId));
        }
        public List<EmployeeDTO> GetByDepartment(int deptId)
        {
            return GetAll("department_id=@d", "code", DbHelper.P("@d", deptId));
        }
    }

    public class ContractDAL : GenericDAL<ContractDTO>
    {
        public DataTable GetExpiring(int days)
        {
            return DbHelper.Query(
                "SELECT c.contract_no, e.code, e.full_name, c.type, c.end_date, DATEDIFF(c.end_date, CURDATE()) AS days_left " +
                "FROM contracts c JOIN employees e ON e.id=c.employee_id " +
                "WHERE c.status='Hiệu lực' AND c.end_date IS NOT NULL AND e.status<>'Nghỉ việc' " +
                "AND c.end_date BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @d DAY) ORDER BY c.end_date", DbHelper.P("@d", days));
        }
        public int MarkExpired()
        {
            return DbHelper.Execute("UPDATE contracts SET status='Hết hạn' WHERE status='Hiệu lực' AND end_date IS NOT NULL AND end_date < CURDATE()");
        }
        public string NextNo()
        {
            int n = DbHelper.ScalarInt("SELECT COUNT(*)+1 FROM contracts WHERE YEAR(start_date)=YEAR(CURDATE())");
            return "HD-" + DateTime.Today.Year + "-" + n.ToString("D4");
        }
    }

    public class RecruitmentDAL : GenericDAL<RecruitmentRequestDTO> { }
    public class CandidateDAL : GenericDAL<CandidateDTO> { }
}
