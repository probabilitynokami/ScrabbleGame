using System.Drawing;
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

    int WordMultiplier{get;}
    int TileMultiplier{get;}
    bool PlaceTile(ITile tile);

    ITile? UnplaceTile();

    bool Occupied{get;}
    void Deactivate();
}

public interface IBoard{
    public ISquare[,] Squares{get;}
    public Size Size{get;}
}

public interface IDictionary{
    public List<string> ValidWords{get;}
}

public interface IRack{
    public List<ITile>? Tiles{get;}
    public ITile? TakeTile(int index);
    public void AddTile(ITile tile);
}