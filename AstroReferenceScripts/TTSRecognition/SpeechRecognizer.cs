using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using static SpeechRecognizerPlugin;
using System.Data;

public class SpeechRecognizer : MonoBehaviour, ISpeechRecognizerPlugin
{
    [SerializeField] private Button startListeningBtn = null;
    // [SerializeField] private Button stopListeningBtn = null;
    //[SerializeField] private Toggle continuousListeningTgle = null;
    //[SerializeField] private TMP_Dropdown languageDropdown = null;
    //[SerializeField] private TMP_InputField maxResultsInputField = null;
    [SerializeField] private Text resultsTxt = null;
    // [SerializeField] private TextMeshProUGUI errorsTxt = null;
    private SpeechRecognizerPlugin plugin = null;
    public AudioSource _audio;
    public GameObject camera;
    AudioClip GlobalAudio;
    public AudioClip SoundNull;
    public AudioClip WhatICan;
    public AudioClip Hello;

    IEnumerator TTSResponser(string text)
    {
        Debug.Log(text);
        text.Replace("-", "");
        text.Replace("—", "");
        text.Replace("–", "");
        text.Replace("(", "");
        text.Replace(")", "");
        text.Replace("~", "");
        text.Replace("\n", "+");
        text.Replace(" ", "+");
        Debug.Log(text);
        Debug.Log(text.Length);
        if (text.Length! >= 144)
        {
            string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + text + "&tl=RU-ru";
            WWW www = new WWW(url);
            yield return www;
            _audio.clip = www.GetAudioClip(false, true, AudioType.MPEG);
            _audio.Play();
        }
    }

    public void SetGlobalAudio(AudioClip ClipToRead)
    {
        GlobalAudio = ClipToRead;
    }

    public void Start()
    {
        plugin = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        startListeningBtn.onClick.AddListener(StartListening);
    }

    private void StartListening()
    {
        plugin.StartListening();
    }

    private void StopListening()
    {
        plugin.StopListening();
    }

    private void SetContinuousListening(bool isContinuous)
    {
        plugin.SetContinuousListening(isContinuous);
    }

    private void SetLanguage(int dropdownValue)
    {
        //string newLanguage = languageDropdown.options[dropdownValue].text;
        //plugin.SetLanguageForNextRecognition(newLanguage);
    }

    private void SetMaxResults(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            return;

        int maxResults = int.Parse(inputValue);
        plugin.SetMaxResultsForNextRecognition(maxResults);
    }

    public void TTSRead()
    {
        _audio.clip = GlobalAudio;
        _audio.Play();
    }

    public void OnResult(string recognizedResult)
    {
        string _text;
        char[] delimiterChars = { '~' };
        string[] result = recognizedResult.Split(delimiterChars);
        resultsTxt.text = "";
        for (int i = 0; i < result.Length; i++)
        {
            resultsTxt.text += result[i].ToLower() + '\n';
        }
        _text = resultsTxt.text;

        if (_text.ToLower().Contains("привет"))
        {
            _audio.clip = Hello;
            _audio.Play();

        }
        else if (_text.ToLower().Contains("умеешь"))
        {
            _audio.clip = WhatICan;
            _audio.Play();

        }
        else if (_text.ToLower().Contains("открой"))
        {
            if (_text.ToLower().Contains("справочник"))
            {
                SceneManager.LoadScene("WikiPage");
            }
            else if (_text.ToLower().Contains("дополненную"))
            {
                SceneManager.LoadScene("ARPageList");
            }
            else if (_text.ToLower().Contains("виртуальную"))
            {
                SceneManager.LoadScene("VRLevel");
            }
            else if (_text.ToLower().Contains("настройки"))
            {
                SceneManager.LoadScene("Settings");
            }
        }
        else if (_text.ToLower().Contains("прямой эфир"))
        {
            Application.OpenURL("https://www.nasa.gov/multimedia/nasatv/iss_ustream.html");
        }
        else if (_text.ToLower().Contains("прочитай"))
        {
            try { GameObject.Find("CanvasWikiPage").GetComponent<WikiPageCreator>().ReadArticle(); }
            catch { }
        }
        else if (_text.ToLower().Contains("закрой"))
        {
            Application.Quit();
        }
    }


    public void OnError(string recognizedError)
    {
        ERROR error = (ERROR)int.Parse(recognizedError);
        switch (error)
        {
            case ERROR.UNKNOWN:
                Debug.Log("<b>ERROR: </b> Unknown");
                break;
            case ERROR.INVALID_LANGUAGE_FORMAT:
                Debug.Log("<b>ERROR: </b> Language format is not valid");
                break;
            default:
                break;
        }
    }
}
