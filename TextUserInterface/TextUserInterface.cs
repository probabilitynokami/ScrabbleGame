using GameUtilities;
using Spectre.Console;
namespace TextUserInterface;

public class TUI
{
    private string[][] content;
    private bool[][] highlight;
    private Action[][] cellFunc;

    private string[][] renderBuffer;

    private readonly GameUtilities.Size TUISize;
    private Grid spectreGrid;

    public bool RerenderFlag{get; set;} = true;
    
    private int cursorRow;
    private int cursorColumn;
    public async Task HandleInput(char input){
        switch(input){
            case 'k': cursorRow = Math.Max(cursorRow-1,0); break;
            case 'j': cursorRow = Math.Min(cursorRow+1,TUISize.Height-1); break;
            case 'l': cursorColumn = Math.Min(cursorColumn+1,TUISize.Width-1); break;
            case 'h': cursorColumn = Math.Max(cursorColumn-1,0); break;
            case ' ': await Task.Run(() => cellFunc[cursorRow][cursorColumn].Invoke()); break;
        }
        RerenderFlag = true;
    }
    
    public TUI(GameUtilities.Size size){
        TUISize = size;

        content = new string[size.Height][];
        content.Initialize("H",size.Width);

        renderBuffer = new string[size.Height][];
        renderBuffer.Initialize(" ",size.Width);

        highlight = new bool[size.Height][];
        highlight.Initialize(false,size.Width);



        cellFunc = new Action[size.Height][];
        cellFunc.Initialize(()=>{}, size.Width);

        spectreGrid = new Grid();
        spectreGrid.AddColumns(size.Width);
        for(int i=0;i<size.Height;i++)
            spectreGrid.AddRow(content[i]);

        for(int i=0;i<size.Height;i++){
            for(int j=0;j<size.Width;j++){
                int storeI=i, storeJ=j;
                cellFunc[i][j] = () => {
                    if(content[storeI][storeJ] == "H")
                        content[storeI][storeJ] = "X";
                    else
                        content[storeI][storeJ] = "H";
                };
            }
        }
    }

    public void UpdateContent(){
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