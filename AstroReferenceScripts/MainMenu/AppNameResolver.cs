using System.Data;
using UnityEngine;

public class AppNameResolver : MonoBehaviour
{
    public GameObject TTSCanvas;
    public Canvas ShareBlock;
    void Start()
    {
        ShareBlock.enabled = false;
        DataTable Settings = DatabaseWorker.GetTable("SELECT * FROM Settings WHERE SettingString = \"18aTSY+sUIJ3Wby4nbJ8sw==\";");
        if (Crypt.Decrypt(Settings.Rows[0][2].ToString()) == "1")
        {
            EnableTTS();
        }
    }
    public void EnableTTS()
    { TTSCanvas.GetComponent<SpeechRecognizer>().enabled = true; }
    public void OpenContacts()
    {
        Application.OpenURL("mailto:073797@gmail.com");
    }
    public void ShareBlockOpen()
    { ShareBlock.enabled = true; }
    public void ShareBlockClose()
    { ShareBlock.enabled = false; }
}
