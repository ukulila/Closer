using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CellMovement))]
public class CellMovementSetUp : Editor
{
    public override void OnInspectorGUI()
    {
        CellMovement cellMovement = (CellMovement)target;


        if (GUILayout.Button("Set References"))
        {
            cellMovement.brothers.Clear();
            cellMovement.PlaneMtlIsSpawn.Clear();
            cellMovement.PositionsDebug.Clear();

            cellMovement.PositionsDebug.Add(new Vector3(-3.05f, -3.05f, -3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(3.05f, -3.05f, -3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(3.05f, -3.05f, 3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(-3.05f, -3.05f, 3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(-3.05f, 3.05f, 3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(3.05f, 3.05f, 3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(3.05f, 3.05f, -3.05f));
            cellMovement.PositionsDebug.Add(new Vector3(-3.05f, 3.05f, -3.05f));


            cellMovement.InitSetup();

            if (GUI.changed)
                EditorUtility.SetDirty(cellMovement);
        }


        base.OnInspectorGUI();
    }
}
