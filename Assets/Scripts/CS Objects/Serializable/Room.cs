using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Room
{
    [Header("General")]
    [SerializeField] private RoomNames roomName;
    [SerializeField] private GameObject room; // object that holds the room and its content

    [Header("Overworld")]
    [SerializeField] private List<GameObject> chars; // chars always in the room

    [Header("Combat")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform playerPos;
    [SerializeField] private List<Transform> enemyPos; // idealy I want 5, but arrays aren't serializablw...

    public RoomNames RoomName { get { return roomName; } }

    public GameObject GameRoom { get { return room; } }

    public List<GameObject> Chars { get { return chars; } }

    public Transform CameraTarget { get { return cameraTarget; } }

    public Transform PlayerPos { get { return playerPos; } }

    public List<Transform> EnemyPos { get {return enemyPos; } }
}