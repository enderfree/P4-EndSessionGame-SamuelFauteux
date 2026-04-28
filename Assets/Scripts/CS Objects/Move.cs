using System;
using UnityEngine;

public class Move
{
    private string moveName;
    private string description;
    private MoveTypes type;
    private float manaCost;
    private Action<Character, GameObject, GameObject> move;

    public Move(string moveName, string description, MoveTypes type, float manaCost, Action<Character, GameObject, GameObject> move)
    {
        this.moveName = moveName;
        this.description = description;
        this.type = type;
        this.manaCost = manaCost;
        this.move = move;
    }

    public string MoveName { get { return moveName; } }
    public string Description { get { return description; } }
    public MoveTypes Type { get { return type; } }
    public float ManaCost { get { return manaCost; } }
    public Action<Character, GameObject, GameObject> MoveMethod { get { return move; } }
}