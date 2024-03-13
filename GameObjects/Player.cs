using GameObjects;


class ScrabblePlayer : IPlayer
{
    public int ID{get;}
    private string _name = "";
    public string Name { get => _name; set {_name = value;} }
}