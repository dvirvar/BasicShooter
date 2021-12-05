using UnityEngine;
using UnityEngine.UI;
using System;

//TODO: Change name
public class FilterSegment : MonoBehaviour
{
    public event Action<FilterSegment> onClick = delegate { };
    public Button button;
    public GameObject content;
    
    public bool isWindowClosed
    {
        get
        {
            return content.activeInHierarchy;
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        button.onClick.AddListener(delegate { onClick(this); } );
    }

    public void setWindowActive(bool active)
    {
        content.SetActive(active);
    }

    void OnDestroy() {
        button.onClick.RemoveAllListeners();
    }
}
