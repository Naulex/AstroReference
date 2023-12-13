using UnityEngine;

public class Teleport : MonoBehaviour
{
    private string MovingMode = "0";
    private int moveTimer = 0;
    private int MovingCounter;

    private void Start()
    {
        if (Crypt.Decrypt(DatabaseWorker.GetTable("SELECT Status FROM Settings WHERE SettingString = \"x26mrokjPZp6jb5Kj+Tu+g==\";").Rows[0][0].ToString()) == "0")
        { MovingMode = "0"; }
        else
        { MovingMode = "1"; }
        try
        {
            MovingCounter = int.Parse(Crypt.Decrypt(DatabaseWorker.GetTable("SELECT Status FROM Settings WHERE SettingString = \"SXxlQYxKfOFytRAO6CIvAg==\";").Rows[0][0].ToString()));
        }
        catch
        {
            MovingCounter = 20;
        }
    }
    void FixedUpdate()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit, 10f) && (hit.rigidbody != null))
        {
            if (MovingMode == "0")
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (hit.collider.tag == "Teleport")
                        {
                            transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1.7f, hit.transform.position.z);
                        }
                    }
                }
            }
            else
            {
                if (hit.collider.tag == "Teleport")
                {
                    moveTimer++;
                }
                else
                { moveTimer = 0; }
                if (moveTimer == MovingCounter)
                {
                    transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1.7f, hit.transform.position.z);
                    moveTimer = 0;
                }
            }
        }
    }
}
