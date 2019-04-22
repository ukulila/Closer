using UnityEngine;

public class ROOM_Manager : MonoBehaviour
{
    public RoomInteraction currentRoom;
    public bool isInteracting = false;


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
        if (currentRoom != null)
        {
            currentRoom.InteractionAppears();
            isInteracting = true;
        }
    }

    /// <summary>
    /// Enleve l'UI
    /// </summary>
    public void DeactivateUI()
    {
        if (currentRoom != null)
            currentRoom.DisableUI();
    }
}
