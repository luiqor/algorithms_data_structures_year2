using System;
using System.Linq;
using System.Collections.Generic;

class Job : IComparable<Job>
{
    public int lj; //довжину ℓj 
    public int dj; //крайній термін dj
    public int Cj; // Час завершення Cj(σ)
    public int λj; //  запізнення λj(σ)
    //відповідь А) запланувати роботу у порядку збільшення дедлайну dj
    public int CompareTo(Job other) //вказується, що порівнювати об'єкти класу Job слід за допомогою їхніх властивостей dj
    {
        return dj.CompareTo(other.dj);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Job[] jobs = new Job[]
        {
            new Job { lj = 2, dj = 3 },
            new Job { lj = 1, dj = 5 },
            new Job { lj = 3, dj = 7 },
            new Job { lj = 2, dj = 6 }
        };

        Array.Sort(jobs); // Сортується зігдно з IComparable<Job>

        int comulative_time = 0; //зберігаєтсья час, який був накопичений для рахування часу завершення
        int min_lateness = 0; //зберігаєтсья мінімізоване запізнення

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

            min_lateness = Math.Max(job.λj, min_lateness); //за мінімізації запізнення обираєтсья що буде максимальним запізненням (поточне запізнення чи якесь інше). Стає новим запізненням
            comulative_time += job.lj;
        }
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Максимальне запізнення в розкладі: " + min_lateness);

        Console.WriteLine("Розклад:");

        foreach (Job job in jobs)
        {
            Console.WriteLine($"Довжина: {job.lj}, Крайній термін: {job.dj}, Запізнення: {job.λj}");
        }
    }
}
