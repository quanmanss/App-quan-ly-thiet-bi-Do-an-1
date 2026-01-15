using DevicesControlApp.Data;
using DevicesControlApp.Models;

using System.Windows;
using System.Xml.Linq;



namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for DevicesWindow.xaml
    /// </summary>
    public partial class AddDeviceWindow : Window
    {
        private readonly int _houseId;
        public Device CreatedDevice{ get; private set; }

        public AddDeviceWindow(int houseId)
        {
            InitializeComponent();
            _houseId = houseId;
            var rooms = DatabaseHelper.GetRoomsByHouse(_houseId);
            RoomComboBox.ItemsSource = rooms;
        }
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thiết bị!");
                return;
            }

            var dialog = new AddDeviceWindow(_houseId);


            if (RoomComboBox.SelectedItem is not Room room)
            {
                MessageBox.Show("Vui long chon phong!");
                return;
            }

            CreatedDevice = new Device
            {
                Name = txtName.Text,
                Type = txtType.Text,
                Description = txtDescription.Text,
                Status = "Tắt",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RoomID = room.ID
            };

            DialogResult = true;
            Close();

            MessageBox.Show("Them thanh cong!");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        
    }
}

