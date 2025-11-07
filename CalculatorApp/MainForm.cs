using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // Главный обработчик вычисления
        private void buttonSolve_Click(object sender, EventArgs e)
        {
            try
            {
                string expr = textBoxOutput.Text;

                // заменяем запятые на точки
                expr = expr.Replace(',', '.');

                // заменяем константы
                expr = expr.Replace("PI", Math.PI.ToString(CultureInfo.InvariantCulture));
                expr = expr.Replace("e", Math.E.ToString(CultureInfo.InvariantCulture));

                // вычисляем возведение в степень (заменяем a^b на результат)
                expr = EvaluatePowers(expr);

                // вычисляем функции sin, cos, sqrt и т.д.
                expr = EvaluateFunctions(expr);

                // вычисляем выражение через DataTable
                var result = new DataTable().Compute(expr, null);
                textBoxOutput.Text = Convert.ToString(result, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка вычисления",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Вычисляем степени a^b
        private string EvaluatePowers(string expr)
        {
            while (expr.Contains("^"))
            {
                var match = Regex.Match(expr, @"([0-9\.]+)\^([0-9\.]+)");
                if (!match.Success) break;

                double a = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                double b = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                double pow = Math.Pow(a, b);

                expr = expr.Replace(match.Value, pow.ToString(CultureInfo.InvariantCulture));
            }
            return expr;
        }

        // Вычисляем функции sin(), cos(), tan(), sqrt(), ln(), log(), abs()
        private string EvaluateFunctions(string expr)
        {
            // sin
            expr = Regex.Replace(expr, @"sin\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Sin(v).ToString(CultureInfo.InvariantCulture);
            });

            // cos
            expr = Regex.Replace(expr, @"cos\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Cos(v).ToString(CultureInfo.InvariantCulture);
            });

            // tan
            expr = Regex.Replace(expr, @"tan\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Tan(v).
                ToString(CultureInfo.InvariantCulture);
            });

            // sqrt
            expr = Regex.Replace(expr, @"sqrt\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Sqrt(v).ToString(CultureInfo.InvariantCulture);
            });

            // ln
            expr = Regex.Replace(expr, @"ln\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Log(v).ToString(CultureInfo.InvariantCulture);
            });

            // log
            expr = Regex.Replace(expr, @"log\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Log10(v).ToString(CultureInfo.InvariantCulture);
            });

            // abs
            expr = Regex.Replace(expr, @"abs\(([^\)]+)\)", m =>
            {
                double v = EvaluateSimple(m.Groups[1].Value);
                return Math.Abs(v).ToString(CultureInfo.InvariantCulture);
            });

            return expr;
        }

        // Вычисляет простые подвыражения вроде "2+3*4"
        private double EvaluateSimple(string s)
        {
            var result = new DataTable().Compute(s, null);
            return Convert.ToDouble(result, CultureInfo.InvariantCulture);
        }
    }
}