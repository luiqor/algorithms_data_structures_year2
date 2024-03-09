using System;
using System.Collections.Generic;
using System.Text;

public class ShannonFano
{
    private class Node
    {
        public char Symbol { get; set; }
        public string Code { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    public static string Encode(string input)
    {
        Dictionary<char, int> frequencyTable = new Dictionary<char, int>(); // спершу будується слвоник як і у Хафмані { "h": 1, "e": 1, "l": 3, "o": 2, " ": 1, "w": 1, "r": 1, "d": 1 } 
        foreach (char c in input)
        {
            if (c != ' ')
            {
                if (frequencyTable.ContainsKey(c))
                {
                    frequencyTable[c]++;
                }
                else
                {
                    frequencyTable[c] = 1;
                }
            }
        }

        List<Node> nodes = new List<Node>();
        foreach (var entry in frequencyTable) 
        {
            nodes.Add(new Node
            {
                Symbol = entry.Key,
                Frequency = entry.Value
            });
        }

        List<Node> tree = BuildShannonFanoTree(nodes);
        Dictionary<char, string> symbolCodes = GenerateSymbolCodes(tree);

        StringBuilder encodedText = new StringBuilder();
        foreach (char c in input)
        {
            if (c == ' ')
            {
                encodedText.Append(" "); // Пробіл є пробілом
            }
            else
            {
                encodedText.Append(symbolCodes[c]);
            }
        }

        return encodedText.ToString();
    }


    private static List<Node> BuildShannonFanoTree(List<Node> nodes)
    {
        if (nodes.Count == 1) 
        {
            return nodes;
        }

        nodes.Sort((a, b) => b.Frequency.CompareTo(a.Frequency)); 

        int totalFrequency = nodes.Sum(node => node.Frequency);   
        int cumulativeFrequency = 0;
        int splitIndex = 0;

        for (int i = 0; i < nodes.Count; i++)  
        {
            cumulativeFrequency += nodes[i].Frequency; 
            if (cumulativeFrequency * 2 >= totalFrequency)
            {
                splitIndex = i;
                break;
            }
        }

        for (int i = 0; i <= splitIndex; i++)  
        {
            nodes[i].Code += "0";
        }
        for (int i = splitIndex + 1; i < nodes.Count; i++)
        {
            nodes[i].Code += "1";
        }
        
        List<Node> leftSubtree = BuildShannonFanoTree(nodes.GetRange(0, splitIndex + 1));
        List<Node> rightSubtree = BuildShannonFanoTree(nodes.GetRange(splitIndex + 1, nodes.Count - splitIndex - 1));
       
        leftSubtree.AddRange(rightSubtree);
        return leftSubtree;
    }

    private static Dictionary<char, string> GenerateSymbolCodes(List<Node> tree)                                                                          
    {
        Dictionary<char, string> symbolCodes = new Dictionary<char, string>();
        foreach (Node node in tree)
        {
            symbolCodes[node.Symbol] = node.Code;
        }
        return symbolCodes;
    }


public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Введіть строку для кодування алгоритмом Фано: ");
        string text = Console.ReadLine();

        string encoded = Encode(text);

        Console.WriteLine("Закодований текст алгоритмом Фано: " + encoded);
    }
}
//best AAABBC  cool  good
