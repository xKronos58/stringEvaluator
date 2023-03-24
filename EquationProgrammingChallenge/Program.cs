using System.Diagnostics.Contracts;
using System.Text;
// ReSharper disable once IdentifierTypo
namespace EquasionProgrammingChallenge;

internal static class Program
{
    //Sample equation 
    private const string Equation = "x * (2 * y) * (8 ^ 3)";

    private static string ConvertXy(int x, int y, string eq)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in eq)
        {
            //Checks for all of the x's and y's and replaces them (Note: this will be expanded upon with more choice for values)
            if (c == 'x')
                sb.Append($"{x}");
            else if (c == 'y')
                sb.Append($"{y}");
            else
                sb.Append(c);
        }
        return sb.ToString();
    }

    private static int Solve(string eq)
    {
        Stack<int> stack = new Stack<int>();
        Stack<char> ops = new Stack<char>();
        int num = 0;
        
        for (int i = 0; i < eq.Length; i++)
        {
            char c = eq[i];
            if (char.IsDigit(c))
            {
                num = num * 10 + (c - '0');
                if (i == eq.Length - 1 || !char.IsDigit(eq[i + 1]))
                {
                    stack.Push(num);
                    num = 0;
                }
            }
            //if it is an open bracket it pushes it
            else if (c == '(')
                ops.Push(c);
            else if (c == ')')
            {   //If it is a closed bracket it evaluates everything inside the brackets
                while (ops.Peek() != '(')
                { 
                    char op = ops.Pop();
                    int b = stack.Pop(), a = stack.Pop();
                    stack.Push(ApplyOp(a, b, op));
                }
                ops.Pop();
            }
            else if (c is '+' or '-' or '*' or '/' or '^')
            {
                //Runs a BODMAS check and evaluates the equation if there are no brackets involved
                while (ops.Count > 0 && BodmasCheck(ops.Peek(), c))
                {
                    char op = ops.Pop();
                    int b = stack.Pop(), a = stack.Pop();
                    stack.Push(ApplyOp(a, b, op));
                }
                ops.Push(c);
            }
        }

        while (ops.Count > 0)
        {
            char op = ops.Pop();
            int b = stack.Pop(), a = stack.Pop();
            stack.Push(ApplyOp(a, b, op));
        }

        return stack.Pop();
    }
    
    /// <summary>
    /// Checks weather one operator has a higher precedence than another one 
    /// </summary>
    /// <param name="op1"></param>
    /// <param name="op2"></param>
    private static bool BodmasCheck(char op1, char op2)
    {
        if (op1 is '(' or ')')
            return false;
        if (op1 is '*' or '/' or '^' && op2 is '+' or '-')
            return false;
        return true;
    }
    
    /// <summary>
    /// Applies an operator from two inputs from a string
    /// </summary>
    /// <returns> a (operator) b</returns>
    /// <exception cref="DivideByZeroException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private static int ApplyOp(int a, int b, char op) =>
        op switch
        {
            //Checks the operator and returns the respective value
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            '^' => (int)Math.Pow(a, b),
            '/' => b == 0 ? throw new DivideByZeroException() : a / b,
            _ => throw new ArgumentException($"Invalid operator: {op}")
        };
    
    public static void Main()
    {
        //Example Use Case
        int x = 7, y = 8;
        Console.WriteLine($"The equation is {Equation}, where x = {x} & y = {y}");
        Console.WriteLine(Solve(ConvertXy(x, y, Equation)));
    }
}