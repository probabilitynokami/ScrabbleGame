using System.Reflection.Metadata;
using System;

namespace GameObjects;

public interface IPlayer{
    public int ID{get;}
    public string Name{get;set;}
}

public interface ITile{
    public char Letter{get;}
    public int Point{get;}
}

public interface IDeck{
    public List<ITile> PopTiles(int n);
    public ITile? PopTile();
    public List<ITile> PeekRemainingTiles();

    public void InsertTiles(IEnumerable<ITile> tiles);

    public void Shuffle();
}

public interface ISquare{
    int GetPoint();
    int PlaceTile(ITile tile);
    void Deactivate();
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