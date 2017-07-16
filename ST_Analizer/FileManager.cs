using Microsoft.Win32;

namespace OneClickUI
{
    public class FileManager
    {

        public static string OpenFile(string dir)
        {
            var openDlg = new OpenFileDialog();

            openDlg.InitialDirectory = dir;
            openDlg.Filter = "Excel documents (*.xls, *.xlsm)|*.xls; *.xlsx; *.xslm|All Files (*.*)|*.*";

            // Set filter for file extension and default file extension
            openDlg.DefaultExt = ".xls";

            // Display OpenFileDialog by calling ShowDialog method
            var result = openDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result.HasValue && result.Value)
            {
                var safename = openDlg.SafeFileName;
                var filename = openDlg.FileName;
                dir = filename; //Path.GetDirectoryName(filename);

            }
            return dir;
        }
    }
}
