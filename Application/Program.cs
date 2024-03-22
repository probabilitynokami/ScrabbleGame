using TextUserInterface;
using SafeGameController;
using GameUtilities;

class Program{
    static async Task Main(){

        SafeGameController.SafeGameController gameController = new(new TestGamePopulator2());

        Console.Clear();
        TUI userInterface = new(new Size(20,10), gameController);
        Task handleInput = Task.Run(async () => {
            while(true){
                var input = Console.ReadKey(true);
                await userInterface.HandleInput(input.KeyChar);
            }
        });

        Task render = Task.Run(async () => {
            while(true){
                userInterface.UpdateContent();
                userInterface.UpdateBuffer();
                userInterface.Render();
                await Task.Delay(10);
            }
        });

        await handleInput;
        await render;
        // GameController.GameControl gameController = new();
    }
}