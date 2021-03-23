using System;
using System.Linq;

namespace rpn2
{

  class Stack
  {
    private string[] stack = new string[0];
    public void StackPush(string str)
    {
      Array.Resize(ref stack, stack.Length + 1);
      stack[stack.Length - 1] = str;
    }
    public string StackPop()
    {
      string tmpStr = stack[stack.Length - 1];
      Array.Resize(ref stack, stack.Length - 1);
      return tmpStr;
    }
    public string StackPeek()
    {
      return stack[stack.Length - 1];
    }
    public int Length() 
    { 
      return stack.Length; 
    }
  }

  class Converter
  {
    private string result;
    public string InfixString { get; set; }
    public string RPNString { get; set; }
    public string Result 
    {
      get { return result;}
    }
    public void GenerateRPNString()
    {
      Stack stack = new Stack();
      RPNString = "";
      string prevChar = "";
      foreach (char c in InfixString)
      {
        if (char.IsDigit(c) || c == '.')
          RPNString += c;
        if (c == '(')
          stack.StackPush(c.ToString());
        if (c == ')')
          while (stack.Length() > 0)
          {
            if (stack.StackPeek() == "(")
            {
              stack.StackPop();
              break;
            }
            else
              RPNString += " " + stack.StackPop();
          }
        if ("+-/*".Contains(c.ToString()))
        {
          if ((prevChar == "(" || prevChar == string.Empty) && (c.ToString() == "-" || c.ToString() == "+"))
            RPNString += "0";
          while (stack.Length() > 0)
          {
            if (("/*".Contains(stack.StackPeek())) || ("+-".Contains(stack.StackPeek()) && "+-".Contains(c.ToString()))) 
              RPNString += " " + stack.StackPop();
            else
              break;
          }
          stack.StackPush(c.ToString());
          RPNString += " ";
        }
        prevChar = c.ToString();
      }
      for (int i = stack.Length() - 1; i >= 0; i--)
        RPNString += " " + stack.StackPop();
    }

    public void Calc()
    {
      Stack stack = new Stack();
      result = "";
      string operand = "";
      foreach (char c in RPNString)
      {
        if (char.IsDigit(c) || c == '.')
          operand += c;
        else
        {
          if (c == ' ')
          {
            if (operand != string.Empty)
            {
              stack.StackPush(operand);
              operand = "";
            }
          }
          else
          {
            double a = double.Parse(stack.StackPop());
            double b = double.Parse(stack.StackPop());
            switch (c.ToString())
            {
              case "*":
                stack.StackPush((b * a).ToString());
                break;
              case "/":
                stack.StackPush((b / a).ToString());
                break;
              case "+":
                stack.StackPush((b + a).ToString());
                break;
              case "-":
                stack.StackPush((b - a).ToString());
                break;
            }
          }
        }
      }
      if (operand != "")
        stack.StackPush(operand);
      result = stack.StackPeek();
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
      while (true)
      {
        string inputString = GetInputString();
        if (inputString.ToLower() == "exit")
          break;
        if (inputString != string.Empty)
        {
          try
          {
            Converter converter = new Converter();
            converter.InfixString = inputString;
            converter.GenerateRPNString();
            converter.Calc();
            Console.WriteLine("Выражение в ОПН: {0}", converter.RPNString);
            Console.WriteLine("Результат расчета: {0}", converter.Result);
          }
          catch (Exception)
          {
            Console.WriteLine("Введенное выражение содержит ошибку");
          }
        }
      }
    }

    static string GetInputString()
    {
      Console.WriteLine("Введите выражение в инфиксной нотации (для выхода наберите \"exit\"):");
      string result = "";
      result = Console.ReadLine().ToLower();
      if (result != "exit")
      {
        foreach (char c in result)
        {
          if (!"0123456789+-/*().".Contains(c))
          {
            Console.WriteLine("Введенное выражение содержит недопустимый символ. Разрешается ввод цифр, точки, знаков арифметических операций, скобок и слова \"exit\"");
            result = "";
            break;
          }
        }
      }
      return result;
    }
  }
}
