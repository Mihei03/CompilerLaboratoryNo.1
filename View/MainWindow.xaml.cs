using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CompilerDemo.ViewModel;
using Microsoft.Win32;


namespace CompilerDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            richTextBox.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(RichTextBox_DragOver), true);
            richTextBox.AddHandler(RichTextBox.DropEvent, new DragEventHandler(RichTextBox_Drop), true);
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, EventArgs e)
        {
                if (MessageBox.Show("Сохранить изменения в текущем файле перед выходом?", "Сохранить изменения?", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "RichText Files (*.txt)|*.txt|All files (*.*)|*.*";
                    if (sfd.ShowDialog() == true)
                    {
                        TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            doc.Save(fs, DataFormats.Text);
                        }
                    }
                }
        }
        private void RichTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        private void RichTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

                var dataFormat = DataFormats.Text;

                if (e.KeyStates == DragDropKeyStates.ShiftKey)
                {
                    dataFormat = DataFormats.Text;
                }

                System.Windows.Documents.TextRange range;
                System.IO.FileStream fStream;

                if (System.IO.File.Exists(docPath[0]))
                {
                    try
                    {
                        range = new System.Windows.Documents.TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        fStream = new System.IO.FileStream(docPath[0], System.IO.FileMode.OpenOrCreate);
                        range.Load(fStream, dataFormat);
                        fStream.Close();
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("File could not be opened. Make sure the file is a text file.");
                    }
                }
            }
        }

        private void fontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null)
            {
                if (cb.SelectedItem != null)
                {
                    string fontSize = ((ComboBoxItem)cb.SelectedItem).Content.ToString();
                    if (richTextBox != null)
                    {
                        richTextBox.FontSize = double.Parse(fontSize);
                    }
                    if (TB != null)
                    {
                        TB.FontSize = double.Parse(fontSize);
                    }

                }
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel? dataContext = DataContext as MainWindowViewModel;
            if (dataContext is null)
            {
                return;
            }

            if (!dataContext.CanClean)
            {
                return;
            }

            string t = dataContext.CleanText;
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(t)));
            dataContext.CanClean = false;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RichText Files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                        doc.Load(fs, DataFormats.Text);
                    else if (Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                        doc.Load(fs, DataFormats.Text);
                    else
                        doc.Load(fs, DataFormats.Xaml);
                }
            }
        }
        private void Load_Test(object sender, RoutedEventArgs e)
        {
            string filePath = @"..\..\..\html\correct_test_case.txt";
            TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                doc.Load(fs, DataFormats.Text);
            }
        }

    }
}