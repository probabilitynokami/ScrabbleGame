using GameUtilities;

namespace TextUserInterface;


public interface ITUIPanel{
    public void UpdateCellContent();
    public void SetCellCommand(ICommand command);

    public void Invoke(BoardPosition position);
    
}
