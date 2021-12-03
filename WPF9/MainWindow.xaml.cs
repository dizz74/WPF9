using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool saved = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//family
        {
            if (textBox != null)
                textBox.FontFamily = new FontFamily((sender as ComboBox).SelectedItem as string);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)//size
        {
            if (textBox != null)
                textBox.FontSize = Convert.ToInt32((sender as ComboBox).SelectedItem as string);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.FontWeight == FontWeights.Normal)
                textBox.FontWeight = FontWeights.Bold;
            else textBox.FontWeight = FontWeights.Normal;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBox.FontStyle == FontStyles.Italic)
                textBox.FontStyle = FontStyles.Normal;
            else textBox.FontStyle = FontStyles.Italic;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (textBox.TextDecorations == null || textBox.TextDecorations.Count == 0)
                textBox.TextDecorations = TextDecorations.Underline;
            else textBox.TextDecorations = null;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Black;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Red;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (EditorSavedOrIgnoreNotSavedText())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текст (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    textBox.Text = File.ReadAllText(openFileDialog.FileName);
                }
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текст (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox.Text);
                saved = true;
            }
        }


        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void SaveCanExec(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = textBox.Text.Length > 0;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saved = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !EditorSavedOrIgnoreNotSavedText("выйти");
        }
        private bool EditorSavedOrIgnoreNotSavedText(string action = "продолжить")
        { //проверка текст сохранен или запросить игнор сохранения текста
            bool status = true;
            if (textBox.Text.Length > 0 && !saved)
            {
                status = MessageBox.Show($"Текст в редакторе не сохранен, {action}?", "Текст не сохранен", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            return status;
        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {            
            Application.Current.Resources.MergedDictionaries.Clear();
            Uri theme = new Uri(ThemeBox.SelectedIndex==0?"LightTheme.xaml": "DarkTheme.xaml", UriKind.Relative);
          
            //ResourceDictionary mainRes = Application.LoadComponent(new Uri("ResDict.xaml",UriKind.Relative)) as ResourceDictionary;
            //Application.Current.Resources.MergedDictionaries.Add(mainRes);
            //работает и без этого...


            ResourceDictionary themeDict = Application.LoadComponent(theme) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
          
        }
    }
}
