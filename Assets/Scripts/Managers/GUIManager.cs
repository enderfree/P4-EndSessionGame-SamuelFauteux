using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class GUIManager : MonoBehaviour
{
    [Header("Blackscreen")]
    [SerializeField] private Image blackscreen;
    [SerializeField] private float fadeDuration;

    [Header("Dialogue Box")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Image pfpHolder;

    [Header("Combat")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject moveButtonPanel;
    [SerializeField] private List<Button> moveButtons;
    [SerializeField] private List<TMP_Text> moveButtonsText;
    [SerializeField] private TMP_Text descriptionTitle;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text statusEffectShorthandHolder;
    [SerializeField] private TMP_Text statusEffectsDescription;
    [SerializeField] private GameObject enemySelectionPanel;
    [SerializeField] private List<Button> enemyButtons;
    [SerializeField] private List<TMP_Text> enemyButtonsText;

    public delegate void BlackscreenUp(ReasonsForBlackscreen reasonsForBlackscreen);
    public static event BlackscreenUp OnBlackscreenUp;

    private const string EMPTYOPTION = "---";

    private MethodInfo isHighlighted;
    private List<Character> fighters = new List<Character>();
    private List<GameObject> healthbars = new List<GameObject>();
    private int highlightedEnemyButtonI;

    // Unity
    void Awake()
    {
        isHighlighted = typeof(Selectable).GetMethod("IsHighlighted", BindingFlags.Instance | BindingFlags.NonPublic);
        highlightedEnemyButtonI = enemyButtons.Count; // initialize invalid
        blackscreen.canvasRenderer.SetAlpha(0.01f); // there is a bug if a ever becomes 0, it stays 0
        enemySelectionPanel.SetActive(false);
        combatPanel.SetActive(false);
        dialogueBox.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
        GameManager.OnFightersReady += OnFightersReady;
        GameManager.OnOverworldReady += OnOverworldReady;
        TurnManager.OnPlayerTurnStart += OnPlayerTurnStart;
        Character.OnHPChanged += OnHPChanged;
        Character.OnMPChanged += OnMPChanged;
        Character.OnStatusEffectChanged += OnStatusEffectChanged;
    }

    private void OnDisable()
    {
        Character.OnStatusEffectChanged -= OnStatusEffectChanged;
        Character.OnMPChanged -= OnMPChanged;
        Character.OnHPChanged -= OnHPChanged;
        TurnManager.OnPlayerTurnStart -= OnPlayerTurnStart;
        GameManager.OnOverworldReady -= OnOverworldReady;
        GameManager.OnFightersReady -= OnFightersReady;
        GameManager.OnGameStateChange -= OnGameStateChange;
    }

    void Update()
    {
        bool anEnemyButtonIsHighlighted = false;

        if (moveButtonPanel.activeSelf)
        {
            for (int i = 0; i < moveButtons.Count; ++i)
            {
                if ((bool)isHighlighted.Invoke(moveButtons[i], null))
                {
                    descriptionTitle.text = moveButtonsText[i].text;

                    if (moveButtonsText[i].text != EMPTYOPTION)
                    {
                        description.text = GameManager.playerChar.Moves[i].Description;
                    }
                    else
                    {
                        description.text = EMPTYOPTION;
                    }
                }
            }
        }
        else if (enemySelectionPanel.activeSelf)
        {
            for (int i = 0; i < enemyButtons.Count; ++i)
            {
                if ((bool)isHighlighted.Invoke(enemyButtons[i], null))
                {
                    if (i != highlightedEnemyButtonI)
                    {
                        if (highlightedEnemyButtonI < healthbars.Count - 1)
                        {
                            healthbars[highlightedEnemyButtonI + 1].transform.Find("Image").gameObject.SetActive(false);
                        }

                        if (i < healthbars.Count - 1)
                        {
                            healthbars[i + 1].transform.Find("Image").gameObject.SetActive(true);
                        }

                        highlightedEnemyButtonI = i;
                    }

                    anEnemyButtonIsHighlighted = true;
                    break;
                }
            }
        }

        if (!enemySelectionPanel.activeSelf || !anEnemyButtonIsHighlighted)
        {
            if (highlightedEnemyButtonI < healthbars.Count - 1)
            {
                healthbars[highlightedEnemyButtonI + 1].transform.Find("Image").gameObject.SetActive(false);
            }
        }
    }

    // Events
    private void OnGameStateChange(GameStates oldGameState, GameStates newGameState)
    {
        SetBlackscreen(ReasonsForBlackscreen.GameStateChange);
        
        if (oldGameState == GameStates.Combat)
        {
            healthbars.Clear();
        }
    }

    private void OnFightersReady(List<Character> fighters, List<GameObject> combatPrefabs, List<GameObject> healthbars)
    {
        // Healthbar Setup
        this.fighters.Add(GameManager.playerChar);
        this.fighters.AddRange(fighters);
        this.healthbars = healthbars;

        for (int i = 0; i < healthbars.Count; ++i)
        {
            healthbars[i].transform.Find("Image").gameObject.SetActive(false); // didn't find a better way to access a prefab child

            Character target;
            if (i == 0)
            {
                target = GameManager.playerChar;
            }
            else
            {
                target = fighters[i - 1];
            }

            GameObject hpBar = healthbars[i].transform.Find("Health Bar").gameObject;
            GameObject mpBar = healthbars[i].transform.Find("Mana Bar").gameObject;

            Slider hpSlider = hpBar.GetComponent<Slider>();
            Slider mpSlider = mpBar.GetComponent<Slider>();
            
            hpSlider.maxValue = target.MaxHP;
            hpSlider.value = target.HP;

            mpSlider.maxValue = target.MaxMana;
            mpSlider.value = target.MP;

            if (target.MaxMana <= 0f)
            {
                mpBar.gameObject.SetActive(false);
            }
        }

        // Fill Enemy Buttons
        for (int i = 0; i < enemyButtonsText.Count; ++i)
        {
            if(i < fighters.Count)
            {
                enemyButtonsText[i].text = fighters[i].CharName.ToString();
            }
            else
            {
                enemyButtonsText[i].text = EMPTYOPTION;
            }
        }

        dialogueBox.SetActive(true);
        UnsetBlackscreen();
    }

    private void OnOverworldReady()
    {
        dialogueBox.SetActive(false);
        UnsetBlackscreen();
    }

    private void OnPlayerTurnStart()
    {
        pfpHolder.sprite = GameManager.playerChar.PFP;
        textBox.text = "";

        dialogueButton.SetActive(false);
        combatPanel.SetActive(true);

        for (int i = 0; i < moveButtons.Count; ++i) // should be 4
        {
            if (GameManager.playerChar.Moves[i] != null)
            {
                moveButtonsText[i].text = GameManager.playerChar.Moves[i].MoveName;
            }
            else
            {
                moveButtonsText[i].text = EMPTYOPTION;
            }
        }
    }

    private void OnHPChanged(Character character)
    {
        healthbars[fighters.IndexOf(character)].transform.Find("Health Bar").GetComponent<Slider>().value = character.HP;
    }

    private void OnMPChanged(Character character)
    {
        healthbars[fighters.IndexOf(character)].transform.Find("Mana Bar").GetComponent<Slider>().value = character.MP;
    }

    private void OnStatusEffectChanged()
    {
        statusEffectShorthandHolder.text = "";
        statusEffectsDescription.text = "";

        foreach (StatusEffect statusEffect in GameManager.playerChar.StatusEffects)
        {
            statusEffectShorthandHolder.text += $"<mark={ColorUtility.ToHtmlStringRGBA(statusEffect.DisplayColor)}>{statusEffect.ShortHand}</mark> ";
            statusEffectsDescription.text += $"<mark={ColorUtility.ToHtmlStringRGBA(statusEffect.DisplayColor)}>{statusEffect.Description}</mark> ";
        }
    }

    // misc
    private IEnumerator WaitThenTrigger(float waitDuration, Action action)
    {
        yield return new WaitForSeconds(waitDuration);
        action();
    }

    private void SetBlackscreen(ReasonsForBlackscreen reasonForBlackscreen)
    {
        blackscreen.CrossFadeAlpha(1, fadeDuration, false);
        // CrossFadeAlpha does not like being called in an IEnumerator and returns nothing
        StartCoroutine(WaitThenTrigger(fadeDuration, () => OnBlackscreenUp(reasonForBlackscreen)));
    }

    private void UnsetBlackscreen()
    {
        blackscreen.CrossFadeAlpha(0.01f, fadeDuration, false);
    }
}
