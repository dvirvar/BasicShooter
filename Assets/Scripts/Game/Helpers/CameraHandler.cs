using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraHandler : MonoBehaviour
{
    private Camera currentCamera;
    private Camera[] camerasList;
    private bool animateCameraFlag = false;

    /// <summary>
    /// Switches to a camera
    /// </summary>
    /// <param name="camera">The camera</param>
    public void SwitchCamera(Camera camera)
    {
        animateCameraFlag = false;
        if (this.currentCamera != null)
            currentCamera.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
        currentCamera = camera;
    }

    /// <summary>
    /// Switches to a camera, and after an amount of time invokes a callback
    /// </summary>
    /// <param name="camera">The camera</param>
    /// <param name="seconds">The amount of time in seconds</param>
    /// <param name="callback">The callback</param>
    public void SwitchCamera(Camera camera, float seconds, Action callback)
    {
        animateCameraFlag = false;
        StartCoroutine(_SwitchCamera(camera, seconds, callback));
    }

    /// <summary>
    /// Looping on a list of cameras, while wating an amount of time between each swap
    /// </summary>
    /// <param name="cameras">The list of cameras</param>
    /// <param name="seconds">The amount of time in seconds</param>
    public void AnimateCameras(Camera[] cameras,float seconds)
    {
        animateCameraFlag = true;
        this.camerasList = cameras;
        StartCoroutine(_AnimateCameras(seconds));
    }

    public void stopAnimating()
    {
        StopAllCoroutines();
    }

    private IEnumerator _AnimateCameras(float seconds)
    {
        int counter = 0;
        while (animateCameraFlag)
        {
            SwitchCamera(camerasList[counter]);
            animateCameraFlag = true;
            counter++;
            if (counter == camerasList.Length)
            {
                counter = 0;
            }
            yield return new WaitForSeconds(seconds);
        }
    }

    private IEnumerator _SwitchCamera(Camera camera, float seconds, Action callback)
    {
        SwitchCamera(camera);
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
