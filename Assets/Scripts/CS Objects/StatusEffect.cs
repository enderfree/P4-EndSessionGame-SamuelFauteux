using UnityEngine;

public class StatusEffect
{
    public static float shadowShroudDmgMultiplier = 1.2f;

    private StatusEffectNames statusEffectName;
    private string description;
    private string shortHand;
    private Color displayColor;
    private int duration; // in rounds
    // maybe add a visual effect if I have the time to complete the 3d assignment before the deadline of this project

    public StatusEffect(StatusEffectNames statusEffectName, string description, string shortHand, Color displayColor, int duration)
    {
        this.statusEffectName = statusEffectName;
        this.description = description;
        this.shortHand = shortHand;
        this.displayColor = displayColor;
        this.duration = duration;
    }

    public StatusEffectNames StatusEffectName { get { return statusEffectName; } }
    public string Description { get { return description; } }
    public string ShortHand { get { return shortHand; } }
    public Color DisplayColor { get { return displayColor; } }
    public int Duration { get { return duration; } set { duration = value; } }
}