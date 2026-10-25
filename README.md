# Lab W07 - Đa hình và Generic

- Họ và tên: **Ngô Ngọc Thái**
- Mã số sinh viên: **202418984**
- Ngôn ngữ: C#
- Nền tảng: .NET 8, ứng dụng console

## Chạy chương trình

Cài **.NET SDK 8.0** (không chỉ .NET Runtime), hoặc SDK mới hơn hỗ trợ `net8.0`. Giải nén rồi mở thư mục chứa `LabW07.csproj` bằng VS Code. Trong terminal tại thư mục đó:

```bash
dotnet build -c Release
dotnet run
```

Menu có các lựa chọn chạy minh họa, chạy kiểm thử và thoát. Mỗi lần minh họa hoặc kiểm thử sử dụng dữ liệu mới.

Chạy trực tiếp từng chế độ:

```bash
dotnet run -- --demo
dotnet run -- --test
```

Có thể chạy bản Release đã biên dịch:

```bash
dotnet bin/Release/net8.0/LabW07.dll --demo
dotnet bin/Release/net8.0/LabW07.dll --test
```

Trong Visual Studio có hỗ trợ .NET 8, mở `LabW07.csproj`, sau đó chạy chương trình. Dự án không dùng thư viện NuGet bên ngoài.

## Cấu trúc

| Đường dẫn | Nội dung |
|---|---|
| `Program.cs` | Điểm vào, menu và lựa chọn chế độ |
| `Models/` | DeviceStatus, TeachingDevice và ba lớp thiết bị |
| `Interfaces/` | Nhận diện, kho generic và chiến lược |
| `Repositories/` | InMemoryRepository<T> dùng List<T> |
| `Strategies/` | FirstAvailableStrategy<T>, HighestPriorityStrategy<T> |
| `Services/` | AllocationService<T> |
| `Exceptions/` | Ba ngoại lệ nghiệp vụ |
| `Demo/` | SampleData, DemoRunner |
| `Tests/` | TestRunner với 34 ca kiểm thử tự động |
| `docs/` | Thiết kế, sơ đồ UML, báo cáo và nhật ký kết quả |

## Nội dung thực hiện

- Lưu, tìm, xóa và lọc thiết bị theo điều kiện.
- Kiểm tra mức pin, chất lượng tín hiệu, tương thích phòng và trạng thái.
- Chọn theo phần tử đầu tiên hoặc điểm ưu tiên cao nhất; đổi chiến lược khi chạy.
- Cấp phát, trả và chuyển trạng thái bảo trì đúng quy tắc.
- Bảo vệ collection nội bộ, không chấp nhận null hoặc mã trùng.
- Tái sử dụng repository cho nhiều loại thiết bị.

Điều khiển điều hòa có điểm mặc định 100. Khi cấp cho một phòng cụ thể, truyền điều kiện `d => d.IsCompatibleWith(roomId)` vào `Allocate`. Điểm mặc định không tự chứa ngữ cảnh phòng. Mã thiết bị và mã phòng phân biệt hoa thường; chuỗi được loại bỏ khoảng trắng ở hai đầu.

`GetAll` trả một bản sao chỉ đọc của danh sách, không sao chép sâu các thiết bị. Vì vậy thiết bị được mượn vẫn cập nhật đúng trạng thái trong kho. Kho giả định Id của đối tượng không thay đổi sau khi thêm; các lớp thiết bị đều đáp ứng điều kiện này.

## Kết quả kiểm thử

Biên dịch bằng SDK 8.0.408: **0 lỗi, 0 cảnh báo**. Chạy bản Release: **34 PASS, 0 FAIL**.

- `docs/KetQuaKiemThu.txt`: đầu ra đầy đủ từ TestRunner.
- `docs/KetQuaMinhHoa.txt`: đầu ra đầy đủ từ DemoRunner.
- Ca 01-11 đối chiếu các tình huống bắt buộc trong đề.
- Ca 12-34 kiểm tra ngưỡng, ngoại lệ, collection, trạng thái và đa hình.

## Đưa lên GitHub

Tải toàn bộ nội dung thư mục đã giải nén, giữ nguyên các thư mục con và tệp `.csproj`. Không tải riêng tệp ZIP nếu cần xem mã trực tiếp trên GitHub. Không cần tải `bin/`, `obj/` hoặc `.vs/`; `.gitignore` đã loại các thư mục sinh tự động này khi dùng Git.

## Giới hạn

Dữ liệu chỉ tồn tại trong bộ nhớ. Chương trình minh họa cấp phát theo giao diện đề W07, chưa lưu người mượn, lịch sử mượn hoặc cơ sở dữ liệu. Các chiến lược mới phải trả về một ứng viên trong danh sách đã lọc và không được sửa danh sách đầu vào.
