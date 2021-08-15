using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    //public Slider masterVolume;
    public Toggle mute;
    //string MasterVolParameter = "MasterVolume";


   
    void Start()
    {
        //masterVolume.onValueChanged.AddListener(HandleSliderChange);
        mute.onValueChanged.AddListener(HandleToggleChange);
    }

    
    void Update()
    {

    }

    void HandleToggleChange(bool value)
    {
        if (value)
        {
            //mixer.SetFloat(MasterVolParameter, -80);
        }
        else
        {
           // mixer.SetFloat(MasterVolParameter, masterVolume.value);
        }
    }

    void HandleSliderChange(float value)
    {
       // if (!mute.isOn)
         //   mixer.SetFloat(MasterVolParameter, value);
    }
}