using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the network logic of the camera
/// </summary>
public class NetworkCamera : MonoBehaviour
{
    //TODO: Change this class
    [GreyOutAtt]
    private NetworkIdentity networkIdentity;

    private Camera cam;
    private AudioListener audioListener;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();
        networkIdentity = GetComponentInParent<NetworkIdentity>();
        if (!networkIdentity.isControlling()) {
            cam.enabled = false;
            audioListener.enabled = false;
        }
    }
}
