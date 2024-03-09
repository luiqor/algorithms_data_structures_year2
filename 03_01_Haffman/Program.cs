using System;
using System.Collections.Generic;
using System.Linq;

class HuffmanCoding
{

    // Node class for Huffman tree
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

    // Function to build Huffman tree
    public static Node buildTree(Dictionary<char, int> charFreqs)
    {
        var nodes = new List<Node>();

        // Create leaf nodes for chars
        foreach (var entry in charFreqs)
        {
            nodes.Add(new Node(entry.Key, entry.Value));
        }

        while (nodes.Count > 1)
        {
            // Sort nodes by frequency
            nodes = nodes.OrderBy(n => n.frequency).ToList();

            // Take two nodes with lowest frequency
            var left = nodes[0];
            var right = nodes[1];

            // Create internal node with sum frequency
            var parent = new Node('\0', left.frequency + right.frequency);
            parent.left = left;
            parent.right = right;

            // Remove lower frequency nodes
            nodes.Remove(left);
            nodes.Remove(right);

            // Add parent node
            nodes.Add(parent);
        }

        // Root node is only one left
        return nodes[0];
    }

    // Function for encoding string
    public static string Encode(string text)
    {
        // Build frequency table
        var freqTable = new Dictionary<char, int>();
        foreach (var c in text) // { "h": 1, "e": 1, "l": 3, "o": 2, " ": 1, "w": 1, "r": 1, "d": 1 }
        {
            if (freqTable.ContainsKey(c))
            {
                freqTable[c]++;
            }
            else
            {
                freqTable[c] = 1;
            }
        }

        // Build Huffman tree
        var tree = buildTree(freqTable);

        // Traverse tree to assign codes
        var codes = new Dictionary<char, string>();
        AssignCodes(tree, "", codes);

        // Encode text
        var encoded = "";
        foreach (var c in text)
        {
            encoded += codes[c];
        }

        return encoded;
    }

    // Recursive function to traverse Huffman tree
    // and assign codes to characters
    static void AssignCodes(Node node, string code,
                             Dictionary<char, string> codes)
    {
        if (node == null) return;

        // Leaf node, assign its code
        if (node.left == null && node.right == null)
        {
            codes[node.character] = code;
            return;
        }

        // Recurse left subtree
        AssignCodes(node.left, code + "0", codes);

        // Recurse right subtree
        AssignCodes(node.right, code + "1", codes);
    }

    public static void Main()
    {
        string text = "hello world";

        Console.WriteLine("Original text: " + text);

        string encoded = Encode(text);

        Console.WriteLine("Encoded text: " + encoded);
    }
}