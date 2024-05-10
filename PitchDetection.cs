using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchDetection : MonoBehaviour
{
    private SpriteRenderer sr;
    AudioSource audioSource;
    public string selectedDevice;
    public int duration = 8;
    private float origin = 1.5014f;
    public static float[] samples = new float[128];
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
        float frequency = AudioPitchEstimator.instance.Estimate(audioSource);
        
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

        if (float.IsNaN(frequency))
        {
            // Algorithm didn't detect fundamental frequency (e.g. silence).
            r6.material.color = Color.white;
            r5.material.color = Color.white;
            r4.material.color = Color.white;
            r3.material.color = Color.white;
            r2.material.color = Color.white;
            r1.material.color = Color.white;
            sr.transform.position = new Vector3(0,origin,0);
        }
        else
        {
            float range;
            if (72.41 <= frequency && frequency <= 92.41) // 6 (E)	82.41 Hz	E2
            {
                range = 82.41f - frequency;
                sr.transform.position = new Vector3(range,origin,0) * Time.deltaTime; 
                r6.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            else if (100.0 <= frequency && frequency <= 120.0) // 5 (A)	110.00 Hz	A2
            {
                range = 110.00f - frequency;
                sr.transform.position = new Vector3(range,origin,0) * Time.deltaTime;
                r5.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            else if (136.83 <= frequency && frequency <= 156.83) // 4 (D)	146.83 Hz	D3
            {
                range = 146.83f - frequency;
                sr.transform.position = new Vector3(range,origin,0)* Time.deltaTime;
                r4.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            else if (186.00 <= frequency && frequency <= 206.00) // 3 (G)	196.00 Hz	G3
            {
                range = 196.00f - frequency;
                sr.transform.position = new Vector3(range,origin,0)* Time.deltaTime;
                r3.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            else if (236.94 <= frequency && frequency <= 256.94) // 2 (B)	246.94 Hz	B3
            {
                range = 246.94f - frequency;
                sr.transform.position = new Vector3(range,origin,0)* Time.deltaTime;
                r2.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            else if (319.63 <= frequency && frequency <= 339.63) // 1 (E)	329.63 Hz	E4
            {
                range = 329.63f - frequency;
                sr.transform.position = new Vector3(range,origin,0)* Time.deltaTime;
                r1.material.color = Color.green;
                coroutine();
                Debug.Log(frequency);
            }
            // Algorithm detected fundamental frequency.
            // The frequency is stored in the variable `frequency` (in Hz).
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

    void Update()
    {
        // It is NOT recommended to run the estimation every frame.
        // This will take a high computational load.
        EstimatePitch();
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