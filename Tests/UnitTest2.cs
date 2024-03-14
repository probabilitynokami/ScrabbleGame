using GameController;
using GameObjects;
using GameUtilities;

namespace Tests;

[TestClass]
public class UnitTest2
{
    GameControl? gameControl;
    public UnitTest2(){
    }

    [TestMethod]
    public void TestGameControlInitialization(){
        gameControl = new(new TestGamePopulator());
        Assert.AreEqual(5, gameControl.GetBoard().Size.Height);
        Assert.AreEqual(5, gameControl.GetBoard().Size.Width);
        Assert.AreEqual(5, gameControl.GetRemainingTiles().Count);
        Assert.IsNotNull(gameControl.GetBoard().Squares[0,0]);
    }

    [TestMethod]
    public void TestDeckDraw(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile();
        Assert.AreNotEqual(null, tile);
        Assert.AreEqual(4, gameControl.GetRemainingTiles().Count);

    }

    [TestMethod]
    public void TestTilePlacement(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile();

        gameControl.PlaceTile(tile,new BoardPosition(0,0));

        Assert.AreEqual(tile, gameControl.GetBoard().Squares[0,0].PeekTile());

    }

    [TestMethod]
    public void Test(){
        gameControl = new(new TestGamePopulator());
    }

}

class DeckPopulator : IDeckPopulator
{
    public List<ITile> GetTiles()
    {
        List<ITile> ret = [];
        foreach(var x in "nhello"){
            ret.Add(new Tile(x,1));
        }
        return ret;
    }

}
class TestGamePopulator : IGamePopulator
{
    public IBoard GetBoard()
    {
        var board =  new Board(new Size(5,5));
        for(int i=0;i<board.Size.Height;i++){
            for(int j=0;j<board.Size.Width;j++){
                board.Squares[i,j] = new Square(1,1, new BoardPosition(i,j));
            }
        }

        board.Squares[1,0] = new Square(10, 1, new BoardPosition(1,0));
        board.Squares[3,1] = new Square(1, 10, new BoardPosition(3,1));

        return board;

    }

    public IDeckPopulator GetDeckPopulator()
    {
        return new DeckPopulator();
    }

    public List<IPlayer> GetPlayers()
    {
        return [new ScrabblePlayer(), new ScrabblePlayer()];
    }

    public IEnumerable<string> GetWordList()
    {
        return ["hello","cruel","world","hellokitty","helloskitty","no","nothing"];
    }
}