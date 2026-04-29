using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerHyperRestriction
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RoomNames startingRoom;
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private DialogueManager dialogueManager;

    public delegate void RoomChange(Room oldRoom, Room newRoom);
    public static event RoomChange OnRoomChange;

    public delegate void FightersReady(List<Character> enemies, List<GameObject> combatPrefabs, List<GameObject> healthbars);
    public static event FightersReady OnFightersReady;

    public delegate void OverworldReady();
    public static event OverworldReady OnOverworldReady;

    public static DialogueManager staticDialogueManager; // this manager is sometimes called from non-monobehavior classes or some that do not exist in the editor
    public static Character playerChar; 

    private static Room currentRoom;

    // Combat tmp
    private static Character[] encounterEnemies = new Character[5];
    private GameObject[] tmpCombatGameObjects = new GameObject[6];
    private static Action afterCombatAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() // Awake and OnEnable were too soon
    {
        staticDialogueManager = dialogueManager;
        playerChar = CharManager.chars[CharNames.Korrah]; // temp until I figure how to do char selection
        currentRoom = RoomManager.rooms[startingRoom];

        CameraManager.Initialization();
    }

    private void OnEnable()
    {
        GUIManager.OnBlackscreenUp += OnBlackscreenUp;
    }

    private void OnDisable()
    {
        GUIManager.OnBlackscreenUp -= OnBlackscreenUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // getters and setters
    public static Room CurrentRoom
    {
        get
        {
            return currentRoom;
        }
        set
        {
            OnRoomChange(currentRoom, value);
            currentRoom = value;
        }
    }

    // Events
    private void OnBlackscreenUp(ReasonsForBlackscreen reasonForBlackscreen)
    {
        switch (reasonForBlackscreen)
        {
            case ReasonsForBlackscreen.GameStateChange: 
                switch (GameState)
                {
                    case GameStates.Overworld:
                        InitiateOverworld();
                        break;
                    case GameStates.Combat: 
                        InitiateCombat();
                        break;
                    default: 
                        throw new NotImplementedException("TurnManager/OnBlackscreenUp: forgot to implement a game state");
                }
                break;
            case ReasonsForBlackscreen.RoomTransition:
                throw new NotImplementedException();
                break;
            default: break; // was probably called by another class
        }
    }

    // state initializations
    private void InitiateOverworld()
    {
        if (afterCombatAction != null)
        {
            afterCombatAction();
        }

        CameraManager.ActiveCinemachine = currentRoom.OverworldCamera;

        // Make overworld chars visible
        foreach (GameObject character in currentRoom.Chars)
        {
            if (character.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = true;
            }
        }

        foreach (GameObject character in RoomManager.rooms[RoomNames.Misc].Chars)
        {
            // this redondance will be removed if I figure how to handle transitive rooms better
            if (character.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = true;
            }
        }

        // delete combat prefabs
        foreach (GameObject combatGameObject in tmpCombatGameObjects)
        {
            Destroy(combatGameObject);
        }

        OnOverworldReady();
    }

    private void InitiateCombat() 
    {
        CameraManager.ActiveCinemachine = currentRoom.CombatCamera;

        // Make overworld chars invisible
        foreach (GameObject character in currentRoom.Chars)
        {
            if (character.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = false;
            }
        }

        foreach (GameObject character in RoomManager.rooms[RoomNames.Misc].Chars)
        {
            // this redondance will be removed if I figure how to handle transitive rooms better
            if (character.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = false;
            }
        }

        // put chars in place
        tmpCombatGameObjects[5] = Instantiate(playerChar.CombatPrefab, currentRoom.PlayerPos.position, Quaternion.Euler(0, 180, 0));
        List<GameObject> healthBars = new List<GameObject>();
        healthBars.Add(Instantiate(healthbarPrefab, tmpCombatGameObjects[5].transform));
        List<GameObject> combatPrefabs = new List<GameObject>();
        combatPrefabs.Add(tmpCombatGameObjects[5]);

        // // enemies
        List<Character> validEnemies = new List<Character>();

        for (int i = 0; i < encounterEnemies.Length; ++i)
        {
            if (encounterEnemies[i] is Character character)
            {
                tmpCombatGameObjects[i] = Instantiate(encounterEnemies[i].CombatPrefab, currentRoom.EnemyPos[i].position, new Quaternion());
                validEnemies.Add(character);
                combatPrefabs.Add(tmpCombatGameObjects[i]);
                healthBars.Add(Instantiate(healthbarPrefab, tmpCombatGameObjects[i].transform));
            }
        }

        OnFightersReady(validEnemies, combatPrefabs, healthBars);
    }

    // misc

    /// <summary>
    /// Initiate the combat state
    /// </summary>
    /// <param name="enemy1">enemy in position 1</param>
    /// <param name="enemy2">enemy in position 2</param>
    /// <param name="enemy3">enemy in position 3</param>
    /// <param name="enemy4">enemy in position 4</param>
    /// <param name="enemy5">enemy in position 5</param>
    public static void StartCombat(Character enemy1 = null, Character enemy2 = null, Character enemy3 = null, Character enemy4 = null, Character enemy5 = null, Action doAfterCombat = null)
    {
        if (enemy1 is null && enemy2 is null && enemy3 is null && enemy4 is null && enemy5 is null)
        {
            Debug.LogError("Combat failed to start as no enemies were provided.");
        }
        else
        {
            GameState = GameStates.Combat;
            encounterEnemies = new Character[5] { enemy1, enemy2, enemy3, enemy4, enemy5 };
            afterCombatAction = doAfterCombat;
        }
    }

    public static void EndCombat()
    {
        GameState = GameStates.Overworld;
    }

    public static void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Death");
    }
}
