using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AREnabler : MonoBehaviour
{
    public GameObject ARCamera;
    void Start()
    {
#if !UNITY_EDITOR
        VuforiaApplication.Instance.Initialize();
        Screen.orientation = ScreenOrientation.Landscape;
#endif
    }
}
