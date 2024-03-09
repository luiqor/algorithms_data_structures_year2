using System;

class MainClass
{

    public static int Knapsack(int[] v, int[] w, int W)
    {
        int n = v.Length; //кількість предметів
        int[,] T = new int[n + 1, W + 1]; //–максимальна сумарна вартість, яку можна набрати першими i предметами так, щоб їхня вага не перевищувала j

        for (int i = 1; i <= n; i++) // обчислюємо для i-го елемента
        {
            for (int j = 0; j <= W; j++) // враховуємо всі ваги від 0 до максимальної ємності `W`
            {
                if (w[i - 1] > j) // T(i, j) = T(i -1, j) при j <wi (місткість рюкзака иенша ніж вага предмета)
                {
                    T[i, j] = T[i - 1, j]; 
                }
                else //при j≥wi
                {
                    T[i, j] = Math.Max(T[i - 1, j], T[i, j - w[i - 1]] + v[i - 1]); //в звичайному рюкзаку було б i-1 T(i, j) = max (T(i -1, j), T(i, j - wi) +ci) 
                }
            }
        }

        // Друкуємо матрицю T
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= W; j++)
            {
                Console.Write(T[i, j] + " ");
            }
            Console.WriteLine();
        }
        PickSelectedItems(T, w, v);
        return T[n, W];
    }
    public static void PickSelectedItems(int[,] T, int[] w, int[] v)
    {
        int i = T.GetLength(0) - 1;
        int j = T.GetLength(1) - 1;
        int totalProfit = T[i, j];

        Console.WriteLine("Обрані предмети: ");
        while (i > 0 && j > 0)
        {
            if (T[i, j] != T[i - 1, j]) // Якщо НЕ T(i -1, j)     T(i, j) = max (T(i -1, j), T(i, j - wi) +ci) при j≥wi  то T(i, j - wi) +ci
            {
                Console.WriteLine($"в рюкзаку предмет - {i} з вагою: {w[i - 1]} та ціною: {v[i - 1]}");
                j -= w[i - 1];
                totalProfit -= v[i - 1];
            }
            else //інакше, однакове значення, тож перескакуємо на попередній предмет у стовпчику
            {
                i -= 1;
            }
        }

        while (totalProfit > 0) //Для 1 предмету
        {
            Console.WriteLine($"в рюкзаку предмет - 1 з вагою: {w[0]} та ціною: {v[0]}");
            totalProfit -= v[0];
        }
    }

    public static void Main(string[] args)
    {
        int[] v = { 15, 50, 60, 90 };
        int[] w = { 1, 3, 4, 5 };
        int W = 8;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Максимальна вартість предметів для 1 підзадачі:  " + Knapsack(v, w, W));
    }
}
