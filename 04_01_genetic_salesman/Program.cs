using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();

    static double[,] GenerateCities(int numCities)
    {

        double[,] cities = new double[numCities, 2];
        for (int i = 0; i < numCities; i++)
        {
            cities[i, 0] = random.NextDouble() * 500;  // X-coordinate in the range [0, 500]
            cities[i, 1] = random.NextDouble() * 500;  // Y-coordinate in the range [0, 500]
            Console.WriteLine($"City {i + 1} з координатами ({cities[i, 0]}, {cities[i, 1]})");
        }
        return cities;
    }

    static double Distance(double[] city1, double[] city2)
    {
        double dx = city1[0] - city2[0];
        double dy = city1[1] - city2[1];
        return Math.Sqrt(dx * dx + dy * dy); //корінь з (квдарт координати першої + квадрат координати другої)
    }

    static double TotalDistance(int[] order, double[,] cities)
    {
        double total = 0;
        for (int i = 0; i < order.Length - 1; i++) //дистанція між 2 міст
        {
            total += Distance(new double[] { cities[order[i], 0], cities[order[i], 1] },
                              new double[] { cities[order[i + 1], 0], cities[order[i + 1], 1] });
        }
        total += Distance(new double[] { cities[order[order.Length - 1], 0], cities[order[order.Length - 1], 1] },
                          new double[] { cities[order[0], 0], cities[order[0], 1] }); //додаєсо до загальної 
        return total;
    }

    static int[][] InitializePopulation(int popSize, int numCities)
    {
        int[][] population = new int[popSize][];
        for (int i = 0; i < popSize; i++)
        {
            population[i] = Enumerable.Range(0, numCities).OrderBy(x => random.Next()).ToArray();
        }
        return population;
    }

    static int[] Crossover(int[] parent1, int[] parent2)
    {
        int crossoverPoint = random.Next(1, parent1.Length - 1);
        int[] child = parent1.Take(crossoverPoint)
                            .Concat(parent2.Except(parent1.Take(crossoverPoint)))
                            .ToArray();
        return child;
    }

    static int[] Mutate(int[] individual, double mutationRate)
    {
        if (random.NextDouble() < mutationRate)
        {
            int idx1 = random.Next(individual.Length);
            int idx2 = random.Next(individual.Length);
            int temp = individual[idx1]; //Здійснюється обмін місцями двох міст в маршруті. 
            individual[idx1] = individual[idx2];
            individual[idx2] = temp;
        }
        return individual;
    }

    static int[] SelectBest(int[][] population, double[,] cities)
    {
        return population.OrderBy(bestInd => TotalDistance(bestInd, cities)).First();
    }

    static void GeneticAlgorithm(int numCities, int popSize, int generations, double mutationRate)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        double[,] cities = GenerateCities(numCities);
        int[][] population = InitializePopulation(popSize, numCities);

        for (int gen = 0; gen < generations; gen++) //генеруємо найкращий маршрут до вказаного числа
        {
            // Відбір найкращих індивідів
            int[] bestIndividual = SelectBest(population, cities);

            // Відображення проміжних результатів
            if (gen % 50 == 0)
            {
                Console.WriteLine($"Генерація {gen}, Найкраща загальна відстань: {TotalDistance(bestIndividual, cities)}");
            }

            // Створення нової популяції за допомогою схрещування та мутації
            List<int[]> newPopulation = new List<int[]> { bestIndividual }; //створюється список newPopulation, і в ньому додається найкращий індивід (маршрут) з поточної популяції.
            while (newPopulation.Count < popSize)
            {
                int[] parent1 = SelectBest(population, cities);//обираємо найліпшого індивідума для 1 найкрщаого маршруту
                int[] parent2 = SelectBest(population, cities);//обираємо найліпшого індивідума для 2 найкрщаого маршруту
                int[] child = Crossover(parent1, parent2);
                child = Mutate(child, mutationRate);
                newPopulation.Add(child);//додається нащадок в нову популяцію
            }

            population = newPopulation.ToArray();//нова поплуяціія додається в масив
        }

        int[] finalBestIndividual = SelectBest(population, cities); //вреші-решт обираєтсяь найкращий маршрут



        Console.WriteLine($"Фінальна найкоротша відстань: {TotalDistance(finalBestIndividual, cities)}");


        Console.WriteLine("Найкращий порядок відвідання міст: " + string.Join(", ", finalBestIndividual));
        Console.WriteLine("City\tCoordinates\tOrder");
        for (int i = 0; i < finalBestIndividual.Length; i++)
        {
            int cityNumber = finalBestIndividual[i] + 1;
            double x = cities[finalBestIndividual[i], 0];
            double y = cities[finalBestIndividual[i], 1];
            Console.WriteLine($"{cityNumber}\t({x}, {y})\t\t{i + 1}");
        }
    }



    static void Main()
    {
        GeneticAlgorithm(numCities: 10, popSize: 100, generations: 200, mutationRate: 0.01);
    }
}
