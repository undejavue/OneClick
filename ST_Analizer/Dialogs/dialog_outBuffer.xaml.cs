using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace OneClickUI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для dialog_outBuffer.xaml
    /// </summary>
    public partial class dialog_outBuffer : Window
    {
        private List<string> resultText;
        private string rootdir;

        public dialog_outBuffer(List<String> outputText)
        {
            InitializeComponent();

            resultText = new List<string>(outputText);
            rootdir = Globals.get_RootDir();

            Loaded += dialog_outBuffer_Loaded;
        }

        private void dialog_outBuffer_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in resultText)
            {
                txt_result.AppendText(s + "\r\n");
            }
        }

        private void btn_Export2txt_Click(object sender, RoutedEventArgs e)
        {

            string dir = rootdir;
      
            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.InitialDirectory = dir;
            saveDlg.Filter = "Text documents (*.txt)|*.txt";

            // Set filter for file extension and default file extension
            saveDlg.DefaultExt = ".txt";
            

            // Display SaveFileDialog by calling ShowDialog method
            bool? result = saveDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                string safename = saveDlg.SafeFileName;
                string filename = saveDlg.FileName;

                dir = filename.Remove(filename.Length - safename.Length - 1);


                StreamWriter file = new StreamWriter(filename);

                //file.WriteLine("copyright");

                foreach (string line in resultText )
                {
                    try
                    {
                        file.WriteLine(line);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка создания файла");
                    }
                }

                file.Close();

            } 


        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
