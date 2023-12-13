using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Data;

public class LevelSwitcher : MonoBehaviour
{
    //private AsyncOperation async;
    public GameObject LongLoad;
    void Start()
    {
        LongLoad.active = false;
    }

    public void LoadWiki()
    {
        LongLoad.active = true;
        LoadWiki2();
    }

    private void LoadWiki2()
    {
        SceneManager.LoadScene("WikiPage");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void LoadARMode()
    {
        LongLoad.active = true;
        LoadARMode2();
    }
    private void LoadARMode2()
    {
        SceneManager.LoadScene("ARPageList");
    }

    public void LoadVRMode()
    {
        LongLoad.active = true;
        LoadVRMode2();
    }
    public void LoadVRMode2()
    {
        SceneManager.LoadScene("VRLevel");
    }

    public void OpenSite()
    {
        DataTable Address = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
        Application.OpenURL(Crypt.Decrypt(Address.Rows[0][1].ToString()));
        Address.Dispose();
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}
