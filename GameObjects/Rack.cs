
namespace GameObjects;

public class Rack : IRack
{
    private List<ITile> _tiles;
    public Rack(){
        _tiles = [];
    }
    List<ITile>? IRack.Tiles {get => [.. _tiles];}

    public ITile? TakeTile(int index){
        if (index<0 || index>_tiles.Count)
            return null;

        var takenTile = _tiles[index];

        _tiles.RemoveAt(index);

        return takenTile;
    }

    public void AddTile(ITile tile){
        _tiles.Add(tile);
    }
}
