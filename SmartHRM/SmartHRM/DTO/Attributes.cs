using System;

namespace SmartHRM.DTO
{
    /// <summary>Tên bảng MySQL của DTO.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; }
        public TableAttribute(string name) { Name = name; }
    }

    /// <summary>Thuộc tính không ánh xạ xuống cột CSDL (vd: mật khẩu mới nhập từ form).</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute { }

    public enum FieldKind { Text, MultiLine, Integer, Decimal, Date, DateAndTime, Time, Combo, Lookup, Password, Bool, File }

    /// <summary>Mô tả trường dữ liệu: nhãn, kiểu nhập, nguồn dữ liệu... để form CRUD tự dựng giao diện.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public string Label { get; }
        public FieldKind Kind { get; set; } = FieldKind.Text;
        /// <summary>Combo: "A|B|C". Lookup: "bảng|cột_id|biểu_thức_hiển_thị".</summary>
        public string Source { get; set; }
        public bool Required { get; set; }
        public bool ReadOnly { get; set; }
        public bool Hidden { get; set; }      // không hiện trong form sửa
        public bool InGrid { get; set; } = true;
        public FieldAttribute(string label) { Label = label; }
    }
}
