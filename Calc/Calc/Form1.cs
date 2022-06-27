namespace Calc
{
    public partial class Form1 : Form
    {
        private string _operation = "";
        private List<string> Operations = new List<string> { "+", "-", "*", "/", "^" };
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "0")
            {
                if(textBox1.Text.Length == 1)
                    textBox1.Text = "0";
                else
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            } 
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (textBox1.Text == "0")
                textBox1.Text = button.Text;
            else
                textBox1.Text = textBox1.Text + button.Text;
        }

        private void buttonSubtraction_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            _operation = button.Text;
            
            if (Operations.Contains(textBox1.Text[textBox1.Text.Length - 1].ToString()))
            {
                int index = 0;
                while (index < Operations.Count)
                {
                    if (Convert.ToChar(Operations[index]) == textBox1.Text[textBox1.Text.Length - 1])
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                        textBox1.Text = textBox1.Text + _operation;
                    }
                    index++;
                }
            }
            else
            {
                textBox1.Text = textBox1.Text + _operation;
            }
        }

        private string Calculation(string number1, string number2, string oper)
        {
            if (oper == "+")
                return Convert.ToString(Convert.ToDouble(number1) + Convert.ToDouble(number2));
            if (oper == "-")
                return Convert.ToString(Convert.ToDouble(number1) - Convert.ToDouble(number2));
            if (oper == "*")
                return Convert.ToString(Convert.ToDouble(number1) * Convert.ToDouble(number2));
            if (oper == "/")
                return Convert.ToString(Convert.ToDouble(number1) / Convert.ToDouble(number2));
            if (oper == "^")
                return Convert.ToString(Math.Pow(Convert.ToDouble(number1), Convert.ToDouble(number2)));
            else
                return "";
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            string sourceExpression = textBox1.Text;
            string num1 = "";
            string num2 = "";
            string lastOperation = "";
            List<string> Arg = new List<string>();
            int symbol = 0;
            while (symbol < sourceExpression.Length)
            {
                int currentIndex = Arg.Count - 1;
                if (Operations.Contains(sourceExpression[symbol].ToString()))
                {
                    Arg.Add(sourceExpression[symbol].ToString());
                    while (Operations.Contains(Arg[currentIndex]) &&
                        Operations.LastIndexOf(sourceExpression[symbol].ToString()) > Operations.IndexOf(Arg[currentIndex]))
                    {
                        Arg[currentIndex + 1] = Arg[currentIndex];
                        currentIndex--;
                    }
                    Arg[currentIndex + 1] = sourceExpression[symbol].ToString();
                    lastOperation = sourceExpression[symbol].ToString();
                    symbol++;
                }
                else if (num1 == "")
                {
                    while (symbol < sourceExpression.Length && !Operations.Contains(sourceExpression[symbol].ToString()))
                    {
                        num1 += sourceExpression[symbol];
                        symbol++;
                    }
                    Arg.Add(num1);
                    num2 = "";
                }
                else if (num2 == "")
                {
                    while (symbol < sourceExpression.Length && !Operations.Contains(sourceExpression[symbol].ToString()))
                    {
                        num2 += sourceExpression[symbol];
                        symbol++;
                    }

                    int index = Arg.LastIndexOf(lastOperation.ToString());
                    Arg.Add(num2);
                    currentIndex = Arg.Count - 1;
                    while (currentIndex > index)
                    {
                        Arg[currentIndex] = Arg[currentIndex - 1];
                        currentIndex--;
                    }

                    Arg[index] = num2;
                    num2 = "";
                }
                textBox3.Text += String.Join(" ", Arg) + Environment.NewLine;
            }

            textBox3.Text = String.Join(" ", Arg);

            while (Arg.Count > 1)
            {
                string a = "", b = "", sign = "", result = "";
                string firstOper = "";
                int indexFirstOper = Arg.Count;
                foreach (string s in Operations)
                {
                    if(Arg.IndexOf(s) > 0 && Arg.IndexOf(s) < indexFirstOper)
                    {
                        indexFirstOper = Arg.IndexOf(s);
                        firstOper = s;
                    }
                }
                a = Arg[indexFirstOper - 2];
                b = Arg[indexFirstOper - 1];
                sign = Arg[indexFirstOper];
                result = Calculation(a,b, sign);

                Arg.RemoveAt(indexFirstOper);
                Arg.RemoveAt(indexFirstOper-1);
                Arg[indexFirstOper - 2] = result;

                textBox3.Text += Environment.NewLine + String.Join(" ", Arg.ToArray()); 
            }
            textBox2.Text = textBox1.Text + " = " + String.Join(" ", Arg.ToArray());
        }
    }
}