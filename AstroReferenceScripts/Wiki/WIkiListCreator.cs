using System.Collections;
using UnityEngine;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WIkiListCreator : MonoBehaviour
{
    public GameObject Card;
    public GameObject Content;
    public Canvas WikiCanvas;
    public Canvas CanvasWikiPage;
    public InputField SearchInput;
    public Button CleanFavsbtn;
    public Text Name;
    public GameObject TTSObject;
    int LastCatID;
    void Start()
    {
        CleanFavsbtn.enabled = false;
        WikiCanvas.enabled = false;
        try
        {
           StartCoroutine(RefreshEverything());
        }
        catch
        { } 
    }

    public void ExitToMenu()
    {
        DatabaseWorker.fileName = "DB.bytes";
        DatabaseWorker.DBPath = DatabaseWorker.GetDatabasePath();
        SceneManager.LoadScene("MainMenu");
    }

    public void CallRefreshEverything()
    {
        StartCoroutine(RefreshEverything());
    }

    public IEnumerator RefreshEverything()
    {
        yield return null;
        Name.text = "Каталог статей";
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        WikiCanvas.enabled = false;
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM WikiCategorys;");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { LastCatID = int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text); CanvasWikiPage.GetComponent<WIkiListCreator>().StartCoroutine(ShowCategory(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text))); });
            Destroy(ArticleCard.GetComponentsInChildren<Button>()[1].gameObject);
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            try
            {
                Sprite mySprite;
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);
                Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[2]));
                mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            }
            catch
            { }
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        StopCoroutine(RefreshEverything());
        
    }

    public IEnumerator ShowCategory(int CategoryID)
    {
        yield return null;
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        WikiCanvas.enabled = false;
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM WikiList WHERE CategoryID = " + CategoryID + "; ");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { WikiCanvas.GetComponent<WikiPageCreator>().WikiCreator(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text), LastCatID); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<WIkiListCreator>().StartCoroutine(AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text))); });
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            try
            {
                Sprite mySprite;
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);
                Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[3]));
                mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            }
            catch
            { }
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        StopCoroutine(ShowCategory(CategoryID));
    }

    public void TryToFindOnline()
    {
        StartCoroutine(GetOnline());
    }
    IEnumerator GetOnline()
    {
        DataTable NetSettings = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
        UnityWebRequest GetManifestBool = new UnityWebRequest();
        GetManifestBool = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[7][1].ToString()));
        GetManifestBool.SetRequestHeader("Accept", "*/*");
        GetManifestBool.SetRequestHeader("Accept-Encoding", "gzip, deflate");
        GetManifestBool.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
        GetManifestBool.chunkedTransfer = false;
        GetManifestBool.useHttpContinue = false;
        yield return GetManifestBool.Send();

        if (!GetManifestBool.isNetworkError || !GetManifestBool.isHttpError)
        {

            if (GetManifestBool.downloadHandler.text == "1")
            {
                UnityWebRequest GetManifestText = new UnityWebRequest();
                GetManifestText = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[8][1].ToString()));
                GetManifestText.SetRequestHeader("Accept", "*/*");
                GetManifestText.SetRequestHeader("Accept-Encoding", "gzip, deflate");
                GetManifestText.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                GetManifestText.chunkedTransfer = false;
                GetManifestText.useHttpContinue = false;
                yield return GetManifestText.Send();
                Name.text = "Доп. контент";
                string phrase = GetManifestText.downloadHandler.text;
                string[] words = phrase.Split('\n');
                WikiCanvas.enabled = false;
                foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
                {
                    Destroy(child);
                }

                for (int i = 0; i < words.Length; i = i + 3)
                {
                    GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
                    ArticleCard.transform.SetParent(Content.transform);
                    ArticleCard.transform.localScale = new Vector3(1, 1, 1);
                    ArticleCard.GetComponentsInChildren<Text>()[2].text = words[i + 2].ToString();
                    Destroy(ArticleCard.GetComponentsInChildren<Button>()[1].gameObject);
                    ArticleCard.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Скачать";
                    ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<WIkiListCreator>().StartCoroutine(DownloadAndOpenBase(ArticleCard.GetComponentsInChildren<Text>()[2].text)); });
                    ArticleCard.transform.SetParent(Content.transform, true);
                    ArticleCard.GetComponentsInChildren<Text>()[0].text = words[i].ToString();
                    try
                    {
                        Sprite mySprite;
                        Texture2D Tex2D;
                        Tex2D = new Texture2D(2, 2);
                        Tex2D.LoadImage(System.Convert.FromBase64String(words[i + 1]));
                        mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                        ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
                    }
                    catch
                    { }
                    ArticleCard.name = i.ToString();
                }
                GetManifestBool.Dispose();
                GetManifestText.Dispose();
            }
            else
            { }
            Canvas.ForceUpdateCanvases();
            CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
            CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        }
    }

    public IEnumerator DownloadAndOpenBase(string url)
    {
        DataTable NetSettings = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
        UnityWebRequest GetManifestBool = new UnityWebRequest();
        GetManifestBool = UnityWebRequest.Get(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[1][1].ToString()) + "/" + url);
        GetManifestBool.SetRequestHeader("Accept", "*/*");
        GetManifestBool.SetRequestHeader("Accept-Encoding", "gzip, deflate");
        GetManifestBool.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
        GetManifestBool.chunkedTransfer = false;
        GetManifestBool.useHttpContinue = false;
        yield return GetManifestBool.Send();
        if (GetManifestBool.isNetworkError || GetManifestBool.isHttpError)
        {
            Debug.Log(GetManifestBool.error);
        }
        else
        {
            string savePath = string.Format("{0}/{1}.bytes", Application.persistentDataPath, "ADDDB");
            System.IO.File.WriteAllBytes(savePath, GetManifestBool.downloadHandler.data);
            DatabaseWorker.fileName = "ADDDB.bytes";
            DatabaseWorker.DBPath = DatabaseWorker.GetDatabasePath();
            StartCoroutine(RefreshEverything());
        }
    }
    
    public void Search()
    {
        StartCoroutine(SearchEnum());
    }
    public IEnumerator SearchEnum()
    {
        yield return null;
        Name.text = "Поиск";
        WikiCanvas.enabled = false;
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM WikiList WHERE Head LIKE lower(\"%" + Crypt.Encrypt(SearchInput.text) + "%\");");

        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { WikiCanvas.GetComponent<WikiPageCreator>().WikiCreator(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text), LastCatID); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<WIkiListCreator>().StartCoroutine(AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text))); });
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());

            try
            {
                Sprite mySprite;
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);
                Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[3]));
                mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            }
            catch
            { }
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        StopCoroutine(SearchEnum());
    }

    public IEnumerator AddToFavs(int ID)
    {
        yield return null;
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE WikiList SET isFav = 1 WHERE ID = " + ID + ";");
        StopCoroutine(AddToFavs(ID));
    }

    public void ShowFavs()
    {
        StartCoroutine(ShowFavsEnum());
    }
    public IEnumerator ShowFavsEnum()
    {
        yield return null;
        Name.text = "Избранное";
        WikiCanvas.enabled = false;
        CleanFavsbtn.enabled = true;
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }

        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM WikiList WHERE isFav = 1;");

        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentInChildren<Button>().onClick.AddListener(delegate { WikiCanvas.GetComponent<WikiPageCreator>().WikiCreator(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text), LastCatID); });
            ArticleCard.transform.SetParent(Content.transform, true);
            Destroy(ArticleCard.GetComponentsInChildren<Button>()[1].gameObject);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            try
            {
                Sprite mySprite;
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);
                Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[3]));
                mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            }
            catch
            { }
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        StopCoroutine(ShowFavsEnum());
    }
    public void EnableTTS()
    {
        TTSObject.GetComponent<SpeechRecognizer>().enabled = true;
    }


    public void CleanFavs()
    {
        StartCoroutine(CleanFavsEnum());
    }    
    public IEnumerator CleanFavsEnum()
    {
        yield return null;
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE WikiList SET isFav = 0 WHERE isFav = 1;");
        WikiCanvas.enabled = false;
        CleanFavsbtn.enabled = true;
        Name.text = "Каталог статей";

        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        WikiCanvas.enabled = false;
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM WikiCategorys;");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { LastCatID = int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text); CanvasWikiPage.GetComponent<WIkiListCreator>().StartCoroutine(ShowCategory(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text))); });
            Destroy(ArticleCard.GetComponentsInChildren<Button>()[1].gameObject);
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            try
            {
                Sprite mySprite;
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);
                Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[2]));
                mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            }
            catch
            { }
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
        StopCoroutine(CleanFavsEnum());
    }
}
