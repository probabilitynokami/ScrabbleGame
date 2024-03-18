using GameObjects;


public class ScrabblePlayer : IPlayer
{
    public int ID{get;}
    private string _name = "";
    public string Name { get => _name; set {_name = value;} }

    public ScrabblePlayer(){
        ID = 0;
        _name = "";
    }
}