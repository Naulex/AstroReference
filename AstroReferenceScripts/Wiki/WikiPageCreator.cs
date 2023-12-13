using System.Data;
using UnityEngine;
using UnityEngine.UI;
public class WikiPageCreator : MonoBehaviour
{
    public Text Name;
    public Text TwoWords;
    public Text Text;
    public Text AfterWords;
    public Image ArticleIMG;
    public Canvas WikiCanvas;
    public Canvas WikiPageCanvas;
    public Canvas ImgCanvas;
    public GameObject TTSObject;
    public GameObject Img;
    public AudioClip[] AudioToSpeech;
    public GameObject HorizontalScrollArea;
    int LastCatID;
    int GlobalPageID;
    public AudioSource _audio;
    public GameObject camera;
    void Start()
    {
        ImgCanvas.enabled = false;
        TTSObject.GetComponent<SpeechRecognizer>().enabled = false;
    }

    public void WikiCreator(int PageID, int _LastCatID)
    {
        LastCatID = _LastCatID;
        HorizontalScrollArea.active = true;
        WikiPageCanvas.enabled = true;
        GlobalPageID = PageID;
        DataTable Article = DatabaseWorker.GetTable("SELECT * FROM WikiArticle WHERE PageID = " + PageID + ";");
        DataTable ImgDB = DatabaseWorker.GetTable("SELECT Content, ContentTable.ContentID FROM ContentTable, MediaWikiComparator, WikiArticle WHERE ContentTable.ContentID = MediaWikiComparator.ContentID AND MediaWikiComparator.MediaID = WikiArticle.MediaID AND WikiArticle.ID = " + PageID.ToString() + ";");
        Sprite mySprite1;
        Texture2D Tex2D1;
        Tex2D1 = new Texture2D(2, 2);
        Tex2D1.LoadImage(System.Convert.FromBase64String((string)ImgDB.Rows[0].ItemArray[0]));
        mySprite1 = Sprite.Create(Tex2D1, new Rect(0.0f, 0.0f, Tex2D1.width, Tex2D1.height), new Vector2(0.5f, 0.5f), 100.0f);
        ArticleIMG.overrideSprite = mySprite1;
        Name.text = Crypt.Decrypt(Article.Rows[0][1].ToString());
        Text.text = Crypt.Decrypt(Article.Rows[0][3].ToString());
        AfterWords.text = Crypt.Decrypt(Article.Rows[0][4].ToString());
        var BTN = AfterWords.GetComponent<Button>();
        BTN.onClick.RemoveAllListeners();
        BTN.onClick.AddListener(delegate { Application.OpenURL(Crypt.Decrypt(Article.Rows[0][2].ToString()));});
        Image[] _image;
        _image = WikiPageCanvas.GetComponentInChildren<HorizontalLayoutGroup>().GetComponentsInChildren<Image>();
        Sprite mySprite;
        int _index = 0;
        int ImgLenght = ImgDB.Rows.Count;
        if (Article.Rows[0][7].ToString() != "0")
        {
            foreach (var TempImg in _image)
            {
                try
                {
                    Texture2D Tex2D;
                    Tex2D = new Texture2D(2, 2);
                    Tex2D.LoadImage(System.Convert.FromBase64String((string)ImgDB.Rows[_index].ItemArray[0]));
                    mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                    TempImg.overrideSprite = mySprite;
                    TempImg.GetComponent<Button>().name = ImgDB.Rows[_index][1].ToString();
                    TempImg.GetComponent<Button>().onClick.AddListener(delegate { ShowImage(int.Parse(TempImg.GetComponent<Button>().name)); });
                }
                catch
                {
                }
                _index++;
            }
            Canvas.ForceUpdateCanvases();
            WikiPageCanvas.transform.GetComponentInChildren<HorizontalLayoutGroup>().enabled = false;
            WikiPageCanvas.transform.GetComponentInChildren<HorizontalLayoutGroup>().enabled = true;
        }
        else
        {
            Canvas.ForceUpdateCanvases();
            HorizontalScrollArea.active = false;
        }
    }
    public void ReadArticle()
    {
        _audio.clip = AudioToSpeech[GlobalPageID - 1];
        _audio.Play();
    }

    public void EnableTTS()
    {
        TTSObject.GetComponent<SpeechRecognizer>().enabled = true;
        TTSObject.GetComponent<SpeechRecognizer>().SetGlobalAudio(AudioToSpeech[GlobalPageID - 1]);
    }


    public void ShowImage(int PicId)
    {
        ImgCanvas.enabled = true;
        DataTable ImgDB = DatabaseWorker.GetTable("SELECT Content, Description FROM ContentTable WHERE ContentID =" + PicId.ToString() + ";");
        Sprite mySprite;
        Texture2D Tex2D;
        Tex2D = new Texture2D(2, 2);
        Tex2D.LoadImage(System.Convert.FromBase64String((string)ImgDB.Rows[0].ItemArray[0]));
        mySprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        ImgCanvas.GetComponentsInChildren<Image>()[1].overrideSprite = mySprite;
        ImgCanvas.GetComponentInChildren<Text>().text = Crypt.Decrypt(ImgDB.Rows[0].ItemArray[1].ToString());
        Canvas.ForceUpdateCanvases();
        ImgDB.Dispose();
    }
    public void BackToArticles()
    {
        WikiCanvas.enabled = true;
        WikiCanvas.GetComponent<WIkiListCreator>().ShowCategory(LastCatID);
        WikiPageCanvas.enabled = false;

    }

    public void BackToArticle1()
    {
        WikiPageCanvas.enabled = true;
        ImgCanvas.enabled = false;
        Canvas.ForceUpdateCanvases();
    }

}
