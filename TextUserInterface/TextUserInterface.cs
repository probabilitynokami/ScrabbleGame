using GameUtilities;
using Spectre.Console;
namespace TextUserInterface;

public class TUI
{
    private string[][] content;
    private Action[][] cellFunc;

    private readonly GameUtilities.Size TUISize;
    private Grid spectreGrid;
    
    private int cursorRow;
    private int cursorColumn;
    public void HandleInput(char input){
        switch(input){
            case 'h': cursorRow = Math.Max(cursorRow-1,0); break;
            case 'l': cursorRow = Math.Min(cursorRow+1,TUISize.Height); break;
            case 'j': cursorColumn = Math.Min(cursorColumn+1,TUISize.Width); break;
            case 'k': cursorColumn = Math.Max(cursorColumn-1,0); break;
            case ' ': cellFunc[cursorRow][cursorColumn].Invoke(); break;
        }
    }
    
    public TUI(GameUtilities.Size size){
        TUISize = size;

        content = new string[size.Height][];
        content.Initialize("X",size.Width);

        cellFunc = new Action[size.Height][];
        cellFunc.Initialize(()=>{}, size.Width);

        spectreGrid = new Grid();
        spectreGrid.AddColumns(size.Width);
        for(int i=0;i<size.Height;i++)
            spectreGrid.AddRow(content[i]);
    }

    public void UpdateBuffer(){
        spectreGrid = new Grid();
        for(int i=0;i<TUISize.Height;i++)
            for(int j=0;j<TUISize.Width;j++)
                content[i][j] = "H";

        for(int i=0;i<TUISize.Height;i++)
            spectreGrid.AddRow(content[i]);
    }

    public void Render(){
        AnsiConsole.Write(spectreGrid);
    }
}