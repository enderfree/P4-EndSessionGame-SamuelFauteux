using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public abstract class Character: ISerializationCallbackReceiver
{
    public delegate void HPChanged(Character character);
    public static event HPChanged OnHPChanged;

    public delegate void MPChanged(Character character);
    public static event MPChanged OnMPChanged;

    public delegate void StatusEffectChanged();
    public static event StatusEffectChanged OnStatusEffectChanged;

    [Header("General")]
    [SerializeField] private CharNames charName;
    [SerializeField] private string description;
    [SerializeField] private string mechanicDescription;
    [SerializeField] private Sprite pfp;
    [SerializeField] private GameObject combatPrefab;

    [Header("Stats")]
    [SerializeField] private float maxHP;
    [SerializeField] private float evasion;
    [SerializeField] private float physDmg;
    [SerializeField] private float magicDmg;
    [SerializeField] private float maxMana;

    protected float hp;
    protected float mana;
    protected Move[] moves = new Move[4];
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private Type type; // to link chars to their type when pulled out of the dict

    /// <summary>
    /// char is char.CharClass
    /// </summary>
    /// <returns>this.GetType();</returns>
    protected abstract Type InitializeCharClass();

    /// <summary>
    /// Gives the char's moveset at initialization. MUST RETURN 4 Move!
    /// </summary>
    /// <returns>4 Move</returns>
    protected abstract Move[] InitializeMoveset();

    public Character()
    {
        moves = InitializeMoveset();
        type = InitializeCharClass();
    }

    public virtual void OnBeforeSerialize()
    {
        // needed
    }

    public virtual void OnAfterDeserialize()
    {
        hp = maxHP;
        mana = maxMana;
    }

    // Getters
    public CharNames CharName { get { return charName; } }
    public string Description { get { return description; } }
    public Sprite PFP { get { return pfp; } }
    public GameObject CombatPrefab { get { return combatPrefab; } }
    public float MaxHP { get { return maxHP; } }
    public float Evasion { get { return evasion; } }
    public float AtkDmg { get { return physDmg; } }
    public float MagicDmg { get { return magicDmg; } }
    public float MaxMana { get { return maxMana; } }
    public virtual float HP 
    { 
        get 
        { 
            return hp; 
        } 
        protected set 
        { 
            hp = value;
            OnHPChanged(this);
        } 
    }
    public virtual float MP 
    { 
        get 
        { 
            return mana; 
        } 
        set 
        { 
            mana = value; 
            OnMPChanged(this);
        } 
    }
    public Move[] Moves { get { return moves; } }
    public List<StatusEffect> StatusEffects 
    { 
        get 
        { 
            return statusEffects; 
        }
        set 
        {
            statusEffects = value;
            OnStatusEffectChanged();
        }
    }
    public Type CharClass { get { return type; } }
    public abstract Action<GameObject, GameObject> CombatAI { get; }

    // misc
    public virtual void TakeDamage(Damage damage, GameObject enemyPrefab, GameObject combatPrefab, string hitMessage, string missMessage, Action extraOnHit = null)
    {
        CombatAnimationsManager animManager = enemyPrefab.GetComponent<CombatAnimationsManager>();
        List<Dialogue> dialogues = new List<Dialogue>();

        if (UnityEngine.Random.Range(0f, 1f) > Evasion)
        {
            dialogues.Add(new Dialogue(PFP, hitMessage));

            animManager.TriggerAnimator(
                enemyPrefab.GetComponent<Animator>(),
                CombatAnimationsManager.Triggers.Hurt
            );
            
            if (HP > 0)
            {
                HP -= damage.Amount;

                if (extraOnHit != null)
                {
                    extraOnHit();
                }

                if (HP <= 0)
                {
                    dialogues.Add(new Dialogue(PFP, CharName + " fainted."));

                    animManager.TriggerAnimator(
                        enemyPrefab.GetComponent<Animator>(),
                        CombatAnimationsManager.Triggers.Death
                    );
                }
            }
        }
        else
        {
            dialogues.Add(new Dialogue(PFP, missMessage));

            animManager.TriggerAnimator(
                enemyPrefab.GetComponent<Animator>(),
                CombatAnimationsManager.Triggers.Dodge
            );
        }

        GameManager.staticDialogueManager.StartDialogue(dialogues, () => TurnManager.EndTurn());
    }
}