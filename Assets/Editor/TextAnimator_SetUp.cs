using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextMeshAnimator))]
public class TextMeshAnimatorEditor : Editor
{
    SerializedProperty useCustomText;
    SerializedProperty customText;


    SerializedProperty shakeAmount;
    SerializedProperty shakeIndependency;


    SerializedProperty waveAmount;
    SerializedProperty waveSpeed;
    SerializedProperty waveSeparation;
    SerializedProperty waveIndependency;


    SerializedProperty drunkAmount;
    SerializedProperty drunkSpeed;
    SerializedProperty drunkSeparation;
    SerializedProperty drunkIndependency;

    SerializedProperty excitedAmount;
    SerializedProperty excitedSpeed;
    SerializedProperty excitedSeparation;
    SerializedProperty excitedIndependency;

    SerializedProperty impatientAmount;
    SerializedProperty impatientSpeed;
    SerializedProperty impatientSeparation;
    SerializedProperty impatientIndependency;

    SerializedProperty sickAmount;
    SerializedProperty sickSpeed;
    SerializedProperty sickSeparation;
    SerializedProperty sickIndependency;

    SerializedProperty happyAmount;
    SerializedProperty happySpeed;
    SerializedProperty happySeparation;
    SerializedProperty happyIndependency;

    SerializedProperty sadAmount;
    SerializedProperty sadSpeed;
    SerializedProperty sadSeparation;
    SerializedProperty sadIndependency;

    SerializedProperty wiggleAmount;
    SerializedProperty wiggleSpeed;
    SerializedProperty wiggleMinimumDuration;
    SerializedProperty wiggleIndependency;


    SerializedProperty clumsyAmount;
    SerializedProperty clumsySpeed;
    SerializedProperty clumsyMinimumDuration;
    SerializedProperty clumsyIndependency;

    SerializedProperty playAmount;
    SerializedProperty playSpeed;
    SerializedProperty playMinimumDuration;
    SerializedProperty playIndependency;

    SerializedProperty horrorAmount;
    SerializedProperty horrorSpeed;
    SerializedProperty horrorMinimumDuration;
    SerializedProperty horrorIndependency;

    SerializedProperty charsVisible;


    void OnEnable()
    {
        useCustomText = serializedObject.FindProperty("useCustomText");
        customText = serializedObject.FindProperty("customText");

        shakeAmount = serializedObject.FindProperty("shakeAmount");
        shakeIndependency = serializedObject.FindProperty("shakeIndependency");

        waveAmount = serializedObject.FindProperty("waveAmount");
        waveSpeed = serializedObject.FindProperty("waveSpeed");
        waveSeparation = serializedObject.FindProperty("waveSeparation");
        waveIndependency = serializedObject.FindProperty("waveIndependency");

        drunkAmount = serializedObject.FindProperty("drunkAmount");
        drunkSpeed = serializedObject.FindProperty("drunkSpeed");
        drunkSeparation = serializedObject.FindProperty("drunkSeparation");
        drunkIndependency = serializedObject.FindProperty("drunkIndependency");

        excitedAmount = serializedObject.FindProperty("excitedAmount");
        excitedSpeed = serializedObject.FindProperty("excitedSpeed");
        excitedSeparation = serializedObject.FindProperty("excitedSeparation");
        excitedIndependency = serializedObject.FindProperty("excitedIndependency");

        impatientAmount = serializedObject.FindProperty("impatientAmount");
        impatientSpeed = serializedObject.FindProperty("impatientSpeed");
        impatientSeparation = serializedObject.FindProperty("impatientSeparation");
        impatientIndependency = serializedObject.FindProperty("impatientIndependency");

        sickAmount = serializedObject.FindProperty("sickAmount");
        sickSpeed = serializedObject.FindProperty("sickSpeed");
        sickSeparation = serializedObject.FindProperty("sickSeparation");
        sickIndependency = serializedObject.FindProperty("sickIndependency");

        happyAmount = serializedObject.FindProperty("happyAmount");
        happySpeed = serializedObject.FindProperty("happySpeed");
        happySeparation = serializedObject.FindProperty("happySeparation");
        happyIndependency = serializedObject.FindProperty("happyIndependency");

        sadAmount = serializedObject.FindProperty("sadAmount");
        sadSpeed = serializedObject.FindProperty("sadSpeed");
        sadSeparation = serializedObject.FindProperty("sadSeparation");
        sadIndependency = serializedObject.FindProperty("sadIndependency");

        wiggleAmount = serializedObject.FindProperty("wiggleAmount");
        wiggleSpeed = serializedObject.FindProperty("wiggleSpeed");
        wiggleMinimumDuration = serializedObject.FindProperty("wiggleMinimumDuration");
        wiggleIndependency = serializedObject.FindProperty("wiggleIndependency");

        playAmount = serializedObject.FindProperty("playAmount");
        playSpeed = serializedObject.FindProperty("playSpeed");
        playMinimumDuration = serializedObject.FindProperty("playMinimumDuration");
        playIndependency = serializedObject.FindProperty("playIndependency");

        clumsyAmount = serializedObject.FindProperty("clumsyAmount");
        clumsySpeed = serializedObject.FindProperty("clumsySpeed");
        clumsyMinimumDuration = serializedObject.FindProperty("clumsyMinimumDuration");
        clumsyIndependency = serializedObject.FindProperty("clumsyIndependency");

        horrorAmount = serializedObject.FindProperty("horrorAmount");
        horrorSpeed = serializedObject.FindProperty("horrorSpeed");
        horrorMinimumDuration = serializedObject.FindProperty("horrorMinimumDuration");
        horrorIndependency = serializedObject.FindProperty("horrorIndependency");

        charsVisible = serializedObject.FindProperty("charsVisible");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (useCustomText.boolValue = EditorGUILayout.Toggle("Custom Text", useCustomText.boolValue))
        {
            customText.stringValue = EditorGUILayout.TextArea(customText.stringValue, GUILayout.Height(96));

        }
        if (GUILayout.Button("Update"))
        {
            TextMeshAnimator script = (TextMeshAnimator)target;
            script.UpdateText();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Text Visibility Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(charsVisible);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Shake Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(shakeAmount);
        EditorGUILayout.PropertyField(shakeIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Panic Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(waveAmount);
        EditorGUILayout.PropertyField(waveSpeed);
        EditorGUILayout.PropertyField(waveSeparation);
        EditorGUILayout.PropertyField(waveIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Drunk Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(drunkAmount);
        EditorGUILayout.PropertyField(drunkSpeed);
        EditorGUILayout.PropertyField(drunkSeparation);
        EditorGUILayout.PropertyField(drunkIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Excited Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(excitedAmount);
        EditorGUILayout.PropertyField(excitedSpeed);
        EditorGUILayout.PropertyField(excitedSeparation);
        EditorGUILayout.PropertyField(excitedIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Impatient Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(impatientAmount);
        EditorGUILayout.PropertyField(impatientSpeed);
        EditorGUILayout.PropertyField(impatientSeparation);
        EditorGUILayout.PropertyField(impatientIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sick Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sickAmount);
        EditorGUILayout.PropertyField(sickSpeed);
        EditorGUILayout.PropertyField(sickSeparation);
        EditorGUILayout.PropertyField(sickIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Happy Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(happyAmount);
        EditorGUILayout.PropertyField(happySpeed);
        EditorGUILayout.PropertyField(happySeparation);
        EditorGUILayout.PropertyField(happyIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sad Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sadAmount);
        EditorGUILayout.PropertyField(sadSpeed);
        EditorGUILayout.PropertyField(sadSeparation);
        EditorGUILayout.PropertyField(sadIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Surprised Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(wiggleAmount);
        EditorGUILayout.PropertyField(wiggleSpeed);
        wiggleMinimumDuration.floatValue = EditorGUILayout.Slider("Surprised Minimum Duration", wiggleMinimumDuration.floatValue, 0.0f, 1.0f);
        EditorGUILayout.PropertyField(wiggleIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Play Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playAmount);
        EditorGUILayout.PropertyField(playSpeed);
        playMinimumDuration.floatValue = EditorGUILayout.Slider("Play Minimum Duration", playMinimumDuration.floatValue, 0.0f, 1.0f);
        EditorGUILayout.PropertyField(playIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Clumsy Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(clumsyAmount);
        EditorGUILayout.PropertyField(clumsySpeed);
        clumsyMinimumDuration.floatValue = EditorGUILayout.Slider("Clumsy Minimum Duration", clumsyMinimumDuration.floatValue, 0.0f, 1.0f);
        EditorGUILayout.PropertyField(clumsyIndependency);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Horror Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(horrorAmount);
        EditorGUILayout.PropertyField(horrorSpeed);
        horrorMinimumDuration.floatValue = EditorGUILayout.Slider("Horror Minimum Duration", horrorMinimumDuration.floatValue, 0.0f, 1.0f);
        EditorGUILayout.PropertyField(horrorIndependency);

        serializedObject.ApplyModifiedProperties();
    }
}

