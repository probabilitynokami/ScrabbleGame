using GameUtilities;
using SafeGameController;
using Spectre.Console;
namespace TextUserInterface;

public class TUI
{
    private string[][] content;
    private bool[][] highlight;
    private Action[][] cellFunc;

    private string[][] renderBuffer;

    private ITUIBackendInterface _api;

    private readonly GameUtilities.Size TUISize;

    public bool RerenderFlag{get; set;} = true;
    
    private int cursorRow;
    private int cursorColumn;

    private BoardPosition _rackOrigin;
    private BoardPosition _boardOrigin;

    private BoardPosition _playerDataOrigin;
    public async Task HandleInput(char input){
        switch(input){
            case 'k': cursorRow = Math.Max(cursorRow-1,0); break;
            case 'j': cursorRow = Math.Min(cursorRow+1,TUISize.Height-1); break;
            case 'l': cursorColumn = Math.Min(cursorColumn+1,TUISize.Width-1); break;
            case 'h': cursorColumn = Math.Max(cursorColumn-1,0); break;
            case ' ': await Task.Run(() => cellFunc[cursorRow][cursorColumn].Invoke()); break;
            case 'o': _api.NextTurn();break;
        }
        RerenderFlag = true;
    }
    
    public TUI(GameUtilities.Size size, ITUIBackendInterface api){
        TUISize = size;
        _api = api;

        content = new string[size.Height][];
        content.Initialize(" ",size.Width);

        renderBuffer = new string[size.Height][];
        renderBuffer.Initialize("",size.Width);

        highlight = new bool[size.Height][];
        highlight.Initialize(false,size.Width);



        cellFunc = new Action[size.Height][];
        cellFunc.Initialize(()=>{}, size.Width);

        SetBoard(0,0);
        SetRack(_api.GetCurrentBoard().Size.Height+2,0);
        _playerDataOrigin.row = 0;
        _playerDataOrigin.column = _api.GetCurrentBoard().Size.Width+2;
    }

    private void UpdatePlayerData(){
        var row = _playerDataOrigin.row;
        var column = _playerDataOrigin.column;
        var playerData = _api.GetCurrentPlayerData();
        var player = _api.GetCurrentPlayer();
        content[row][column] = player.Name;
        content[row+1][column] = "Total Score:" + playerData.Score.ToString();
        content[row+2][column] = "Turn Score:" + _api.GetCurrentTurnScore();
    }

    private void SetBoard(int row, int column){
        _boardOrigin.row = row;
        _boardOrigin.column = column;
        var board = _api.GetCurrentBoard();
        for(int ii=0;ii<board.Size.Height;ii++){
            for(int jj=0;jj<board.Size.Width;jj++){
                int i = ii,j = jj;
                cellFunc[row+i][column+j] = () => {
                    if(_api.IsHoldingTile){
                        _api.PlaceTileOnBoard(new BoardPosition(i,j));
                    }
                    else if(_api.GetPlacedSquares().Contains(board.Squares[i,j])){
                        content[row+i][column+j] = "#";
                    }
                };
            }
        }
    }

    private void SetRack(int row, int column){
        _rackOrigin.row = row;
        _rackOrigin.column = column;

        var rack = _api.GetCurrentPlayerData().Rack;
        var tiles = rack.Tiles;
        for(int ii=0;ii<rack.RackSize;ii++){
            int i = ii;
            cellFunc[row][column+i] = () => {
                _api.SelectTileOnRack(i);
            };
        }
    }

    private void UpdateCursorHighlight(){
        for(int i=0;i<TUISize.Height;i++){
            for(int j=0;j<TUISize.Width;j++){
                if(i==cursorRow && j==cursorColumn){
                    highlight[i][j] = true;
                }
                else{
                    highlight[i][j] = false;
                }
            }
        }
    }

    private void UpdateGameBoard(){
        var row = _boardOrigin.row;
        var column = _boardOrigin.column;
        var board = _api.GetCurrentBoard();
        for(int i=0;i<board.Size.Height;i++){
            for(int j=0;j<board.Size.Width;j++){
                var sq = board.Squares[i,j];
                if(!sq.Occupied){
                    content[row+i][column+j] = "_";
                    continue;
                }
                content[row+i][column+j] = ""+sq.PeekTile()!.Letter;
            }
        }
    }

    private void UpdateRack(){
        var rack = _api.GetCurrentPlayerData().Rack;
        var tiles = rack.Tiles;
        for(int i=0;i<rack.RackSize;i++){
            if(i<tiles.Count)
                content[_rackOrigin.row][_rackOrigin.column+i] = ""+tiles[i].Letter;
            else
                content[_rackOrigin.row][_rackOrigin.column+i] = "_";

        }
    }
    public void UpdateContent(){
        UpdateCursorHighlight();
        UpdateRack();
        UpdateGameBoard();
        UpdatePlayerData();
    }
    public void UpdateBuffer(){
        for(int i =0;i<TUISize.Height;i++){
            for(int j=0;j<TUISize.Width;j++){
                if(highlight[i][j]){
                    renderBuffer[i][j] = $"[underline green]{content[i][j]}[/] ";
                }
                else{
                    renderBuffer[i][j] = $"[red]{content[i][j]}[/] ";
                }
            }
        }
    }

    public void Render(){
        if(!RerenderFlag){
            return;
        }

        AnsiConsole.Cursor.SetPosition(0,0);
        for(int i =0;i<TUISize.Height;i++){
            for(int j=0;j<TUISize.Width;j++){
                AnsiConsole.Markup(renderBuffer[i][j]);
            }
            AnsiConsole.Markup("\n");
        }
        RerenderFlag = false;
    }
}