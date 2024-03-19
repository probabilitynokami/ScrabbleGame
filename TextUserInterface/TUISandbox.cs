using GameUtilities;
using Spectre.Console;
using TextUserInterface;


public class TUISandbox {
    static void Main(){
        var tui = new TUI(new GameUtilities.Size(5,5));
        while(true){
            tui.Render();
        }
    }

}
