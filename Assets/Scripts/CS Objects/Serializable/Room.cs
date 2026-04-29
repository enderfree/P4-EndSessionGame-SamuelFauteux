using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

[System.Serializable]
public class Room
{
    [Header("General")]
    [SerializeField] private RoomNames roomName;
    [SerializeField] private GameObject room; // object that holds the room and its content

    [Header("Overworld")]
    [SerializeField] private List<GameObject> chars; // chars always in the room
    [SerializeField] private CinemachineCamera overworldCamera;

    [Header("Combat")]
    [SerializeField] private CinemachineCamera combatCamera;
    [SerializeField] private Transform playerPos;
    [SerializeField] private List<Transform> enemyPos; // idealy I want 5, but arrays aren't serializablw...

    public RoomNames RoomName { get { return roomName; } }

    public GameObject GameRoom { get { return room; } }

    public List<GameObject> Chars { get { return chars; } }

    public CinemachineCamera OverworldCamera { get { return overworldCamera; } }

    public CinemachineCamera CombatCamera { get { return combatCamera; } }

    public Transform PlayerPos { get { return playerPos; } }

    public List<Transform> EnemyPos { get {return enemyPos; } }
}