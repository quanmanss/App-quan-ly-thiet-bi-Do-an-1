using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System.Windows;
using System.Windows.Input;

namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for HouseSetupWindow.xaml
    /// </summary>
    public partial class HouseSetupWindow : Window
    {
        private User _currentUser;
        public HouseSetupWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void RoleChanged(object sender, RoutedEventArgs e)
        {
            if (OwnerRadio.IsChecked == true)
            {
                OwnerPanel.Visibility = Visibility.Visible;
                MemberPanel.Visibility = Visibility.Collapsed;
            }
            else if (MemberRadio.IsChecked == true)
            {
                OwnerPanel.Visibility = Visibility.Collapsed;
                MemberPanel.Visibility = Visibility.Visible;
            }
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (OwnerRadio.IsChecked == true)
            {
                // OWNER MODE → CREATE HOUSE
                string name = HouseNameBox.Text.Trim();
                string location = LocationBox.Text.Trim();

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Cần điền tên Nhà!");
                    return;
                }

                House newHouse = new House()
                {
                    Name = name,
                    Location = location,
                    OwnerUserID = _currentUser.ID
                };

                int houseId = DatabaseHelper.CreateHouse(newHouse);
                DatabaseHelper.AddHouseMember(houseId, _currentUser.ID, "Owner");

                DatabaseHelper.CreateRoom(houseId, "Phòng khách");
                MessageBox.Show("Tạo nhà thành công!");

                // MỞ MAINWINDOW
                MainWindow mainWindow = new MainWindow(houseId, _currentUser.ID);
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
            //else if (MemberRadio.IsChecked == true)
            //{
            //    // MEMBER MODE → JOIN HOUSE
            //    string joinCode = JoinCodeBox.Text.Trim();

            //    int houseId = DatabaseHelper.GetHouseIdByJoinCode(joinCode);

            //    if (houseId <= 0)
            //    {
            //        MessageBox.Show("Invalid join code!");
            //        return;
            //    }

            //    DatabaseHelper.AssignUserToHouse(_currentUser.ID, houseId);

            //    MessageBox.Show("Joined house successfully!");

                //MainWindow main = new MainWindow();
                //main.Show();
                //this.Close();
            }
            //else
            //{
            //    MessageBox.Show("Please choose Owner or Member.");
            //}
        }
    }

