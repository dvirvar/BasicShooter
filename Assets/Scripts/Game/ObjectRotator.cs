using UnityEngine;

public class ObjectRotator : MonoBehaviour
{

    [SerializeField] private float mouseSpeedMultiplier = 8;
    [SerializeField] private float smoothSpeed = 0.04f;
    private float mouseX;
    private float mouseY;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseX -= Input.GetAxis("Horizontal Look") * mouseSpeedMultiplier;
            mouseY -= Input.GetAxis("Vertical Look") * mouseSpeedMultiplier;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mouseX, mouseY), smoothSpeed);
        }
    }
}
