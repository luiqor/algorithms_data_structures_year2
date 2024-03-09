using System;

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
    static double CalculateArea(double x1, double y1, double x2, double y2, double x3, double y3)
    {
        return Math.Abs((x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)) / 2.0);
    }

    static double OptimalTriangularization(Vertex[] vertices)
    {
        int n = vertices.Length;
        if (n < 3) 
            return 0;

        double[][] dp = new double[n][]; // ось це все, всеодно що  double[][] dp = new double[n][n];
        for (int row = 0; row < n; row++)
        {
            dp[row] = new double[n]; // [0, 0, 0] [0, 0, 0] [0, 0, 0] [0, 0, 0] (це для 4 коментар)
        }

        for (int gap = 2; gap < n; gap++)
        {
            for (int i = 0; i < n - gap; i++)
            {
                int j = i + gap;
                dp[i][j] = double.PositiveInfinity;
                for (int k = i + 1; k < j; k++)
                {
                    double triangleArea = CalculateArea(vertices[i].x, vertices[i].y, vertices[j].x, vertices[j].y, vertices[k].x, vertices[k].y);
                    double cost = dp[i][k] + dp[k][j] + triangleArea;
                    dp[i][j] = Math.Min(dp[i][j], cost);
                }
            }
        }

        return dp[0][n - 1];
    }

    static void Main(string[] args)
    {
        // інпутові дані, вершини багатокутнкиа
        //Vertex[] vertices = { new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1), new Vertex(0, 1)};
        Vertex[] vertices = { new Vertex(0, 10), new Vertex(0, 20), new Vertex(8, 26), new Vertex(15, 26), new Vertex(27, 21), new Vertex(22, 12), new Vertex(10, 0) };
        double result = OptimalTriangularization(vertices);
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Мінімальна вартість тріангуляції, коли вартість трикутника - площа: " + result);
    }
}
