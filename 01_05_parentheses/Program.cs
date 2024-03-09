using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    
    static int FindMaxResult(string input)
    {
        List<int> data = new List<int>();
        List<char> ops = new List<char>();
        int num = 0;
        char op = ' ';

       
        string[] tokens = input.Split(new char[] { '+', '-', '*' });
        string[] operators = input.Split(tokens, StringSplitOptions.RemoveEmptyEntries);

   
        for (int i = 0; i < tokens.Length; i++)
        {
            if (int.TryParse(tokens[i], out num))
            {
                data.Add(num);
            }

            if (i < operators.Length)
            {
                char.TryParse(operators[i], out op);
                ops.Add(op);
            }
        }

        int size_i = data.Count;
        int[,] maxResult = new int[size_i, size_i];

        for (int i = 0; i < size_i; i++) 
        {
            maxResult[i, i] = data[i];
        }

        for (int len = 2; len <= size_i; len++)  
        {
            for (int i = 0; i <= size_i - len; i++) 
            {
                int j = i + len - 1; 
                maxResult[i, j] = int.MinValue; 

                for (int k = i; k < j; k++) 
                {
                    int leftMax = maxResult[i, k]; 
                    int rightMax = maxResult[k + 1, j]; 

                    switch (ops[k]) 
                    {
                        case '+':
                            maxResult[i, j] = Math.Max(maxResult[i, j], leftMax + rightMax);
                            break;
                        case '-':
                            maxResult[i, j] = Math.Max(maxResult[i, j], leftMax - rightMax);
                            break;
                        case '*':
                            maxResult[i, j] = Math.Max(maxResult[i, j], leftMax * rightMax);
                            break;
                    }
                }
            }
        }

        return maxResult[0, size_i - 1];
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write("Введіть вираз: ");
        string expression = Console.ReadLine();

        int maxResult = FindMaxResult(expression);
        Console.WriteLine("Максимальне значення після розтавляння дужок: " + maxResult);
    }
}
