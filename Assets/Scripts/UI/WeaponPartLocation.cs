using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponPartLocation : MonoBehaviour
{
    public event Action<Transform> onButtonPressed = delegate { };
    [SerializeField] private Button btn;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = mainCamera;
    }

    void Start()
    {
        btn.onClick.AddListener(delegate
        {
            onButtonPressed(transform);
        });
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
}
