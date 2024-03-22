using GameUtilities;

namespace TextUserInterface;

public class Cell
{
    public string Content;
    public bool IsHighlight;
    public BoardPosition Position;

    private ITUIPanel _ownerPanel;

    public Cell(ITUIPanel panel, int row, int column){
        _ownerPanel = panel;
        Content = " ";
        Position = new BoardPosition
        {
            row = row,
            column = column
        };
    }

    public void Invoke(){
        _ownerPanel.Invoke(Position);
    }
}
