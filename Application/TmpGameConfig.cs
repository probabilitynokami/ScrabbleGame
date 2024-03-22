using GameController;
using GameObjects;
using GameUtilities;

class TestGamePopulator2 : IGamePopulator{
    // method hiding because malas...

    class DeckPopulator2 : IDeckPopulator{
        public List<ITile> GetTiles()
        {
            List<ITile> ret = [];
            List<int> letterpoints = [1,3,3,2,1,4,2,4,1,8,5,1,3,1,1,3,10,1,1,1,1,4,4,8,4,10];
            // horn fam paste ob bit
            // p1 : horn paste bit
            // p2 : fam ob
    
            // hornpas famobxx tebitxx xxxxxxx
            
            foreach(char x in "hornpasfamobxxtebitttttttttt".Reverse()){
                ret.Add(new Tile(x,letterpoints[x-'a']));
            }
            return ret;
        }   
    }
    
    
    public IEnumerable<string> GetWordList()
    {
        return ["horn","farm","farms","paste","mob","not","bit","pi","at","be"];
    }

    public IDeckPopulator GetDeckPopulator()
    {
        return new DeckPopulator2();
    }
    public IBoard GetBoard()
    {
        var board =  new Board(new Size(6,6));
        for(int i=0;i<board.Size.Height;i++){
            for(int j=0;j<board.Size.Width;j++){
                board.Squares[i,j] = new Square(1,1, new BoardPosition(i,j));
            }
        }

        // dark blue
        board.Squares[0,1] = new Square(3, 1, new BoardPosition(0,1));
        board.Squares[0,5] = new Square(3, 1, new BoardPosition(0,5));
        board.Squares[4,5] = new Square(3, 1, new BoardPosition(4,5));
        board.Squares[4,1] = new Square(3, 1, new BoardPosition(4,1));

        // light blue
        board.Squares[1,2] = new Square(2, 1, new BoardPosition(1,2));
        board.Squares[1,4] = new Square(2, 1, new BoardPosition(1,4));
        board.Squares[3,2] = new Square(2, 1, new BoardPosition(3,2));
        board.Squares[3,4] = new Square(2, 1, new BoardPosition(3,4));

        // pink
        board.Squares[2,3] = new Square(1, 2, new BoardPosition(2,3));
        board.Squares[5,0] = new Square(1, 2, new BoardPosition(5,0));

        return board;

    }

    public List<IPlayer> GetPlayers()
    {
        var p1 = new ScrabblePlayer();
        var p2 = new ScrabblePlayer();
        p1.Name = "Player 1";
        p2.Name = "Player 2";
        return [p1,p2];
    }
}