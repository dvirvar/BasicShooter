using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GunMasterDisplay : Display<GunMasterRowInfo>, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<string> OnBeganHovering = delegate { };
    public event Action OnStoppedHovering = delegate { };
    [SerializeField] private Text levelText, numOfPlayersText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnBeganHovering(info.weaponName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnStoppedHovering();
    }

    protected override void setView(GunMasterRowInfo info)
    {
        levelText.text = $"Level {info.level}";
        numOfPlayersText.text = $"{info.numOfPlayers}";
        numOfPlayersText.color = info.isSelfHere ? Color.green : Color.black;
    }
}
