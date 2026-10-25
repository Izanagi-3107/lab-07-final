# Thiết kế Lab W07

Sinh viên: Ngô Ngọc Thái - 202418984

## Phạm vi
Quản lý và cấp phát thiết bị trong bộ nhớ. Ba loại thiết bị: điều khiển máy chiếu, điều khiển điều hòa và micro không dây. Không bổ sung phiếu mượn hay cơ sở dữ liệu vì giao diện dịch vụ của W07 chỉ yêu cầu chọn, mượn và trả thiết bị.

## Các phương án
1. Một lớp quản lý dùng if/else theo loại thiết bị: cấu trúc đơn giản nhưng phải sửa nhiều nhánh khi thêm loại mới. Luồng: Program → Manager → từng loại thiết bị.
2. Mỗi loại có kho và dịch vụ riêng: dễ đọc trong chương trình nhỏ nhưng lặp thuật toán, khó bảo trì. Luồng: Program → ba dịch vụ → ba kho tương ứng.
3. Lớp cơ sở trừu tượng, repository generic và Strategy: tái sử dụng mã, an toàn kiểu, thay đổi thuật toán khi chạy. Luồng: Program → AllocationService<T> → IRepository<T> và ISelectionStrategy<T>. Có thêm interface và lớp nhưng phù hợp yêu cầu đề. Đây là phương án được chọn.

## Phân công trách nhiệm
- IIdentifiable: cung cấp Id để kho tìm đối tượng.
- TeachingDevice: dữ liệu chung, kiểm tra đầu vào và chuyển trạng thái mượn/trả.
- ProjectorRemote, AirConditionerRemote, WirelessMicrophone: điều kiện sẵn sàng và điểm ưu tiên riêng.
- IRepository<T>, InMemoryRepository<T>: thêm, xóa, tìm và lọc. T có constraint class, IIdentifiable để biểu diễn null khi tìm không thấy và truy cập Id.
- ISelectionStrategy<T>: giao diện chọn một đối tượng từ danh sách ứng viên.
- HighestPriorityStrategy<T>: nhận hàm tính điểm từ ngoài; chọn điểm cao nhất, hòa điểm giữ phần tử đầu tiên.
- FirstAvailableStrategy<T>: chọn ứng viên đầu tiên; danh sách đã được dịch vụ lọc.
- AllocationService<T>: T kế thừa TeachingDevice; lọc, chọn, gọi Borrow. Không kiểm tra loại thiết bị bằng if/else.

## Đa hình và generic
Đa hình thứ nhất nằm ở IsReadyForUse và GetDefaultPriorityScore khi gọi qua TeachingDevice. Đa hình thứ hai nằm ở Select khi gọi qua ISelectionStrategy<T>. Generic giữ đúng kiểu đối tượng cho kho, chiến lược và dịch vụ, không thay thế việc ghi đè hành vi.

## Quyết định và trade-off
Dùng List<T> để giữ đúng thứ tự thêm cho FirstAvailableStrategy và đơn giản hóa chương trình nhỏ. Tìm, xóa và kiểm tra trùng mã là O(n); Dictionary có tra cứu trung bình O(1) nhưng muốn cam kết thứ tự lựa chọn nên quản lý thứ tự rõ ràng. GetAll và FindAll trả bản sao danh sách chỉ đọc, các thiết bị vẫn là tham chiếu chung để trạng thái mượn được cập nhật trong kho.

Id bất biến, so sánh phân biệt hoa thường. Các chuỗi định danh được Trim; giá trị trắng bị từ chối. CompatibleRooms là tập bất biến. Điểm điều khiển điều hòa bằng 100 vì phương thức tính điểm không nhận phòng; điều kiện tương thích phòng được lọc bắt buộc ở lời gọi cấp phát cho phòng cụ thể. Phòng không tương thích không lọt vào danh sách ứng viên.

UnderMaintenance không được cấp phát. Bổ sung StartMaintenance và FinishMaintenance để có thể thao tác trạng thái này hợp lệ. Trả khi chưa mượn và chuyển bảo trì sai trạng thái phát sinh InvalidOperationException. Không tạo generic cho từng lớp thiết bị vì dữ liệu và quy tắc đã xác định cụ thể.

## Kế hoạch kiểm thử
Khởi tạo 3 điều khiển máy chiếu, 2 điều khiển điều hòa và 3 micro. Kiểm tra đủ 11 tình huống trong đề, sau đó kiểm tra ngưỡng pin/tín hiệu, đầu vào không hợp lệ, bảo vệ collection, mượn/trả sai trạng thái và bảo trì. Kết quả chạy được lưu trong docs/KetQuaKiemThu.txt.
