using UnityEngine;
using UnityEngine.UI;

public class LoadoutSpawnUI : SpawnUI
{
    [SerializeField] private LoadoutSpawnHolder[] loadoutHolders;
    [SerializeField] private Toggle[] loadoutButtons;
    
    // Start is called before the first frame update
    private void Awake()
    {
        spawnBtn.onClick.AddListener(delegate{
            spawnBtn.enabled = false;
            int i;
            for (i = 0; i < loadoutButtons.Length; i++)
            {
                if (loadoutButtons[i].isOn)
                {
                    break;
                }
            }
            onSpawnPressed.Invoke(User.currentUser().playerLoadouts[i]);
        });
    }

    private void Start()
    {
        PlayerLoadouts playerLoadouts = User.currentUser().playerLoadouts;
        loadoutHolders[0].init(playerLoadouts.loadout1);
        loadoutHolders[1].init(playerLoadouts.loadout2);
        loadoutHolders[2].init(playerLoadouts.loadout3);
    }

    private void OnEnable()
    {
        spawnBtn.enabled = true;
    }

    //private void TextAnimation_OnAnimationEnd()
    //{
    //    if (time > 0)
    //    {
    //        textAnimation.setText(time.ToString());
    //        textAnimation.startAnimation();
    //        time--;
    //    }
    //    else
    //    {
    //        textAnimation.setText("Press enter to spawn");
    //    }
    //}
}
