
using GameUtilities;

namespace GameObjects;

public class Deck : IDeck
{
    List<ITile> _tiles;

    public Deck(){
        _tiles = [];
    }

    public Deck(IDeckPopulator populator){
        _tiles = populator.GetTiles();
    }

    public void InsertTiles(IEnumerable<ITile> tiles)
    {
        _tiles.AddRange(tiles);
    }

    public List<ITile> PeekRemainingTiles()
    {
        return [.._tiles];
    }

    public ITile? PopTile()
    {
        if(_tiles.Count <= 0)
            return null;

        var topTile = _tiles[^1];

        _tiles = _tiles[..^1];

        return topTile;
    }

    public List<ITile> PopTiles(int n)
    {
        if(_tiles.Count < n)
            n = _tiles.Count;

        var topTile = _tiles[^n..];

        _tiles = _tiles[..^n];

        return topTile;
    }

    public void Shuffle()
    {
        _tiles.Shuffle();
    }

}
