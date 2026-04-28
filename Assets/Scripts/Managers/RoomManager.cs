using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    // These are very useful info about rooms that need to be accessed plenty, 
    // but clutter plenty when I give them to managers so I made it its own

    // Rooms
    [SerializeField] private Room centralPlace;
    [SerializeField] private Room misc;

    public static Dictionary<RoomNames, Room> rooms = new Dictionary<RoomNames, Room>();

    private void Awake()
    {
        rooms.Add(centralPlace.RoomName, centralPlace);
        rooms.Add(misc.RoomName, misc);
    }
}
