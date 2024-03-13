namespace GameObjects;

public class Square : ISquare
{
    public int WordMultiplier{get; private set;}
    public int TileMultiplier{get; private set;}

    private ITile? occupyingTile = null;

    public Square(int tileMultiplier, int wordMultiplier){
        WordMultiplier = wordMultiplier;
        TileMultiplier = tileMultiplier;
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
}
