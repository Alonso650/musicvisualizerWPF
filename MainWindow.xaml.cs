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
            
            //openFileDialog.Filter = "Audio Files |*.mp3;*.wav|All files (*.*)|*.*";

            if(openFileDialog.ShowDialog() == true)
            {
                // User selected a music file 
                string selectedFileName = openFileDialog.FileName;

                // Checking if selectedFileName is a music file
                string fileExtension = System.IO.Path.GetExtension(openFileDialog.FileName);
                if(fileExtension.Equals(".mp3", StringComparison.OrdinalIgnoreCase) ||
                   fileExtension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    // Can perform operations with the file user selected
                    MusicFile.Content = selectedFileName;
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
    }
}
