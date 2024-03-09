using System;

class KnapsackItem
{
    public double Value { get; set; }
    public double Weight { get; set; }
    public bool Fractionable { get; set; }

    public double ValueToWeightRatio
    {
        get { return Value / Weight; }
    }
}

class Knapsack
{
    static double PartlyFractKnapsackSolver(int W, KnapsackItem[] items)
    {
        Array.Sort(items, (x, y) => y.ValueToWeightRatio.CompareTo(x.ValueToWeightRatio)); // Сортує 

        double totalProfit = 0;
        double spaceLeft = W;

        foreach (var item in items)
            if (spaceLeft - item.Weight >= 0) //перевірка чи влазить всією вагою поточний предмет
            {
                totalProfit += item.Value;
                spaceLeft -= item.Weight;
                Console.WriteLine($"Предмет з ціною {item.Value} та вагою {item.Weight} був доданий повністю");
            }
            else if (spaceLeft > 0 && item.Fractionable)
            {
                totalProfit += item.ValueToWeightRatio * spaceLeft;  // до ціни рюкзака додається ціна чатинки предмета 
                spaceLeft = 0; //тоді вже місце в рюказку закінчилось
                Console.WriteLine($"Предмет з ціною {item.Value} та вагою {item.Weight} був доданий частково");
            }
            else 
            {
                continue;
            }
        Console.WriteLine("Ще залишилось місця у рюкзаку: " + spaceLeft);
        return totalProfit;
    }

    static void Main(string[] args)
    {
        int W = 60;
        KnapsackItem[] items = {
            new KnapsackItem { Value = 60, Weight = 20, Fractionable = true },
            new KnapsackItem { Value = 100, Weight = 50, Fractionable = false },
            new KnapsackItem { Value = 120, Weight = 30, Fractionable = true }
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Максимальна вартість предметів для 3 підзадачі:  " + PartlyFractKnapsackSolver(W, items));
    }
}