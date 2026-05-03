using System;
using UnityEngine;

public class KorrahCombatAnimationsManager: CombatAnimationsManager
{
    [SerializeField] private GameObject waterBallPrefab;

    public void WaterGun(Vector3 target)
    {
        PauseAnimation();
        GameObject waterball = Instantiate(waterBallPrefab, gameObject.transform);
        Waterball waterballScript = waterball.GetComponent<Waterball>();
        waterballScript.target = target;
        waterballScript.callback = () => ResumeAnimation();
    }
}