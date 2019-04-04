using UnityEngine;

public class ROOM_Manager : MonoBehaviour
{
    public RoomInteraction currentRoom;


    public static ROOM_Manager Instance;



    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Invoque l'UI selon les presets de la Room
    /// </summary>
    public void LaunchUI()
    {
        currentRoom.InteractionAppears();
    }

    /// <summary>
    /// Enleve l'UI
    /// </summary>
    public void DeactivateUI()
    {
        currentRoom.DisableUI();
    }
}
