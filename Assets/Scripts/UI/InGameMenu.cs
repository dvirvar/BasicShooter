using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public event Action OnResumePressed = delegate { };
    public event Action OnExitPressed = delegate { };
    [SerializeField] private Button resumeBtn, exitBtn;
    
    private void Awake()
    {
        
        resumeBtn.onClick.AddListener(delegate
        {
            OnResumePressed();
            gameObject.SetActive(false);
        });
        exitBtn.onClick.AddListener(delegate
        {
            OnExitPressed();
        });
    }

    private void OnDestroy()
    {
        resumeBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }
}
