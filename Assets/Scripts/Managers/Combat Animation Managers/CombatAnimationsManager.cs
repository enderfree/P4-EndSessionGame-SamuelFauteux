using UnityEngine;
using System;

public class CombatAnimationsManager : MonoBehaviour
{
    // File path mentionned assume you are in the Scripts folder
    // Walking animations are handled in Characters\OverworldLogic
    // This class or one of its child is attached in every combat prefabs (those are in ..\Characters)
    // Character moves call TriggerAnimator from this class and provide the animator of the
    // impacted character, the trigger (all valid triggers accross animation controllers are in
    // this class' enum) and a callback method if they want to do something at a specific point
    // of the animation... usually at the end since my animations are so short. 
    // The animation event calls one of the other methods of this class. 
    // Their goal is mostly to call the callback, but I made them separated to be able to add
    // extra logic. A goal I had, but ran out of time to perform was to override Cast for Korrah
    // In order to add SFX when she casts a spell.

    public enum Triggers
    {
        Cast, 
        Death, 
        Dodge,
        Hurt, 
        Stab
    }

    protected Action pendingMove;

    public virtual void TriggerAnimator(Animator animator, Triggers trigger, Action callback = null)
    {
        pendingMove = callback;
        animator.SetTrigger(trigger.ToString());
    }

    public virtual void Cast()
    {
        if (pendingMove != null)
        {
            pendingMove();
        }
    }

    public virtual void Stab()
    {
        if (pendingMove != null)
        {
            pendingMove();
        }
    }
}
