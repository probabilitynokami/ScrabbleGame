using TextUserInterface;
using SafeGameController;
using GameUtilities;

class Program{
    static void Main(){
        TUI userInterface = new(new Size(20,20));
        GameController.GameControl gameController = new();
    }
}