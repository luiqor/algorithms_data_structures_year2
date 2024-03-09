using System;

class Job
{
    public int sj { get; set; }
    public int tj { get; set; }

    public Job()
    {
        sj = 0;
        tj = 0;
    }
}


class Program
{
    static Job[] IntervalScheduling(Job[] jobs)
    {
        Array.Sort(jobs, (a, b) => a.tj - b.tj);

        Job[] selectedJobs = new Job[jobs.Length];
        int selectedCount = 0;
        Job lastSelectedJob = null;

        foreach (Job job in jobs)
        {
            if (lastSelectedJob == null || job.sj >= lastSelectedJob.tj)
            {
                selectedJobs[selectedCount++] = job;
                lastSelectedJob = job;
            }
        }

        // масив для зберігання лише вибраних завдань
        Job[] trimmedSelectedJobs = new Job[selectedCount];
        Array.Copy(selectedJobs, trimmedSelectedJobs, selectedCount);

        return trimmedSelectedJobs;
    }

    static void Main(string[] args)
    {
        Job[] jobs = new Job[]
        {
            new Job { sj = 0, tj = 3 },
            new Job { sj = 2, tj = 5 },
            new Job { sj = 4, tj = 7 }
        };

        Job[] selectedJobs = IntervalScheduling(jobs);

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Підмножини максимального розміру з роботами, які не мають конфліктів:");
        for (int i = 0; i < selectedJobs.Length; i++)
        {
            Job job = selectedJobs[i];
            Console.WriteLine($"{i + 1}) Початок: {job.sj}, Кінець: {job.tj}");
        }
    }
}
/**
Спершу всі роботи сортуються за часом завершення в зростаючому порядку. Це дозволяє розглядати роботи, які завершуються раніше, як потенційно оптимальні для вибору.

Створюється порожній список, де будуть зберігатися вибрані роботи.

Починаємо ітеруватися по відсортованому списку робіт.

Для кожної роботи перевіряємо, чи вона конфліктує з останньою вибраною роботою, якщо така робота вже була вибрана (перший раз це буде null).

Якщо поточна робота не конфліктує з останньою вибраною роботою (тобто час початку поточної роботи більший або рівний часу закінчення останньої вибраної роботи), то додаємо цю роботу до списку вибраних робіт і оновлюємо останню вибрану роботу.

Продовжуємо цей процес для всіх робіт у відсортованому списку.

Результатом є список вибраних робіт, які не конфліктують між собою за часом і максимізують кількість робіт.
**/