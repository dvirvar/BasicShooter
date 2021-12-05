using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    public enum VolumeGroup
    {
        MasterVolume,MusicVolume,SFXVolume
    }
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip[] bgClips;
    private AudioSource audioSource;
    [SerializeField] private int currentBgAudioClip = 0;
    private IEnumerator currentAudioLoop;
    private void Awake() {
        audioSource = this.GetComponent<AudioSource>();
    }
    private void Start()
    {
        foreach (var volumeGroup in EnumUtil.getListOf<VolumeGroup>())
        {
            //Must be called from Start
            audioMixer.SetFloat(volumeGroup.ToString(), UserPrefs.getVolumeGroupValue(volumeGroup));
        }
    }

    public void startBgAudio()
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.clip = bgClips[currentBgAudioClip];

        audioSource.Play();
        currentAudioLoop = StartLoopAudio();
        StartCoroutine(currentAudioLoop);
    }

    public void stopBgAudio()
    {
        StopCoroutine(currentAudioLoop);
        audioSource.Stop();   
    }

    private IEnumerator StartLoopAudio()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        audioSource.clip = bgClips[++currentBgAudioClip % bgClips.Length];
        audioSource.Play();
        StartCoroutine(StartLoopAudio());
    }

    /// <summary>
    /// Set the volume of a specific group
    /// </summary>
    /// <param name="group">Volume group</param>
    /// <param name="value">The linear value between 0.0001 and 1</param>
    public void setVolumeForGroup(VolumeGroup group, float value)
    {
        value = linearToDecibel(Mathf.Clamp(value, 0.0001f, 1f));
        audioMixer.SetFloat(group.ToString(), value);
        UserPrefs.setVolumeGroupValue(group, value);
    }

    public float getVolumeForGroup(VolumeGroup group)
    {
        audioMixer.GetFloat(group.ToString(), out float value);
        return decibelToLinear(value);
    }

    private float decibelToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }

    private float linearToDecibel(float linear)
    {
        return Mathf.Log10(linear) * 20f;
    }

}
