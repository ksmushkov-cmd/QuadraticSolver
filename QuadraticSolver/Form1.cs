using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace QuadraticSolver {
  public partial class Form1 : Form {

    private TextBox txtA, txtB, txtC;
    private Label lblResult, lblStatus;
    public Form1() {
      this.Text = "Решение квадратных уравнений";
      this.ClientSize = new Size(700, 400);
      this.StartPosition = FormStartPosition.CenterScreen;

      BuildUI(); 
    }

    private void BuildUI() {
      // Меню
      var menuStrip = new MenuStrip(); 
      var menuCalc = new ToolStripMenuItem("Решить");
      menuCalc.Click += menuCalc_Click;

      var menuClear = new ToolStripMenuItem("Очистить");
      menuClear.Click += menuClear_Click;

      menuStrip.Items.Add(menuCalc);
      menuStrip.Items.Add(menuClear);
      this.Controls.Add(menuStrip); 

      var lblA = new Label { Text = "a =", Location = new Point(15, 40), AutoSize = true };
      txtA = new TextBox { Location = new Point(50, 37), Size = new Size(100, 20), Text = "1" };

      var lblB = new Label { Text = "b =", Location = new Point(15, 70), AutoSize = true };
      txtB = new TextBox { Location = new Point(50, 67), Size = new Size(100, 20), Text = "-5" };

      var lblC = new Label { Text = "c =", Location = new Point(15, 100), AutoSize = true };
      txtC = new TextBox { Location = new Point(50, 97), Size = new Size(100, 20), Text = "6" };

      this.Controls.Add(lblA); this.Controls.Add(txtA);
      this.Controls.Add(lblB); this.Controls.Add(txtB);
      this.Controls.Add(lblC); this.Controls.Add(txtC);

      lblResult = new Label {
        Location = new Point(15, 130),
        Size = new Size(650, 200),
        Font = new Font("Consolas", 10F),
        Text = ""
      };
      this.Controls.Add(lblStatus);  
    }

    private void menuCalc_Click(object sender, EventArgs e) {
      lblResult.Text = "";
      lblStatus.ForeColor = Color.Red;

      if (!TryParseDouble(txtA.Text, out double a)) { lblStatus.Text = "Коэффициент a задан некорректно."; return; }
      if (!TryParseDouble(txtB.Text, out double b)) { lblStatus.Text = "Коэффициент b задан некорректно."; return; }
      if (!TryParseDouble(txtC.Text, out double c)) { lblStatus.Text = "Коэффициент c задан некорректно."; return; }

      if (Math.Abs(a) < 1e-9) { lblStatus.Text = "Коэффициент a не может быть равен 0."; return; }

      lblStatus.ForeColor = Color.Green;
      lblStatus.Text = "Расчет выполнен успешно";

      double x1, x2;     
      // Дискриминант
      double d = b * b - 4 * a * c;
      string resultText = $"Уравнение: {a}x² + ({b})x + ({c}) = 0\n\n";
      resultText += "Метод 1: Дискриминант\n";
      resultText += $"D = b² - 4ac = ({b})² - 4·({a})·({c}) = {d:F4}\n";
      
      if (d > 0) {
        x1 = (-b + Math.Sqrt(d)) / (2 * a);
        x2 = (-b - Math.Sqrt(d)) / (2 * a);
        resultText += $"D > 0. Два действительных корня:\n";
        resultText += $"x2 = {x2:F6}\n";
      } else if (Math.Abs(d) < 1e-9) {
        x1 = -b / (2 * a);
        x2 = x1;
        resultText += $"D ≈ 0. Один корень (кратный):\n";
        resultText += $"x1 = x2 = {x1:F6}\n";
      } else {
        resultText += "D < 0. Действительных корней нет.\n";
        lblResult.Text = resultText;
        return;
      }

      // Теорема Виета
      resultText += "\nПроверка по теореме Виета\n";
      double sumExpected = -b / a;
      double prodExpected = c / a;
      double sumActual = x1 + x2;
      double prodActual = x1 * x2;

      resultText += $"Ожидаемая сумма (-b/a) = {sumExpected:F6}\n";
      resultText += $"Фактическая сумма (x1+x2) = {sumActual:F6}\n";
      resultText += $"Ожидаемое произведение (c/a) = {prodExpected:F6}\n";
      resultText += $"Фактическое произведение (x1*x2) = {prodActual:F6}\n\n";

      double eps = 1e-6;
      bool sumOk = Math.Abs(sumActual - sumExpected) < eps;
      bool prodOk = Math.Abs(prodActual - prodExpected) < eps;

      if (sumOk && prodOk)
        resultText += "Проверка пройдена! Корни удовлетворяют теореме Виета.";
      else
        resultText += "Проверка не пройдена (возможна вычислительная погрешность).";

      lblResult.Text = resultText;
      
    }

    private void menuClear_Click(object sender, EventArgs e) {
      txtA.Text = "";
      txtB.Text = "";
      txtC.Text = "";
      lblResult.Text = "";
      lblStatus.Text = "";   
    }

    private bool TryParseDouble(string s, out double value) {
      s = s.Trim().Replace(',', '.');
      return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }
  }
}
