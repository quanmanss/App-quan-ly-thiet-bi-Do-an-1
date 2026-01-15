
using DevicesControlApp.Data;
using DevicesControlApp.Models;
using DevicesControlApp.Views;
﻿using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace DevicesControlApp
{
    public partial class MainWindow : Window
    {
        private int _houseId;
        private int _userId;
        public ObservableCollection<Device> Devices { get; set; }

        DispatcherTimer scheduleTimer = new DispatcherTimer();
        public MainWindow(int houseId, int userId)
        {
            InitializeComponent();
            DataContext = this;
            _houseId = houseId;
            _userId = userId;

            Devices = new ObservableCollection<Device>(
                DatabaseHelper.GetDevices(_houseId)
               );
     
            LoadRooms();


            // Cau hinh timer chay moi 1 phut
            scheduleTimer.Interval = TimeSpan.FromSeconds(10);
            scheduleTimer.Tick += ScheduleTimer_Tick;
            scheduleTimer.Start();

        }


       private void LoadRooms()
        {
            var rooms = DatabaseHelper.GetRoomsByHouse(_houseId);
            RoomsList.ItemsSource = rooms;
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
                    foreach (var d in DatabaseHelper.GetDevices(_houseId))
                        Devices.Add(d);
                }
            }
        }
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDeviceWindow(_houseId);

            // Hiển thị cửa sổ dưới dạng modal và kiểm tra kết quả
            if (dialog.ShowDialog() == true)
            {

                // Tạo thiết bị mới
                var newDevice = dialog.CreatedDevice;

                // Thêm vào DB
                DatabaseHelper.AddDevice(newDevice);

                Devices.Clear(); // load lại DB
                // Thêm vào ObservableCollection, ListView sẽ tự cập nhật
                foreach (var d in DatabaseHelper.GetDevices(_houseId))
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
                device.IsOn = !device.IsOn;
                var button = sender as Button;
                button.Content = device.ButtonLabel;
                button.Background = device.IsOn ? Brushes.Green : Brushes.Gray;
                device.Status = device.IsOn ? "Bật" : "Tắt";

                // Cập nhật DB
                DatabaseHelper.UpdateStatus(device.ID, newStatus);

                // Refresh UI nếu không dùng INotifyPropertyChanged
                DevicesList.Items.Refresh();
            }
        }
       

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            

            var scheduleWindow = new ScheduleWindow(_userId, _houseId);
            scheduleWindow.Owner = this; // đặt MainWindow là owner (modal), là cha của Schdule để khi mở thì khóa cha
            scheduleWindow.ShowDialog();

            // Sau khi cửa sổ đóng, refresh danh sách thiết bị
            Devices.Clear();
            foreach (var d in DatabaseHelper.GetDevices(_houseId))
                Devices.Add(d);
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Device selectedDevice)
            {
                var detailsWindow = new DeviceDetailsWindow(selectedDevice);
                detailsWindow.ShowDialog();
            }
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var addRoomWindow = new AddRoomWindow(_houseId);
            if (addRoomWindow.ShowDialog() == true)
            {
                LoadRooms(); 
            }

        }
    }


}