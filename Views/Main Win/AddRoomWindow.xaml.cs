using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System.Windows;


namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for AddRoomWindow.xaml
    /// </summary>
    public partial class AddRoomWindow : Window
    {
        private readonly int _houseId;
        public AddRoomWindow(int houseId)
        {
            InitializeComponent();
            _houseId = houseId;
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!");
                return;
            }

            DatabaseHelper.CreateRoom(_houseId, txtRoomName.Text);
            DialogResult = true;
            Close();
        }
    }
}
