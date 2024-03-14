using System;
using System.Diagnostics;
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
        Assert.AreEqual(6, gameControl.GetRemainingTiles().Count);
        Assert.IsNotNull(gameControl.GetBoard().Squares[0,0]);
    }

    [TestMethod]
    public void TestDeckDraw(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile();
        Assert.AreNotEqual(null, tile);
        Assert.AreEqual(5, gameControl.GetRemainingTiles().Count);

    }

    [TestMethod]
    public void TestTilePlacement(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile();

        gameControl.PlaceTile(tile,new BoardPosition(0,0));

        Assert.AreEqual(tile, gameControl.GetBoard().Squares[0,0].PeekTile());

    }

    [TestMethod]
    public void TestWordStart(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile(); // o
        gameControl.PlaceTile(tile,new BoardPosition(1,4));
        tile = gameControl.DrawTile(); // l
        gameControl.PlaceTile(tile,new BoardPosition(1,3));
        tile = gameControl.DrawTile(); // l
        gameControl.PlaceTile(tile,new BoardPosition(1,2));
        tile = gameControl.DrawTile(); // e
        gameControl.PlaceTile(tile,new BoardPosition(1,1));
        tile = gameControl.DrawTile(); // h
        gameControl.PlaceTile(tile,new BoardPosition(1,0));
        tile = gameControl.DrawTile(); // n
        gameControl.PlaceTile(tile,new BoardPosition(0,4));

        var sqs = gameControl.Beam(gameControl.GetBoard().Squares[1,0],0,1);
        Assert.AreEqual("hello",sqs.Stringify());
        Assert.AreEqual(5,sqs.Count);

        Assert.IsTrue(gameControl.IsStartSquare(sqs[0]));
        Assert.IsFalse(gameControl.IsStartSquare(sqs[1]));
        Assert.IsFalse(gameControl.IsStartSquare(sqs[^1]));

        var wordStarts = gameControl.GetWordStarts(gameControl.gameState.placedSquares);
        Assert.AreEqual(2,wordStarts.Count);
    }

    [TestMethod]
    public void TestWordConstruct(){
        gameControl = new(new TestGamePopulator());
        gameControl.PlaceTile(new Tile('h',1), new BoardPosition(1,0));
        gameControl.PlaceTile(new Tile('e',1), new BoardPosition(1,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(1,2));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(1,3));
        gameControl.PlaceTile(new Tile('o',1), new BoardPosition(1,4));

        gameControl.PlaceTile(new Tile('n',1), new BoardPosition(0,4));

        gameControl.PlaceTile(new Tile('h',1), new BoardPosition(0,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(2,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(3,1));

        var wordStarts = gameControl.GetWordStarts(gameControl.gameState.placedSquares);

        var words = gameControl.GetWords(wordStarts);

        Assert.AreEqual(3,words.Count);

        List<string> words_str = [];

        foreach(var w in words)
            words_str.Add(w.Stringify());
        
        Assert.IsTrue(words_str.Contains("hello"));
        Assert.IsTrue(words_str.Contains("no"));
        Assert.IsTrue(words_str.Contains("hell"));
    }
    [TestMethod]
    public void TestScoring(){
        gameControl = new(new TestGamePopulator());
        gameControl.PlaceTile(new Tile('h',2), new BoardPosition(1,0));
        gameControl.PlaceTile(new Tile('e',1), new BoardPosition(1,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(1,2));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(1,3));
        gameControl.PlaceTile(new Tile('o',1), new BoardPosition(1,4));


        int score = gameControl.GetTurnScore();
        Assert.AreEqual(24,score);

        gameControl.PlaceTile(new Tile('n',1), new BoardPosition(0,4));
        score = gameControl.GetTurnScore();
        Assert.AreEqual(0,score); // 0 because illegal placement

        gameControl.GetBoard().Squares[0,4].UnplaceTile();
        gameControl.gameState.Reset();
        gameControl.PlaceTile(new Tile('n',1), new BoardPosition(0,4));

        score = gameControl.GetTurnScore();
        Assert.AreEqual(2,score);

        gameControl.PlaceTile(new Tile('h',1), new BoardPosition(0,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(2,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(3,1));

        score = gameControl.GetTurnScore();
        Assert.AreEqual(26+40,score);

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