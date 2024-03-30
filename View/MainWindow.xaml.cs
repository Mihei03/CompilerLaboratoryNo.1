using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CompilerDemo.ViewModel;


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

                // By default, open as Rich Text (RTF).
                var dataFormat = DataFormats.Text;

                TextRange range;
                FileStream fStream;
                if (File.Exists(docPath[0]))
                {
                    try
                    {
                        // Open the document in the RichTextBox.
                        range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        fStream = new FileStream(docPath[0], FileMode.OpenOrCreate);
                        range.Load(fStream, dataFormat);
                        fStream.Close();
                    }
                    catch (Exception)
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
    }
}
