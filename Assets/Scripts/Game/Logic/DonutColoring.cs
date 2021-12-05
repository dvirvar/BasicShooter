using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DonutColoring : MonoBehaviour
{

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();   
    }

    public void setColors(PlayerCustomization playerCustomization)
    {
        Color donutColor;
        ColorUtility.TryParseHtmlString(playerCustomization.donutColor, out donutColor);
        meshRenderer.materials[(int)DonutColorable.donut].SetColor("_Color", donutColor);

        Color jellyColor;
        ColorUtility.TryParseHtmlString(playerCustomization.jellyColor, out jellyColor);
        meshRenderer.materials[(int)DonutColorable.jelly].SetColor("_Color", jellyColor);
    }

}

public enum DonutColorable
{
    donut, jelly
}