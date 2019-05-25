using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrEnvironment : MonoBehaviour
{
    [Header("   ***** Values : Need Assignment [See Tooltip] *****")]
    [Space(5)]
    [Tooltip("La position de base à laquelle le joueur se trouve dans la cell")]
    public Transform basePos;
    [Space(5)]

    [Tooltip("Chaque liste correspond à un chemin (déplacement, obj, pnj) cf. Docu Drive")]
    public WayPointList paths;
    [Space(5)]
    [Tooltip("Toutes les portes doivent être reférencées ici")]
    public List<Transform> doorWayPoints;
    [Space(5)]
    [Tooltip("Toutes les trappes doivent être reférencées ici")]
    public List<Transform> HatchesWayPoints;
    [Space(5)]
    [Header("   HELP : Documentation en ligne")]
    [Tooltip("Copy-Paste me")]
    public string DriveLinkToDocumentation = "https://docs.google.com/document/d/1xnNaKkt93k7VyAbdshTUKTLybfpX1DaPGwhrvcgqN54/edit";

    [Space(10)]

    public bool ContainsTrap;

}
