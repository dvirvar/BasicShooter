using UnityEngine;

/// <summary>
/// Handles the logic of mouse movement
/// </summary>
public class PlayerMouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensetivity = 100f;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private new Camera camera;
    private float xCameraRotation = 0f;

    public virtual void moveLook(float horizontalAxis, float verticalAxis)
    {
        float mouseX = horizontalAxis * mouseSensetivity * Time.deltaTime;
        float mouseY = verticalAxis * mouseSensetivity * Time.deltaTime;

        xCameraRotation -= mouseY;
        xCameraRotation = Mathf.Clamp(xCameraRotation, -45f, 60f);

        camera.transform.localRotation = Quaternion.Euler(xCameraRotation, 0f, 0f);
        weaponHolder.transform.localRotation = Quaternion.Euler(0, 90f, xCameraRotation);
        transform.Rotate(Vector3.up * mouseX);
    }
}
