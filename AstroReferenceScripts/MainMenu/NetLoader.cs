using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Data;

public class NetLoader : MonoBehaviour
{
    public Text NetText;
    public Text ChangelogText;
    public Canvas UpdateBlock;
    public GameObject UpdateAvaliable;
    public GameObject AcceptUpdate;
    public GameObject NeverAsk;
    private void Start()
    {
        UpdateBlock.enabled = false;
        UpdateAvaliable.SetActive(false);
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        try
        {
            DataTable NetSettings = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
            UnityWebRequest GetManifestBool = new UnityWebRequest();
            GetManifestBool = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[4][1].ToString()));
            GetManifestBool.SetRequestHeader("Accept", "*/*");
            GetManifestBool.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            GetManifestBool.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
            GetManifestBool.chunkedTransfer = false;
            GetManifestBool.useHttpContinue = false;
            yield return GetManifestBool.Send();

            if (GetManifestBool.downloadHandler.text == "1")
            {
                UnityWebRequest GetManifestText = new UnityWebRequest();
                GetManifestText = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[5][1].ToString()));
                GetManifestText.SetRequestHeader("Accept", "*/*");
                GetManifestText.SetRequestHeader("Accept-Encoding", "gzip, deflate");
                GetManifestText.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                GetManifestText.chunkedTransfer = false;
                GetManifestText.useHttpContinue = false;
                yield return GetManifestText.Send();
                UpdateBlock.enabled = true;
                AcceptUpdate.SetActive(false);
                NeverAsk.SetActive(false);
                NetText.text = GetManifestText.downloadHandler.text;
                GetManifestBool.Dispose();
                GetManifestText.Dispose();
            }
            else
            {
                DataTable Settings = DatabaseWorker.GetTable("SELECT * FROM Settings WHERE SettingString = \"l2WqlmMIBw2dExB3fmp/5Qjs+sJzZn67LsO43Xco+Hk=\";");
                if (Crypt.Decrypt(Settings.Rows[0][2].ToString()) != "1")
                {
                    UnityWebRequest IsUpdates = new UnityWebRequest();
                    IsUpdates = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[2][1].ToString()));
                    IsUpdates.SetRequestHeader("Accept", "*/*");
                    IsUpdates.SetRequestHeader("Accept-Encoding", "gzip, deflate");
                    IsUpdates.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                    IsUpdates.chunkedTransfer = false;
                    IsUpdates.useHttpContinue = false;
                    yield return IsUpdates.Send();
                    if (!IsUpdates.isNetworkError)
                    {

                        if (int.Parse(Application.version) != int.Parse(IsUpdates.downloadHandler.text))
                        {
                            UpdateBlock.enabled = true;
                            UpdateAvaliable.SetActive(true);
                            NetText.text = "Доступно обновление: " + IsUpdates.downloadHandler.text + ".\r\n\r\nВы используете версию: " + Application.version + ".";
                            UnityWebRequest ChangeLog = new UnityWebRequest();
                            ChangeLog = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[6][1].ToString()));
                            ChangeLog.SetRequestHeader("Accept", "*/*");
                            ChangeLog.SetRequestHeader("Accept-Encoding", "gzip, deflate");
                            ChangeLog.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                            ChangeLog.chunkedTransfer = false;
                            ChangeLog.useHttpContinue = false;
                            yield return ChangeLog.Send();
                            ChangelogText.text = ChangeLog.downloadHandler.text;
                            ChangeLog.Dispose();
                        }

                        IsUpdates.Dispose();
                    }
                }

            }
        }
        finally
        { }
    }
    public void GoUpdate()
    {
        DataTable NetSettings = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
        Application.OpenURL(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[3][1].ToString()));
    }
    public void DeclineUpdate()
    {
        UpdateBlock.enabled = false;
    }
    public void NeverAskVoid()
    {
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE Settings SET Status = \"Y84iUwhH8oGaUObG0fO7JQ==\" WHERE SettingString = \"l2WqlmMIBw2dExB3fmp/5Qjs+sJzZn67LsO43Xco+Hk=\"");
        UpdateBlock.enabled = false;
    }

    public void OpenUpdateFromButton()
    {
        UpdateBlock.enabled = true;
    }

}
