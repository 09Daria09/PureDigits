using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace PureDigits
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string currentText = ExpressionInput.Text;
                string btnContent = btn.Content.ToString();

                bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/';

                if (currentText.Length > 0 && IsOperator(currentText[currentText.Length - 1]) && IsOperator(btnContent[0]))
                {
                    return; 
                }

                if (btnContent == ".")
                {
                    if (string.IsNullOrEmpty(currentText))
                    {
                        return;
                    }
                    else
                    {
                        char lastChar = currentText[currentText.Length - 1];
                        if (!Char.IsDigit(lastChar) && lastChar != '.')
                        {
                            return;
                        }
                    }
                }

                ExpressionInput.Text += btnContent;
            }


        }

        private void ButtonRow6Col2_Click(object sender, RoutedEventArgs e)
        {
            string expression = ExpressionInput.Text;
            if (string.IsNullOrEmpty(expression)) return;

            if (Regex.IsMatch(expression, @"/0(?![.])"))
            {
                MessageBox.Show("Деление на ноль недопустимо!");
                return;
            }

            if (!Regex.IsMatch(expression, @"^[\d+\-*/.()]+$"))
            {
                MessageBox.Show("Введено некорректное выражение!");
                return;
            }
            double result;
            try
            {
                DataTable table = new DataTable();
                result = Convert.ToDouble(table.Compute(expression, string.Empty));

                double roundedResult = Math.Round(result, 5);

                InputHistory.Items.Add($"{expression} = {roundedResult}");
                ExpressionInput.Text = roundedResult.ToString("G");

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при вычислении выражения!");
            }
        }


        private void ButtonRow0Col2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ExpressionInput.Text))
            {
                ExpressionInput.Text = ExpressionInput.Text.Substring(0, ExpressionInput.Text.Length - 1);
            }
        }

        private void ButtonRow0Col1_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInput.Clear();
        }

        private void ButtonRow0Col0_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ExpressionInput.Text))
                return;

            int i = ExpressionInput.Text.Length - 1;
            bool hasDot = false;

            while (i >= 0 && (Char.IsDigit(ExpressionInput.Text[i]) ||
                              ExpressionInput.Text[i] == '.' && !hasDot ||
                              ExpressionInput.Text[i] == '-' && i == 0))
            {
                if (ExpressionInput.Text[i] == '.')
                    hasDot = true;
                i--;
            }

            ExpressionInput.Text = ExpressionInput.Text.Substring(0, i + 1);

        }

        private void InputHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InputHistory.SelectedItem != null)
            {
                string fullEntry = InputHistory.SelectedItem.ToString();
                int index = fullEntry.IndexOf('=');
                if (index > 0)
                {
                    ExpressionInput.Text = fullEntry.Substring(0, index).Trim();
                }
                else
                {
                    ExpressionInput.Text = fullEntry; 
                }
            }
        }
    }
}
