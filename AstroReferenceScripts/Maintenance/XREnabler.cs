using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class XREnabler : MonoBehaviour
{
    void Start()
    {
        StartXRCoroutine();
        StartXR(0);
        Screen.orientation = ScreenOrientation.Landscape;
    }
    XRLoader m_SelectedXRLoader;
    void StartXR(int loaderIndex)
    {
        if (m_SelectedXRLoader == null)
        { m_SelectedXRLoader = XRGeneralSettings.Instance.Manager.activeLoaders[loaderIndex]; }
        StartCoroutine(StartXRCoroutine());
    }
    IEnumerator StartXRCoroutine()
    {
        var initSuccess = m_SelectedXRLoader.Initialize();
        if (!initSuccess)
        {  }
        else
        {
            yield return null;
            var startSuccess = m_SelectedXRLoader.Start();
            if (!startSuccess)
            {
                yield return null;
                m_SelectedXRLoader.Deinitialize();
            }
        }
    }
    public void StopXR()
    {
        m_SelectedXRLoader.Stop();
        m_SelectedXRLoader.Deinitialize();
        m_SelectedXRLoader = null;
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
