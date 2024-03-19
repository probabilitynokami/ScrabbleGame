using GameUtilities;
using Spectre.Console;
using TextUserInterface;


public class TUISandbox {
    static async Task Main(){
        var tui = new TUI(new GameUtilities.Size(5,5));
        var inputTask = Task.Run(() => {
            while(true)
                tui.HandleInput(Console.ReadKey(true).KeyChar);
        });

        var renderTask = Task.Run(async () => {
            while(true){
                tui.UpdateContent();
                tui.UpdateBuffer();
                tui.Render();
                await Task.Delay(100);
            }
        });

        await inputTask;
        await renderTask;
    }

}
