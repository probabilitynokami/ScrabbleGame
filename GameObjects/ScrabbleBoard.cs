using GameObjects;
using GameUtilities;


class ScrabbleBoard : IBoard
{
    public ScrabbleBoard(Size size){
        Size = size;
        _squares = new ISquare[size.Height,size.Width];
    }

    public ScrabbleBoard(IScrabbleBoardInitializer initializer) 
    : this(initializer.GetSize())
    {
        InitializeBoard(initializer);
    }

    public void InitializeBoard(IScrabbleBoardInitializer initializer){
        for(int i=0; i<Size.Height; i++){
            for(int j=0; j<Size.Width; j++){
                _squares[i,j] = initializer.GetSquare(i,j);
            }
        }
    }

    public Size Size { get; private set;}
    private ISquare[,] _squares;
    public ISquare[,] Squares { get => _squares;}

}

interface IScrabbleBoardInitializer{
    public ISquare GetSquare(int i, int j);
    public Size GetSize();
}