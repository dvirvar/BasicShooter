using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicsVC : ViewController
{
    private enum AppDisplayMode
    {
        Fullscreen,
        Window,
        Borderless
    }
    [SerializeField] private Dropdown displayModeDropdown, displayResolutionDropdown;

    private void Awake()
    {
        displayModeDropdown.onValueChanged.AddListener(index =>
        {
            Screen.SetResolution(Screen.width, Screen.height, getCurrentFullScreenMode());
        });

        displayResolutionDropdown.onValueChanged.AddListener(index =>
        {
            var resolution = displayResolutionDropdown.options[index].text.Split(' ');
            Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[2]), getCurrentFullScreenMode());
        });
    }

    private void Start()
    {
        #region Display mode
        displayModeDropdown.AddOptions(EnumUtil.getListOf<AppDisplayMode>().Select(dm => dm.ToString()).ToList());
        displayModeDropdown.SetValueWithoutNotify((int)GetAppDisplayMode(Screen.fullScreenMode));
        #endregion

        #region Display resolution
        var resolutions = new List<string>();
        var resolutionsSet = new HashSet<(int,int)>();
        foreach (var item in Screen.resolutions)
        {
            if (item.width >= 800 && !resolutionsSet.Contains((item.width,item.height)))
            {
                resolutionsSet.Add((item.width, item.height));
                resolutions.Add($"{item.width} x {item.height}");
            }
        }
        displayResolutionDropdown.AddOptions(resolutions);
        displayResolutionDropdown.SetValueWithoutNotify(displayResolutionDropdown.options.FindIndex(o => o.text.StartsWith(Screen.width.ToString()) && o.text.EndsWith(Screen.height.ToString())));
        #endregion
    }

    private FullScreenMode getCurrentFullScreenMode() => displayModeDropdown.value switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.MaximizedWindow,
            3 => FullScreenMode.FullScreenWindow,
            _ => throw new System.NotImplementedException()
        };

    private AppDisplayMode GetAppDisplayMode(FullScreenMode mode) => mode switch
    {
        FullScreenMode.ExclusiveFullScreen => AppDisplayMode.Fullscreen,
        FullScreenMode.MaximizedWindow => AppDisplayMode.Borderless,
        FullScreenMode.Windowed => AppDisplayMode.Window,
        FullScreenMode.FullScreenWindow => AppDisplayMode.Borderless,
        _ => throw new System.NotImplementedException(),
    };

    private void OnDestroy()
    {
        displayModeDropdown.onValueChanged.RemoveAllListeners();
        displayResolutionDropdown.onValueChanged.RemoveAllListeners();
    }
}
