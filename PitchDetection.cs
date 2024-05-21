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
        Renderer r6 = s6.GetComponent<Renderer>();
        GameObject s5 = GameObject.Find("A2");
        Renderer r5 = s5.GetComponent<Renderer>();
        GameObject s4 = GameObject.Find("D3");
        Renderer r4 = s4.GetComponent<Renderer>();
        GameObject s3 = GameObject.Find("G3");
        Renderer r3 = s3.GetComponent<Renderer>();
        GameObject s2 = GameObject.Find("B3");
        Renderer r2 = s2.GetComponent<Renderer>();
        GameObject s1 = GameObject.Find("E4");
        Renderer r1 = s1.GetComponent<Renderer>();

        if (float.IsNaN(freq))
        {
            // Algorithm didn't detect fundamental frequency (e.g. silence).
            r6.material.color = Color.white;
            r5.material.color = Color.white;
            r4.material.color = Color.white;
            r3.material.color = Color.white;
            r2.material.color = Color.white;
            r1.material.color = Color.white;
            // sr.transform.position = new Vector3(0,origin,0);
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
    void Update()
    {
        // It is NOT recommended to run the estimation every frame.
        // This will take a high computational load.
        // EstimatePitch();
        // InvokeRepeating("EstimatePitch", 0, 0.1f);
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
        // Color active = new Color(219,241,225,1);
        rd.material.color = new Color(0.86f, 0.95f, 0.88f);
        if (pitch_value - 1.0f <= freq && freq <= pitch_value + 1.0f)
        {
            rd.material.color = Color.green;
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
// {
//     public int sampleRate = 44100; // Sample rate of the audio input
//     public int bufferSize = 1024; // Size of the audio buffer
//     public string microphoneName = null; // Name of the microphone (set to null for default microphone)

//     private float[] audioBuffer;
//     private float[] spectrum;
//     private float pitch;
//     private float[] notesFrequencies = { 82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f, 130.81f, 138.59f, 146.83f, 155.56f }; // Frequencies of standard guitar tuning

//     void Start()
//     {
//         audioBuffer = new float[bufferSize];
//         spectrum = new float[bufferSize / 2];

//         StartMicrophone();
//     }

//     void Update()
//     {
//         // Check if the microphone is recording
//         if (Microphone.IsRecording(microphoneName))
//         {
//             // Get audio data from the microphone into the audio buffer
//             Microphone.GetPosition(microphoneName, out int position);
//             AudioListener.GetOutputData(audioBuffer, 0);

//             // Calculate spectrum using FFT
//             FFT.Calculate(audioBuffer, spectrum, false);

//             // Find peak frequency in the spectrum
//             pitch = FindPeakFrequency(spectrum, sampleRate);

//             // Check the detected pitch against standard guitar tuning frequencies
//             float closestFrequency = FindClosestFrequency(pitch);

//             // Output the detected pitch
//             Debug.Log("Detected pitch: " + closestFrequency + " Hz");
//         }
//     }

//     void StartMicrophone()
//     {
//         if (microphoneName == null)
//             microphoneName = Microphone.devices[0]; // Use default microphone if not specified

//         Microphone.Start(microphoneName, true, 1, sampleRate);
//     }

//     float FindPeakFrequency(float[] spectrum, float sampleRate)
//     {
//         float maxMagnitude = 0f;
//         int maxIndex = 0;

//         for (int i = 0; i < spectrum.Length; i++)
//         {
//             if (spectrum[i] > maxMagnitude)
//             {
//                 maxMagnitude = spectrum[i];
//                 maxIndex = i;
//             }
//         }

//         // Convert index to frequency
//         float frequency = maxIndex * (sampleRate / bufferSize);
//         return frequency;
//     }

//     float FindClosestFrequency(float pitch)
//     {
//         float closestFrequency = notesFrequencies[0];
//         float minDistance = Mathf.Abs(pitch - notesFrequencies[0]);

//         foreach (float frequency in notesFrequencies)
//         {
//             float distance = Mathf.Abs(pitch - frequency);
//             if (distance < minDistance)
//             {
//                 minDistance = distance;
//                 closestFrequency = frequency;
//             }
//         }

//         return closestFrequency;
//     }
// }