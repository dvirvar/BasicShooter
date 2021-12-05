using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    void Update()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0)
        {
            Vector3 nextPos = transform.position + transform.forward * mouseWheel * Time.deltaTime * 20;
            transform.position = nextPos;
        } else if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 nextPos = transform.position + transform.right * -Input.GetAxis("Horizontal Look") * Time.deltaTime * 8;
            nextPos += transform.up * -Input.GetAxis("Vertical Look") * Time.deltaTime * 8;
            transform.position = nextPos;
        }
    }
}
