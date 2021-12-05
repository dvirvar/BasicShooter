using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField]
    private AudioHandler bgAudioHandler;
    private void Awake()
    {
        if (GameMode.FindObjectOfType<AudioHandler>() == null)
        {
            GameObject gm = Instantiate(bgAudioHandler.gameObject);
            DontDestroyOnLoad(gm);
        };
    }
}
