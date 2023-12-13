using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARScaleChanger : MonoBehaviour
{
    public GameObject ARModel;
    public Slider Slider;
    public Canvas Anchor;
    public GameObject targetParent;
    public Text Desc;
    public GameObject DescField;
    public GameObject TTSObject;

    private void Start()
    {
        Anchor.enabled = false;
        string stringToSave = Crypt.Decrypt(PlayerPrefs.GetString("ARModel"));
        string ARID = Crypt.Decrypt(PlayerPrefs.GetString("ARID"));
        ARModel = Instantiate(Resources.Load(stringToSave, typeof(GameObject))) as GameObject;
        ARModel.transform.position = new Vector3(0, 0.5f, 0);
        ARModel.transform.SetParent(targetParent.transform);
        Desc.text = Crypt.Decrypt(DatabaseWorker.GetTable("SELECT * FROM ARList WHERE ID = " + ARID + ";").Rows[0][5].ToString());
        DescField.active = false;
    }
    public void EnableTTS()
    {
        TTSObject.GetComponent<SpeechRecognizer>().enabled = true;
    }
    public void ShowDesc()
    {
        DescField.active = true;
    }
    public void HideDesc()
    {
        DescField.active = false;
    }
    public void TryUpscale()
    {
        try { ARModel.transform.localScale = ARModel.transform.localScale + new Vector3(0.1f * Slider.value, 0.1f * Slider.value, 0.1f * Slider.value); }
        catch { }
    }
    public void TryDownscale()
    {
        try
        { ARModel.transform.localScale = ARModel.transform.localScale - new Vector3(0.1f * Slider.value, 0.1f * Slider.value, 0.1f * Slider.value); }
        catch { }
    }
    public void MoveLeft()
    {
        try
        { ARModel.transform.position = ARModel.transform.position - new Vector3(1 * Slider.value, 0, 0); }
        catch { }
    }
    public void MoveRight()
    {
        try
        { ARModel.transform.position = ARModel.transform.position + new Vector3(1 * Slider.value, 0, 0); }
        catch { }
    }
    public void MoveBack()
    {
        try
        { ARModel.transform.position = ARModel.transform.position + new Vector3(0, 0, 1 * Slider.value); }
        catch { }
    }
    public void MoveForward()
    {
        try
        { ARModel.transform.position = ARModel.transform.position - new Vector3(0, 0, 1 * Slider.value); }
        catch { }
    }
    public void TurnLeft()
    {
        try
        { ARModel.transform.eulerAngles = ARModel.transform.eulerAngles + new Vector3(0, 10 * Slider.value, 0); }
        catch { }
    }
    public void TurnRight()
    {
        try
        { ARModel.transform.eulerAngles = ARModel.transform.eulerAngles - new Vector3(0, 10 * Slider.value, 0); }
        catch { }
    }
    public void TurnUp()
    {
        try
        { ARModel.transform.eulerAngles = ARModel.transform.eulerAngles + new Vector3(10 * Slider.value, 0, 0); }
        catch { }
    }

    public void TurnDown()
    {
        try
        {
            ARModel.transform.eulerAngles = ARModel.transform.eulerAngles - new Vector3(10 * Slider.value, 0, 0);
        }
        catch
        { }
    }
    public void ShowAnchor()
    {
        Anchor.enabled = true;
    }

    public void HideAnchor()
    {
        Anchor.enabled = false;
    }

    public void ExitToMenu()
    {
        try
        {
            VuforiaApplication.Instance.Deinit();
            Screen.orientation = ScreenOrientation.Portrait;
            SceneManager.LoadScene("ARPageList");
        }
        catch
        { }
    }
}
