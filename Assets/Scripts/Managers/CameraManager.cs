using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CinemachineCamera activeCinemachine;
    
    /// <summary>
    /// Is called a single time by the GameManager. Start was too soon...
    /// </summary>
    public static void Initialization()
    {
        foreach (Room room in RoomManager.rooms.Values)
        {
            if (room.OverworldCamera != null)
            {
                room.OverworldCamera.enabled = false;
            }

            if (room.CombatCamera != null)
            {
                room.CombatCamera.enabled = false;
            }
        }

        activeCinemachine = GameManager.CurrentRoom.OverworldCamera;
        activeCinemachine.enabled = true;
    }

    public static CinemachineCamera ActiveCinemachine 
    { 
        get 
        { 
            return activeCinemachine; 
        } 
        set
        {
            activeCinemachine.enabled = false;
            activeCinemachine = value;
            activeCinemachine.enabled = true;
        }
    }
}
