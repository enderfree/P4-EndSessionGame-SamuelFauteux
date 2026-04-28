using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Korrah: Character
{
    protected override Type InitializeCharClass()
    {
        return this.GetType();
    }

    protected override Move[] InitializeMoveset()
    {
        return new Move[] 
        { // water gun, healing light, holy light, random
            new Move(
                "Water Gun", 
                "Throws a ball of water dealing 5-20 dmg.", 
                MoveTypes.Water, 
                5f, 
                (Character character, GameObject enemyPrefab, GameObject combatPrefab) => WaterGunAnim(character, enemyPrefab, combatPrefab)
            ), 
            new Move(
                "Healing Light", 
                "Provides Light Healing.", 
                MoveTypes.Healing,
                5f,
                (Character character, GameObject enemyPrefab, GameObject combatPrefab) => HealingLightAnim(character, enemyPrefab, combatPrefab)
            ), 
            null, null 
        };
    }

    private void AI(GameObject enemyPrefab, GameObject combatPrefab)
    {
        throw new NotImplementedException();
    }

    // Getter and setters
    public override Action<GameObject, GameObject> CombatAI { get { return (GameObject enemyPrefab, GameObject combatPrefab) => AI(enemyPrefab, combatPrefab); } }
    public override float MP { get => base.MP; set => mana = 8; }

    // Move
    private void WaterGunAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Cast,
            () => WaterGun(target, enemyPrefab, combatPrefab)
        );
     }

    private void WaterGun(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        target.TakeDamage(
            new Damage(UnityEngine.Random.Range(5, 20) * MagicDmg, MoveTypes.Water), 
            enemyPrefab, 
            combatPrefab, 
            "Korrah splashed " + target.CharName + " with a water baloon!", 
            target.CharName + " dodged this waterbaloon with swiftness!"
        );
    }

    private void HealingLightAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Cast,
            () => HealingLight(target, enemyPrefab, combatPrefab)
        );
    }

    private void HealingLight(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        float healing = 10f * MagicDmg;
        float missingHP = MaxHP - HP;

        if (healing > missingHP)
        {
            HP += missingHP;
        }
        else
        {
            HP += healing;
        }

        GameManager.staticDialogueManager.StartDialogue(
            new List<Dialogue>() { new Dialogue(PFP, "Korrah tended to her wounds.") }, 
            () => TurnManager.EndTurn()
        );
    }
}