public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public char _value;

    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    // Search for a word in the trie
    public bool Search(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                return false;
            }
            current = current.Children[c];
        }
        return current.IsEndOfWord;
    }

    public bool Insert(string word)
    {
        TrieNode current = root;
        // For each character in the word
        foreach (char c in word)
        {
            // If the current node does not have the child with the current character
            if (!current.HasChild(c))
            {
                // Add the new child with the current character
                current.Children[c] = new TrieNode(c);
            }
            // Move to the child node with the current character
            current = current.Children[c];
        }
        if (current.IsEndOfWord)
        {
            // Word already exists in the trie
            return false;
        }
        // Mark the end of the word
        current.IsEndOfWord = true;
        return true;
    }
    
    /// <summary>
    /// Provides a list of all words in the trie that start with the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for in the trie.</param>
    /// <returns>A list of words that start with the given prefix. If no words are found, an empty list is returned.</returns>
    public List<string> AutoSuggest(string prefix)
    {
        TrieNode currentNode = root;
        foreach (char c in prefix)
        {
            if (!currentNode.HasChild(c))
            {
                return new List<string>();
            }
            currentNode = currentNode.Children[c];
        }
        return GetAllWordsWithPrefix(currentNode, prefix);
    }

    private List<string> GetAllWordsWithPrefix(TrieNode node, string prefix)
    {
        List<string> words = new List<string>();

        if (node.IsEndOfWord)
        {
            words.Add(prefix);
        }

        foreach (var child in node.Children)
        {
            words.AddRange(GetAllWordsWithPrefix(child.Value, prefix + child.Key));
        }

        return words;
    }

    // Helper method to delete a word from the trie by recursively removing its nodes
    private bool _delete(TrieNode node, string word, int index)
    {
        // Base case: if the end of the word is reached
        if (index == word.Length)
        {
            // If the current node is not the end of a word, return false
            if (!node.IsEndOfWord)
            {
                return false;
            }
            // Unmark the end of the word
            node.IsEndOfWord = false;
            // If the current node has no children, return true to indicate it can be deleted
            return node.Children.Count == 0;
        }
    
        // Get the character at the current index
        char c = word[index];
        // If the current node does not have a child for the character, return false
        if (!node.HasChild(c))
        {
            return false;
        }
    
        // Recursively call DeleteWord on the child node
        TrieNode child = node.Children[c];
        bool shouldDeleteCurrentNode = _delete(child, word, index + 1);
    
        // If the child node should be deleted, remove it from the current node's children
        if (shouldDeleteCurrentNode && !node.IsEndOfWord)
        {
            node.Children.Remove(c);
            // Return true if the current node has no other children
            return node.Children.Count == 0;
        }
    
        // Return false if the current node should not be deleted
        return false;
    }

    public bool Delete(string word)
    {
        return _delete(root, word, 0);
    }

    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    public void PrintTrieStructure()
    {
        Console.WriteLine("\nroot");
        _printTrieNodes(root);
    }

    private void _printTrieNodes(TrieNode root, string format = " ", bool isLastChild = true) 
    {
        if (root == null)
            return;

        Console.Write($"{format}");

        if (isLastChild)
        {
            Console.Write("└─");
            format += "  ";
        }
        else
        {
            Console.Write("├─");
            format += "│ ";
        }

        Console.WriteLine($"{root._value}");

        int childCount = root.Children.Count;
        int i = 0;
        var children = root.Children.OrderBy(x => x.Key);

        foreach(var child in children)
        {
            i++;
            bool isLast = i == childCount;
            _printTrieNodes(child.Value, format, isLast);
        }
    }

    public List<string> GetSpellingSuggestions(string word)
    {
        char firstLetter = word[0];
        List<string> suggestions = new();
        List<string> words = GetAllWordsWithPrefix(root.Children[firstLetter], firstLetter.ToString());
        
        foreach (string w in words)
        {
            int distance = LevenshteinDistance(word, w);
            if (distance <= 2)
            {
                suggestions.Add(w);
            }
        }

        return suggestions;
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        if (m == 0)
        {
            return n;
        }

        if (n == 0)
        {
            return m;
        }

        for (int i = 0; i <= m; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= n; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }
}