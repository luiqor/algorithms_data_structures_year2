using System;
using System.Drawing;

public class Vertex
{
    public double x { get; }
    public double y { get; }

    public Vertex(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

class Program
{
    public static double Distance(Vertex p1, Vertex p2)
    {
        return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2)); //формула відстані між двома точками
    }


    static double OptimalTriangularization(Vertex[] vertices) //вартостями тріангуляції будемо вважати сумарну довжину хорд (внутршніх ребер), які з'єднують не сусідні вершини
    {
        int n = vertices.Length;
        if (n < 3)
            return 0;

        double[][] dp = new double[n][];
        for (int i = 0; i < n; i++)
        {
            dp[i] = new double[n];
        }

        for (int gap = 2; gap < n; gap++)
        {
            for (int i = 0; i < n - gap; i++)
            {
                int j = i + gap;
                dp[i][j] = double.MaxValue;

                for (int k = i + 1; k < j; k++)
                {
                    double lengthBetween;

                    // Перевірка чи сусідні вершини або перша та остання вершини
                    if ((j == i + 1) || (i == 0 && j == n - 1))
                    {
                        lengthBetween = 0; //тод це не хорда, до вартості будемо додавати 0
                    }
                    else
                    {
                        lengthBetween = Distance(vertices[i], vertices[j]); 
                    }

                    double cost = dp[i][k] + dp[k][j] + lengthBetween; //тріангуляція лівого багатокутника , правого багатокутника і сума довжин
                    dp[i][j] = Math.Min(dp[i][j], cost);
                }
            }
        }

        return dp[0][n - 1];
    }


    static void Main(string[] args)
    {
        // інпутові дані, вершини багатокутнкиа
        Vertex[] vertices = { new Vertex(0, 10), new Vertex(0, 20), new Vertex(8, 26), new Vertex(15, 26), new Vertex(27, 21), new Vertex(22, 12), new Vertex(10, 0) };

        double result = OptimalTriangularization(vertices);
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Мінімальна вартість тріангуляції, коли необхідно мінімізувати сумарну довжину проведених діагоналей: " + result);
    }
}
