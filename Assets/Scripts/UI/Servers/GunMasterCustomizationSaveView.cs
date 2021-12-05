using System;
using UnityEngine;
using UnityEngine.UI;

public class GunMasterCustomizationSaveView : MonoBehaviour
{
    public event Action<string> savePressed = delegate { };
    [SerializeField] private InputField nameIF;
    [SerializeField] private Button saveBtn;

    private void Awake()
    {
        //TODO: Static class for character limitations
        nameIF.characterLimit = 16;
        saveBtn.onClick.AddListener(delegate
        {
            savePressed(nameIF.text);
        });
    }

    public void setName(string name)
    {
        nameIF.text = name;
    }

    public string getCustomizationName()
    {
        return nameIF.text;
    }

    private void OnDestroy()
    {
        saveBtn.onClick.RemoveAllListeners();
    }
}
