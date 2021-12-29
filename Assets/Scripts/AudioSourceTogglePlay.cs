using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTogglePlay : MonoBehaviour
{
    public AudioSource audio1;
    public AudioSource audio2;

    private bool toggle = true;

    public void playAudio()
    {

        if (toggle)
        {
            audio1.Play();
        }
        else
        {
            audio2.Play();

        }
        toggle = !toggle;
    }

}
