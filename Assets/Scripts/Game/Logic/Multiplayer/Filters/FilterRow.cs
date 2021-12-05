using UnityEngine;
using UnityEngine.UI;
using System;
using ExtensionMethods;

public abstract class FilterRow<T> : MonoBehaviour where T: Enum
{
    public event Action<T,bool> toggleChanged = delegate { };

    [SerializeField]
    private Text label;
    [SerializeField]
    private Toggle toggle;

    public T filterType;

    protected virtual void Awake()
    {
        toggle.onValueChanged.AddListener(toggled =>
        {
            toggleChanged(filterType, toggled);
        });
    }

    public void BuildRow(T filterType, bool toggled) {
        this.filterType = filterType;
        label.text = filterType.GetDescription();
        toggle.isOn = toggled;
    }

    public void BuildRow(T filterType, bool toggled, ToggleGroup group)
    {
        this.filterType = filterType;
        label.text = filterType.GetDescription();
        toggle.isOn = toggled;
        toggle.group = group;
    }

    public void ChangeToggle(bool isOn) {
        this.toggle.isOn = isOn;
    }

    protected virtual void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}