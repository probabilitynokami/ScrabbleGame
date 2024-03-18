using GameUtilities;

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

    public BoardPosition Position{get;}
    public int WordMultiplier{get;}
    public int TileMultiplier{get;}
    public bool PlaceTile(ITile tile);

    public ITile? UnplaceTile();

    public ITile? PeekTile();

    public bool Occupied{get;}
    public void Deactivate();
}

public interface IBoard{
    public ISquare[,] Squares{get;}
    public Size Size{get;}
}

public interface IDictionary{
    public List<string> ValidWords{get;}
}

public interface IRack{
    public int RackSize{get;}
    public List<ITile> Tiles{get;}
    public ITile? TakeTile(int index);
    public ITile? TakeTile();
    public void AddTile(ITile tile);
}

//TODO: move this somwhwerjje 
public static class ExtensionStringify{    
    public static string Stringify(this IEnumerable<ISquare> squares){
        string ret = "";
        foreach(var sq in squares){
            var tile = sq.PeekTile();
            if (tile is null)
                return "";
            ret += tile.Letter;
        }
        return ret;
    }
    public static string Stringify(this IEnumerable<ITile> squares){
        string ret = "";
        foreach(var tile in squares){
            if (tile is null)
                return "";
            ret += tile.Letter;
        }
        return ret;
    }
    public static IEnumerable<string> Stringify(this IEnumerable<IEnumerable<ISquare>> squares){
        List<string> ret = new();
        foreach(IEnumerable<ISquare> sq in squares){
            ret.Add(sq.Stringify());
        }
        return ret;
    }
}