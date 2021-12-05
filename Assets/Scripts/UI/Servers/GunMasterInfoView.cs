using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ExtensionMethods;

public class GunMasterInfoView : GameModeInfoView
{
    public event Action<string> customizedSelected = delegate { };
    [SerializeField] private Dropdown weaponsDropdown;
    [SerializeField] private Button customizeWeaponsBtn;
    [SerializeField] private InputField pointsToLevelUpIF;
    [SerializeField] private DefaultGunMasterWeapons defaultWeapons;
    [SerializeField] private bool hideCustomizeWeapons;
    private Dropdown.OptionData lastOptionData = new Dropdown.OptionData("Customize");

    private void Awake()
    {
        customizeWeaponsBtn.onClick.AddListener(delegate
        {
            customizedSelected(weaponsDropdown.captionText.text);
        });
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        List<Dropdown.OptionData> allGunMasterWeapons = new List<Dropdown.OptionData>();
        #region Default gun master weapons
        foreach (var gunMasterWeapons in defaultWeapons.gunMasterWeapons)
        {
            allGunMasterWeapons.Add(new Dropdown.OptionData(gunMasterWeapons.name));
        }
        #endregion
        #region User gun master weapons
        List<GunMasterWeapons> userWeapons = UserPrefs.getGunMasterWeapons();
        foreach (var gunMasterWeapons in userWeapons)
        {
            allGunMasterWeapons.Add(new Dropdown.OptionData(gunMasterWeapons.name));
        }
        #endregion
        weaponsDropdown.options = allGunMasterWeapons;
        if (!hideCustomizeWeapons)
        {
            weaponsDropdown.options.Add(lastOptionData);
        }
        #region weaponsDropdown value changed
        weaponsDropdown.onValueChanged.AddListener(value =>
        {
            string currentWeapon = weaponsDropdown.getCurrentValue();
            customizeWeaponsBtn.gameObject.SetActive(!hideCustomizeWeapons && !defaultWeapons.contains(currentWeapon) && currentWeapon != lastOptionData.text);
            if (!hideCustomizeWeapons && value == weaponsDropdown.options.Count - 1)
            {
                customizedSelected(null);
            }
            else
            {
                UserPrefs.setGunMasterWeaponsLastSelected(currentWeapon);
            }
        });
        #endregion
        #region Set last selected gun master weapons
        string lastSelected = UserPrefs.getGunMasterWeaponsLastSelected();
        int lastSelectedIndex = 0;
        if (lastSelected != null)
        {
            for (int i = 0; i < userWeapons.Count; i++)
            {
                GunMasterWeapons userWeapon = userWeapons[i];
                if (userWeapon.name == lastSelected)
                {
                    lastSelectedIndex = i + defaultWeapons.gunMasterWeapons.Count;
                    break;
                }
            }
        }
        weaponsDropdown.SetValueWithoutNotify(lastSelectedIndex);
        weaponsDropdown.onValueChanged.Invoke(lastSelectedIndex);
        #endregion
    }

    private void OnDisable()
    {
        weaponsDropdown.onValueChanged.RemoveAllListeners();
    }

    protected override string title()
    {
        return $"{GameModeType.GunMaster.GetDescription()} Info";
    }

    public override void fillData(WorldState worldState)
    {
        if (worldState.gunMasterInfo.HasValue)
        {
            var gunMasterInfo = worldState.gunMasterInfo.Value;
            pointsToLevelUpIF.text = gunMasterInfo.toLevelUp.ToString();
        }
    }

    public override JSONObject createGameModeInfoData()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("pointsToLevelUp", int.Parse(pointsToLevelUpIF.text));
        data.AddField("gunMasterWeapons", getGunMasterWeapons().toJSONObject());
        return data;
    }

    private GunMasterWeapons getGunMasterWeapons()
    {
        string gunMasterWeaponsName = weaponsDropdown.captionText.text;
        GunMasterWeapons gunMasterWeapons = defaultWeapons.getGunMasterWeapons(gunMasterWeaponsName);
        if (gunMasterWeapons != null)
        {
            return gunMasterWeapons;
        }
        gunMasterWeapons = UserPrefs.getGunMasterWeapons().Find(gmw => gmw.name == gunMasterWeaponsName);
        return gunMasterWeapons;
    }

    public override (bool valid, string reason) isValidPlusReason()
    {
        bool isValid;
        string reason;
        if (!defaultWeapons.contains(weaponsDropdown.captionText.text) && !UserPrefs.getGunMasterWeapons().Exists(gmw => gmw.name == weaponsDropdown.getCurrentValue()))
        {
            isValid = false;
            reason = "Gun Master weapons not found";
        } else if (int.TryParse(pointsToLevelUpIF.text, out int pointsToLevelUp))
        {
            isValid = pointsToLevelUp >= 1 && pointsToLevelUp <= 4;
            reason = isValid ? "" : "Points to level up must be between 1 - 4";
        }
        else
        {
            isValid = false;
            reason = "Points to level up must be between 1 - 4";
        }

        return (isValid, reason);
    }

    private void OnDestroy()
    {
        customizeWeaponsBtn.onClick.RemoveAllListeners();
    }
}
