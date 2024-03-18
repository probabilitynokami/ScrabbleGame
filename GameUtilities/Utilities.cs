using System.Collections;

namespace GameUtilities;


public static class Extensions{
    public static T Shuffle<T>(this T collections) where T : IList{
        // TODO: do real shuffle here
        return collections;
    }
    public static void Initialize<T>(this T[,] array, T value)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = value;
            }
        }
    }
}

public struct Size{
    public int Width{get;}
    public int Height{get;}

    public Size(int width, int height){
        Width = width;
        Height = height;
    }
}

public struct BoardPosition{
    public int row;
    public int column;

    public BoardPosition(int r, int c){
        row = r;
        column = c;
    }
}

public class AhoCorasickTrie{
    private AhoCorasickNode root;

    public AhoCorasickTrie(){
        root = new AhoCorasickNode('.',false);
    }
    public AhoCorasickTrie(IEnumerable<string> wordList) : this(){
        // build wordlist here
        foreach(var word in wordList){
            RegisterWord(word);
        }
    }

    public void RegisterWord(string word){
        var currentNode = root;
        foreach(var c in word){
            var nextNode = currentNode.nextNode(c);
            if (nextNode is null){
                var newNode = new AhoCorasickNode(c,false);
                currentNode.RegisterNode(newNode);
                currentNode = newNode;
            }
            else{
                currentNode = nextNode;
            }
        }
        currentNode.Stopable = true;
    }

    public bool CheckWord(string word){
        var currentNode = root;
        foreach(var c in word){
            var nextNode = currentNode.nextNode(c);
            if (nextNode is null){
                return false;
            }
            else{
                currentNode = nextNode;
            }
        }
        return currentNode.Stopable;
    }
}

internal class AhoCorasickNode{
    public char Value{get; set;}
    public bool Stopable{get; set;}

    public AhoCorasickNode(char value, bool stopable){
        Value = value;
        Stopable = stopable;
        transitionTable = [];
    }
    private Dictionary<char,AhoCorasickNode> transitionTable;

    internal AhoCorasickNode? nextNode(char edge){
        if (!transitionTable.TryGetValue(edge, out AhoCorasickNode? value))
            return null;
        return value;
    }

    internal void RegisterNode(AhoCorasickNode node){
        var edge = node.Value;

        if(transitionTable.ContainsKey(edge))
            return;

        transitionTable.Add(edge, node);
    }
}