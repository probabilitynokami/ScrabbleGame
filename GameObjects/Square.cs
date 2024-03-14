using GameUtilities;

namespace GameObjects;

public class Square : ISquare
{
    public int WordMultiplier{get; private set;}
    public int TileMultiplier{get; private set;}

    public bool Occupied => occupyingTile is not null;

    public BoardPosition Position {get; private set;}

    private ITile? occupyingTile = null;

    public Square(int tileMultiplier, int wordMultiplier, BoardPosition position){
        WordMultiplier = wordMultiplier;
        TileMultiplier = tileMultiplier;
        Position = position;
    }

    public void Deactivate()
    {
        WordMultiplier = 1;
        TileMultiplier = 1;
    }

    public void PlaceTile(ITile tile){
        occupyingTile = tile;
    }

    public ITile? GetTile(){
        return occupyingTile;
    }

    bool ISquare.PlaceTile(ITile tile)
    {
        throw new NotImplementedException();
    }

    public ITile? UnplaceTile()
    {
        throw new NotImplementedException();
    }

    public ITile? PeekTile()
    {
        throw new NotImplementedException();
    }
}
