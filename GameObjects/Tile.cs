using GameObjects;


class Tile : ITile {
    private char _letter;
    private int _point;
    public char Letter => _letter;
    public int Point => _point;

    public Tile(char letter, int point){
        _letter = letter;
        _point = point;
    }
}