# SmartHRM (.NET) - Hệ Thống Quản Trị Nhân Sự, Chấm Công & Tiền Lương

Ứng dụng desktop Windows Forms quản trị nhân sự, chấm công và tính lương toàn diện cho doanh nghiệp, được xây dựng trên nền tảng **C# .NET Framework 4.7.2**, mô hình kiến trúc **3 lớp (3-Tier Architecture)** và hệ quản trị cơ sở dữ liệu **MySQL**.

---

## 📋 Mục lục
1. [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
2. [Cài đặt & Kết nối Cơ sở dữ liệu MySQL](#-cài-đặt--kết-nối-cơ-sở-dữ-liệu-mysql)
   - [Bước 1: Khởi động MySQL Server](#bước-1-khởi-động-mysql-server)
   - [Bước 2: Import cơ sở dữ liệu `smarthrm.sql`](#bước-2-import-cơ-sở-dữ-liệu-smarthrmsql)
   - [Bước 3: Cấu hình chuỗi kết nối trong dự án](#bước-3-cấu-hình-chuỗi-kết-nối-trong-dự-án)
3. [Hướng dẫn chạy ứng dụng](#-hướng-dẫn-chạy-ứng-dụng)
   - [Cách 1: Chạy bằng Visual Studio (Khuyên dùng)](#cách-1-chạy-bằng-visual-studio-khuyên-dùng)
   - [Cách 2: Chạy trực tiếp từ file thực thi (.exe)](#cách-2-chạy-trực-tiếp-từ-file-thực-thi-exe)
4. [Tài khoản đăng nhập mặc định](#-tài-khoản-đăng-nhập-mặc-định)
5. [Cấu trúc dự án](#-cấu-trúc-dự-án)
6. [Xử lý sự cố thường gặp (Troubleshooting)](#-xử-lý-sự-cố-thường-gặp-troubleshooting)

---

## 💻 Yêu cầu hệ thống

Trước khi bắt đầu, hãy đảm bảo máy tính của bạn đã cài đặt các phần mềm sau:

- **Hệ điều hành:** Windows 10 / 11 (64-bit).
- **.NET Framework:** Phiên bản **4.7.2** trở lên (thường đã tích hợp sẵn trên Windows 10/11).
- **IDE:** [Visual Studio 2019 / 2022](https://visualstudio.microsoft.com/) (chọn workload **.NET desktop development**).
- **Hệ quản trị CSDL:** **MySQL Server 8.0+** (hoặc thông qua **XAMPP**, **Laragon**, **WampServer**).
- **Công cụ quản trị CSDL (tùy chọn):** [MySQL Workbench](https://www.mysql.com/products/workbench/), [DBeaver](https://dbeaver.io/), [HeidiSQL](https://www.heidisql.com/) hoặc [Navicat](https://www.navicat.com/).

---

## 🗄️ Cài đặt & Kết nối Cơ sở dữ liệu MySQL

Toàn bộ cấu trúc bảng và dữ liệu mẫu được lưu trong tệp:  
👉 **`Database/smarthrm.sql`**

### Bước 1: Khởi động MySQL Server
- Nếu cài đặt MySQL Server độc lập: Đảm bảo service `MySQL80` (hoặc tên tương đương) đang ở trạng thái **Running** (mặc định Port: `3306`).
- Nếu dùng **XAMPP / Laragon**: Bật module **MySQL** (nhấn **Start**).

---

### Bước 2: Import cơ sở dữ liệu `smarthrm.sql`

Bạn có thể chọn một trong các cách import sau:

#### Cách 2.1: Sử dụng MySQL Workbench (Khuyên dùng)
1. Mở **MySQL Workbench** và kết nối vào máy chủ MySQL Local.
2. Trên thanh menu, chọn **File** > **Open SQL Script...** (hoặc `Ctrl + Shift + O`).
3. Điều hướng và chọn tệp **`Database/smarthrm.sql`** trong thư mục dự án.
4. Nhấp vào biểu tượng **Tia sét (Execute)** trên thanh công cụ để thực thi toàn bộ script.
5. Sau khi chạy xong, nhấn chuột phải vào khung **Schemas** chọn **Refresh All**, bạn sẽ thấy cơ sở dữ liệu `smarthrm` với đầy đủ 23 bảng và dữ liệu mẫu.

#### Cách 2.2: Sử dụng Command Line (Terminal / CMD / PowerShell)
Mở cửa sổ dòng lệnh tại thư mục gốc của dự án và chạy:
```bash
mysql -u root -p < Database/smarthrm.sql
```
*(Hệ thống sẽ hỏi mật khẩu root MySQL của bạn, nhập mật khẩu và nhấn Enter)*.

Hoặc mở MySQL CLI rồi chạy lệnh `source`:
```sql
mysql -u root -p
-- Nhập mật khẩu, sau đó gõ:
source D:/BT Classroom Lab/SmartHRM (.NET)/Database/smarthrm.sql;
```

#### Cách 2.3: Sử dụng phpMyAdmin / HeidiSQL / DBeaver
1. Mở phpMyAdmin (hoặc HeidiSQL / DBeaver).
2. Tạo cơ sở dữ liệu mới mang tên `smarthrm` với Character set `utf8mb4` và Collation `utf8mb4_unicode_ci` (nếu script chưa tự tạo).
3. Chọn tab **Import** > Chọn tệp `Database/smarthrm.sql` > Bấm **Go / Execute**.

---

### Bước 3: Cấu hình chuỗi kết nối trong dự án

Mở tệp cấu hình **`SmartHRM/SmartHRM/App.config`** và kiểm tra thẻ `<connectionStrings>`:

```xml
<connectionStrings>
    <add name="SmartHRM"
         connectionString="Server=localhost;Port=3306;Database=smarthrm;Uid=root;Pwd=MAT_KHAU_MYSQL_CUA_BAN;CharSet=utf8mb4;AllowUserVariables=true;" />
</connectionStrings>
```

> [!IMPORTANT]
> - Thay `MAT_KHAU_MYSQL_CUA_BAN` ở thuộc tính `Pwd=` bằng **mật khẩu root MySQL trên máy của bạn** (Ví dụ nếu mật khẩu MySQL của bạn để trống: `Pwd=;`, hoặc nếu là `123456`: `Pwd=123456;`).
> - Giữ nguyên `Port=3306`, `Database=smarthrm` và `AllowUserVariables=true` để đảm bảo hệ thống truy vấn các biến và Stored Procedure hoạt động chính xác.
> - **Lưu ý:** Nếu bạn chạy ứng dụng qua file `.exe` trực tiếp (không qua Visual Studio), hãy cập nhật tương tự trong tệp **`SmartHRM/SmartHRM/bin/Debug/SmartHRM.exe.config`**.

---

## 🚀 Hướng dẫn chạy ứng dụng

### Cách 1: Chạy bằng Visual Studio (Khuyên dùng)
1. Mở thư mục dự án và nhấp đúp mở solution:
   - File: **`SmartHRM/SmartHRM.slnx`** (hoặc mở file dự án **`SmartHRM/SmartHRM/SmartHRM.csproj`**).
2. Kiểm tra thanh công cụ Visual Studio, chọn cấu hình: **Debug** hoặc **Release**, nền tảng **Any CPU** hoặc **x86/x64**.
3. Nếu mở lần đầu trên máy mới, chuột phải vào Solution trong khung *Solution Explorer* > Chọn **Restore NuGet Packages** để tự động tải các gói thư viện phụ thuộc (`MySqlConnector`, `BCrypt.Net-Next`, `ClosedXML`, ...).
4. Nhấn phím **F5** (hoặc nút **Start ▶**) để biên dịch và khởi chạy chương trình. Hoặc nhấn **Ctrl + F5** để chạy không kèm Debugger.

---

### Cách 2: Chạy trực tiếp từ file thực thi (.exe)
Nếu máy bạn đã có sẵn .NET Framework 4.7.2 và không cần mở mã nguồn:
1. Điều hướng đến thư mục đã biên dịch sẵn:
   ```
   SmartHRM/SmartHRM/bin/Debug/
   ```
2. Mở file **`SmartHRM.exe.config`** bằng Notepad / VS Code, đảm bảo thông tin `connectionString` (mật khẩu MySQL, port) đã đúng với máy bạn.
3. Nhấp đúp vào tệp **`SmartHRM.exe`** để mở ứng dụng.

---

## 🔑 Tài khoản đăng nhập mặc định

Hệ thống đã nạp sẵn các tài khoản demo tương ứng với từng nhóm quyền phân cấp trong CSDL:

| Tên đăng nhập | Email | Mật khẩu mặc định | Nhóm quyền / Vai trò | Quyền hạn tiêu biểu |
|:---|:---|:---:|:---|:---|
| **`admin`** | `admin@congty.vn` | **`admin123`** | **Quản trị hệ thống (Admin)** | Toàn quyền toàn bộ 25 phân hệ, phân quyền, cấu hình hệ thống, xem nhật ký kiểm toán (Audit Log). |
| **`hr`** | `hoa.dt@congty.vn` | **`123456`** | **Quản lý Nhân sự (HR)** | Quản lý hồ sơ nhân viên, hợp đồng, tuyển dụng, đào tạo, KPI, duyệt phép. |
| **`ketoan`** | `huy.tq@congty.vn` | **`123456`** | **Kế toán tiền lương** | Chấm công, chốt bảng công, bảng lương tháng, trích nộp bảo hiểm, thuế TNCN, xuất báo cáo Excel. |
| **`nv0007`** | `anh.vd@congty.vn` | **`123456`** | **Nhân viên (Self-service)** | Xem thông tin cá nhân, gửi đơn xin nghỉ phép, tự đánh giá KPI, xem phiếu lương cá nhân. |

> [!NOTE]
> Hệ thống tích hợp sẵn cơ chế **`DbInitializer`**: Nếu cơ sở dữ liệu khởi tạo lần đầu mà chưa có tài khoản nào, chương trình sẽ tự động cấp phát tài khoản quản trị tối cao `admin` với mật khẩu mặc định `admin123` và tự động sinh ma trận phân quyền chuẩn.

---

## 📂 Cấu trúc dự án

```text
SmartHRM (.NET)/
├── Database/
│   └── smarthrm.sql             # Script SQL tạo Database và toàn bộ dữ liệu mẫu
├── SmartHRM/
│   ├── SmartHRM.slnx            # File Solution Visual Studio
│   └── SmartHRM/
│       ├── App.config           # Tệp cấu hình ứng dụng & chuỗi kết nối MySQL
│       ├── Program.cs           # Điểm khởi chạy chính của ứng dụng (Main Entry)
│       ├── packages.config      # Danh sách các NuGet Package sử dụng
│       ├── SmartHRM.csproj      # File cấu hình Project C#
│       ├── DTO/                 # Data Transfer Objects (Entity ánh xạ bảng CSDL)
│       ├── DAL/                 # Data Access Layer (DbHelper, GenericDAL, truy vấn MySQL)
│       ├── BUS/                 # Business Logic Layer (Quy tắc nghiệp vụ, tính lương, chấm công)
│       ├── GUI/                 # Graphical User Interface (Windows Forms, User Controls, Chart)
│       ├── Common/              # Các tiện ích chung (AppLogger, Session, Const, Extensions)
│       ├── Reports/             # Mẫu in ấn báo cáo Crystal Reports (GenericGrid.rpt)
│       └── bin/Debug/           # Thư mục chứa file nhị phân SmartHRM.exe đã build
├── BAO_CAO_DU_AN_SMARTHRM.txt   # Báo cáo tổng kết chi tiết kiến trúc dự án
└── README.md                    # Tài liệu hướng dẫn cài đặt & vận hành dự án
```

---

## 🛠️ Xử lý sự cố thường gặp (Troubleshooting)

### 1. Lỗi: "Không kết nối được cơ sở dữ liệu MySQL" khi khởi động
- **Nguyên nhân 1:** Dịch vụ MySQL chưa được khởi động.  
  👉 **Khắc phục:** Mở `services.msc`, tìm `MySQL80` (hoặc bảng điều khiển XAMPP/Laragon) và bấm **Start**.
- **Nguyên nhân 2:** Sai mật khẩu đăng nhập MySQL (`Access denied for user 'root'@'localhost'`).  
  👉 **Khắc phục:** Mở `App.config` (và `SmartHRM.exe.config`), sửa tham số `Pwd=...` thành mật khẩu chính xác của máy bạn.
- **Nguyên nhân 3:** MySQL đang chạy ở cổng khác cổng mặc định `3306` (ví dụ `3307`).  
  👉 **Khắc phục:** Sửa `Port=3307` trong chuỗi kết nối.
- **Nguyên nhân 4:** Chưa import cơ sở dữ liệu `smarthrm`.  
  👉 **Khắc phục:** Thực hiện lại [Bước 2](#bước-2-import-cơ-sở-dữ-liệu-smarthrmsql) để nạp tệp `smarthrm.sql`.

### 2. Xem file log kỹ thuật chi tiết
Khi gặp bất kỳ lỗi ngoại lệ nào trong quá trình vận hành, hệ thống sẽ tự động ghi vết lỗi an toàn vào tệp nhật ký:
- Đường dẫn file log:  
  `%LocalAppData%\SmartHRM\Logs\` (Mở cửa sổ **Run** (`Win + R`), gõ `%LocalAppData%\SmartHRM\Logs` và nhấn Enter).
