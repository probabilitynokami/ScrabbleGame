using System.Dynamic;
using GameObjects;
using GameUtilities;

namespace GameController;

public interface IGamePopulator{
    public IBoard GetBoard();
    public IEnumerable<string> GetWordList();
    
    public List<IPlayer> GetPlayers();

    public IDeckPopulator GetDeckPopulator();
}

public class GameControl
{
    IBoard board;
    AhoCorasickTrie wordChecker;
    public GameState gameState;
    Dictionary<IPlayer,PlayerData> playerData;

    IDeck deck;

    List<IPlayer> players;

    private int playingIndex;

    public IBoard GetBoard(){
       return board; 
    }
    public List<ITile> GetRemainingTiles(){
        return deck.PeekRemainingTiles();
    }

    public IPlayer CurrentPlayer{get => players[playingIndex];}

    public GameControl(IGamePopulator populator){
        board = populator.GetBoard();
        wordChecker = new(populator.GetWordList());
        players = populator.GetPlayers();
        gameState = new();

        playerData = [];
        foreach(var player in players){
            playerData[player] = new PlayerData();
        }

        playingIndex = 0;

        deck = new Deck(populator.GetDeckPopulator());

    }

    public ITile? DrawTile(){
        return deck.PopTile();
    }


    public ITile? TakeTile(IPlayer player, int index){
        var data = playerData[player];
        return data.Rack.TakeTile(index);
    }

    public void PlaceTile(ITile tile, BoardPosition position ){
        var square = board.Squares[position.row,position.column];
        square.PlaceTile(tile);
        gameState.placedSquares.Add(square);
    }

    public ITile? UnplaceTile(BoardPosition position){
        var square = board.Squares[position.row,position.column];
        if(!square.Occupied)
            return null;
        gameState.placedSquares.Remove(square);

        return square.UnplaceTile();
    }
    
    public List<List<ISquare>> GetTurnWords(){
        var wordStarts = GetWordStarts(gameState.placedSquares);
        return GetWords(wordStarts);
    }

    public int GetTurnScore(){
        if (!gameState.IsSquarePositionsValid())
            return 0;
        
        var wordStarts = GetWordStarts(gameState.placedSquares);

        var words = GetWords(wordStarts);

        foreach(var word in words){
            if(!wordChecker.CheckWord(word.Stringify()))
                return 0;
        }

        int point = 0;
        foreach(var word in words){
            point += TilePoint(word)*TotalMultiplier(word);
        }

        return point;
    }

    private int TilePoint(List<ISquare> word){
        int point = 0;
        foreach(var sq in word){
            var tile = sq.PeekTile();
            if (tile is null)
                return 0;
            point += tile.Point*sq.TileMultiplier;
        }
        return point;
    }
    private int TotalMultiplier(List<ISquare> word){
        int multiplier = 1;
        foreach(var sq in word){
            multiplier *= sq.WordMultiplier;
        }
        return multiplier;
    }

    public List<List<ISquare>> GetWords(List<ISquare> wordStarts){
        List<List<ISquare>> ret = [];

        foreach(var sq in wordStarts){
            var down = Beam(sq,1,0);
            if (down.Count > 1)
                ret.Add(down);

            var right = Beam(sq,0,1);
            if (right.Count > 1)
                ret.Add(right);
        }

        return ret;
    }

    

    public List<ISquare> GetWordStarts(List<ISquare> placedSquares){

        HashSet<ISquare> set = new(placedSquares);
        foreach(var sq in placedSquares){
            set.UnionWith(Beam(sq,1,0));
            set.UnionWith(Beam(sq,0,1));
        }
        List<ISquare> ret = [];
        foreach(var sq in set){
            if(IsStartSquare(sq))
                ret.Add(sq);
        }

        return ret;
    }

    public bool IsStartSquare(ISquare sq){
        bool emptyUp = false;
        bool emptyLeft = false;

        int up = sq.Position.row-1;
        int left = sq.Position.column-1;

        if(up<0)
            emptyUp = true;
        else if(!board.Squares[up,sq.Position.column].Occupied)
            emptyUp = true;

        if(left<0)
            emptyLeft = true;
        else if(!board.Squares[sq.Position.row,left].Occupied)
            emptyLeft = true;
        
        return emptyUp&&emptyLeft;
    }

    public List<ISquare> Beam(ISquare start, int rowStep, int columnStep){
        var current = start;
        List<ISquare> ret = [];
        while(current.Occupied){
            if(ret.Contains(current))
                break;

            ret.Add(current);

            BoardPosition nextPosition;
            nextPosition.row = Math.Min(current.Position.row+rowStep, board.Size.Height-1);
            nextPosition.column = Math.Min(current.Position.column+columnStep, board.Size.Width-1);

            current = board.Squares[nextPosition.row,nextPosition.column];
        }
        return ret;
    }

}

public struct GameState{
    public List<ISquare> placedSquares;

    public GameState(){
        placedSquares = [];
    }

    public bool IsSquarePositionsValid(){
        bool sameColumn = true, sameRow = true;
        for(int i=1;i<placedSquares.Count;i++){
            sameColumn = sameColumn && 
                        (placedSquares[i].Position.column == placedSquares[i-1].Position.column);
            sameRow = sameRow &&
                        (placedSquares[i].Position.row == placedSquares[i-1].Position.row);
        }
        return sameColumn ^ sameRow;
    }

    public void Reset(){
        placedSquares = [];
    }

}
