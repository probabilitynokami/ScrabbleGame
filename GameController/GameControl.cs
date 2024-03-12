using GameObjects;

namespace GameController;

public class GameControl
{
    IBoard board;
    IDeck deck;
    IDictionary dictionary;
    List<IPlayer> players;

    public List<ITile> GetTilePlayer(IPlayer player){
        throw new NotImplementedException();
    }
    public IPlayer CheckWinner(){
        throw new NotImplementedException();
    }

    public bool CheckPlacingTile(int x, int y){
        throw new NotImplementedException();
    }

    public void SetTilesToBoard(IPlayer setPlayer, ITile setTile, ISquare setSquare){
        throw new NotImplementedException();
    }

    public bool CheckWordValidity(string word){
        throw new NotImplementedException();
    }

    public int CalculatePointWords(int x, int y, int idTile, int idPlayer){
        throw new NotImplementedException();
    }

    public int SetPointPlayer(int idPlayer, int score){
        throw new NotImplementedException();
    }

    public char ChangeTileEmptyValue(int tileID, char letter){
        throw new NotImplementedException();
    }

    public bool PassTurn(int idPlayer){
        throw new NotImplementedException();
    }

    public ITile AddTileToPlayer(ITile tile, int idPlayer){
        throw new NotImplementedException();
    }

    public void RemoveTIle(ITile tile, int idPlayer){
        throw new NotImplementedException();
    }

}
