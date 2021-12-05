using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TabbarController : ViewController
{
    [SerializeField] private List<Tabbar> tabbars;
    [SerializeField] [HideInInspector] private int currentTab;

    private void Awake()
    {
        for (int i = 0; i < tabbars.Count; i++)
        {
            int copy = i;
            tabbars[i].vc.tabbarController = this;
            tabbars[i].vc.SetActive(currentTab == copy);
            tabbars[i].button.onClick.AddListener(delegate
            {
                setCurrentTab(copy);
            });
        }
    }

    public void setCurrentTab(int index)
    {
        if (currentTab == index)
        {
            return;
        }
        tabbars[currentTab].vc.SetActive(false);
        currentTab = index;
        tabbars[currentTab].vc.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (var tabbar in tabbars)
        {
            tabbar.vc.tabbarController = null;
            tabbar.button.onClick.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TabbarController))]
    [CanEditMultipleObjects]
    public class TabbarControllerLayoutEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            var tabbarController = (TabbarController)target;
            var maxValue = tabbarController.tabbars.Count;
            if (maxValue > 0)
            {
                maxValue -= 1;
            }
            tabbarController.currentTab = EditorGUILayout.IntSlider(tabbarController.currentTab, 0, maxValue);
            EditorGUI.EndChangeCheck();
        }
    }
#endif
}
[System.Serializable]
class Tabbar
{
    public ViewController vc;
    public Button button;
}
