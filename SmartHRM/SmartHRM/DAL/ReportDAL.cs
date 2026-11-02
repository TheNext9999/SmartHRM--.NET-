using System;
using System.Data;

namespace SmartHRM.DAL
{
    public class ReportDAL
    {
        const string Active = "e.status<>'Nghỉ việc'";

        public DataTable HeadcountByDepartment()
        {
            return DbHelper.Query("SELECT d.name AS `Phòng ban`, COUNT(e.id) AS `Số nhân viên` FROM departments d " +
                "LEFT JOIN employees e ON e.department_id=d.id AND e.status<>'Nghỉ việc' GROUP BY d.id, d.name ORDER BY d.code");
        }

        public DataTable GenderRatio()
        {
            return DbHelper.Query("SELECT IFNULL(e.gender,'Chưa rõ') AS `Giới tính`, COUNT(*) AS `Số lượng` FROM employees e WHERE " + Active + " GROUP BY e.gender");
        }

        public DataTable AgeGroups()
        {
            return DbHelper.Query(
                "SELECT g AS `Nhóm tuổi`, COUNT(*) AS `Số lượng` FROM (SELECT CASE " +
                "WHEN e.birth_date IS NULL THEN 'Chưa rõ' " +
                "WHEN TIMESTAMPDIFF(YEAR,e.birth_date,CURDATE())<25 THEN 'Dưới 25' " +
                "WHEN TIMESTAMPDIFF(YEAR,e.birth_date,CURDATE())<35 THEN '25 - 34' " +
                "WHEN TIMESTAMPDIFF(YEAR,e.birth_date,CURDATE())<45 THEN '35 - 44' " +
                "WHEN TIMESTAMPDIFF(YEAR,e.birth_date,CURDATE())<55 THEN '45 - 54' ELSE '55 trở lên' END g " +
                "FROM employees e WHERE " + Active + ") x GROUP BY g ORDER BY g");
        }

        public DataTable Seniority()
        {
            return DbHelper.Query(
                "SELECT g AS `Thâm niên`, COUNT(*) AS `Số lượng` FROM (SELECT CASE " +
                "WHEN e.join_date IS NULL THEN 'Chưa rõ' " +
                "WHEN TIMESTAMPDIFF(MONTH,e.join_date,CURDATE())<12 THEN 'Dưới 1 năm' " +
                "WHEN TIMESTAMPDIFF(MONTH,e.join_date,CURDATE())<36 THEN '1 - 3 năm' " +
                "WHEN TIMESTAMPDIFF(MONTH,e.join_date,CURDATE())<60 THEN '3 - 5 năm' ELSE 'Trên 5 năm' END g " +
                "FROM employees e WHERE " + Active + ") x GROUP BY g ORDER BY g");
        }

        /// <summary>Biến động nhân sự theo tháng trong năm: tuyển mới, nghỉ việc, tỷ lệ nghỉ việc.</summary>
        public DataTable Turnover(int year)
        {
            var dt = new DataTable();
            dt.Columns.Add("Tháng", typeof(string));
            dt.Columns.Add("Nhân viên mới", typeof(int));
            dt.Columns.Add("Nghỉ việc", typeof(int));
            dt.Columns.Add("Tỷ lệ nghỉ việc (%)", typeof(decimal));
            for (int m = 1; m <= 12; m++)
            {
                int hired = DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE MONTH(join_date)=@m AND YEAR(join_date)=@y", DbHelper.P("@m", m), DbHelper.P("@y", year));
                int left = DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE MONTH(leave_date)=@m AND YEAR(leave_date)=@y", DbHelper.P("@m", m), DbHelper.P("@y", year));
                int headcount = DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE (join_date IS NULL OR join_date<=LAST_DAY(@d)) AND (leave_date IS NULL OR leave_date>LAST_DAY(@d))",
                    DbHelper.P("@d", new DateTime(year, m, 1)));
                dt.Rows.Add("T" + m, hired, left, headcount == 0 ? 0m : Math.Round(left * 100m / headcount, 2));
            }
            return dt;
        }

        /// <summary>Chi phí lương - thưởng - bảo hiểm (phần doanh nghiệp) theo tháng.</summary>
        public DataTable PayrollCost(int year)
        {
            return DbHelper.Query(
                "SELECT CONCAT('T',m.n) AS `Tháng`, IFNULL(SUM(p.gross_income-p.bonus),0) AS `Lương`, IFNULL(SUM(p.bonus),0) AS `Thưởng`, " +
                "IFNULL(SUM(p.employer_insurance),0) AS `Bảo hiểm (DN)` " +
                "FROM (SELECT 1 n UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10 UNION SELECT 11 UNION SELECT 12) m " +
                "LEFT JOIN payroll p ON p.month=m.n AND p.year=@y GROUP BY m.n ORDER BY m.n", DbHelper.P("@y", year));
        }

        public DataTable PayrollCostLastMonths(int count)
        {
            return DbHelper.Query(
                "SELECT CONCAT(p.month,'/',p.year) AS `Kỳ`, SUM(p.gross_income-p.bonus) AS `Lương`, SUM(p.bonus) AS `Thưởng`, SUM(p.employer_insurance) AS `Bảo hiểm (DN)` " +
                "FROM payroll p GROUP BY p.year, p.month ORDER BY p.year DESC, p.month DESC LIMIT " + count);
        }

        // ----- Số liệu tổng quan Dashboard -----
        public int ActiveEmployees() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE status<>'Nghỉ việc'"); }
        public int NewThisMonth() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE YEAR(join_date)=YEAR(CURDATE()) AND MONTH(join_date)=MONTH(CURDATE())"); }
        public int LeftThisMonth() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE YEAR(leave_date)=YEAR(CURDATE()) AND MONTH(leave_date)=MONTH(CURDATE())"); }
        public int Probation() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM employees WHERE status='Thử việc'"); }
        public int PendingLeave() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM leave_requests WHERE status='Chờ duyệt'"); }
        public int OpenCandidates() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM candidates WHERE status IN ('Mới','Lên lịch PV','Đạt')"); }
        public int PresentToday() { return DbHelper.ScalarInt("SELECT COUNT(*) FROM attendance WHERE work_date=CURDATE() AND check_in IS NOT NULL"); }
        public decimal LastPayrollNet()
        {
            return DbHelper.ScalarDecimal("SELECT IFNULL(SUM(net_salary),0) FROM payroll WHERE (year,month)=(SELECT year,month FROM payroll ORDER BY year DESC, month DESC LIMIT 1)");
        }
    }
}
