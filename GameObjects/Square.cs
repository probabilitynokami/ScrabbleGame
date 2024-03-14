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

    public bool PlaceTile(ITile tile){
        if (Occupied)
            return false;

        occupyingTile = tile;

        return true;
    }

    public ITile? GetTile(){
        return occupyingTile;
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
