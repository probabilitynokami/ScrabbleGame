using System.Data;
using GameController;
using GameUtilities;
using SafeGameController;
namespace TextUserInterface;

public class TUIBoard : ITUIPanel
{
    ITUIBackendInterface _gameControl;
    IEnumerable<Cell> _cells;
    ICommand _command;
    BoardPosition _zeroPosition;

    public TUIBoard(ITUIBackendInterface gameController, IEnumerable<Cell> cells, BoardPosition zeroPosition){
        _gameControl = gameController;
        _cells = cells;
        _command = new DoNothingCommand();
        _zeroPosition = zeroPosition;
    }

    public void SetCellCommand(ICommand command)
    {
        _command = command;
    }

    public void UpdateCellContent()
    {
        var board = _gameControl.GetCurrentBoard();
        foreach(var cell in _cells){
            // var pos = cell.Position;
            BoardPosition pos = new()
            {
                row = cell.Position.row - _zeroPosition.row,
                column = cell.Position.column - _zeroPosition.column
            };

            var square = board.Squares[pos.row,pos.column];
            if(!square.Occupied){
                cell.Content = " ";
                continue;
            }

            cell.Content = ""+square.PeekTile()!.Letter;
        }
    }

    public void Invoke(BoardPosition cellPosition){
        _command.Execute();
    }
}
