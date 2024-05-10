using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PitchDetector2 : MonoBehaviour
{
    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
