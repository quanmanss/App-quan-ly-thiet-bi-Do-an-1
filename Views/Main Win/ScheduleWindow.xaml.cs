using DevicesControlApp.Models;
using DevicesControlApp.Data;
using System;
using System.Windows;
using System.Collections.Generic;

namespace DevicesControlApp.Views
{
    public partial class ScheduleWindow : Window
    {
        private int _userID;
        private int _houseID;
        public ScheduleWindow(int userID, int houseID)
        {
            InitializeComponent();
            _userID = userID;
            _houseID = houseID;
            // Load devices vào ComboBox
            List<Device> devices = DatabaseHelper.GetDevices(_houseID);
            cbDevices.ItemsSource = devices;
            cbDevices.DisplayMemberPath = "Name";
            cbDevices.SelectedValuePath = "ID";

            // Load action On/Off
            cbAction.ItemsSource = new string[] { "Bật", "Tắt" };
            cbAction.SelectedIndex = 0;
        }

        private void btnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (cbDevices.SelectedItem == null || cbAction.SelectedItem == null || dpTime.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin!");
                return;
            }

            if (!int.TryParse(txtHour.Text, out int hour) || !int.TryParse(txtMinute.Text, out int minute))
            {
                MessageBox.Show("Giờ hoặc phút không hợp lệ!");
                return;
            }

            DateTime date = dpTime.SelectedDate.Value;
            DateTime executeTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);

            var schedule = new Schedule
            {
                DeviceID = (int)cbDevices.SelectedValue,
                UserID = _userID,
                Action = cbAction.SelectedItem.ToString(),
                Status = "Pending",
                Time = executeTime,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            DatabaseHelper.AddSchedule(schedule);
            MessageBox.Show("Đặt lịch thành công!");
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
