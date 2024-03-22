namespace TextUserInterface;


public interface ICommand{
    public void Execute();
}

public class DoNothingCommand : ICommand{
    public void Execute(){
        ;
    }
}