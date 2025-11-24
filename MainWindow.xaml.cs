
﻿using System.Collections.ObjectModel;
using System.Windows;
using DevicesControlApp.Views;
using DevicesControlApp.Models;
using DevicesControlApp.Data;
using System.Windows.Threading;

namespace DevicesControlApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Device> Devices { get; set; }

        DispatcherTimer scheduleTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            Devices = new ObservableCollection<Device>(DatabaseHelper.GetDevices());
            DevicesList.ItemsSource = Devices;

            //        {
            //            new Device { Name = "Đèn phòng khách", Status = "Tắt", ButtonLabel = "Bật" },
            //            new Device { Name = "Quạt trần", Status = "Tắt", ButtonLabel = "Bật" }
            //        };

            //        //  Gán vào ListView sau khi Devices đã có dữ liệu
            //        DevicesList.ItemsSource = Devices;
            //    }

            // Cau hinh timer chay moi 1 phut
            scheduleTimer.Interval = TimeSpan.FromSeconds(10);
            scheduleTimer.Tick += ScheduleTimer_Tick;
            scheduleTimer.Start();

        }

        private void ScheduleTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var schedules = DatabaseHelper.GetPendingSchedules();
            foreach (var s in schedules)
            {
                if (s.Time <= now)
                {
                    // thực hiện hành động
                    string newStatus = s.Action == "On" ? "Bật" : "Tắt";
                    DatabaseHelper.UpdateStatus(s.DeviceID, newStatus);

                    // đánh dấu schedule đã xong
                    DatabaseHelper.UpdateScheduleStatus(s.ID, "Done");

                    // refresh UI
                    Devices.Clear();
                    foreach (var d in DatabaseHelper.GetDevices())
                        Devices.Add(d);
                }
            }
        }
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDeviceWindow();

            // Hiển thị cửa sổ dưới dạng modal và kiểm tra kết quả
            if (dialog.ShowDialog() == true)
            {

                // Tạo thiết bị mới
                var newDevice = dialog.CreatedDevice;

                // Thêm vào DB
                DatabaseHelper.AddDevice(newDevice);

                Devices.Clear(); // load lại DB
                // Thêm vào ObservableCollection, ListView sẽ tự cập nhật
                foreach (var d in DatabaseHelper.GetDevices())
                    Devices.Add(d);

            }
        }

        private void RemoveDevice_Click(object sender, RoutedEventArgs e)
        {
            if (DevicesList.SelectedItem is Device device)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa \"{device.Name}\"?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm != MessageBoxResult.Yes)
                    return;

                try
                {
                    DatabaseHelper.DeleteDevice(device.ID);
                    Devices.Remove(device);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Không thể xóa thiết bị: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thiết bị để xóa!");
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Device device)
            {
                // Toggle trạng thái
                string newStatus = device.Status == "Bật" ? "Tắt" : "Bật";
                device.Status = newStatus;
                device.ButtonLabel = newStatus == "Bật" ? "Tắt" : "Bật";

                // Cập nhật DB
                DatabaseHelper.UpdateStatus(device.ID, newStatus);

                // Refresh UI nếu không dùng INotifyPropertyChanged
                DevicesList.Items.Refresh();
            }
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            int currentUserId = 1; // Thay bằng user hiện tại nếu có multi-user

            var scheduleWindow = new ScheduleWindow(currentUserId);
            scheduleWindow.Owner = this; // đặt MainWindow là owner (modal)
            scheduleWindow.ShowDialog();

            // Sau khi cửa sổ đóng, có thể refresh danh sách thiết bị nếu muốn
            Devices.Clear();
            foreach (var d in DatabaseHelper.GetDevices())
                Devices.Add(d);
        }
    }
}