using GameUtilities;
using Spectre.Console;
using TextUserInterface;


public class TUISandbox {
    static async Task Main(){
        var tui = new TUI(new GameUtilities.Size(20,10));

        AnsiConsole.Clear();
        var inputTask = Task.Run(async () => {
            while(true)
                await tui.HandleInput(Console.ReadKey(true).KeyChar);
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
