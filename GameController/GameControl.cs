using GameObjects;
using GameUtilities;

namespace GameController;

public class GameControl
{
    AhoCorasickTrie wordChecker;
    IBoard board;
    public List<ITile> GetTilePlayer(IPlayer player){
        throw new NotImplementedException();
    }
    public IPlayer CheckWinner(){
        throw new NotImplementedException();
    }

    public bool CheckPlacingTile(int x, int y){
        return !board.Squares[x,y].Occupied;
    }

    public void SetTilesToBoard(IPlayer setPlayer, ITile setTile, ISquare setSquare){
        throw new NotImplementedException();
    }

    public bool CheckWordValidity(string word){
        return wordChecker.CheckWord(word);
    }

    public int CalculatePointWords(List<ISquare> placedSquares){
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
