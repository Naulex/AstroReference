using UnityEngine;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARPageCreator : MonoBehaviour
{
    public GameObject Card;
    public GameObject Content;
    public Canvas CanvasWikiPage;
    public InputField SearchInput;
    public Button CleanFavsbtn;
    public GameObject TTS;
    void Start()
    {
        RefreshEverything();
        CleanFavsbtn.enabled = false;
    }

    public void EnableTTS()
    {
        TTS.GetComponent<SpeechRecognizer>().enabled = true;
    }

    public void SaveAndStartAR(string Model, string ID)
    {
        PlayerPrefs.SetString("ARModel", Crypt.Encrypt(Model));
        PlayerPrefs.SetString("ARID", Crypt.Encrypt(ID));
        PlayerPrefs.Save();
        SceneManager.LoadScene("ARLevel");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RefreshEverything()
    {

        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM ARList;");

        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Text>()[4].text = Crypt.Decrypt(Wikilist.Rows[i][4].ToString());
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { SaveAndStartAR(ArticleCard.GetComponentsInChildren<Text>()[4].text, ArticleCard.GetComponentsInChildren<Text>()[2].text); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<ARPageCreator>().AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text)); });
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            Sprite mySprite;
            Texture2D Tex2D;
            Tex2D = new Texture2D(2, 2);
            Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text) - 1].ItemArray[2]));
            mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
    }


    public void Search()
    {
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM ARList WHERE Name LIKE \"%" + Crypt.Encrypt(SearchInput.text) + "%\";");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Crypt.Decrypt(Wikilist.Rows[i][0].ToString());
            ArticleCard.GetComponentsInChildren<Text>()[4].text = Crypt.Decrypt(Wikilist.Rows[i][4].ToString());
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { SaveAndStartAR(ArticleCard.GetComponentsInChildren<Text>()[4].text, ArticleCard.GetComponentsInChildren<Text>()[2].text); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<ARPageCreator>().AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text)); });
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            Sprite mySprite;
            Texture2D Tex2D;
            Tex2D = new Texture2D(2, 2);
            Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[i].ItemArray[2]));
            mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
    }
    public void AddToFavs(int ID)
    {
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE ARList SET isFav = 1 WHERE ID = " + ID + ";");
    }

    public void ShowFavs()
    {
        CleanFavsbtn.enabled = true;
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM ARList WHERE isFav = 1;");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Text>()[4].text = Crypt.Decrypt(Wikilist.Rows[i][4].ToString());
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { SaveAndStartAR(ArticleCard.GetComponentsInChildren<Text>()[4].text, ArticleCard.GetComponentsInChildren<Text>()[2].text); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<ARPageCreator>().AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text)); });
            ArticleCard.transform.SetParent(Content.transform, true);
            Destroy(ArticleCard.GetComponentsInChildren<Button>()[1].gameObject);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            Sprite mySprite;
            Texture2D Tex2D;
            Tex2D = new Texture2D(2, 2);
            DataTable ImgDB = DatabaseWorker.GetTable("SELECT * FROM ARList;");
            Tex2D.LoadImage(System.Convert.FromBase64String((string)ImgDB.Rows[int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text) - 1].ItemArray[2]));
            mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
    }

    public void CleanFavs()
    {
        DatabaseWorker.ExecuteQueryWithAnswer("UPDATE ARList SET isFav = 0 WHERE isFav = 1;");
        CleanFavsbtn.enabled = true;
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("ArticleCard"))
        {
            Destroy(child);
        }
        DataTable Wikilist = DatabaseWorker.GetTable("SELECT * FROM ARList WHERE isFav = 0;");
        for (int i = 0; i < Wikilist.Rows.Count; i++)
        {
            GameObject ArticleCard = Instantiate<GameObject>(Card, new Vector3(0, 0, 0), Quaternion.identity);
            ArticleCard.transform.SetParent(Content.transform);
            ArticleCard.transform.localScale = new Vector3(1, 1, 1);
            ArticleCard.GetComponentsInChildren<Text>()[2].text = Wikilist.Rows[i][0].ToString();
            ArticleCard.GetComponentsInChildren<Text>()[4].text = Crypt.Decrypt(Wikilist.Rows[i][4].ToString());
            ArticleCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { SaveAndStartAR(ArticleCard.GetComponentsInChildren<Text>()[4].text, ArticleCard.GetComponentsInChildren<Text>()[2].text); });
            ArticleCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { CanvasWikiPage.GetComponent<ARPageCreator>().AddToFavs(int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text)); });
            ArticleCard.transform.SetParent(Content.transform, true);
            ArticleCard.GetComponentsInChildren<Text>()[0].text = Crypt.Decrypt(Wikilist.Rows[i][1].ToString());
            Sprite mySprite;
            Texture2D Tex2D;
            Tex2D = new Texture2D(2, 2);
            Tex2D.LoadImage(System.Convert.FromBase64String((string)Wikilist.Rows[int.Parse(ArticleCard.GetComponentsInChildren<Text>()[2].text) - 1].ItemArray[2]));
            mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            ArticleCard.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
            ArticleCard.name = i.ToString();
        }
        Canvas.ForceUpdateCanvases();
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = false;
        CanvasWikiPage.transform.GetComponentInChildren<VerticalLayoutGroup>().enabled = true;
    }

}
