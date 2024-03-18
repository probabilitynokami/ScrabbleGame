using GameUtilities;
namespace GameObjects;

public class Board : IBoard
{
    private ISquare[,] _squares;
    private Size _size;
    public ISquare[,] Squares => _squares;
    public Size Size => _size;

    public Board(Size size){
        _squares = new ISquare[size.Height,size.Width];
        _size = size;
    }

}
