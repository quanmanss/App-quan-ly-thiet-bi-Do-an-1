using System.Windows;
using DevicesControlApp.Models;

namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for DeviceDetailsWindow.xaml
    /// </summary>
    public partial class DeviceDetailsWindow : Window
    {
        public DeviceDetailsWindow(Device device)
        {
            InitializeComponent();
            DataContext = device;
        }
    }
}
