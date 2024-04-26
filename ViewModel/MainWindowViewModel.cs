using CompilerDemo.View;
using CompilerDemo.Model;
using CompilerDemo.Model.Parser;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CompilerDemo.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private string path = string.Empty;
        private string _text;
        private List<Token> _tokens = new List<Token>();
        private Lexer _lexer = new Lexer();
        private Parser _parser = new Parser();
        private ObservableCollection<TokenViewModel> _tokenViewModels = new ObservableCollection<TokenViewModel>();
        private ObservableCollection<ParseError> _parsingError = new ObservableCollection<ParseError>();
        public string CleanText;
        public bool CanClean { get; set; }

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        public ICommand NeutralizationCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FormulationCommand { get; }
        public ICommand GrammarCommand { get; }
        public ICommand ClassificationCommand { get; }
        public ICommand AnalysisCommand { get; }
        public ICommand DiagnosticsCommand { get; }
        public ICommand LiteratureCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand ReferenceCommand { get; }
        public ICommand AboutProgramCommand { get; }

        public MainWindowViewModel()
        {
            RunCommand = new RelayCommand(Run);
            CreateCommand = new RelayCommand(Create);
            OpenCommand = new RelayCommand(TryOpen);
            SaveCommand = new RelayCommand(Save);
            SaveAsCommand = new RelayCommand(SaveAs);
            DeleteCommand = new RelayCommand(Delete);
            FormulationCommand = new RelayCommand(Formulation);
            GrammarCommand = new RelayCommand(Grammar);
            ClassificationCommand = new RelayCommand(Classification);
            AnalysisCommand = new RelayCommand(Analysis);
            DiagnosticsCommand = new RelayCommand(Diagnostics);
            LiteratureCommand = new RelayCommand(Literature);
            InfoCommand = new RelayCommand(Info);
            ReferenceCommand = new RelayCommand(Reference);
            AboutProgramCommand = new RelayCommand(AboutProgram);
        }

        private void Run()
        {
            Scan();
            Parse();
        }

        private void Parse()
        {
            if (Text is null)
            {
                return;
            }

            if(Text.Length == 0)
            {
                return;
            }

            ParsingErrors.Clear();
            List<ParseError> errorList = _parser.Parse(_tokens);
            CanClean = true;
            foreach (ParseError error in errorList)
            {
                ParsingErrors.Add(error);
            }
        }
        private void Scan()
        {
            if (Text == string.Empty)
            {
                return;
            }

            TokenViewModels.Clear();
            _tokens = _lexer.Scan(Text).ToList();
            foreach (Token token in _tokens)
            {
                TokenViewModels.Add(new TokenViewModel(token));
            }
        }
        private void Create()
        {
            if (string.IsNullOrWhiteSpace(Text)) return;
            var result = MessageBox.Show("Вы хотите сохранить изменения в файле?", "Компилятор", MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        path = saveFileDialog.FileName;
                        File.WriteAllText(path, Text);
                        Text = string.Empty;
                    }
                    break;

                case MessageBoxResult.No:
                    Text = string.Empty;
                    break;

                case MessageBoxResult.Cancel:
                    return;
            }
        }
        private void Save()
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, Text);
            }
            else SaveAs();
        }
        private void TryOpen()
        {
            if (!string.IsNullOrWhiteSpace(Text) && !File.Exists(path))
            {
                var result = MessageBox.Show("Вы хотите сохранить изменения в файле?", "Компилятор",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            path = saveFileDialog.FileName;
                            File.WriteAllText(path, Text);
                            Text = string.Empty;
                            Open();
                        }
                        break;

                    case MessageBoxResult.No:
                        Text = string.Empty;
                        Open();
                        break;

                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            else if (!string.IsNullOrWhiteSpace(Text) && File.Exists(path))
            {
                var result = MessageBox.Show("Вы хотите сохранить изменения в файле \n" + path + "?", "Компилятор",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        File.WriteAllText(path, Text);
                        Text = string.Empty;
                        Open();
                        break;

                    case MessageBoxResult.No:
                        Text = string.Empty;
                        Open();
                        break;

                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            else Open();
        }
        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                string buffer;
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        buffer = sr.ReadToEnd();
                    }
                }
                Text = buffer;
            }
        }
        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
                File.WriteAllText(path, Text);
            }
        }

        private void Delete()
        {
            Text = string.Empty;
        }

        private void Reference()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\Reference.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Formulation()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\FormulationOfTheProblem.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Grammar()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\Grammar.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Classification()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\ClassificationOfGrammar.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Analysis()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\MethodOfAnalysis.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Diagnostics()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\DiagnosticsAndNeutralizationOfErrors.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Literature()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"..\..\..\html\ListOfLiterature.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void Info()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"https://disk.yandex.ru/d/8tOLehrsWQav6A")
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void AboutProgram()
        {
            AboutProgramWindow window = new AboutProgramWindow();
            window.DataContext = this;
            window.Show();
        }
        public ObservableCollection<TokenViewModel> TokenViewModels
        {
            get { return _tokenViewModels; }
            set { _tokenViewModels = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ParseError> ParsingErrors
        {
            get { return _parsingError; }
            set { _parsingError = value; OnPropertyChanged(); }
        }
    }
}