using System.Reflection.Metadata;
using System;

namespace GameObjects;

public interface IPlayer{
    public int ID{get;}
    public string Name{get;}
}

public interface ITile{
    public char Letter{get;set;}
}

public interface IDeck{
    public List<ITile> Tiles{get;set;}
    public void Shuffle();
}

public interface ISquare{
}

public interface IBoard{
    public List<ISquare> Squares{get; set;}
    public int Size{get; set;}
}

public interface IDictionary{
    public List<string> ValidWords{get; set;}
}

public interface IRack{
    public List<ITile> Tiles{get; set;}
}