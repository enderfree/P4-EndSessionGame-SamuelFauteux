using UnityEngine;
using System.Collections.Generic;

public class CharManager : MonoBehaviour
{
    [SerializeField] private Korrah korrah;
    [SerializeField] private Zeolia zeolia;

    public static Dictionary<CharNames, Character> chars = new Dictionary<CharNames, Character>();

    // Unity 
    private void Awake()
    {
        chars.Add(korrah.CharName, korrah);
        chars.Add(zeolia.CharName, zeolia);
    }
}
