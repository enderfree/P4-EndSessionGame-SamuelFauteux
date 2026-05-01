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
                MoveTypes.Light,
                5f,
                (Character character, GameObject enemyPrefab, GameObject combatPrefab) => HealingLightAnim(character, enemyPrefab, combatPrefab)
            ), 
            new Move(
                "Holy Light", 
                "Hit every enemy of 10 and clear the effects of darkness.", 
                MoveTypes.Light,
                5f,
                (Character character, GameObject enemyPrefab, GameObject combatPrefab) => HolyLightAnim(character, enemyPrefab, combatPrefab)
            ), 
            null 
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

        StatusEffects.RemoveAll(x => x.StatusEffectName == StatusEffectNames.ShadowShroud);

        GameManager.staticDialogueManager.StartDialogue(
            new List<Dialogue>() { new Dialogue(PFP, "Korrah tended to her wounds.") }, 
            () => TurnManager.EndTurn()
        );
    }

    private void HolyLightAnim(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        CombatAnimationsManager animManager = combatPrefab.GetComponent<CombatAnimationsManager>();
        animManager.TriggerAnimator(
            combatPrefab.GetComponent<Animator>(),
            CombatAnimationsManager.Triggers.Cast,
            () => HolyLight(target, enemyPrefab, combatPrefab)
        );
    }

    private void HolyLight(Character target, GameObject enemyPrefab, GameObject combatPrefab)
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        for (int i = 0; i < TurnManager.enemies.Count; ++i)
        {
            CombatAnimationsManager animManager = TurnManager.combatPrefabs[i + 1].GetComponent<CombatAnimationsManager>();
            Animator animator = TurnManager.combatPrefabs[i + 1].GetComponent<Animator>();

            TurnManager.enemies[i].StatusEffects.RemoveAll(x => x.StatusEffectName == StatusEffectNames.ShadowShroud);

            if (UnityEngine.Random.Range(0f, 1f) > TurnManager.enemies[i].Evasion)
            {
                dialogues.Add(new Dialogue(TurnManager.enemies[i].PFP, "The light is shining on " + TurnManager.enemies[i].CharName + "!"));

                animManager.TriggerAnimator(
                    animator,
                    CombatAnimationsManager.Triggers.Hurt
                );

                if (TurnManager.enemies[i].HP > 0)
                {
                    TurnManager.enemies[i].HP -= 10f;

                    if (TurnManager.enemies[i].HP <= 0f)
                    {
                        TurnManager.enemies[i].StatusEffects.Clear();
                        dialogues.Add(new Dialogue(TurnManager.enemies[i].PFP, TurnManager.enemies[i].CharName + " fainted."));

                        animManager.TriggerAnimator(
                            animator,
                            CombatAnimationsManager.Triggers.Death
                        );
                    }
                }
            }
            else
            {
                dialogues.Add(new Dialogue(TurnManager.enemies[i].PFP, TurnManager.enemies[i].CharName + " hid from the light!"));

                animManager.TriggerAnimator(
                    animator,
                    CombatAnimationsManager.Triggers.Dodge
                );
            }

            GameManager.staticDialogueManager.StartDialogue(dialogues, () => TurnManager.EndTurn());
        }
    }
}