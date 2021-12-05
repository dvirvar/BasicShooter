using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVC : ViewController
{
    private AudioHandler audioHandler;
    [SerializeField] private AudioRow masterRow, musicRow, sfxRow;

    private void Awake()
    {
        audioHandler = FindObjectOfType<AudioHandler>();
        masterRow.setView("Master", audioHandler.getVolumeForGroup(AudioHandler.VolumeGroup.MasterVolume), 1);
        musicRow.setView("Music", audioHandler.getVolumeForGroup(AudioHandler.VolumeGroup.MusicVolume), masterRow.getValue());
        sfxRow.setView("SFX", audioHandler.getVolumeForGroup(AudioHandler.VolumeGroup.SFXVolume), masterRow.getValue());

        masterRow.onValueChanged += MasterRow_onValueChanged;
        musicRow.onValueChanged += MusicRow_onValueChanged;
        sfxRow.onValueChanged += SfxRow_onValueChanged;
    }

    private void MasterRow_onValueChanged(float value)
    {
        audioHandler.setVolumeForGroup(AudioHandler.VolumeGroup.MasterVolume, value);
        musicRow.setMaxValue(value);
        sfxRow.setMaxValue(value);
    }

    private void MusicRow_onValueChanged(float value)
    {
        audioHandler.setVolumeForGroup(AudioHandler.VolumeGroup.MusicVolume, value);
    }

    private void SfxRow_onValueChanged(float value)
    {
        audioHandler.setVolumeForGroup(AudioHandler.VolumeGroup.SFXVolume, value);
    }
}
