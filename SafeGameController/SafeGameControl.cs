using GameController;
using GameObjects;
using GameUtilities;
namespace SafeGameController;

public interface ITUIBackendInterface{
    public IPlayer GetCurrentPlayer();
    public PlayerData GetCurrentPlayerData();
    public IBoard GetCurrentBoard();
    public List<ISquare> GetPlacedSquares();
    

    // commands
    public bool SelectTileOnRack(int index);
    public bool PlaceTileOnBoard(BoardPosition position);
    public bool SkipTurn();
    public bool NextTurn();
    public bool ShuffleDeck();

    public int GetCurrentTurnScore();

    public bool IsHoldingTile{get;}
}
public class SafeGameController : ITUIBackendInterface
{
    private GameControl gameController;
    
    private ITile? tileOnHand = null;

    public bool IsHoldingTile{get => tileOnHand is not null;}
    
    public SafeGameController(IGamePopulator populator){
        gameController = new(populator);
        gameController.FirstDeal();
    }

    public IBoard GetCurrentBoard()
    {
        return gameController.GetBoard();
 
    }

    public PlayerData GetCurrentPlayerData()
    {
        return gameController.GetCurrentPlayerData();
    }

    public List<ISquare> GetPlacedSquares()
    {
        return gameController.gameState.placedSquares;
    }

    public bool NextTurn()
    {
        if(!gameController.gameState.IsSquarePositionsValid())
            return false;
        gameController.NextTurn();
        return true;
    }

    public bool PlaceTileOnBoard(BoardPosition position)
    {
        if(tileOnHand is null)
            return false;
        if(gameController.GetBoard().Squares[position.row,position.column].Occupied)
            return false;

        gameController.PlaceTile(tileOnHand,position);

        tileOnHand = null;

        return true;
    }

    public bool SelectTileOnRack(int index)
    {
        var rack = gameController.GetCurrentPlayerData().Rack;
        if(!(0 <= index && index <= rack.RackSize))
            return false;
        if(tileOnHand is not null)
            return false;

        tileOnHand = rack.TakeTile(index);
        

        return tileOnHand is not null;
    }

    public bool ShuffleDeck()
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public bool SkipTurn()
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public IPlayer GetCurrentPlayer()
    {
        return gameController.CurrentPlayer;
    }

    public int GetCurrentTurnScore()
    {
        return gameController.GetTurnScore();
    }
}
