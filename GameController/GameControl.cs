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
    GameState gameState;
    Dictionary<IPlayer,PlayerData> playerData;

    IDeck deck;

    List<IPlayer> players;

    public GameControl(IGamePopulator populator){
        board = populator.GetBoard();
        wordChecker = new(populator.GetWordList());
        players = populator.GetPlayers();

        playerData = [];
        foreach(var player in players){
            playerData[player] = new PlayerData();
        }

        deck = new Deck(populator.GetDeckPopulator());

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
            point += sq.PeekTile().Point*sq.TileMultiplier;
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

    private List<List<ISquare>> GetWords(List<ISquare> wordStarts){
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

    

    private List<ISquare> GetWordStarts(List<ISquare> placedSquares){

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

    private bool IsStartSquare(ISquare sq){
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

    private List<ISquare> Beam(ISquare start, int columnStep, int rowStep){
        var current = start;
        List<ISquare> ret = [];
        while(current.Occupied){
            if(ret.Contains(current))
                break;

            ret.Add(current);

            BoardPosition nextPosition;
            nextPosition.row = Math.Min(current.Position.row+rowStep, board.Size.Height);
            nextPosition.column = Math.Min(current.Position.column+columnStep, board.Size.Width);

            current = board.Squares[nextPosition.row,nextPosition.column];
        }
        return ret;
    }

}

public struct GameState{
    public List<ISquare> placedSquares;

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
