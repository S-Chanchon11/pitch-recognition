using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class PitchDetectDemo : MonoBehaviour
{
    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);

    [DllImport("AudioPluginDemo")]
    private static extern int PitchDetectorDebug(float[] data);
    private SpriteRenderer sr;
    private float origin = 1.5014f;
    float[] history = new float[1000];
    float[] debug = new float[65536];
    string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    public string frequency = "detected frequency";
    public string note = "detected note";
    public AudioMixer mixer;
    public Text pitchText;



    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        InvokeRepeating("detectPitch", 0, 0.3f);
    }

    void detectPitch()
    {
        float freq = PitchDetectorGetFreq(0), deviation = 0.0f;
        frequency = freq.ToString() + " Hz";

        GameObject s6 = GameObject.Find("E2");
        GameObject s5 = GameObject.Find("A2");
        GameObject s4 = GameObject.Find("D3");
        GameObject s3 = GameObject.Find("G3");
        GameObject s2 = GameObject.Find("B3");
        GameObject s1 = GameObject.Find("E4");

        Renderer r6 = s6.GetComponent<Renderer>();

        Renderer r5 = s5.GetComponent<Renderer>();

        Renderer r4 = s4.GetComponent<Renderer>();

        Renderer r3 = s3.GetComponent<Renderer>();

        Renderer r2 = s2.GetComponent<Renderer>();

        Renderer r1 = s1.GetComponent<Renderer>();

        // float flg;
        // float length = 4.618f; // 0 : -1.237, 100 : 1, 200 : 3.237 
        
        
        if (freq > 0.0f)
        {
            sr.enabled = false;
            float noteval = 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
            float f = Mathf.Floor(noteval + 0.5f);
            deviation = Mathf.Floor((noteval - f) * 100.0f);
            int noteIndex = (int)f % 12;
            int octave = (int)Mathf.Floor((noteval + 0.5f) / 12.0f);
            note = noteNames[noteIndex] + " " + octave;
            // float range = 0;

            if (78.41 <= freq && freq <= 86.41)
            {
                // flg = freq*100.0f/82.41f;
                calculateDiffAndMoveIndicator(
                    freq,
                    82.41f,
                    r6,
                    sr
                );
                // range = (freq - 82.41f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r6.material.color = Color.green;
                // if (81.41 <= freq && freq <= 83.41)
                // {
                //     r6.material.color = Color.blue;
                // }
            }
            else if (106.00 <= freq && freq <= 114.00)
            {
                calculateDiffAndMoveIndicator(
                    freq,
                    110.0f,
                    r5,
                    sr
                );
                // range = (freq - 110.0f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r5.material.color = Color.green;
                // if (109.00 <= freq && freq <= 111.00)
                // {
                //     r5.material.color = Color.blue;
                // }
            }
            else if (142.83 <= freq && freq <= 150.83)
            {
                calculateDiffAndMoveIndicator(
                    freq,
                    146.83f,
                    r4,
                    sr
                );
                // range = (freq - 146.83f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r4.material.color = Color.green;
            }
            else if (192.00 <= freq && freq <= 200.00)
            {
                calculateDiffAndMoveIndicator(
                    freq,
                    196.0f,
                    r3,
                    sr
                );
                // range = (freq - 196.0f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r3.material.color = Color.green;
            }
            else if (242.94 <= freq && freq <= 250.94)
            {
                calculateDiffAndMoveIndicator(
                    freq,
                    246.94f,
                    r2,
                    sr
                );
                // range = (freq - 246.94f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r2.material.color = Color.green;
            }
            else if (325.63 <= freq && freq <= 333.63)
            {
                calculateDiffAndMoveIndicator(
                    freq,
                    329.63f,
                    r1,
                    sr
                );
                // range = (freq - 329.63f) * multiplier;

                // sr.transform.position = new Vector3(range, origin, 0);
                // r1.material.color = Color.green;
            }
        }
        else
        {
            // sr.transform.position = new Vector3(1, origin, 0);
            sr.enabled = false;
            note = "unknown";
        }

        if (pitchText != null)
            pitchText.text = "Detected frequency: " + frequency + "\nDetected note: " + note + " (deviation: " + deviation + " cents)";
    }

    private void calculateDiffAndMoveIndicator(
        float freq,
        float pitch_value,
        Renderer rd,
        SpriteRenderer sr
    )
    {
        // if pitch reached first or second quarter convert it into blue color
        float range;
        float multiplier = 0.50f;
        float origin = 1.5014f;

        range = (freq - pitch_value) * multiplier;
        sr.transform.position = new Vector3(range, origin, 0);
        rd.material.color = Color.green;
        if (pitch_value-1.0f <= freq && freq <= pitch_value+1.0f)
        {
            rd.material.color = Color.blue;
        }
    }

}
