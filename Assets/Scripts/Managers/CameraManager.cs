using Unity.Cinemachine;
using UnityEngine;
using static GameManager;

public class CameraManager : MonoBehaviour
{
    private static CinemachineCamera activeCinemachine;

    private void OnEnable()
    {
        GameManager.OnFinishedGameManagerInitialisation += OnFinishedGameManagerInitialisation;
    }

    private void OnDisable()
    {
        GameManager.OnFinishedGameManagerInitialisation -= OnFinishedGameManagerInitialisation;
    }

    private void OnFinishedGameManagerInitialisation()
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
