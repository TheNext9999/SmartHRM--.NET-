using MySqlConnector;
using SmartHRM.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SmartHRM.DAL
{
    public class DepartmentDAL : GenericDAL<DepartmentDTO>
    {
        public override List<DepartmentDTO> GetAll(string where = null, string orderBy = "id DESC", params MySqlParameter[] ps)
        {
            return base.GetAll(where, orderBy == "id DESC" ? "code" : orderBy, ps);
        }
        /// <summary>Số nhân viên đang làm theo từng phòng ban (một truy vấn duy nhất).</summary>
        public Dictionary<int, int> CountAll()
        {
            var dt = DbHelper.Query("SELECT department_id, COUNT(*) AS c FROM employees WHERE status<>'Nghỉ việc' AND department_id IS NOT NULL GROUP BY department_id");
            return dt.Rows.Cast<DataRow>().ToDictionary(r => Convert.ToInt32(r["department_id"]), r => Convert.ToInt32(r["c"]));
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
        /// <summary>Danh sách nhân viên (để hiển thị) của một hoặc nhiều phòng ban.</summary>
        public DataTable GetTableByDepartments(IEnumerable<int> deptIds)
        {
            var ids = string.Join(",", deptIds.Select(i => i.ToString()).DefaultIfEmpty("0"));   // chỉ gồm số nguyên nên an toàn
            return DbHelper.Query(
                "SELECT e.code AS `Mã NV`, e.full_name AS `Họ tên`, IFNULL(p.name,'') AS `Chức vụ`, IFNULL(d.name,'') AS `Đơn vị`, " +
                "e.status AS `Trạng thái`, IFNULL(e.email,'') AS `Email`, IFNULL(e.phone,'') AS `Điện thoại` " +
                "FROM employees e LEFT JOIN positions p ON p.id=e.position_id LEFT JOIN departments d ON d.id=e.department_id " +
                "WHERE e.department_id IN (" + ids + ") ORDER BY d.code, e.code");
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