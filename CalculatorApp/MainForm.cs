using System;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            textBoxOutput.Text = "0";
        }

        // Общий обработчик для цифровых и операторных кнопок
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (textBoxOutput.Text == "0" && btn.Text != "," && btn.Text != ")")
                textBoxOutput.Text = "";

            textBoxOutput.Text += btn.Text;
        }

        // Очистить всё (C)
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textBoxOutput.Text = "0";
        }

        // Удалить последний символ (del)
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (textBoxOutput.Text.Length > 0)
                textBoxOutput.Text = textBoxOutput.Text.Substring(0, textBoxOutput.Text.Length - 1);
            if (textBoxOutput.Text == "")
                textBoxOutput.Text = "0";
        }

        // Вставка функций sin(, cos(, sqrt( и т.д.)
        private void ButtonFunction_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            textBoxOutput.Text += btn.Text + "(";
        }
    }
}