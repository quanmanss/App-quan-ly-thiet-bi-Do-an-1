# Demo_ĐA1
Demo app GUI mô phỏng quản lý thiết bị thông minh - Đồ án 1 HUST
Tất cả các chức năng mới chỉ là mô phỏng, chưa kết nối với phần cứng thật
# Cách cài đặt
Clone về và chạy project trong Visual Studio
File .xaml là giao diện
File .cs là file C# xử lí các hành động tương tác.
Các file khác là các file cài đặt có sẵn khi tạo project.
Tạo 1 database trong SQL server để connect tới, gồm các bảng như trong thiết kế 
Cài dapper và micorosoft.data.sqlclient trong visual studio
# Các chức năng chính
# Bản 1.0:
- Quản lý thiết bị : giao diện hiện ra các thiết bị cùng trạng thái (Tắt/mở, lỗi, ... )
- Tắt/ mở thiết bị
- Xem chi tiết thiết bị, thông số,... (Update sau)
- Hẹn giờ bật tắt (Update sau)
- Thêm/sửa/xóa thiết bị (Update sau)
# Bản 2.0: ( đang thực hiện ở Tuần 10 )
- Sửa lại toàn bộ code theo tiệm cận mô hình MVVM
- Mapping toàn bộ dữ liệu vào Sql server local dùng dapper
- Update hẹn giờ, thêm xóa thiết bị
  
# Công nghệ sử dụng:
WPF và .NET framework
Figma design thiết kế giao diện tương tác
SQL server cho database
Dapper để map dữ liệu sang database
