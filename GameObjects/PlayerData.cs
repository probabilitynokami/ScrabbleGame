using GameObjects;

public class PlayerData
{
    public PlayerData(){
        Rack = new Rack();
    }
    public int Score{get;set;}
    public IRack Rack{get;set;}

}
