
namespace GameObjects;

public class Rack : IRack
{
    private List<ITile?> _tiles;
    public int RackSize{get; private set;}
    public Rack(){
        // TODO: make this variable
        RackSize = 7;

        _tiles = [];

    }
    List<ITile> IRack.Tiles {get => [.. _tiles];}

    public ITile? TakeTile(int index){
        if (index<0 || index>_tiles.Count)
            return null;

        var takenTile = _tiles[index];

        _tiles.RemoveAt(index);

        return takenTile;
    }

    public ITile? TakeTile(){
        if(_tiles.Count == 0)
            return null;

        var takenTile = _tiles[^1];
        _tiles = _tiles[..^1];

        return takenTile;
    }

    public void AddTile(ITile tile){
        _tiles.Add(tile);
    }
}
