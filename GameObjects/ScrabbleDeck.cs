using GameObjects;
using GameUtilities;


class ScrabbleDeck : IDeck {

    private List<ITile> _tiles;
    public ScrabbleDeck(){
        _tiles = [];
    }
    public void InsertTiles(IEnumerable<ITile> tiles){
        _tiles.AddRange(tiles);
    }

    public List<ITile> PeekRemainingTiles(){
        if (_tiles is null)
            return [];
        return [.. _tiles];
    }

    public List<ITile> PopTiles(int n) {
        if (n<=0)
            return [];

        if (n>_tiles.Count)
            n = _tiles.Count;

        _tiles = _tiles[^n..^0];
        return [.. _tiles];
    }

    public ITile? PopTile()
    {
        if (_tiles is null)
            return null;
        if(_tiles.Count == 0)
            return null;

        var ret = _tiles[^1];
        _tiles = _tiles[0..^1];

        return ret;
    }

    public void Shuffle(){
        _tiles = _tiles?.Shuffle();
    }
}