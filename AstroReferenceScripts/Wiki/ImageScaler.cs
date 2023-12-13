using UnityEngine;

public class ImageScaler : MonoBehaviour
{
    public GameObject Image;
    public void TryUpscale()
    {
        try
        {
            Image.transform.localScale = Image.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
        }
        catch
        { }
    }
    public void TryDownscale()
    {
        try
        {
            Image.transform.localScale = Image.transform.localScale - new Vector3(0.1f , 0.1f, 0.1f);
        }
        catch
        { }
    }
    public void MoveLeft()
    {
        try
        {
            Image.transform.position = Image.transform.position - new Vector3(100, 0, 0);
        }
        catch
        { }
    }
    public void MoveRight()
    {
        try
        {
            Image.transform.position = Image.transform.position + new Vector3(100, 0, 0);
        }
        catch
        { }
    }
    public void MoveBack()
    {
        try
        {
            Image.transform.position = Image.transform.position - new Vector3(0, 100, 0);
        }
        catch
        { }
    }
    public void MoveForward()
    {
        try
        {
            Image.transform.position = Image.transform.position + new Vector3(0, 100, 0);
        }
        catch
        { }
    }
}
