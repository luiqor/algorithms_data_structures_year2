using System.Drawing;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Embed Text\n2. Extract Text\n3. Exit");
            Console.Write("Select an option: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        EmbedText();
                        break;
                    case 2:
                        ExtractText();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    static void EmbedText()
    {
        Console.Write("Enter the text to embed: ");
        string textToEmbed = Console.ReadLine();

        Console.Write("Enter the path to the source image: ");
        string imagePath = Console.ReadLine();

        Bitmap bmp = new Bitmap(imagePath); // Переведення в бмп провсяк

        Bitmap resultBitmap = Steganography.embedText(textToEmbed, new Bitmap(bmp)); // Передаю у функцію, далі Embed text у картинку 
        Console.WriteLine("Text embedded successfully.");

        Console.Write("Enter the path to save the resulting image: ");
        string resultPath = Console.ReadLine();

        resultBitmap.Save(resultPath);   // Зберегти картинку
    }

    static void ExtractText()
    {
        Console.Write("Enter the path to the image containing embedded text: ");
        string imagePath = Console.ReadLine();

        Bitmap bmp = new Bitmap(imagePath); //  Переведення в бмп провсяк

        string extractedText = Steganography.extractText(bmp);  // Передача в extract text:
        Console.WriteLine("Extracted text: " + extractedText);
    }
}


public static class LSB
{
    public static int value = 2;
}
class Steganography
{
    public static Bitmap embedText(string text, Bitmap bmp)
    {
        bool bitsForHiding = true;
        int symbolIndex = 0;
        int symbolValue = 0;
        long pixelChannelIndex = 0;
        int zerosNumber = 0;
        int R = 0, G = 0, B = 0;


        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                Color pixel = bmp.GetPixel(j, i);

                R = pixel.R - pixel.R % LSB.value;
                G = pixel.G - pixel.G % LSB.value;
                B = pixel.B - pixel.B % LSB.value;

                for (int n = 0; n < 3; n++)
                {
                    if (pixelChannelIndex % 8 == 0)
                    {
                        if (bitsForHiding == false && zerosNumber == 8)
                        {
                            if ((pixelChannelIndex - 1) % 3 < LSB.value)
                            {
                                bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                            }
                            return bmp;
                        }

                        if (symbolIndex >= text.Length)
                        {
                            bitsForHiding = false;
                        }
                        else
                        {
                            symbolValue = text[symbolIndex++];
                        }
                    }

                    switch (pixelChannelIndex % 3)
                    {
                        case 0:
                            {
                                if (bitsForHiding == true)
                                {
                                    R += symbolValue % LSB.value;

                                    symbolValue /= LSB.value;
                                }
                            }
                            break;
                        case 1:
                            {
                                if (bitsForHiding == true)
                                {
                                    G += symbolValue % LSB.value;

                                    symbolValue /= LSB.value;
                                }
                            }
                            break;
                        case 2:
                            {
                                if (bitsForHiding == true)
                                {
                                    B += symbolValue % LSB.value;

                                    symbolValue /= LSB.value;
                                }

                                bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                            }
                            break;
                    }

                    pixelChannelIndex++;

                    if (bitsForHiding == false)
                    {
                        zerosNumber++;
                    }
                }
            }
        }

        return bmp;
    }

    public static string extractText(Bitmap bmp)
    {
        int colorChannelIndex = 0;
        int symbolValue = 0;

        string extractedText = String.Empty; // міститиме текст, який буде витягнуто із зображення

        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                Color pixel = bmp.GetPixel(j, i);

                for (int n = 0; n < 3; n++)  // (RGB)
                {
                    switch (colorChannelIndex % 3)
                    { //додаєтсья значення LSB як наступний symbolValue тексту
                        case 0:
                            {
                                symbolValue = symbolValue * LSB.value + pixel.R % LSB.value;
                            }
                            break;
                        case 1:
                            {
                                symbolValue = symbolValue * LSB.value + pixel.G % LSB.value;
                            }
                            break;
                        case 2:
                            {
                                symbolValue = symbolValue * LSB.value + pixel.B % LSB.value;
                            }
                            break;
                    }

                    colorChannelIndex++;

                    if (colorChannelIndex % 8 == 0) // якщо було додано 8 біт, це повноцінний символ     (додається поточний символ до тексту результату)
                    { // відбувається обернення бітів.
                        //Коли вбудовується кожен біт тексту, він додається до менш значущого розряду, тобто старші біти зсуваються вправо.
                        //Після вбудовування 8 бітів ми отримуємо число, в якому порядок бітів обернутий. Обертаючи їх назад, ми відновлюємо початковий текст.
                        int result = 0;
                        for (int v = 0; v < 8; v++)
                        {
                            result = result * LSB.value + symbolValue % LSB.value;

                            symbolValue /= LSB.value;
                        }
                        symbolValue = result;

                        if (symbolValue == 0) // все-все значення symbolValue  може бути рівним 0, тільки якщо це 8 нулів, тому повертається текст
                        {
                            return extractedText;
                        }

                        char c = (char)symbolValue; // перетворити символьне значення з типу int в тип char

                        extractedText += c.ToString(); // додається поточний символ до тексту результату
                    }
                }
            }
        }

        return extractedText;
    }
}

