using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
public enum TextMeshAnimator_IndependencyType
{
    United,
    Word,
    Character,
    Vertex
}
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshAnimator : MonoBehaviour
{

    public int currentFrame = 0;
    public bool useCustomText = false;
    public string customText;
    public string text
    {
        get { return customText; }
        set
        {
            customText = value;
            //UpdateText(value);
            if (useCustomText)
            {
                TMProGUI.text = ParseText(value);
                //charsVisible = 0;
                SyncToTextMesh();
            }
        }
    }

    public int totalChars
    {
        get
        {
            return TMProGUI.textInfo.characterCount;
        }
    }
    //MODIFIER VARIABLES

    //SHAKE
    public float shakeAmount = 1;
    public TextMeshAnimator_IndependencyType shakeIndependency = TextMeshAnimator_IndependencyType.Character;

    //PANIC
    public float waveAmount = 1;
    public float waveSpeed = 1;
    public float waveSeparation = 1;
    public TextMeshAnimator_IndependencyType waveIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //DRUNK
    public float drunkAmount = 1;
    public float drunkSpeed = 1;
    public float drunkSeparation = 1;
    public TextMeshAnimator_IndependencyType drunkIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //EXCITED
    public float excitedAmount = 1;
    public float excitedSpeed = 1;
    public float excitedSeparation = 1;
    public TextMeshAnimator_IndependencyType excitedIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //IMPATIENT
    public float impatientAmount = 1;
    public float impatientSpeed = 1;
    public float impatientSeparation = 1;
    public TextMeshAnimator_IndependencyType impatientIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //SICK
    public float sickAmount = 1;
    public float sickSpeed = 1;
    public float sickSeparation = 1;
    public TextMeshAnimator_IndependencyType sickIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //HAPPY
    public float happyAmount = 1;
    public float happySpeed = 1;
    public float happySeparation = 1;
    public TextMeshAnimator_IndependencyType happyIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //SAD
    public float sadAmount = 1;
    public float sadSpeed = 1;
    public float sadSeparation = 1;
    public TextMeshAnimator_IndependencyType sadIndependency = TextMeshAnimator_IndependencyType.Vertex;

    //SURPRISED
    public float wiggleAmount = 1;
    public float wiggleSpeed = 0.125f;
    public float wiggleMinimumDuration = 0.5f;
    public TextMeshAnimator_IndependencyType wiggleIndependency = TextMeshAnimator_IndependencyType.Character;

    //PLAY
    public float playAmount = 1;
    public float playSpeed = 0.125f;
    public float playMinimumDuration = 0.5f;
    public TextMeshAnimator_IndependencyType playIndependency = TextMeshAnimator_IndependencyType.Character;

    //CLUMSY
    public float clumsyAmount = 1;
    public float clumsySpeed = 0.125f;
    public float clumsyMinimumDuration = 0f;
    public TextMeshAnimator_IndependencyType clumsyIndependency = TextMeshAnimator_IndependencyType.Character;

    //HORROR
    public float horrorAmount = 1;
    public float horrorSpeed = 0.125f;
    public float horrorMinimumDuration = 0.5f;
    public TextMeshAnimator_IndependencyType horrorIndependency = TextMeshAnimator_IndependencyType.Character;

    private TextMeshProUGUI TMProGUI;
    // Use this for initialization

    private Vector3[][] vertex_Base; // The base vertex array the animator will animate from.

    //PRIVATE TEXT CACHE VARIABLES

    //SHAKE
    private bool[] shakesEnabled;
    private float[] shakeVelocities;
    private TextMeshAnimator_IndependencyType[] shakeIndependencies;

    //PANIC
    private bool[] wavesEnabled;
    private float[] waveVelocities;
    private float[] waveSpeeds;
    private float[] waveSeparations;
    private TextMeshAnimator_IndependencyType[] waveIndependencies;

    //DRUNK
    private bool[] drunkEnabled;
    private float[] drunkVelocities;
    private float[] drunkSpeeds;
    private float[] drunkSeparations;
    private TextMeshAnimator_IndependencyType[] drunkIndependencies;

    //EXCITED
    private bool[] excitedEnabled;
    private float[] excitedVelocities;
    private float[] excitedSpeeds;
    private float[] excitedSeparations;
    private TextMeshAnimator_IndependencyType[] excitedIndependencies;

    //IMPATIENT
    private bool[] impatientEnabled;
    private float[] impatientVelocities;
    private float[] impatientSpeeds;
    private float[] impatientSeparations;
    private TextMeshAnimator_IndependencyType[] impatientIndependencies;

    //SICK
    private bool[] sickEnabled;
    private float[] sickVelocities;
    private float[] sickSpeeds;
    private float[] sickSeparations;
    private TextMeshAnimator_IndependencyType[] sickIndependencies;

    //HAPPY
    private bool[] happyEnabled;
    private float[] happyVelocities;
    private float[] happySpeeds;
    private float[] happySeparations;
    private TextMeshAnimator_IndependencyType[] happyIndependencies;

    //SAD
    private bool[] sadEnabled;
    private float[] sadVelocities;
    private float[] sadSpeeds;
    private float[] sadSeparations;
    private TextMeshAnimator_IndependencyType[] sadIndependencies;

    //SURPRISED
    private bool[] wigglesEnabled;
    private float[] wiggleVelocities;
    private float[] wiggleSpeeds;
    private float[] wigglePrevPos;
    private float[] wiggleTargetPos;
    private float[] wiggleTimeCurrent;
    private float[] wiggleTimeTotal;
    private TextMeshAnimator_IndependencyType[] wiggleIndependencies;

    //PLAY
    private bool[] playEnabled;
    private float[] playVelocities;
    private float[] playSpeeds;
    private float[] playPrevPos;
    private float[] playTargetPos;
    private float[] playTimeCurrent;
    private float[] playTimeTotal;
    private TextMeshAnimator_IndependencyType[] playIndependencies;

    //CLUMSY
    private bool[] clumsyEnabled;
    private float[] clumsyVelocities;
    private float[] clumsySpeeds;
    private float[] clumsyPrevPos;
    private float[] clumsyTargetPos;
    private float[] clumsyTimeCurrent;
    private float[] clumsyTimeTotal;
    private TextMeshAnimator_IndependencyType[] clumsyIndependencies;

    //HORROR
    private bool[] horrorEnabled;
    private float[] horrorVelocities;
    private float[] horrorSpeeds;
    private float[] horrorPrevPos;
    private float[] horrorTargetPos;
    private float[] horrorTimeCurrent;
    private float[] horrorTimeTotal;
    private TextMeshAnimator_IndependencyType[] horrorIndependencies;


    Vector3 sv = new Vector3(); //SHAKE

    Vector3 wv = new Vector3(); //PANIC

    Vector3 dv = new Vector3(); //DRUNK

    Vector3 ev = new Vector3(); //EXCITED

    Vector3 iv = new Vector3(); //IMPATIENT

    Vector3 siv = new Vector3(); //SICK

    Vector3 hv = new Vector3(); //HAPPY

    Vector3 sav = new Vector3(); //SAD

    Vector3 wgv = new Vector3(); //SURPRISED

    Vector3 pv = new Vector3(); //PLAY

    Vector3 cv = new Vector3(); //CLUMSY

    Vector3 hov = new Vector3(); //HORROR

    //SHOW TEXT
    [SerializeField]
    public int charsVisible;

    [SerializeField]
    public int[] scrollSpeeds;


    public struct TextSpeedItem
    {
        public int speed;
        public int index;
    }

    public void SyncToTextMesh()
    {
        TMProGUI.ForceMeshUpdate();
        vertex_Base = new Vector3[TMProGUI.textInfo.meshInfo.Length][];
        int biggest_num_verts = 0;
        for (int i = 0; i < TMProGUI.textInfo.meshInfo.Length; ++i)
        {
            vertex_Base[i] = new Vector3[TMProGUI.textInfo.meshInfo[i].vertices.Length];
            if (biggest_num_verts < vertex_Base[i].Length) biggest_num_verts = vertex_Base[i].Length;
            System.Array.Copy(TMProGUI.textInfo.meshInfo[i].vertices, vertex_Base[i], TMProGUI.textInfo.meshInfo[i].vertices.Length);
        }

        wigglePrevPos = new float[biggest_num_verts * 2];
        wiggleTargetPos = new float[biggest_num_verts * 2];
        wiggleTimeCurrent = new float[biggest_num_verts * 2];
        wiggleTimeTotal = new float[biggest_num_verts * 2];

        playPrevPos = new float[biggest_num_verts * 2];
        playTargetPos = new float[biggest_num_verts * 2];
        playTimeCurrent = new float[biggest_num_verts * 2];
        playTimeTotal = new float[biggest_num_verts * 2];

        clumsyPrevPos = new float[biggest_num_verts * 2];
        clumsyTargetPos = new float[biggest_num_verts * 2];
        clumsyTimeCurrent = new float[biggest_num_verts * 2];
        clumsyTimeTotal = new float[biggest_num_verts * 2];

        horrorPrevPos = new float[biggest_num_verts * 2];
        horrorTargetPos = new float[biggest_num_verts * 2];
        horrorTimeCurrent = new float[biggest_num_verts * 2];
        horrorTimeTotal = new float[biggest_num_verts * 2];

        TMProGUI.ForceMeshUpdate();
    }

    public void UpdateText(string text = null)
    {
        //charsVisible = 0;
        if (TMProGUI == null) TMProGUI = gameObject.GetComponent<TextMeshProUGUI>();
        if (useCustomText)
        {
            if (text == null) this.text = this.text;
            else this.text = text;
        }
        else
        {
            TMProGUI.text = ParseText(TMProGUI.text); SyncToTextMesh();
        }
    }

    string ParseText(string inputText)
    {

        //SHAKE
        List<bool> shakesEnabled = new List<bool>();
        List<float> shakeVelocities = new List<float>();
        List<TextMeshAnimator_IndependencyType> shakeIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool shaking = false;
        float shakeAmount = 1;
        TextMeshAnimator_IndependencyType shakeIndependency = this.shakeIndependency;

        //PANIC
        List<bool> wavesEnabled = new List<bool>();
        List<float> waveVelocities = new List<float>();
        List<float> waveSpeeds = new List<float>();
        List<float> waveSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> waveIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool waving = false;
        float waveAmount = 1;
        float waveSpeed = 1;
        float waveSeparation = 1;
        TextMeshAnimator_IndependencyType waveIndependency = this.waveIndependency;

        //DRUNK
        List<bool> drunkEnabled = new List<bool>();
        List<float> drunkVelocities = new List<float>();
        List<float> drunkSpeeds = new List<float>();
        List<float> drunkSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> drunkIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool drinking = false;
        float drunkAmount = 1;
        float drunkSpeed = 1;
        float drunkSeparation = 1;
        TextMeshAnimator_IndependencyType drunkIndependency = this.drunkIndependency;

        //EXCITED
        List<bool> excitedEnabled = new List<bool>();
        List<float> excitedVelocities = new List<float>();
        List<float> excitedSpeeds = new List<float>();
        List<float> excitedSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> excitedIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool exciting = false;
        float excitedAmount = 1;
        float excitedSpeed = 1;
        float excitedSeparation = 1;
        TextMeshAnimator_IndependencyType excitedIndependency = this.excitedIndependency;

        //IMPATIENT
        List<bool> impatientEnabled = new List<bool>();
        List<float> impatientVelocities = new List<float>();
        List<float> impatientSpeeds = new List<float>();
        List<float> impatientSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> impatientIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool impatient = false;
        float impatientAmount = 1;
        float impatientSpeed = 1;
        float impatientSeparation = 1;
        TextMeshAnimator_IndependencyType impatientIndependency = this.impatientIndependency;

        //SICK
        List<bool> sickEnabled = new List<bool>();
        List<float> sickVelocities = new List<float>();
        List<float> sickSpeeds = new List<float>();
        List<float> sickSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> sickIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool sicked = false;
        float sickAmount = 1;
        float sickSpeed = 1;
        float sickSeparation = 1;
        TextMeshAnimator_IndependencyType sickIndependency = this.sickIndependency;

        //HAPPY
        List<bool> happyEnabled = new List<bool>();
        List<float> happyVelocities = new List<float>();
        List<float> happySpeeds = new List<float>();
        List<float> happySeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> happyIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool joy = false;
        float happyAmount = 1;
        float happySpeed = 1;
        float happySeparation = 1;
        TextMeshAnimator_IndependencyType happyIndependency = this.happyIndependency;

        //SAD
        List<bool> sadEnabled = new List<bool>();
        List<float> sadVelocities = new List<float>();
        List<float> sadSpeeds = new List<float>();
        List<float> sadSeparations = new List<float>();
        List<TextMeshAnimator_IndependencyType> sadIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool crying = false;
        float sadAmount = 1;
        float sadSpeed = 1;
        float sadSeparation = 1;
        TextMeshAnimator_IndependencyType sadIndependency = this.sadIndependency;

        //SUPRISED
        List<bool> wigglesEnabled = new List<bool>();
        List<float> wiggleVelocities = new List<float>();
        List<float> wiggleSpeeds = new List<float>();
        List<TextMeshAnimator_IndependencyType> wiggleIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool wiggling = false;
        float wiggleAmount = 1;
        float wiggleSpeed = 1;
        TextMeshAnimator_IndependencyType wiggleIndependency = this.wiggleIndependency;

        //PLAY
        List<bool> playEnabled = new List<bool>();
        List<float> playVelocities = new List<float>();
        List<float> playSpeeds = new List<float>();
        List<TextMeshAnimator_IndependencyType> playIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool playing = false;
        float playAmount = 1;
        float playSpeed = 1;
        TextMeshAnimator_IndependencyType playIndependency = this.playIndependency;

        //CLUMSY
        List<bool> clumsyEnabled = new List<bool>();
        List<float> clumsyVelocities = new List<float>();
        List<float> clumsySpeeds = new List<float>();
        List<TextMeshAnimator_IndependencyType> clumsyIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool clumy = false;
        float clumsyAmount = 1;
        float clumsySpeed = 1;
        TextMeshAnimator_IndependencyType clumsyIndependency = this.clumsyIndependency;

        //HORROR
        List<bool> horrorEnabled = new List<bool>();
        List<float> horrorVelocities = new List<float>();
        List<float> horrorSpeeds = new List<float>();
        List<TextMeshAnimator_IndependencyType> horrorIndependencies = new List<TextMeshAnimator_IndependencyType>();
        bool horror = false;
        float horrorAmount = 1;
        float horrorSpeed = 1;
        TextMeshAnimator_IndependencyType horrorIndependency = this.horrorIndependency;


        // SCROLL SPEED
        List<int> scrollSpeeds = new List<int>();
        int currentScrollSpeed = 2;


        string outputText = "";
        for (int index = 0; index < inputText.Length; ++index)
        {
            if (inputText[index] == '<')
            {
                int startTagIndex = index;
                while (index < inputText.Length)
                {
                    if (inputText[index++] == '>')
                    {
                        string tag = inputText.Substring(startTagIndex, index - startTagIndex);
                        Debug.Log(tag);
                        if (tag.ToUpper().Contains("COLOR") || tag.ToUpper().Contains("SIZE"))
                        {
                            Debug.Log("THE TAG IS " + tag);
                            outputText += tag;


                        }
                        //SHAKE

                        else if (tag.ToUpper().Contains("/SHAKE"))
                        {
                            shaking = false;
                            shakeAmount = 1;
                        }
                        else if (tag.ToUpper().Contains("SHAKE"))
                        {
                            shaking = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out shakeAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for shake amount.", amount_string));
                                }
                            }
                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                shakeIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                shakeIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                shakeIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                shakeIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //PANIC

                        else if (tag.ToUpper().Contains("/PANIC"))
                        {
                            waving = false;
                            waveAmount = 1;
                            waveSpeed = 1;
                            waveSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("PANIC"))
                        {
                            waving = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out waveAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out waveSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out waveSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                waveIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                waveIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                waveIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                waveIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }


                        //DRUNK

                        else if (tag.ToUpper().Contains("/DRUNK"))
                        {
                            drinking = false;
                            drunkAmount = 1;
                            drunkSpeed = 1;
                            drunkSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("DRUNK"))
                        {
                            drinking = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out drunkAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wavev amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out drunkSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wavev speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out drunkSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wavev separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                drunkIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                drunkIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                drunkIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                drunkIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //EXCITED

                        else if (tag.ToUpper().Contains("/EXCITED"))
                        {
                            exciting = false;
                            excitedAmount = 1;
                            excitedSpeed = 1;
                            excitedSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("EXCITED"))
                        {
                            exciting = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out excitedAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out excitedSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out excitedSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                excitedIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                excitedIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                excitedIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                excitedIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //IMPATIENT

                        else if (tag.ToUpper().Contains("/YEAH"))
                        {
                            impatient = false;
                            impatientAmount = 1;
                            impatientSpeed = 1;
                            impatientSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("YEAH"))
                        {
                            impatient = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out impatientAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out impatientSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out impatientSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //SICK

                        else if (tag.ToUpper().Contains("/SICK"))
                        {
                            sicked = false;
                            sickAmount = 1;
                            sickSpeed = 1;
                            sickSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("SICK"))
                        {
                            sicked = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out sickAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out sickSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out sickSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                sickIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                sickIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                sickIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                sickIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //HAPPY

                        else if (tag.ToUpper().Contains("/HAPPY"))
                        {
                            joy = false;
                            happyAmount = 1;
                            happySpeed = 1;
                            happySeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("HAPPY"))
                        {
                            joy = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out happyAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out happySpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out happySeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                happyIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //SAD

                        else if (tag.ToUpper().Contains("/SAD"))
                        {
                            crying = false;
                            sadAmount = 1;
                            sadSpeed = 1;
                            sadSeparation = 1;
                        }
                        else if (tag.ToUpper().Contains("SAD"))
                        {
                            crying = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out sadAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out sadSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", speed_string));
                                }
                            }

                            //SEPARATION

                            string separationLabel = "SEPARATION=";
                            if (tag.ToUpper().Contains(separationLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(separationLabel) + separationLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string separation_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(separation_string, out sadSeparation))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", separation_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                sadIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                sadIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                sadIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                sadIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }



                        //SURPRISED

                        else if (tag.ToUpper().Contains("/SURPRISED"))
                        {
                            wiggling = false;
                            wiggleAmount = 1;
                            wiggleSpeed = 1;
                        }
                        else if (tag.ToUpper().Contains("SURPRISED"))
                        {
                            wiggling = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out wiggleAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out wiggleSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle speed.", speed_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                wiggleIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                wiggleIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                wiggleIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                wiggleIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //PLAY

                        else if (tag.ToUpper().Contains("/PLAY"))
                        {
                            playing = false;
                            playAmount = 1;
                            playSpeed = 1;
                        }
                        else if (tag.ToUpper().Contains("PLAY"))
                        {
                            playing = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out playAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out playSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle speed.", speed_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                playIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                playIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                playIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                playIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //CLUMSY

                        else if (tag.ToUpper().Contains("/CLUMSY"))
                        {
                            clumy = false;
                            clumsyAmount = 1;
                            clumsySpeed = 1;
                        }
                        else if (tag.ToUpper().Contains("CLUMSY"))
                        {
                            clumy = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out clumsyAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out clumsySpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle speed.", speed_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                clumsyIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                clumsyIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                clumsyIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                clumsyIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        //HORROR

                        else if (tag.ToUpper().Contains("/HORROR"))
                        {
                            horror = false;
                            horrorAmount = 1;
                            horrorSpeed = 1;
                        }
                        else if (tag.ToUpper().Contains("HORROR"))
                        {
                            horror = true;

                            //INTENSITY

                            string amountLabel = "INTENSITY=";
                            if (tag.ToUpper().Contains(amountLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(amountLabel) + amountLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string amount_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(amount_string, out horrorAmount))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle amount.", amount_string));
                                }
                            }

                            //SPEED

                            string speedLabel = "SPEED=";
                            if (tag.ToUpper().Contains(speedLabel))
                            {
                                int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                                int iiii = startIndex;
                                for (; iiii < tag.Length; iiii++)
                                {
                                    if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                        break;
                                }
                                string speed_string = tag.Substring(startIndex, iiii - startIndex);
                                if (!float.TryParse(speed_string, out horrorSpeed))
                                {
                                    Debug.LogError(string.Format("'{0}' is not a valid value for wiggle speed.", speed_string));
                                }
                            }

                            if (tag.ToUpper().Contains("UNITED"))
                            {
                                horrorIndependency = TextMeshAnimator_IndependencyType.United;
                            }
                            if (tag.ToUpper().Contains("WORD"))
                            {
                                horrorIndependency = TextMeshAnimator_IndependencyType.Word;
                            }
                            if (tag.ToUpper().Contains("CHARACTER"))
                            {
                                horrorIndependency = TextMeshAnimator_IndependencyType.Character;
                            }
                            if (tag.ToUpper().Contains("VERTEX"))
                            {
                                horrorIndependency = TextMeshAnimator_IndependencyType.Vertex;
                            }
                        }

                        // SCROLL SPEED
                        else if (tag.ToUpper().Contains("/SPEED"))
                        {
                            currentScrollSpeed = 3;
                        }
                        else if (tag.ToUpper().Contains("SPEED"))
                        {
                            string speedLabel = "AMT=";
                            int startIndex = tag.ToUpper().IndexOf(speedLabel) + speedLabel.Length;
                            int iiii = startIndex;
                            for (; iiii < tag.Length; iiii++)
                            {
                                if (!char.IsDigit(tag[iiii]) && (tag[iiii] != '.'))
                                    break;
                            }
                            string speed_string = tag.Substring(startIndex, iiii - startIndex);

                            currentScrollSpeed = int.Parse(speed_string);
                        }
                        break;
                    }
                }
            }
            if (index >= inputText.Length)
                continue;
            if (!char.IsControl(inputText[index]) && (inputText[index] != ' '))
            {

                //SHAKE

                shakesEnabled.Add(shaking);
                shakeVelocities.Add(shakeAmount);
                shakeIndependencies.Add(shakeIndependency);


                //PANIC

                wavesEnabled.Add(waving);
                waveVelocities.Add(waveAmount);
                waveSpeeds.Add(waveSpeed);
                waveSeparations.Add(waveSeparation);
                waveIndependencies.Add(waveIndependency);


                //DRUNK

                drunkEnabled.Add(drinking);
                drunkVelocities.Add(drunkAmount);
                drunkSpeeds.Add(drunkSpeed);
                drunkSeparations.Add(drunkSeparation);
                drunkIndependencies.Add(drunkIndependency);

                //EXCITED

                excitedEnabled.Add(exciting);
                excitedVelocities.Add(excitedAmount);
                excitedSpeeds.Add(excitedSpeed);
                excitedSeparations.Add(excitedSeparation);
                excitedIndependencies.Add(excitedIndependency);

                //IMPATIENT

                impatientEnabled.Add(impatient);
                impatientVelocities.Add(impatientAmount);
                impatientSpeeds.Add(impatientSpeed);
                impatientSeparations.Add(impatientSeparation);
                impatientIndependencies.Add(impatientIndependency);

                //SICK

                sickEnabled.Add(sicked);
                sickVelocities.Add(sickAmount);
                sickSpeeds.Add(sickSpeed);
                sickSeparations.Add(sickSeparation);
                sickIndependencies.Add(sickIndependency);

                //HAPPY

                happyEnabled.Add(joy);
                happyVelocities.Add(happyAmount);
                happySpeeds.Add(happySpeed);
                happySeparations.Add(happySeparation);
                happyIndependencies.Add(happyIndependency);

                //SAD

                sadEnabled.Add(crying);
                sadVelocities.Add(sadAmount);
                sadSpeeds.Add(sadSpeed);
                sadSeparations.Add(sadSeparation);
                sadIndependencies.Add(sadIndependency);


                //SURPRISED

                wigglesEnabled.Add(wiggling);
                wiggleVelocities.Add(wiggleAmount);
                wiggleSpeeds.Add(wiggleSpeed);
                wiggleIndependencies.Add(wiggleIndependency);

                //PLAY

                playEnabled.Add(playing);
                playVelocities.Add(playAmount);
                playSpeeds.Add(playSpeed);
                playIndependencies.Add(playIndependency);

                //CLUMSY

                clumsyEnabled.Add(clumy);
                clumsyVelocities.Add(clumsyAmount);
                clumsySpeeds.Add(clumsySpeed);
                clumsyIndependencies.Add(clumsyIndependency);

                //HORROR

                horrorEnabled.Add(horror);
                horrorVelocities.Add(horrorAmount);
                horrorSpeeds.Add(horrorSpeed);
                horrorIndependencies.Add(horrorIndependency);

                scrollSpeeds.Add(currentScrollSpeed);


            }

            outputText += inputText[index];
        }

        //SHAKE

        this.shakesEnabled = shakesEnabled.ToArray();
        this.shakeVelocities = shakeVelocities.ToArray();
        this.shakeIndependencies = shakeIndependencies.ToArray();

        //PANIC

        this.wavesEnabled = wavesEnabled.ToArray();
        this.waveVelocities = waveVelocities.ToArray();
        this.waveSpeeds = waveSpeeds.ToArray();
        this.waveSeparations = waveSeparations.ToArray();
        this.waveIndependencies = waveIndependencies.ToArray();

        //DRUNK

        this.drunkEnabled = drunkEnabled.ToArray();
        this.drunkVelocities = drunkVelocities.ToArray();
        this.drunkSpeeds = drunkSpeeds.ToArray();
        this.drunkSeparations = drunkSeparations.ToArray();
        this.drunkIndependencies = drunkIndependencies.ToArray();

        //EXCITED

        this.excitedEnabled = excitedEnabled.ToArray();
        this.excitedVelocities = excitedVelocities.ToArray();
        this.excitedSpeeds = excitedSpeeds.ToArray();
        this.excitedSeparations = excitedSeparations.ToArray();
        this.excitedIndependencies = excitedIndependencies.ToArray();

        //IMPATIENT

        this.impatientEnabled = impatientEnabled.ToArray();
        this.impatientVelocities = impatientVelocities.ToArray();
        this.impatientSpeeds = impatientSpeeds.ToArray();
        this.impatientSeparations = impatientSeparations.ToArray();
        this.impatientIndependencies = impatientIndependencies.ToArray();

        //SICK

        this.sickEnabled = sickEnabled.ToArray();
        this.sickVelocities = sickVelocities.ToArray();
        this.sickSpeeds = sickSpeeds.ToArray();
        this.sickSeparations = sickSeparations.ToArray();
        this.sickIndependencies = sickIndependencies.ToArray();

        //HAPPY

        this.happyEnabled = happyEnabled.ToArray();
        this.happyVelocities = happyVelocities.ToArray();
        this.happySpeeds = happySpeeds.ToArray();
        this.happySeparations = happySeparations.ToArray();
        this.happyIndependencies = happyIndependencies.ToArray();

        //SAD

        this.sadEnabled = sadEnabled.ToArray();
        this.sadVelocities = sadVelocities.ToArray();
        this.sadSpeeds = sadSpeeds.ToArray();
        this.sadSeparations = sadSeparations.ToArray();
        this.sadIndependencies = sadIndependencies.ToArray();

        //SURPRISED

        this.wigglesEnabled = wigglesEnabled.ToArray();
        this.wiggleVelocities = wiggleVelocities.ToArray();
        this.wiggleSpeeds = wiggleSpeeds.ToArray();
        this.wiggleIndependencies = wiggleIndependencies.ToArray();

        //PLAY

        this.playEnabled = playEnabled.ToArray();
        this.playVelocities = playVelocities.ToArray();
        this.playSpeeds = playSpeeds.ToArray();
        this.playIndependencies = playIndependencies.ToArray();

        //CLUMSY

        this.clumsyEnabled = clumsyEnabled.ToArray();
        this.clumsyVelocities = clumsyVelocities.ToArray();
        this.clumsySpeeds = clumsySpeeds.ToArray();
        this.clumsyIndependencies = clumsyIndependencies.ToArray();

        //HORROR

        this.horrorEnabled = horrorEnabled.ToArray();
        this.horrorVelocities = horrorVelocities.ToArray();
        this.horrorSpeeds = horrorSpeeds.ToArray();
        this.horrorIndependencies = horrorIndependencies.ToArray();

        // SCROLL SPEED
        this.scrollSpeeds = scrollSpeeds.ToArray();

        return outputText;
    }

    void Start()
    {
        UpdateText();
    }

    public void BeginAnimation(string text = null)
    {
        UpdateText(text);
        currentFrame = 0;
    }

    //SHAKE
    Vector3 ShakeVector(float amount)
    {
        return new Vector3(Random.Range(-amount, amount), Random.Range(-amount, amount));
    }


    //PANIC
    Vector3 WaveVector(float amount, float time)
    {
        return new Vector3(Mathf.Sin(time) * amount, 0);
    }


    //DRUNK
    Vector3 DrunkVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }

    //EXCITED
    Vector3 ExcitedVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }

    //IMPATIENT
    Vector3 ImpatientVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }

    //SICK
    Vector3 SickVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }

    //HAPPY
    Vector3 HappyVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }

    //SAD
    Vector3 SadVector(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount);
    }


    //SURPRISED
    Vector3 WiggleVector(float amount, float speed, ref int i)
    {
        wiggleTimeCurrent[i * 2] += speed;

        if ((wiggleTimeTotal[i * 2] == 0) || (wiggleTimeCurrent[i * 2] / wiggleTimeTotal[i * 2]) >= 1)
        {
            wiggleTimeCurrent[i * 2] -= wiggleTimeTotal[i * 2];
            wiggleTimeTotal[i * 2] = Random.Range(wiggleMinimumDuration, 1.0f);
            wigglePrevPos[i * 2] = wiggleTargetPos[i * 2];
            wiggleTargetPos[i * 2] = Random.Range(-amount, amount);
        }
        wiggleTimeCurrent[i * 2 + 1] += speed;
        if ((wiggleTimeTotal[i * 2 + 1] == 0) || (wiggleTimeCurrent[i * 2 + 1] / wiggleTimeTotal[i * 2 + 1]) >= 1)
        {
            wiggleTimeCurrent[i * 2 + 1] -= wiggleTimeTotal[i * 2 + 1];
            wiggleTimeTotal[i * 2 + 1] = Random.Range(wiggleMinimumDuration, 1.0f);
            wigglePrevPos[i * 2 + 1] = wiggleTargetPos[i * 2 + 1];
            wiggleTargetPos[i * 2 + 1] = Random.Range(-amount, amount);
        }
        Vector3 outputVector = new Vector3(Mathf.SmoothStep(wigglePrevPos[i * 2], wiggleTargetPos[i * 2], (wiggleTimeCurrent[i * 2] / wiggleTimeTotal[i * 2])), Mathf.SmoothStep(wigglePrevPos[i * 2 + 1], wiggleTargetPos[i * 2 + 1], (wiggleTimeCurrent[i * 2 + 1] / wiggleTimeTotal[i * 2 + 1])));

        ++i;
        return outputVector;
    }

    //PLAY
    Vector3 PlayVector(float amount, float speed, ref int i)
    {
        playTimeCurrent[i * 2] += speed;

        if ((playTimeTotal[i * 2] == 0) || (playTimeCurrent[i * 2] / playTimeTotal[i * 2]) >= 1)
        {
            playTimeCurrent[i * 2] -= playTimeTotal[i * 2];
            playTimeTotal[i * 2] = Random.Range(playMinimumDuration, 1.0f);
            playPrevPos[i * 2] = playTargetPos[i * 2];
            playTargetPos[i * 2] = Random.Range(-amount, amount);
        }
        playTimeCurrent[i * 2 + 1] += speed;
        if ((playTimeTotal[i * 2 + 1] == 0) || (playTimeCurrent[i * 2 + 1] / playTimeTotal[i * 2 + 1]) >= 1)
        {
            playTimeCurrent[i * 2 + 1] -= playTimeTotal[i * 2 + 1];
            playTimeTotal[i * 2 + 1] = Random.Range(playMinimumDuration, 1.0f);
            playPrevPos[i * 2 + 1] = playTargetPos[i * 2 + 1];
            playTargetPos[i * 2 + 1] = Random.Range(-amount, amount);
        }
        Vector3 outputVector = new Vector3(Mathf.SmoothStep(playPrevPos[i * 2], playTargetPos[i * 2], (playTimeCurrent[i * 2] / playTimeTotal[i * 2])), Mathf.SmoothStep(playPrevPos[i * 2 + 1], playTargetPos[i * 2 + 1], (playTimeCurrent[i * 2 + 1] / playTimeTotal[i * 2 + 1])));

        ++i;
        return outputVector;
    }

    //CLUMSY
    Vector3 ClumsyVector(float amount, float speed, ref int i)
    {
        clumsyTimeCurrent[i * 2] += speed;

        if ((clumsyTimeTotal[i * 2] == 0) || (clumsyTimeCurrent[i * 2] / clumsyTimeTotal[i * 2]) >= 1)
        {
            clumsyTimeCurrent[i * 2] -= clumsyTimeTotal[i * 2];
            clumsyTimeTotal[i * 2] = Random.Range(clumsyMinimumDuration, 1.0f);
            clumsyPrevPos[i * 2] = clumsyTargetPos[i * 2];
            clumsyTargetPos[i * 2] = Random.Range(-amount, amount);
        }
        clumsyTimeCurrent[i * 2 + 1] += speed;
        if ((clumsyTimeTotal[i * 2 + 1] == 0) || (clumsyTimeCurrent[i * 2 + 1] / clumsyTimeTotal[i * 2 + 1]) >= 1)
        {
            clumsyTimeCurrent[i * 2 + 1] -= clumsyTimeTotal[i * 2 + 1];
            clumsyTimeTotal[i * 2 + 1] = Random.Range(clumsyMinimumDuration, 1.0f);
            clumsyPrevPos[i * 2 + 1] = clumsyTargetPos[i * 2 + 1];
            clumsyTargetPos[i * 2 + 1] = Random.Range(-amount, amount);
        }
        Vector3 outputVector = new Vector3(Mathf.SmoothStep(clumsyPrevPos[i * 2], clumsyTargetPos[i * 2], (clumsyTimeCurrent[i * 2] / clumsyTimeTotal[i * 2])), Mathf.SmoothStep(clumsyPrevPos[i * 2 + 1], clumsyTargetPos[i * 2 + 1], (clumsyTimeCurrent[i * 2 + 1] / clumsyTimeTotal[i * 2 + 1])));

        ++i;
        return outputVector;
    }

    //HORROR
    Vector3 HorrorVector(float amount, float speed, ref int i)
    {
        horrorTimeCurrent[i * 2] += speed;

        if ((horrorTimeTotal[i * 2] == 0) || (horrorTimeCurrent[i * 2] / horrorTimeTotal[i * 2]) >= 1)
        {
            horrorTimeCurrent[i * 2] -= horrorTimeTotal[i * 2];
            horrorTimeTotal[i * 2] = Random.Range(horrorMinimumDuration, 1.0f);
            horrorPrevPos[i * 2] = horrorTargetPos[i * 2];
            horrorTargetPos[i * 2] = Random.Range(-amount, amount);
        }
        horrorTimeCurrent[i * 2 + 1] += speed;
        if ((horrorTimeTotal[i * 2 + 1] == 0) || (horrorTimeCurrent[i * 2 + 1] / horrorTimeTotal[i * 2 + 1]) >= 1)
        {
            horrorTimeCurrent[i * 2 + 1] -= horrorTimeTotal[i * 2 + 1];
            horrorTimeTotal[i * 2 + 1] = Random.Range(horrorMinimumDuration, 1.0f);
            horrorPrevPos[i * 2 + 1] = horrorTargetPos[i * 2 + 1];
            horrorTargetPos[i * 2 + 1] = Random.Range(-amount, amount);
        }
        Vector3 outputVector = new Vector3(Mathf.SmoothStep(horrorPrevPos[i * 2], horrorTargetPos[i * 2], (horrorTimeCurrent[i * 2] / horrorTimeTotal[i * 2])), Mathf.SmoothStep(horrorPrevPos[i * 2 + 1], horrorTargetPos[i * 2 + 1], (horrorTimeCurrent[i * 2 + 1] / horrorTimeTotal[i * 2 + 1])));

        ++i;
        return outputVector;
    }

    // Update is called once per frame
    void Update()
    {

        sv = new Vector3(); //SHAKE



        wv = new Vector3(); //PANIC



        dv = new Vector3(); //DRUNK

        ev = new Vector3(); //EXCITED

        iv = new Vector3(); //IMPATIENT

        siv = new Vector3(); //SICK

        hv = new Vector3(); //HAPPY

        sav = new Vector3(); //SAD



        wgv = new Vector3(); //SURPRISED

        pv = new Vector3(); //PLAY

        cv = new Vector3(); //CLUMSY

        hov = new Vector3(); //HORROR

        for (int i = 0; i < TMProGUI.textInfo.meshInfo.Length; ++i)
        {
            int j = 0;

            //SHAKE


            float shakeAmount = 1;
            if (shakeVelocities.Length > j)
            {
                shakeAmount = shakeVelocities[j];
            };
            int sl = 0;
            TextMeshAnimator_IndependencyType shakeIndependency = this.shakeIndependency;
            if (shakeIndependency == TextMeshAnimator_IndependencyType.United) sv = ShakeVector(this.shakeAmount);

            //PANIC

            float waveAmount = 1;
            if (waveVelocities.Length > j)
            {
                waveAmount = waveVelocities[j];
            };

            float waveSpeed = 1;
            if (waveSpeeds.Length > j)
            {
                waveSpeed = waveSpeeds[j];
            };
            int wl = 0;
            TextMeshAnimator_IndependencyType waveIndependency = this.waveIndependency;
            if (waveIndependency == TextMeshAnimator_IndependencyType.United) wv = WaveVector(this.waveAmount, currentFrame * (this.waveSpeed * waveSpeed));


            //DRUNK

            float drunkAmount = 1;
            if (drunkVelocities.Length > j)
            {
                drunkAmount = drunkVelocities[j];
            };

            float drunkSpeed = 1;
            if (drunkSpeeds.Length > j)
            {
                drunkSpeed = drunkSpeeds[j];
            };
            int dl = 0;
            TextMeshAnimator_IndependencyType drunkIndependency = this.drunkIndependency;
            if (drunkIndependency == TextMeshAnimator_IndependencyType.United) dv = DrunkVector(this.drunkAmount, currentFrame * (this.drunkSpeed * drunkSpeed));

            //EXCITED

            float excitedAmount = 1;
            if (excitedVelocities.Length > j)
            {
                excitedAmount = excitedVelocities[j];
            };

            float excitedSpeed = 1;
            if (excitedSpeeds.Length > j)
            {
                excitedSpeed = excitedSpeeds[j];
            };
            int evl = 0;
            TextMeshAnimator_IndependencyType excitedIndependency = this.excitedIndependency;
            if (excitedIndependency == TextMeshAnimator_IndependencyType.United) ev = ExcitedVector(this.excitedAmount, currentFrame * (this.excitedSpeed * excitedSpeed));

            //IMPATIENT

            float impatientAmount = 1;
            if (impatientVelocities.Length > j)
            {
                impatientAmount = impatientVelocities[j];
            };

            float impatientSpeed = 1;
            if (impatientSpeeds.Length > j)
            {
                impatientSpeed = impatientSpeeds[j];
            };
            int il = 0;
            TextMeshAnimator_IndependencyType impatientIndependency = this.impatientIndependency;
            if (impatientIndependency == TextMeshAnimator_IndependencyType.United) iv = ExcitedVector(this.impatientAmount, currentFrame * (this.impatientSpeed * impatientSpeed));

            //SICK

            float sickAmount = 1;
            if (sickVelocities.Length > j)
            {
                sickAmount = sickVelocities[j];
            };

            float sickSpeed = 1;
            if (sickSpeeds.Length > j)
            {
                sickSpeed = sickSpeeds[j];
            };
            int sivl = 0;
            TextMeshAnimator_IndependencyType sickIndependency = this.sickIndependency;
            if (sickIndependency == TextMeshAnimator_IndependencyType.United) siv = SickVector(this.sickAmount, currentFrame * (this.sickSpeed * sickSpeed));

            //HAPPY

            float happyAmount = 1;
            if (happyVelocities.Length > j)
            {
                happyAmount = happyVelocities[j];
            };

            float happySpeed = 1;
            if (happySpeeds.Length > j)
            {
                happySpeed = happySpeeds[j];
            };
            int havl = 0;
            TextMeshAnimator_IndependencyType happyIndependency = this.drunkIndependency;
            if (happyIndependency == TextMeshAnimator_IndependencyType.United) hv = HappyVector(this.happyAmount, currentFrame * (this.happySpeed * happySpeed));

            //SAD

            float sadAmount = 1;
            if (sadVelocities.Length > j)
            {
                sadAmount = sadVelocities[j];
            };

            float sadSpeed = 1;
            if (sadSpeeds.Length > j)
            {
                sadSpeed = sadSpeeds[j];
            };
            int savl = 0;
            TextMeshAnimator_IndependencyType sadIndependency = this.sadIndependency;
            if (sadIndependency == TextMeshAnimator_IndependencyType.United) sav = SadVector(this.sadAmount, currentFrame * (this.sadSpeed * sadSpeed));


            //SURPRISED

            float wiggleAmount = 1;
            if (wiggleVelocities.Length > j)
            {
                wiggleAmount = wiggleVelocities[j];
            };

            float wiggleSpeed = 1;
            if (wiggleSpeeds.Length > j)
            {
                wiggleSpeed = wiggleSpeeds[j];
            };
            int wgl = 0;
            int wgll = 0;
            TextMeshAnimator_IndependencyType wiggleIndependency = this.wiggleIndependency;
            if (wiggleIndependency == TextMeshAnimator_IndependencyType.United) wgv = WiggleVector(this.wiggleAmount, this.wiggleSpeed * wiggleSpeed, ref wgll);

            //PLAY

            float playAmount = 1;
            if (playVelocities.Length > j)
            {
                playAmount = playVelocities[j];
            };

            float playSpeed = 1;
            if (playSpeeds.Length > j)
            {
                playSpeed = playSpeeds[j];
            };
            int pl = 0;
            int ppl = 0;
            TextMeshAnimator_IndependencyType playIndependency = this.playIndependency;
            if (playIndependency == TextMeshAnimator_IndependencyType.United) pv = PlayVector(this.playAmount, this.playSpeed * playSpeed, ref ppl);

            //CLUMSY

            float clumsyAmount = 1;
            if (clumsyVelocities.Length > j)
            {
                clumsyAmount = clumsyVelocities[j];
            };

            float clumsySpeed = 1;
            if (clumsySpeeds.Length > j)
            {
                clumsySpeed = clumsySpeeds[j];
            };
            int cl = 0;
            int ccl = 0;
            TextMeshAnimator_IndependencyType clumsyIndependency = this.clumsyIndependency;
            if (clumsyIndependency == TextMeshAnimator_IndependencyType.United) cv = WiggleVector(this.clumsyAmount, this.clumsySpeed * clumsySpeed, ref ccl);

            //HORROR

            float horrorAmount = 1;
            if (horrorVelocities.Length > j)
            {
                horrorAmount = horrorVelocities[j];
            };

            float horrorSpeed = 1;
            if (horrorSpeeds.Length > j)
            {
                horrorSpeed = horrorSpeeds[j];
            };
            int hol = 0;
            int hool = 0;
            TextMeshAnimator_IndependencyType horrorIndependency = this.horrorIndependency;
            if (horrorIndependency == TextMeshAnimator_IndependencyType.United) hov = HorrorVector(this.horrorAmount, this.horrorSpeed * horrorSpeed, ref hool);

            for (int v = 0; v < TMProGUI.textInfo.meshInfo[i].vertices.Length; v += 4, ++j)
            {

                for (byte k = 0; k < 4; ++k)
                    TMProGUI.textInfo.meshInfo[i].vertices[v + k] = vertex_Base[i][v + k];

                //SHAKE

                TextMeshAnimator_IndependencyType prevShakeIndependency = shakeIndependency;
                if (j < shakeIndependencies.Length)
                {
                    shakeIndependency = shakeIndependencies[j];
                }
                if ((j >= 1) && (j < shakeIndependencies.Length + 1))
                {
                    prevShakeIndependency = shakeIndependencies[j - 1];
                }
                if (shakeIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (sl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[sl] == ' ') || char.IsControl(TMProGUI.text[sl]) || (prevShakeIndependency != TextMeshAnimator_IndependencyType.Word) || (sl == 0))
                        {
                            sv = ShakeVector(this.shakeAmount);
                            if ((TMProGUI.text[sl] == ' ') || char.IsControl(TMProGUI.text[sl])) ++sl;
                        }
                    }
                }
                ++sl;
                bool shake = false;
                if (shakesEnabled.Length > j)
                {
                    shake = shakesEnabled[j];
                }
                if (shake)
                {
                    if (shakeVelocities.Length > j)
                    {
                        shakeAmount = shakeVelocities[j];
                    };
                    if (shakeIndependency == TextMeshAnimator_IndependencyType.Character) sv = ShakeVector(this.shakeAmount);
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (shakeIndependency == TextMeshAnimator_IndependencyType.Vertex) sv = ShakeVector(this.shakeAmount);
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += sv * shakeAmount;
                    }
                }

                //PANIC
                if (waveSpeeds.Length > j)
                {
                    waveSpeed = waveSpeeds[j];
                };

                float waveSeparation = this.waveSeparation;
                if (waveSeparations.Length > j)
                {
                    waveSeparation = waveSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevWaveIndependency = waveIndependency;
                if (j < waveIndependencies.Length)
                {
                    waveIndependency = waveIndependencies[j];
                }
                if ((j >= 1) && (j < waveIndependencies.Length + 1))
                {
                    prevWaveIndependency = waveIndependencies[j - 1];
                }
                if (waveIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (wl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[wl] == ' ') || char.IsControl(TMProGUI.text[wl]) || (prevWaveIndependency != TextMeshAnimator_IndependencyType.Word) || (wl == 0))
                        {
                            wv = WaveVector(this.waveAmount, currentFrame * (this.waveSpeed * waveSpeed) + this.waveSpeed * waveSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.waveSeparation * waveSeparation));
                            if ((TMProGUI.text[wl] == ' ') || char.IsControl(TMProGUI.text[wl])) ++wl;
                        }
                    }
                }
                ++wl;

                bool wave = false;
                if (wavesEnabled.Length > j)
                {
                    wave = wavesEnabled[j];
                }
                if (wave)
                {
                    if (waveVelocities.Length > j)
                    {
                        waveAmount = waveVelocities[j];
                    };
                    if (waveIndependency == TextMeshAnimator_IndependencyType.Character) wv = WaveVector(this.waveAmount, currentFrame * (this.waveSpeed * waveSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.waveSeparation * waveSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (waveIndependency == TextMeshAnimator_IndependencyType.Vertex) wv = WaveVector(this.waveAmount, currentFrame * (this.waveSpeed * waveSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.waveSeparation * waveSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += wv * waveAmount;
                    }
                }

                //DRUNK
                if (drunkSpeeds.Length > j)
                {
                    drunkSpeed = drunkSpeeds[j];
                };

                float drunkSeparation = this.drunkSeparation;
                if (drunkSeparations.Length > j)
                {
                    drunkSeparation = drunkSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevWavevIndependency = drunkIndependency;
                if (j < drunkIndependencies.Length)
                {
                    drunkIndependency = drunkIndependencies[j];
                }
                if ((j >= 1) && (j < drunkIndependencies.Length + 1))
                {
                    prevWavevIndependency = drunkIndependencies[j - 1];
                }
                if (drunkIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (dl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[dl] == ' ') || char.IsControl(TMProGUI.text[dl]) || (prevWavevIndependency != TextMeshAnimator_IndependencyType.Word) || (dl == 0))
                        {
                            dv = DrunkVector(this.drunkAmount, currentFrame * (this.drunkSpeed * drunkSpeed) + this.drunkSpeed * drunkSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.drunkSeparation * drunkSeparation));
                            if ((TMProGUI.text[dl] == ' ') || char.IsControl(TMProGUI.text[dl])) ++dl;
                        }
                    }
                }
                ++dl;

                bool wavev = false;
                if (drunkEnabled.Length > j)
                {
                    wavev = drunkEnabled[j];
                }
                if (wavev)
                {
                    if (drunkVelocities.Length > j)
                    {
                        drunkAmount = drunkVelocities[j];
                    };
                    if (drunkIndependency == TextMeshAnimator_IndependencyType.Character) dv = DrunkVector(this.drunkAmount, currentFrame * (this.drunkSpeed * drunkSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.drunkSeparation * drunkSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (drunkIndependency == TextMeshAnimator_IndependencyType.Vertex) dv = DrunkVector(this.drunkAmount, currentFrame * (this.drunkSpeed * drunkSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.drunkSeparation * drunkSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += dv * drunkAmount;
                    }
                }

                //EXCITED
                if (excitedSpeeds.Length > j)
                {
                    excitedSpeed = excitedSpeeds[j];
                };

                float excitedSeparation = this.excitedSeparation;
                if (excitedSeparations.Length > j)
                {
                    excitedSeparation = excitedSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevExcitedIndependency = excitedIndependency;
                if (j < excitedIndependencies.Length)
                {
                    excitedIndependency = excitedIndependencies[j];
                }
                if ((j >= 1) && (j < excitedIndependencies.Length + 1))
                {
                    prevExcitedIndependency = excitedIndependencies[j - 1];
                }
                if (excitedIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (evl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[evl] == ' ') || char.IsControl(TMProGUI.text[evl]) || (prevExcitedIndependency != TextMeshAnimator_IndependencyType.Word) || (evl == 0))
                        {
                            ev = ExcitedVector(this.excitedAmount, currentFrame * (this.excitedSpeed * excitedSpeed) + this.excitedSpeed * excitedSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.excitedSeparation * excitedSeparation));
                            if ((TMProGUI.text[evl] == ' ') || char.IsControl(TMProGUI.text[evl])) ++evl;
                        }
                    }
                }
                ++evl;

                bool exv = false;
                if (excitedEnabled.Length > j)
                {
                    exv = drunkEnabled[j];
                }
                if (exv)
                {
                    if (drunkVelocities.Length > j)
                    {
                        excitedAmount = drunkVelocities[j];
                    }

                    if (excitedIndependency == TextMeshAnimator_IndependencyType.Character) ev = ExcitedVector(this.excitedAmount, currentFrame * (this.excitedSpeed * excitedSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.excitedSeparation * excitedSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (excitedIndependency == TextMeshAnimator_IndependencyType.Vertex) ev = ExcitedVector(this.excitedAmount, currentFrame * (this.excitedSpeed * excitedSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.excitedSeparation * excitedSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += ev * excitedAmount;
                    }
                }

                //IMPATIENT
                if (impatientSpeeds.Length > j)
                {
                    impatientSpeed = impatientSpeeds[j];
                };

                float impatientSeparation = this.impatientSeparation;
                if (impatientSeparations.Length > j)
                {
                    impatientSeparation = impatientSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevImpatientIndependency = impatientIndependency;
                if (j < impatientIndependencies.Length)
                {
                    impatientIndependency = impatientIndependencies[j];
                }
                if ((j >= 1) && (j < impatientIndependencies.Length + 1))
                {
                    prevImpatientIndependency = impatientIndependencies[j - 1];
                }
                if (impatientIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (il < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[il] == ' ') || char.IsControl(TMProGUI.text[il]) || (prevImpatientIndependency != TextMeshAnimator_IndependencyType.Word) || (il == 0))
                        {
                            iv = ImpatientVector(this.sickAmount, currentFrame * (this.impatientSpeed * impatientSpeed) + this.impatientSpeed * impatientSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.impatientSeparation * impatientSeparation));
                            if ((TMProGUI.text[il] == ' ') || char.IsControl(TMProGUI.text[il])) ++il;
                        }
                    }
                }
                ++il;

                bool ill = false;
                if (impatientEnabled.Length > j)
                {
                    ill = impatientEnabled[j];
                }
                if (ill)
                {
                    if (impatientVelocities.Length > j)
                    {
                        impatientAmount = impatientVelocities[j];
                    }

                    if (impatientIndependency == TextMeshAnimator_IndependencyType.Character) iv = ImpatientVector(this.impatientAmount, currentFrame * (this.impatientSpeed * impatientSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.impatientSeparation * impatientSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (impatientIndependency == TextMeshAnimator_IndependencyType.Vertex) iv = ImpatientVector(this.impatientAmount, currentFrame * (this.impatientSpeed * impatientSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.impatientSeparation * impatientSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += iv * impatientAmount;
                    }
                }

                //SICK
                if (sickSpeeds.Length > j)
                {
                    sickSpeed = sickSpeeds[j];
                };

                float sickSeparation = this.sickSeparation;
                if (sickSeparations.Length > j)
                {
                    sickSeparation = sickSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevSickIndependency = sickIndependency;
                if (j < sickIndependencies.Length)
                {
                    sickIndependency = sickIndependencies[j];
                }
                if ((j >= 1) && (j < sickIndependencies.Length + 1))
                {
                    prevSickIndependency = sickIndependencies[j - 1];
                }
                if (sickIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (sivl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[sivl] == ' ') || char.IsControl(TMProGUI.text[sivl]) || (prevSickIndependency != TextMeshAnimator_IndependencyType.Word) || (sivl == 0))
                        {
                            siv = SickVector(this.sickAmount, currentFrame * (this.sickSpeed * sickSpeed) + this.sickSpeed * sickSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.sickSeparation * sickSeparation));
                            if ((TMProGUI.text[sivl] == ' ') || char.IsControl(TMProGUI.text[sivl])) ++sivl;
                        }
                    }
                }
                ++sivl;

                bool ssv = false;
                if (sickEnabled.Length > j)
                {
                    ssv = sickEnabled[j];
                }
                if (ssv)
                {
                    if (sickVelocities.Length > j)
                    {
                        sickAmount = sickVelocities[j];
                    }

                    if (sickIndependency == TextMeshAnimator_IndependencyType.Character) siv = SickVector(this.sickAmount, currentFrame * (this.sickSpeed * sickSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.sickSeparation * sickSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (sickIndependency == TextMeshAnimator_IndependencyType.Vertex) siv = SickVector(this.sickAmount, currentFrame * (this.sickSpeed * sickSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.sickSeparation * sickSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += siv * sickAmount;
                    }
                }

                //HAPPY
                if (happySpeeds.Length > j)
                {
                    happySpeed = happySpeeds[j];
                };

                float happySeparation = this.happySeparation;
                if (happySeparations.Length > j)
                {
                    happySeparation = happySeparations[j];
                };

                TextMeshAnimator_IndependencyType prevHappyIndependency = happyIndependency;
                if (j < happyIndependencies.Length)
                {
                    happyIndependency = happyIndependencies[j];
                }
                if ((j >= 1) && (j < happyIndependencies.Length + 1))
                {
                    prevHappyIndependency = happyIndependencies[j - 1];
                }
                if (happyIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (havl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[havl] == ' ') || char.IsControl(TMProGUI.text[havl]) || (prevHappyIndependency != TextMeshAnimator_IndependencyType.Word) || (havl == 0))
                        {
                            hv = HappyVector(this.happyAmount, currentFrame * (this.happySpeed * happySpeed) + this.happySpeed * happySpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.happySeparation * happySeparation));
                            if ((TMProGUI.text[havl] == ' ') || char.IsControl(TMProGUI.text[havl])) ++havl;
                        }
                    }
                }
                ++havl;

                bool happ = false;
                if (happyEnabled.Length > j)
                {
                    happ = happyEnabled[j];
                }
                if (happ)
                {
                    if (happyVelocities.Length > j)
                    {
                        happyAmount = happyVelocities[j];
                    }

                    if (happyIndependency == TextMeshAnimator_IndependencyType.Character) hv = HappyVector(this.happyAmount, currentFrame * (this.happySpeed * happySpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.happySeparation * happySeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (happyIndependency == TextMeshAnimator_IndependencyType.Vertex) hv = HappyVector(this.happyAmount, currentFrame * (this.happySpeed * happySpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.happySeparation * happySeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += hv * happyAmount;
                    }
                }

                //SAD
                if (sadSpeeds.Length > j)
                {
                    sadSpeed = sadSpeeds[j];
                };

                float sadSeparation = this.sadSeparation;
                if (sadSeparations.Length > j)
                {
                    sadSeparation = sadSeparations[j];
                };

                TextMeshAnimator_IndependencyType prevSadIndependency = sadIndependency;
                if (j < sadIndependencies.Length)
                {
                    sadIndependency = sadIndependencies[j];
                }
                if ((j >= 1) && (j < sadIndependencies.Length + 1))
                {
                    prevSadIndependency = sadIndependencies[j - 1];
                }
                if (sadIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (savl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[savl] == ' ') || char.IsControl(TMProGUI.text[savl]) || (prevWavevIndependency != TextMeshAnimator_IndependencyType.Word) || (savl == 0))
                        {
                            sav = SadVector(this.sadAmount, currentFrame * (this.sadSpeed * sadSpeed) + this.sadSpeed * sadSpeed + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.sadSeparation * sadSeparation));
                            if ((TMProGUI.text[savl] == ' ') || char.IsControl(TMProGUI.text[savl])) ++savl;
                        }
                    }
                }
                ++savl;

                bool saa = false;
                if (sadEnabled.Length > j)
                {
                    saa = sadEnabled[j];
                }
                if (saa)
                {
                    if (sadVelocities.Length > j)
                    {
                        sadAmount = sadVelocities[j];
                    };
                    if (sadIndependency == TextMeshAnimator_IndependencyType.Character) sav = SadVector(this.sadAmount, currentFrame * (this.sadSpeed * sadSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v].x / (this.sadSeparation * sadSeparation));
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (sadIndependency == TextMeshAnimator_IndependencyType.Vertex) sav = SadVector(this.sadAmount, currentFrame * (this.sadSpeed * sadSpeed) + TMProGUI.textInfo.meshInfo[i].vertices[v + k].x / (this.sadSeparation * sadSeparation));
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += sav * sadAmount;
                    }
                }

                //SURPRISED

                wiggleSpeed = this.wiggleSpeed;
                if (wiggleSpeeds.Length > j)
                {
                    wiggleSpeed = wiggleSpeeds[j];
                };

                TextMeshAnimator_IndependencyType prevwiggleIndependency = wiggleIndependency;
                if (j < wiggleIndependencies.Length)
                {
                    wiggleIndependency = wiggleIndependencies[j];
                }
                if ((j >= 1) && (j < wiggleIndependencies.Length + 1))
                {
                    prevwiggleIndependency = wiggleIndependencies[j - 1];
                }
                if (wiggleIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (wgl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[wgl] == ' ') || char.IsControl(TMProGUI.text[wgl]) || (prevwiggleIndependency != TextMeshAnimator_IndependencyType.Word) || (wgl == 0))
                        {
                            wgv = WiggleVector(this.wiggleAmount, this.wiggleSpeed * wiggleSpeed, ref wgll);
                            if ((TMProGUI.text[wgl] == ' ') || char.IsControl(TMProGUI.text[wgl])) ++wgl;
                        }
                    }
                }
                ++wgl;

                bool wiggle = false;
                if (wigglesEnabled.Length > j)
                {
                    wiggle = wigglesEnabled[j];
                }
                if (wiggle)
                {
                    if (wiggleVelocities.Length > j)
                    {
                        wiggleAmount = wiggleVelocities[j];
                    };
                    if (wiggleIndependency == TextMeshAnimator_IndependencyType.Character)
                    {
                        wgv = WiggleVector(this.wiggleAmount, this.wiggleSpeed * wiggleSpeed, ref wgll);
                    }
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (wiggleIndependency == TextMeshAnimator_IndependencyType.Vertex) wgv = WiggleVector(this.wiggleAmount, this.wiggleSpeed * wiggleSpeed, ref wgll);
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += wgv * wiggleAmount;
                    }
                }

                //PLAY

                playSpeed = this.playSpeed;
                if (playSpeeds.Length > j)
                {
                    playSpeed = playSpeeds[j];
                };

                TextMeshAnimator_IndependencyType prevPlayIndependency = playIndependency;
                if (j < playIndependencies.Length)
                {
                    playIndependency = playIndependencies[j];
                }
                if ((j >= 1) && (j < playIndependencies.Length + 1))
                {
                    prevPlayIndependency = playIndependencies[j - 1];
                }
                if (playIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (pl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[pl] == ' ') || char.IsControl(TMProGUI.text[pl]) || (prevPlayIndependency != TextMeshAnimator_IndependencyType.Word) || (pl == 0))
                        {
                            pv = PlayVector(this.playAmount, this.playSpeed * playSpeed, ref ppl);
                            if ((TMProGUI.text[pl] == ' ') || char.IsControl(TMProGUI.text[pl])) ++pl;
                        }
                    }
                }
                ++pl;

                bool plal = false;
                if (playEnabled.Length > j)
                {
                    plal = playEnabled[j];
                }
                if (plal)
                {
                    if (playVelocities.Length > j)
                    {
                        playAmount = playVelocities[j];
                    };
                    if (playIndependency == TextMeshAnimator_IndependencyType.Character)
                    {
                        pv = PlayVector(this.playAmount, this.playSpeed * playSpeed, ref ppl);
                    }
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (playIndependency == TextMeshAnimator_IndependencyType.Vertex) pv = PlayVector(this.playAmount, this.playSpeed * playSpeed, ref ppl);
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += pv * playAmount;
                    }
                }

                //CLUMSY

                clumsySpeed = this.clumsySpeed;
                if (clumsySpeeds.Length > j)
                {
                    clumsySpeed = clumsySpeeds[j];
                };

                TextMeshAnimator_IndependencyType prevClumsyIndependency = clumsyIndependency;
                if (j < clumsyIndependencies.Length)
                {
                    clumsyIndependency = clumsyIndependencies[j];
                }
                if ((j >= 1) && (j < clumsyIndependencies.Length + 1))
                {
                    prevClumsyIndependency = clumsyIndependencies[j - 1];
                }
                if (clumsyIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (cl < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[cl] == ' ') || char.IsControl(TMProGUI.text[cl]) || (prevClumsyIndependency != TextMeshAnimator_IndependencyType.Word) || (cl == 0))
                        {
                            cv = ClumsyVector(this.clumsyAmount, this.clumsySpeed * clumsySpeed, ref ccl);
                            if ((TMProGUI.text[cl] == ' ') || char.IsControl(TMProGUI.text[cl])) ++cl;
                        }
                    }
                }
                ++cl;

                bool clum = false;
                if (clumsyEnabled.Length > j)
                {
                    clum = clumsyEnabled[j];
                }
                if (clum)
                {
                    if (clumsyVelocities.Length > j)
                    {
                        clumsyAmount = clumsyVelocities[j];
                    };
                    if (clumsyIndependency == TextMeshAnimator_IndependencyType.Character)
                    {
                        cv = ClumsyVector(this.clumsyAmount, this.clumsySpeed * clumsySpeed, ref ccl);
                    }
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (clumsyIndependency == TextMeshAnimator_IndependencyType.Vertex) cv = ClumsyVector(this.clumsyAmount, this.clumsySpeed * clumsySpeed, ref ccl);
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += cv * clumsyAmount;
                    }
                }

                //HORROR

                horrorSpeed = this.horrorSpeed;
                if (wiggleSpeeds.Length > j)
                {
                    horrorSpeed = wiggleSpeeds[j];
                }

                TextMeshAnimator_IndependencyType prevHorrorIndependency = horrorIndependency;
                if (j < horrorIndependencies.Length)
                {
                    horrorIndependency = horrorIndependencies[j];
                }
                if ((j >= 1) && (j < horrorIndependencies.Length + 1))
                {
                    prevHorrorIndependency = horrorIndependencies[j - 1];
                }
                if (horrorIndependency == TextMeshAnimator_IndependencyType.Word)
                {
                    if (hol < TMProGUI.text.Length)
                    {
                        if ((TMProGUI.text[hol] == ' ') || char.IsControl(TMProGUI.text[hol]) || (prevHorrorIndependency != TextMeshAnimator_IndependencyType.Word) || (hol == 0))
                        {
                            hov = HorrorVector(this.horrorAmount, this.horrorSpeed * horrorSpeed, ref hool);
                            if ((TMProGUI.text[hol] == ' ') || char.IsControl(TMProGUI.text[hol])) ++hol;
                        }
                    }
                }
                ++hol;

                bool hor = false;
                if (horrorEnabled.Length > j)
                {
                    hor = horrorEnabled[j];
                }
                if (hor)
                {
                    if (horrorVelocities.Length > j)
                    {
                        horrorAmount = horrorVelocities[j];
                    };
                    if (horrorIndependency == TextMeshAnimator_IndependencyType.Character)
                    {
                        hov = HorrorVector(this.horrorAmount, this.horrorSpeed * horrorSpeed, ref hool);
                    }
                    for (byte k = 0; k < 4; ++k)
                    {
                        if (horrorIndependency == TextMeshAnimator_IndependencyType.Vertex) hov = HorrorVector(this.horrorAmount, this.horrorSpeed * horrorSpeed, ref hool);
                        TMProGUI.textInfo.meshInfo[i].vertices[v + k] += hov * horrorAmount;
                    }
                }


                // CHAR VISIBILITY
                if ((v / 4) + 1 > charsVisible)
                {
                    for (int g = 0; g < 4; g++)
                    {
                        TMProGUI.textInfo.meshInfo[i].vertices[v + g] = Vector3.zero;

                        //Color32 currentColor = TMProGUI.textInfo.characterInfo[(v/4)+1].color;
                        //TMProGUI.textInfo.characterInfo[(v/4)+1].color = new Color32(currentColor.r, currentColor.g, currentColor.b, (byte)0);

                    }
                }
            }

            TMProGUI.UpdateVertexData();
            //TMProGUI.ForceMeshUpdate();
        }
        ++currentFrame;
    }
}

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
