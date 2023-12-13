using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Toggle AutoUpdateRemain;
    public Toggle AutoStartAstra;
    public Toggle AltMove;
    public Canvas SiteChangeCanvas;
    public InputField NewSiteText;
    public Slider SensSlider;
    public Text SensText;
    private int _sens;

    int _HiddenTouchCounter = 0;

    public void HiddenSettingsOpener()
    {
        _HiddenTouchCounter++;
        if (_HiddenTouchCounter >= 10)
            ShowSiteChange();
    }
    void ShowSiteChange()
    {
        SiteChangeCanvas.enabled = true;
        NewSiteText.text = Crypt.Decrypt(DatabaseWorker.GetTable("SELECT WebAddress FROM NetWorkData WHERE id = 1;").Rows[0][0].ToString());
        _HiddenTouchCounter = 0;
    }
    public void CloseAndSaveSiteChange()
    {
        if (NewSiteText.text.Length != 0)
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE NetWorkData SET WebAddress = \"" + Crypt.Encrypt(NewSiteText.text) + "\" WHERE ID = 1;");
            SiteChangeCanvas.enabled = false;
        }
        _HiddenTouchCounter = 0;
    }
    public void CloseSiteChange()
    {
        SiteChangeCanvas.enabled = false;
        _HiddenTouchCounter = 0;
    }
    void Start()
    {
        SiteChangeCanvas.enabled = false;
        DataTable Settings = DatabaseWorker.GetTable("SELECT * FROM Settings;");
        if (int.Parse(Crypt.Decrypt(Settings.Rows[0][2].ToString())) == 1)
        {
            AutoUpdateRemain.isOn = true;
        }
        else
        {
            AutoUpdateRemain.isOn = false;
        }
        if (int.Parse(Crypt.Decrypt(Settings.Rows[1][2].ToString())) == 1)
        {
            AutoStartAstra.isOn = true;
        }
        else
        {
            AutoStartAstra.isOn = false;
        }
        if (int.Parse(Crypt.Decrypt(Settings.Rows[2][2].ToString())) == 1)
        {
            AltMove.isOn = true;
        }
        else
        {
            AltMove.isOn = false;
        }
        _sens = int.Parse(Crypt.Decrypt(Settings.Rows[3][2].ToString()));
        SensSlider.value = _sens;
        SensText.text = _sens.ToString();
    }
    public void onChangeSlider()
    {
        _sens = (int)SensSlider.value;
        SensText.text = _sens.ToString();
    }

    public void SaveAndExit()
    {
        if (AutoUpdateRemain.isOn == true)
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"Y84iUwhH8oGaUObG0fO7JQ==\" WHERE SettingString = \"l2WqlmMIBw2dExB3fmp/5Qjs+sJzZn67LsO43Xco+Hk=\"");
        }
        else
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"TMU7WjxGtPjPuBAsB0MBvA==\" WHERE SettingString = \"l2WqlmMIBw2dExB3fmp/5Qjs+sJzZn67LsO43Xco+Hk=\"");
        }

        if (AutoStartAstra.isOn == true)
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"Y84iUwhH8oGaUObG0fO7JQ==\" WHERE SettingString = \"18aTSY+sUIJ3Wby4nbJ8sw==\"");
        }
        else
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"TMU7WjxGtPjPuBAsB0MBvA==\" WHERE SettingString = \"18aTSY+sUIJ3Wby4nbJ8sw==\"");
        }
        if (AltMove.isOn == true)
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"Y84iUwhH8oGaUObG0fO7JQ==\" WHERE SettingString = \"x26mrokjPZp6jb5Kj+Tu+g==\"");
        }
        else
        {
            DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"TMU7WjxGtPjPuBAsB0MBvA==\" WHERE SettingString = \"x26mrokjPZp6jb5Kj+Tu+g==\"");
        }
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"" + Crypt.Encrypt(_sens.ToString()) + "\" WHERE SettingString = \"SXxlQYxKfOFytRAO6CIvAg==\"");
        SceneManager.LoadScene("MainMenu");

    }
}
