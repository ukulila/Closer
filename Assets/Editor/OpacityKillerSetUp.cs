using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(OpacityKiller))]
public class OpacityKillerSetUp : Editor
{
    public override void OnInspectorGUI()
    {
        OpacityKiller ok = (OpacityKiller)target;


        if(GUILayout.Button("Set References"))
        {
          //  ok.KillChilds.Clear();
           // ok.myMaterial.Clear();

        //    ok.OpacitySetup();

            if (GUI.changed)
                EditorUtility.SetDirty(ok);
        }


        base.OnInspectorGUI();
    }
}
