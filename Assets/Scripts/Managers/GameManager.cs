using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode { PuzzleMode, InvestigationMode, InteractingMode, CinematicMode, MenuMode }

    [Header("Mode de jeu")]
    public GameMode currentGameMode;


    public static GameManager Instance;



    private void Awake()
    {
        Instance = this;
    }
}
