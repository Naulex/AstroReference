using UnityEngine;
using System.Data;
public class ARPrinter : MonoBehaviour
{
    public void OpenPrintDialog()
    {
        DataTable NetSettings = DatabaseWorker.GetTable("SELECT * FROM NetWorkData;");
        Application.OpenURL(Crypt.Decrypt(NetSettings.Rows[0][1].ToString()) + Crypt.Decrypt(NetSettings.Rows[10][1].ToString()));
    }

}