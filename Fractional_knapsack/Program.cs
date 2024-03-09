using System;
using System.Collections.Generic;

class KnapsackItem
{
    public double Value { get; set; }
    public double Weight { get; set; }

    public double ValueToWeightRatio
    {
        get { return Value / Weight; }
    }
}

class Knapsack
{
    static double KnapsackSolver(int W, KnapsackItem[] items)
    {
        Array.Sort(items, (x, y) => y.ValueToWeightRatio.CompareTo(x.ValueToWeightRatio)); // Сортує

        double totalProfit = 0;
        double spaceLeft = W; 

        foreach (var item in items)
        {
            if (spaceLeft - item.Weight >= 0) // чи влазить всією вагою поточний предмет
            {
                totalProfit += item.Value; 
                spaceLeft -= item.Weight; 
                Console.WriteLine($"Предмет з ціною {item.Value} та вагою {item.Weight} був доданий повністю");
            }
            else if (spaceLeft > 0) //інакше (при тому що місця залишилось більше 0)
            {
                totalProfit += item.ValueToWeightRatio * spaceLeft; 
                spaceLeft = 0;
                Console.WriteLine($"Предмет з ціною {item.Value} та вагою {item.Weight} був доданий частково");
            }
        }

        return totalProfit;
    }

    static void Main(string[] args)
    {
        int W = 60;
        KnapsackItem[] items = {
            new KnapsackItem { Value = 60, Weight = 20 },
            new KnapsackItem { Value = 100, Weight = 50 },
            new KnapsackItem { Value = 120, Weight = 30 }
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Максимальна вартість предметів для 2 підзадачі:  " + KnapsackSolver(W, items));
    }
}