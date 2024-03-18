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
    public AhoCorasickTrie wordChecker;
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
        gameState = new();
        deck = new Deck(populator.GetDeckPopulator());
        players = populator.GetPlayers();

        playerData = [];
        foreach(var player in players){
            playerData[player] = new PlayerData();
        }

        playingIndex = 0;


    }

   public void FirstDeal(){
        foreach(var player in players){
            RefillRack(player);
        }
   } 
    
    public PlayerData GetCurrentPlayerData(){
        return playerData[CurrentPlayer];
    }
    
    public PlayerData GetPlayerData(IPlayer player){
        return playerData[player];
    }
    

    public ITile? DrawTile(){
        return deck.PopTile();
    }


    public ITile? TakeTile(IPlayer player, int index){
        var data = playerData[player];
        return data.Rack.TakeTile(index);
    }

    public void PlaceTile(ITile tile, BoardPosition position ){
        // TODO: add security here
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
        List<ISquare> wordStartsDown, wordStartsRight;
        (wordStartsDown, wordStartsRight) = GetWordStarts(gameState.placedSquares);
        var ret = GetWords(wordStartsDown,1,0);
        ret.AddRange(GetWords(wordStartsRight,0,1));
        return ret;
    }

    public int GetTurnScore(){
        if (!gameState.IsSquarePositionsValid())
            return 0;
        

        var words = GetTurnWords();

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

    public List<List<ISquare>> GetWords(List<ISquare> wordStarts, int rowStep, int columnStep){
        List<List<ISquare>> ret = [];

        foreach(var sq in wordStarts){
            var result = Beam(sq,rowStep,columnStep);
            if (result.Count > 1)
                ret.Add(result);
        }

        return ret;
    }

    

    public (List<ISquare>,List<ISquare>) GetWordStarts(List<ISquare> placedSquares){

        HashSet<ISquare> set = new();
        foreach(var sq in placedSquares){
            //set.UnionWith(Beam(sq,1,0));
            //set.UnionWith(Beam(sq,0,1));
            set.UnionWith(Beam(sq,-1,0));
            set.UnionWith(Beam(sq,0,-1));
        }
        List<ISquare> retDown = [];
        List<ISquare> retRight = [];
        foreach(var sq in set){
            if(IsStartSquare(sq,1,0))
                retDown.Add(sq);
            if(IsStartSquare(sq,0,1))
                retRight.Add(sq);
        }

        return (retDown, retRight);
    }

    public bool IsStartSquare(ISquare sq, int rowStep, int columnStep){


        bool ret = false;
        int up = sq.Position.row-rowStep;
        int left = sq.Position.column-columnStep;

        if(up<0 && rowStep>=1 || left<0 && columnStep>=1)
            ret = true;
        else{
            // ret = !affectedSquares.Contains(board.Squares[up,sq.Position.column]);
            ret = !board.Squares[up,left].Occupied;
            ret = ret && (Beam(sq,rowStep,columnStep).Count > 1);
        }

        return ret;
    }

    public List<ISquare> Beam(ISquare start, int rowStep, int columnStep){
        var current = start;
        List<ISquare> ret = [];
        while(current.Occupied){
            if(ret.Contains(current))
                break;

            ret.Add(current);

            BoardPosition nextPosition;
            nextPosition.row = Math.Max(Math.Min(current.Position.row+rowStep, board.Size.Height-1),0);
            nextPosition.column = Math.Max(Math.Min(current.Position.column+columnStep, board.Size.Width-1),0);

            current = board.Squares[nextPosition.row,nextPosition.column];
        }
        return ret;
    }

    public ISquare BeamLast(ISquare start, int rowStep, int columnStep){
        var current = start;
        var ret = start;
        while(current.Occupied){
            ret = current;

            BoardPosition nextPosition;
            nextPosition.row = Math.Max(Math.Min(current.Position.row+rowStep, board.Size.Height-1),0);
            nextPosition.column = Math.Max(Math.Min(current.Position.column+columnStep, board.Size.Width-1),0);

            current = board.Squares[nextPosition.row,nextPosition.column];

            if (current == ret){
                break;
            }
        }

        return ret;
    }

    public bool NextTurn(bool refill=true, bool deactivateSquares=true){

        if(!gameState.IsSquarePositionsValid())
            return false;
        
        playerData[CurrentPlayer].Score += GetTurnScore();

        if(refill)
            RefillRack(CurrentPlayer);

        if(deactivateSquares)
            gameState.DeactivateSquares();

        gameState.Reset();

        playingIndex = (playingIndex+1)%players.Count;
        return true;
    }
    
    public void RefillRack(IPlayer player){
        var tile = deck.PopTile();
        var rack = playerData[player].Rack;
        while((tile is not null) && rack.Tiles.Count < rack.RackSize){
            rack.AddTile(tile);
            tile = deck.PopTile();
        }
        if(tile is not null)
            deck.InsertTiles([tile]);
    }
    
    public void Skip(){
        var placedSquares = gameState.placedSquares;
        foreach(var sq in placedSquares){
            var tile = sq.UnplaceTile();
            if(tile is null)
                continue;
            
        }
    }

}

public struct GameState{
    public List<ISquare> placedSquares;

    public GameState(){
        placedSquares = [];
    }

    public bool IsSquarePositionsValid(){
        if (placedSquares.Count <= 2){
            return true;
        }
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
    
    public void DeactivateSquares(){
        foreach(var sq in placedSquares){
            sq.Deactivate();
        }
    }

}
