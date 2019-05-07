using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCloseUp : MonoBehaviour
{
    public Image polaroideImage;
    public TextMeshProUGUI levelNameCU;
    public TextMeshProUGUI levelDescriptionCU;
    public TextMeshProUGUI clueNameCU;
    public TextMeshProUGUI clueDescriptionCU;

    public static LevelCloseUp Instance;

    private void Awake()
    {
        Instance = this;
    }
}
