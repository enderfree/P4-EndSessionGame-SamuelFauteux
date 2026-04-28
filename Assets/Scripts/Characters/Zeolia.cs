using System;
using UnityEngine;

[System.Serializable]
public class Zeolia: Character
{
    protected override Type InitializeCharClass()
    {
        return this.GetType();
    }

    protected override Move[] InitializeMoveset()
    {
        return new Move[] 
        { 
            new Move(
                "Stab", 
                "Deals 5 points of physical damage. If you have no mana, steal one, if targets has no mana to steal, they take extra damage.", 
                MoveTypes.Physical, 
                0f, 
                (Character target, GameObject enemyPrefab, GameObject combatPrefab) => StabAnim(target, enemyPrefab, combatPrefab)
            ), 
            new Move(
                "Mana Leech", 
                "Drain 2 mana, overdoing it might have desirable side effects.", 
                MoveTypes.Shadow, 
                1f,
                (Character target, GameObject enemyPrefab, GameObject combatPrefab) => ManaLeechAnim(target, enemyPrefab, combatPrefab)
            ), 
            null, // I don't have enough time to handle status effects in the end
            new Move(
                "Dark Power", 
                "Deals 50 points of damage.", 
                MoveTypes.Shadow, 
                8f,
                (Character target, GameObject enemyPrefab, GameObject combatPrefab) => DarkPowerAnim(target, enemyPrefab, combatPrefab)
            ) 
        };
    }

    public override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
        mana = 0; // Zeolia starts with no mana, has a normal attack, one that gives mana, one that she spams costs mana and increases her dodge by 0.2, and a ult
    }

    private void AI(GameObject enemyPrefab, GameObject combatPrefab)
    {
        // Zeolia is supposed to be a very tough opponent for anyone, but Korrah as she drains mana
        // She has low HP, but high evasion. (one of her move is supposed to increase it, but I ran out of time for status effects)
        // Her only move that costs no mana is Stab, so against an enemy without mana, she can only do that
        // If she has mana, she can use a spell to steal more mana that what it costs to cast. 
        // This spell is her go to as her goal is to get mana quickly in order to spam her ult
        // before her low HP runs out.

        if (MP <= 0)
        {
            StabAnim(GameManager.playerChar, enemyPrefab, combatPrefab);
        }
        else if (MP >= Moves[3].ManaCost)
        {
            MP -= Moves[3].ManaCost;
            DarkPowerAnim(GameManager.playerChar, enemyPrefab, combatPrefab);
        }
        else
        {
            MP -= 1;
            ManaLeechAnim(GameManager.playerChar, enemyPrefab, combatPrefab);
        }
    }

    // Getter
    public override Action<GameObject, GameObject> CombatAI { get { return (GameObject enemyPrefab, GameObject combatPrefab) => AI(enemyPrefab, combatPrefab); } }

    // Moves
    private void StabAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Stab,
            () => Stab(target, enemyPrefab, combatPrefab)
        );
    }

    private void Stab(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        float damage = 5;

        if (target.MP > 0)
        {
            if (MP < MaxMana)
            {
                target.MP -= 1;
                MP += 1;
            }
        }
        else
        {
            damage *= 2;
        }

        target.TakeDamage(
            new Damage(damage * AtkDmg, MoveTypes.Physical), 
            enemyPrefab, 
            combatPrefab, 
            "Zeolia stabbed " + target.CharName + "!", 
            "Zeolia attempted to stab " + target.CharName + ", but they swiftly dodged."
        );
    }

    private void ManaLeechAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Stab,
            () => ManaLeech(target, enemyPrefab, combatPrefab)
        );
    }

    private void ManaLeech(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        float maxManaDrain = 2f;
        float manaDrain = maxManaDrain;
        float damage = 0f;
        string hitText = "Zeolia drains " + target.CharName + ".";

        if (target.MP < maxManaDrain)
        {
            manaDrain = target.MP;
            damage = (maxManaDrain - manaDrain) * 5f;
        }

        target.MP -= manaDrain;
        ++manaDrain; // gives drained mana + 1
        float personalManaDiff = MaxMana - MP;

        if (personalManaDiff < manaDrain)
        {
            MP += personalManaDiff;
            float healing = (manaDrain - personalManaDiff) * 5f;
            float personalHPDiff = MaxHP - HP;

            if (personalHPDiff < healing)
            {
                HP += personalHPDiff;
                damage += (healing - personalHPDiff);
                hitText = "Zeolia seems to be consuming " + target.CharName + "'s very life force!";
            }
            else
            {
                HP += healing;
                hitText += " She seems quite satisfied with the result";
            }
        }
        else
        {
            MP += manaDrain;
        }

        if (target.MP <= 0f)
        {
            damage += 5f;
        }

        if (MP >= MaxMana)
        {
            damage += 5f;
        }

        target.TakeDamage(
            new Damage(damage * MagicDmg, MoveTypes.Shadow), 
            enemyPrefab, 
            combatPrefab, 
            hitText, 
            "Zeolia steals mana from " + target.CharName + ", but " + target.CharName + " managed to avoid the brunt of it."
        );
    }

    private void DarkPowerAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Cast,
            () => DarkPower(target, enemyPrefab, combatPrefab)
        );
    }

    private void DarkPower(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {  
        string hitMessage = target.CharName + " can feel the darkness in their heart.";
        float damage = 50f * MagicDmg;

        if (damage >= target.HP)
        {
            hitMessage = target.CharName + " was consumed by darkness";
        }

        target.TakeDamage(
            new Damage(damage, MoveTypes.Shadow),
            enemyPrefab,
            combatPrefab,
            hitMessage, 
            target.CharName + "'s heart shines bright in face of Zeolia's darkness."
        );
    }
}