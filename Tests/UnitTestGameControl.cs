using System;
using System.Diagnostics;
using GameController;
using GameObjects;
using GameUtilities;

namespace Tests;

[TestClass]
public class UnitTestGameControl
{
    GameControl? gameControl;
    public UnitTestGameControl(){
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

        var tile = gameControl.DrawTile()!;

        gameControl.PlaceTile(tile,new BoardPosition(0,0));

        Assert.AreEqual(tile, gameControl.GetBoard().Squares[0,0].PeekTile());

    }

    [TestMethod]
    public void TestWordStart(){
        gameControl = new(new TestGamePopulator());

        var tile = gameControl.DrawTile()!; // o
        gameControl.PlaceTile(tile,new BoardPosition(1,4));
        tile = gameControl.DrawTile()!; // l
        gameControl.PlaceTile(tile,new BoardPosition(1,3));
        tile = gameControl.DrawTile()!; // l
        gameControl.PlaceTile(tile,new BoardPosition(1,2));
        tile = gameControl.DrawTile()!; // e
        gameControl.PlaceTile(tile,new BoardPosition(1,1));
        tile = gameControl.DrawTile()!; // h
        gameControl.PlaceTile(tile,new BoardPosition(1,0));
        tile = gameControl.DrawTile()!; // n
        gameControl.PlaceTile(tile,new BoardPosition(0,4));

        var sqs = gameControl.Beam(gameControl.GetBoard().Squares[1,0],0,1);
        Assert.AreEqual("hello",sqs.Stringify());
        Assert.AreEqual(5,sqs.Count);
        List<ISquare> wordStartsDown, wordStartsRight;
        (wordStartsDown, wordStartsRight) = gameControl.GetWordStarts(gameControl.gameState.placedSquares);
        Assert.AreEqual(2,wordStartsDown.Count+wordStartsRight.Count);
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

        var words = gameControl.GetWords(wordStarts.Item1,1,0);
        words.AddRange(gameControl.GetWords(wordStarts.Item2,0,1));

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
        Assert.AreEqual(1,gameControl.gameState.placedSquares.Count);
        var words = gameControl.GetTurnWords();
        Assert.AreEqual(1,words.Count);
        
        Assert.AreEqual("no",words[0].Stringify());
        Assert.IsTrue(gameControl.wordChecker.CheckWord("no"));
        
        
        score = gameControl.GetTurnScore();
        Assert.AreEqual(2,score);
        
        gameControl.gameState.Reset();

        gameControl.PlaceTile(new Tile('h',1), new BoardPosition(0,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(2,1));
        gameControl.PlaceTile(new Tile('l',1), new BoardPosition(3,1));

        score = gameControl.GetTurnScore();
        Assert.AreEqual(40,score);
        
        
    }
    
    
    [TestMethod]
    public void TestTurnChange(){
        gameControl = new(new TestGamePopulator2());
        gameControl.FirstDeal();
        
        // First Turn P1
        var player = gameControl.CurrentPlayer;
        ITile tile;

        Assert.AreEqual("hornpas",gameControl.GetCurrentPlayerData().Rack.Tiles.Stringify());

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('h',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(2,1));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('o',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(2,2));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('r',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(2,3));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('n',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(2,4));


        gameControl.NextTurn();
        Assert.AreEqual(14,gameControl.GetPlayerData(player).Score);

        // Second turn P2
        player = gameControl.CurrentPlayer;

        Assert.AreEqual("famobxx",gameControl.GetCurrentPlayerData().Rack.Tiles.Stringify());

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('f',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(0,3));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('a',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(1,3));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('m',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(3,3));


        gameControl.NextTurn();
        Assert.AreEqual(9,gameControl.GetPlayerData(player).Score);

        // third turn P1 paste [farm]s
        player = gameControl.CurrentPlayer;

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('p',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(4,1));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('a',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(4,2));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('s',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(4,3));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('t',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(4,4));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('e',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(4,5));

        gameControl.NextTurn();
        Assert.AreEqual(14+25,gameControl.GetPlayerData(player).Score);


        // fourth turn P2
        player = gameControl.CurrentPlayer;


        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('o',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(3,4));

        tile = gameControl.GetCurrentPlayerData().Rack.TakeTile(0)!;
        Assert.AreEqual('b',tile.Letter);
        gameControl.PlaceTile(tile, new BoardPosition(3,5));

        gameControl.NextTurn();
        Assert.AreEqual(9+16,gameControl.GetPlayerData(player).Score);


    }



}



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
            
            foreach(char x in "hornpasfamobxxtebitxxxxxxxxx".Reverse()){
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
        return [new ScrabblePlayer(), new ScrabblePlayer()];
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
        return ["hello","cruel","world","hellokitty","helloskitty","no","nothing","hell"];
    }
}