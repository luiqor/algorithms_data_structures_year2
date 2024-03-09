using System;
using System.Linq;
using System.Collections.Generic;

class Job : IComparable<Job>
{
    public int lj;
    public int dj;
    public int Cj; // Completion Time
    public int λj; // Lateness
   
    public int CompareTo(Job other) ///ВІДМІННІСТЬ: роботи заплановані у порядку зростання добутку dj*pj (pj = lj у моєму випадку, pj- час обробки) - Правильна віповідь: В)
    {
        int productThis = dj * lj;
        int productOther = other.dj * other.lj;

        return productThis.CompareTo(productOther);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Job[] jobs = new Job[]
        {
            new Job { lj = 3, dj = 5 },
            new Job { lj = 2, dj = 7 },
            new Job { lj = 5, dj = 9 },
            new Job { lj = 6, dj = 8 }
        };

        Array.Sort(jobs);  // Сортується зігдно з IComparable<Job>

        int comulative_time = 0;
        int total_lateness = 0; //ВІДМІННІСТЬ: інша навза змінної

        foreach (Job job in jobs)
        {
            job.Cj = comulative_time + job.lj;

            if (job.Cj <= job.dj)
            {
                job.λj = 0;
            }
            else
            {
                job.λj = job.Cj - job.dj;
            }

            total_lateness += job.λj; //ВІДМІННІСТЬ: сумарне запізнення накопичується 
            comulative_time += job.lj;
        }
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Сумарне запізнення: " + total_lateness);

        Console.WriteLine("Розклад:");

        foreach (Job job in jobs)
        {
            Console.WriteLine($"Довжина: {job.lj}, Крайній термін: {job.dj}, Запізнення: {job.λj}");
        }
    }
}
