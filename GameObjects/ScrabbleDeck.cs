using GameObjects;
using GameUtilities;


class ScrabbleDeck : IDeck {

    private List<ITile>? _tiles;
    public ScrabbleDeck(){
        _tiles = [];
    }
    public void InsertTiles(IEnumerable<ITile> tiles){
        _tiles?.AddRange(tiles);
    }

    public List<ITile> PeekRemainingTiles(){
        if (_tiles is null)
            return [];
        return new List<ITile>(_tiles);
    }

    public List<ITile> PopTiles(int n) {
        if (n<=0)
            return [];

        if(_tiles is null)
            return [];

        if (n>_tiles.Count)
            n = _tiles.Count;

        return new List<ITile>(_tiles[^n..^0]);
    }

    public ITile? PopTile()
    {
        if (_tiles is null)
            return null;
        if(!_tiles.Any())
            return null;

        var ret = _tiles[_tiles.Count-1];
        _tiles.RemoveAt(_tiles.Count-1);

        return ret;
    }

    public void Shuffle(){
        _tiles = _tiles?.Shuffle();
    }
}