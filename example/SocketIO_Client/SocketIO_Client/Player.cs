using System;

[Serializable]
class Player
{
    public string name { get; }
    
    public Player(string name)
    {
        this.name = name;
    }
}
