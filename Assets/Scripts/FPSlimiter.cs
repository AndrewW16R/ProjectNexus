using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSlimiter : MonoBehaviour
{
    public int targetFPS = 60;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }


}
