using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceController : MonoBehaviour {

    [SerializeField]
    private string[] m_Keywords;
    private KeywordRecognizer m_Recognizer;

    public static float speed = 1.0f;
    private int counter = 0;
    private int cooldown = 0;

    public Material daytime;
    public Material nighttime;
    public Light light;

	// Use this for initialization
	void Start () {
        m_Keywords = new string[1];
        m_Keywords[0] = "Fruit";
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (speed == 0.1f)
        {
            counter++;
        }
        else
        {
            cooldown--;
        }
        if (counter >= 600)
        {
            string value = "#FFF3CDFF";
            Color newColor;
            if (ColorUtility.TryParseHtmlString(value, out newColor))
            {
                RenderSettings.ambientSkyColor = newColor;
                RenderSettings.ambientLight = newColor;
                light.intensity = 1.1f;
            }
            RenderSettings.skybox = daytime;
            counter = 0;
            speed = 1.0f;
        }
	}

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text == m_Keywords[0] && cooldown <= 0)
        {
            string value = "#000000";
            Color newColor;
            if (ColorUtility.TryParseHtmlString(value, out newColor))
            {
                RenderSettings.ambientSkyColor = newColor;
                RenderSettings.ambientLight = newColor;
                light.intensity = 0;
            }
                
            RenderSettings.skybox = nighttime;
            cooldown = 1800;
            speed = 0.1f;
        }
    }
}
