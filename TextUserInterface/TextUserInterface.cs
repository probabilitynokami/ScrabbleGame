using GameUtilities;
namespace TextUserInterface;

public class TextUserInterface
{
    private string[,] content;
    private Func<bool>[,] cellFunc;
    
    private int cursorRow;
    private int cursorColumn;
    public void HandleInput(char input){
        switch(input){
            case 'h': cursorRow = Math.Max(cursorRow-1,0); break;
            case 'l': cursorRow = Math.Min(cursorRow+1,0); break;
            case 'j': cursorColumn = Math.Min(cursorColumn+1,0); break;
            case 'k': cursorColumn = Math.Max(cursorColumn-1,0); break;
            case ' ': cellFunc[cursorRow,cursorColumn].Invoke(); break;
        }
    }
    
    public TextUserInterface(Size size){
        content = new string[size.Height, size.Width];
        content.Initialize();
        cellFunc = new Func<bool>[size.Height, size.Width];
        content.Initialize();
    }
}