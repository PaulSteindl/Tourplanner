using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Shapes;
using Tourplanner.ViewModels;

namespace Tourplanner.Views
{
    /// <summary>
    /// Interaction logic for ImportWindowView.xaml
    /// </summary>
    public partial class ImportWindowView : Window
    {
        public ImportWindowView()
        {
            InitializeComponent();
        }

        /*private void FileDropStackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string directoryName = System.IO.Path.GetFullPath(files[0]);

                FileNameLabel.Content = directoryName;
            }
        }*/
    }
}
