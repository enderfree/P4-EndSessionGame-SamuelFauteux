using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject combatPanel;
    [SerializeField] GameObject moveButtonPanel;
    [SerializeField] GameObject enemyButtonPanel;

    public delegate void PlayerTurnStart();
    public static event PlayerTurnStart OnPlayerTurnStart;

    public delegate void TurnEnd();
    public static event TurnEnd OnTurnEnd;

    private List<Character> turnOrder = new List<Character>();
    public static  List<Character> enemies = new List<Character>();
    private List<GameObject> combatPrefabs = new List<GameObject>();
    private int selectedMove = 0;

    // I have a GameStates enum, not a TurnState enum... 
    // I check if combat or not with the GameStates enum
    // I did not really know what to do with a TurnState enum.
    // What's important to know is who's turn it is, then all do the same thing in their turn: 
    // they use one of their move, and then it's not their turn anymore. 
    // The char who's turn it is is always turnOrder[0].
    // When they end their turn, they are moved to the back of the list.

    private void OnEnable()
    {
        GameManager.OnFightersReady += OnFightersReady;
        OnTurnEnd += ManageEndTurn;
    }

    private void OnDisable()
    {
        OnTurnEnd -= ManageEndTurn;
        GameManager.OnFightersReady -= OnFightersReady;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Events
    private void OnFightersReady(List<Character> fighters, List<GameObject> combatPrefabs, List<GameObject> healthbars)
    {
        enemies = fighters;
        this.combatPrefabs = combatPrefabs;
        turnOrder = new List<Character>();

        // for now, I don't have the speed stat so player first.
        turnOrder.Add(GameManager.playerChar);
        turnOrder.AddRange(enemies);
        PlayTurn();
    }

    private void ManageEndTurn()
    {
        if (GameManager.playerChar.HP <= 0)
        {
            GameManager.GameOver();
        }

        bool allEnemiesAreDead = true;
        foreach (Character enemy in enemies)
        {
            if (enemy.HP > 0)
            {
                allEnemiesAreDead = false;
                break;
            }
        }

        if (allEnemiesAreDead) 
        {
            Victory();
        }

        turnOrder.Add(turnOrder[0]);
        turnOrder.RemoveAt(0);
        PlayTurn();
    }

    // Buttons
    public void MoveButtonClicked(int moveI)
    {
        if (GameManager.playerChar.Moves[moveI] != null &&
            GameManager.playerChar.Moves[moveI].ManaCost <= GameManager.playerChar.MP)
        {
            selectedMove = moveI;
            moveButtonPanel.SetActive(false);
            enemyButtonPanel.SetActive(true);
        }
    }

    public void EnemyButtonClicked(int enemyI)
    {
        if (enemyI < enemies.Count)
        {
            enemyButtonPanel.SetActive(false);
            moveButtonPanel.SetActive(true);
            combatPanel.SetActive(false);
            Move move = GameManager.playerChar.Moves[selectedMove];
            GameManager.playerChar.MP -= move.ManaCost;
            move.MoveMethod(enemies[enemyI], combatPrefabs[enemyI + 1], combatPrefabs[0]);
        }
    }

    // misc
    private void PlayTurn()
    {
        if (turnOrder[0].HP <= 0)
        {
            OnTurnEnd();
            return;
        }

        if (turnOrder[0] == GameManager.playerChar)
        {
            OnPlayerTurnStart();
        }
        else
        {
            turnOrder[0].CombatAI(combatPrefabs[0], combatPrefabs[enemies.IndexOf(turnOrder[0]) + 1]);
        }
    }

    public static void EndTurn() // events can't be fired from other classes
    {
        OnTurnEnd();
    }

    private void Victory()
    {
        Sprite zeoliaPFP = CharManager.chars[CharNames.Zeolia].PFP;
        GameManager.staticDialogueManager.StartDialogue(
            new List<Dialogue>()
            {
                new Dialogue(zeoliaPFP, "Ow, ow, ow! Ok, ok! I get it!"),
                new Dialogue(zeoliaPFP, "You're fully warmed up!"),
                new Dialogue(zeoliaPFP, "Which is a shame since it's already the end of the game, but I hope you had fun!"),
                new Dialogue(zeoliaPFP, "Well, at least I can bring you back to the main menu!")
            },
            () => { UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); }
        );
    }
}
