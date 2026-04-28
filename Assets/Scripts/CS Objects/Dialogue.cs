using UnityEngine;

public class Dialogue
{
    private Sprite interlocutor;
    private string message;

    public Dialogue(Sprite interlocutor, string message)
    {
        this.interlocutor = interlocutor;
        this.message = message;
    }

    public Sprite Interlocutor { get { return interlocutor; } }
    public string Message { get { return message; } }
}