namespace TrieDictionaryTest;

[TestClass]
public class TrieTest
{
    // Test that a word is inserted in the trie
    [TestMethod]
    public void InsertWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        trie.Insert(word);

        // Assert
        Assert.IsTrue(trie.Search(word));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";
        trie.Insert(word);

        // Act
        trie.Delete(word);

        // Assert
        Assert.IsFalse(trie.Search(word));
    }

    // Test that a word is not inserted twice in the trie
    [TestMethod]
    public void InsertWordTwice()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        var firstInsert = trie.Insert(word);
        var secondInsert = trie.Insert(word);

        // Assert
        Assert.IsTrue(firstInsert);
        Assert.IsFalse(secondInsert);
    }

    // Test that a word is not deleted from the trie if it is not present
    [TestMethod]
    public void DeleteNonExistentWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        var result = trie.Delete(word);

        // Assert
        Assert.IsFalse(result);
    }

    // Test that the trie returns correct suggestions for a given prefix
    [TestMethod]
    public void AutoSuggestWords()
    {
        // Arrange
        var trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hell");
        trie.Insert("heaven");
        trie.Insert("heavy");

        // Act
        var suggestions = trie.AutoSuggest("he");

        // Assert
        CollectionAssert.AreEquivalent(new List<string> { "hello", "hell", "heaven", "heavy" }, suggestions);
    }

    // Test that the trie returns correct spelling suggestions for a given word
    [TestMethod]
    public void GetSpellingSuggestions()
    {
        // Arrange
        var trie = new Trie();
        trie.Insert("hello");        
        trie.Insert("help");

        // Act
        var suggestions = trie.GetSpellingSuggestions("helo");

        // Assert
        CollectionAssert.AreEquivalent(new List<string> { "hello", "help" }, suggestions);
    }
    
    // Test that a word is deleted from the trie if it is a prefix of another word
    [TestMethod]
    public void DeleteWordThatIsPrefixOfAnotherWord()
    {
        // Arrange
        var trie = new Trie();
        var prefixWord = "hell";
        var longerWord = "hello";
        trie.Insert(prefixWord);
        trie.Insert(longerWord);

        // Act
        trie.Delete(prefixWord);

        // Assert
        Assert.IsFalse(trie.Search(prefixWord));
        Assert.IsFalse(trie.Search(prefixWord));
        Assert.IsTrue(trie.Search(longerWord));
    }

    // Test that the trie returns correct suggestions for a prefix not present in the trie
    [TestMethod]
    public void AutoSuggestWordsForNonExistentPrefix()
    {
        // Arrange
        var trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("catatonic");
        trie.Insert("caterpillar");

        // Act
        var suggestions = trie.AutoSuggest("cat");

        // Assert
        CollectionAssert.AreEquivalent(new List<string> { "catastrophe", "catatonic", "caterpillar" }, suggestions);
    }

    // Test that the trie returns no spelling suggestions for a word not present in the trie
    [TestMethod]
    public void GetSpellingSuggestionsForNonExistentWord()
    {
        // Arrange
        var trie = new Trie();
        trie.Insert("hello");        
        trie.Insert("help");

        // Act
        var suggestions = trie.GetSpellingSuggestions("haaaaa");

        // Assert
        Assert.AreEqual(0, suggestions.Count);
    }
}