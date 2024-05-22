using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitchDetection : MonoBehaviour
{
    private SpriteRenderer sr;
    AudioSource audioSource;
    public string selectedDevice;
    public int duration = 8;
    private float origin = 1.5014f;
    public static float[] samples = new float[128];
    string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    public Text pitchText;
    public string frequency = "detected frequency";
    public string note = "detected note";
    public UpdateImage updateImage;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (Microphone.devices.Length > 0)
        {
            selectedDevice = Microphone.devices[0].ToString();
            audioSource.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(selectedDevice) > 0))
            {
                audioSource.Play();
            }
        }
        // It is recommended to estimate at appropriate time intervals.
        // This example runs every 0.1 seconds.
    }
    void Update()
    {
        InvokeRepeating("EstimatePitch", 0, 0.1f);
    }
    void EstimatePitch()
    {
        // estimator = GetComponent<AudioPitchEstimator>();
        // Estimates fundamental frequency from AudioSource output.
        // float frequency = AudioPitchEstimator.instance.Estimate(audioSource);
        float freq = AudioPitchEstimator.instance.Estimate(audioSource), deviation = 0.0f;
        frequency = freq.ToString() + " Hz";


        GameObject s6 = GameObject.Find("E2");
        Image r6 = s6.GetComponent<Image>();
        GameObject s5 = GameObject.Find("A2");
        Image r5 = s5.GetComponent<Image>();
        GameObject s4 = GameObject.Find("G3");
        Image r4 = s4.GetComponent<Image>();
        GameObject s3 = GameObject.Find("B3");
        Image r3 = s3.GetComponent<Image>();
        GameObject s2 = GameObject.Find("D4");
        Image r2 = s2.GetComponent<Image>();
        GameObject s1 = GameObject.Find("E4");
        Image r1 = s1.GetComponent<Image>();

        if (float.IsNaN(freq))
        {

            sr.enabled = false;
            note = "unknown";

        }
        else
        {
            float range;
            sr.enabled = true;
            float noteval = 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
            float f = Mathf.Floor(noteval + 0.5f);
            deviation = Mathf.Floor((noteval - f) * 100.0f);
            int noteIndex = (int)f % 12;
            int octave = (int)Mathf.Floor((noteval + 0.5f) / 12.0f);
            note = noteNames[noteIndex] + " " + octave;

            if (78.41 <= freq && freq <= 86.41)
            {
                // Debug.Log("E2");
                calculateDiffAndMoveIndicator(
                    freq,
                    82.41f,
                    r6,
                    sr
                );
                coroutine();

            }
            else if (106.00 <= freq && freq <= 114.00)
            {
                // Debug.Log("A2");
                calculateDiffAndMoveIndicator(
                    freq,
                    110.0f,
                    r5,
                    sr
                );
                coroutine();
            }
            else if (142.83 <= freq && freq <= 150.83)
            {
                // Debug.Log("G3");
                calculateDiffAndMoveIndicator(
                    freq,
                    146.83f,
                    r4,
                    sr
                );
                coroutine();
            }
            else if (192.00 <= freq && freq <= 200.00)
            {
                // Debug.Log("B3");
                calculateDiffAndMoveIndicator(
                    freq,
                    196.0f,
                    r3,
                    sr
                );
                coroutine();
            }
            else if (242.94 <= freq && freq <= 250.94)
            {
                Debug.Log("D4");
                calculateDiffAndMoveIndicator(
                    freq,
                    246.94f,
                    r2,
                    sr
                );
                coroutine();
            }
            else if (325.63 <= freq && freq <= 333.63)
            {
                // Debug.Log("E4");
                calculateDiffAndMoveIndicator(
                    freq,
                    329.63f,
                    r1,
                    sr
                );
                coroutine();

            }
            // Algorithm detected fundamental frequency.
            // The frequency is stored in the variable `frequency` (in Hz).
            if (pitchText != null)
                pitchText.text = "Detected frequency: " + frequency + "\nDetected note: " + "note";
        }

    }
    private void calculateDiffAndMoveIndicator(
        float freq,
        float pitch_value,
        Image img,
        SpriteRenderer sr
    )
    {
        float range;
        float multiplier = 0.50f;
        float origin = 1.126f;

        range = (freq - pitch_value) * multiplier;
        sr.transform.position = new Vector3(range, origin, 0);
        img.sprite = Resources.Load<Sprite>("tuning");

        if (pitch_value - 1.0f <= freq && freq <= pitch_value + 1.0f)
        {
            img.sprite = Resources.Load<Sprite>("tuned");

        }
    }

    void getOutputData()
    {
        audioSource.GetOutputData(samples, 0);

        float tmp = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            tmp += Mathf.Abs(samples[i]);
        }
        Debug.Log(tmp);
    }
    IEnumerator coroutine()
    {
        yield return new WaitForSeconds(2);
    }
}