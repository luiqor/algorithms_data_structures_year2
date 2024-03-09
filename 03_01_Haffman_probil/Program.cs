using System;
using System.Collections.Generic;
using System.Linq;

class HuffmanCoding
{

    public class Node
    {
        public char character;
        public int frequency;
        public Node left, right;

        public Node(char ch, int freq)
        {
            character = ch;
            frequency = freq;
        }
    }

   
    public static Node buildTree(Dictionary<char, int> charFreqs)
    {
        var nodes = new List<Node>();

        
        foreach (var entry in charFreqs)
        {
            nodes.Add(new Node(entry.Key, entry.Value));
        }

        while (nodes.Count > 1) 
        {
            nodes = nodes.OrderBy(n => n.frequency).ToList();

            var left = nodes[0];
            var right = nodes[1];

            var parent = new Node('\0', left.frequency + right.frequency);
            parent.left = left;
            parent.right = right;

            nodes.Remove(left);
            nodes.Remove(right);

            nodes.Add(parent);
        }
        return nodes[0];
    }

    public static string Encode(string text)
    {

        var freqTable = new Dictionary<char, int>();
        foreach (var c in text) 
        {
            if (c != ' ') 
            {
                if (freqTable.ContainsKey(c)) // { "h": 1, "e": 1, "l": 3, "o": 2, " ": 1, "w": 1, "r": 1, "d": 1 } 
                {
                    freqTable[c]++;
                }
                else
                {
                    freqTable[c] = 1;
                }
            }
        }

        //на основі словнкиа будуємо дерево
        var tree = buildTree(freqTable);

        var codes = new Dictionary<char, string>();
        AssignCodes(tree, "", codes);

        var encoded = "";
        foreach (var c in text)
        {
            if (c != ' ') // Пробіл
            {
                encoded += codes[c];
            }
            else
            {
                encoded += " "; // Пробіл
            }
        }

        return encoded;
    }

    static void AssignCodes(Node node, string code,
                             Dictionary<char, string> codes)
    {
        if (node == null) return;

        if (node.left == null && node.right == null) 
        {
            codes[node.character] = code;
            return;
        }

        AssignCodes(node.left, code + "0", codes);

        AssignCodes(node.right, code + "1", codes);
    }

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Введіть строку для кодування алгоритмом Хаффмана: ");
        string text = Console.ReadLine();
        //string text = "hello world";

        string encoded = Encode(text);

        Console.WriteLine("Закодований текст алгоритмом Хаффмана: " + encoded);
    }
}