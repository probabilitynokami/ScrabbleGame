# Scrabble

Scrabblin scrabblelin

## new design

```mermaid
classDiagram
namespace GameObjects{
    class IPlayer{
        <<interface>>
        + int ID get;
        + string Name get;set;
    }
    class ITile{
        <<interface>>
        + char Letter get
        + char Point get
    }
    class IDeck{
        + IEnumberable<ITile> PopTiles(int n)
        + ITile PopTile()
        + IEnumerable<ITile> PeekRemainingTiles()
        + void InsertTiles(IEnumerable<ITile> tiles)
        + void Shuffle()
    }

    class ISquare{
        + BoardPosition Position get
        + int WordMultiplier get
        + int TileMultiplier get
        + bool PlaceTile(ITile tile)
        + ITile UnplaceTile()
        + ITile PeekTile()
        + bool Occupied get
        + void Deactivate()
    }
    class IBoard{
        + ISquare[,] Squares get
        + Size Size get
    }
    class IRack{
        + int RackSize get
        + List<ITile> Tiles get
        + ITile TakeTile(int index)
        + ITIle TakeTile()
        + void AddTile()
    }


    %% implementations
    class ScrabblePlayer{
        - string _name
    }
}
ScrabblePlayer --|> IPlayer
```
