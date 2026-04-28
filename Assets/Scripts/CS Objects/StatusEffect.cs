using UnityEngine;
using System.Collections.Generic;

public class StatusEffect
{
    public static Dictionary<StatusEffectNames, StatusEffect> statusEffects = new Dictionary<StatusEffectNames, StatusEffect>();

    private StatusEffectNames statusEffectName;
    private string description;
    private string shortHand;
    private Color displayColor;
    // maybe add a visual effect if I have the time to complete the 3d assignment before the deadline of this project

    public StatusEffect(StatusEffectNames statusEffectName, string description, string shortHand, Color displayColor)
    {
        this.statusEffectName = statusEffectName;
        this.description = description;
        this.shortHand = shortHand;
        this.displayColor = displayColor;

        statusEffects.Add(this.statusEffectName, this);
    }

    public StatusEffectNames StatusEffectName { get { return statusEffectName; } }
    public string Description { get { return description; } }
    public string ShortHand { get { return shortHand; } }
    public Color DisplayColor { get { return displayColor; } }
}