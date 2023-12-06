using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Interactivity;

namespace musicvisualizerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UploadMusicFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if(openFileDialog.ShowDialog() == true)
            {
                // User selected a music file and only retrieving the file name
                string selectedFileName = System.IO.Path.GetFileName(openFileDialog.FileName);

                // Checking if selectedFileName is a music file
                string fileExtension = System.IO.Path.GetExtension(selectedFileName);
                if(fileExtension.Equals(".mp3", StringComparison.OrdinalIgnoreCase) ||
                   fileExtension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                {

                    // Can perform operations with the file user selected
                    MusicFile.Content = "Play Song";
                    fileUploadPanel.Visibility = Visibility.Collapsed;
                    musicPlayerPanel.Visibility = Visibility.Visible; 
                }
                else
                {
                    // Displaying the error message for invalid file type
                    MessageBox.Show("Please select a valid music file (mp3 or wav)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }
            else
            {
                // User canceled the file selection
                MessageBox.Show("Please select a music file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlayClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void PauseClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void StopClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
