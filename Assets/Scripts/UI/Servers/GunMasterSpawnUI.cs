using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMasterSpawnUI : SpawnUI
{
    
    [SerializeField] private Text weaponNameText;
    [SerializeField] private GunMasterTableView gunMasterTableView;
    private string currentWeaponName;

    private void Awake()
    {
        spawnBtn.onClick.AddListener(delegate
        {
            spawnBtn.enabled = false;
            onSpawnPressed.Invoke(null);
        });
        gunMasterTableView.OnItemHover += GunMasterTableView_OnItemHover;
        gunMasterTableView.OnItemFinishHover += GunMasterTableView_OnItemFinishHover;
    }

    private void GunMasterTableView_OnItemHover(string obj)
    {
        weaponNameText.text = obj;
    }

    private void GunMasterTableView_OnItemFinishHover()
    {
        weaponNameText.text = currentWeaponName;
    }

    public void setRows(List<GunMasterRowInfo> rows, string currentWeaponName)
    {
        this.currentWeaponName = currentWeaponName;
        weaponNameText.text = currentWeaponName;
        gunMasterTableView.setInfos(rows);
    }

    public void updateRow(int index, int pointsToAdd, bool? isSelfHere)
    {
        var info = gunMasterTableView.getInfoAt(index);
        info.numOfPlayers += pointsToAdd;
        if (isSelfHere != null)
        {
            info.isSelfHere = (bool)isSelfHere;
        }        
        gunMasterTableView.getDisplay(index).refreshDisplay();
    }

    private void OnEnable()
    {
        spawnBtn.enabled = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        gunMasterTableView.OnItemHover -= GunMasterTableView_OnItemHover;
        gunMasterTableView.OnItemFinishHover -= GunMasterTableView_OnItemFinishHover;
    }
}
