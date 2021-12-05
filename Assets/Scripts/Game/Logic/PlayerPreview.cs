using UnityEngine;
/// <summary>
/// Handles the logic of previweing a player
/// </summary>
public class PlayerPreview : MonoBehaviour
{
    public MeshRenderer characterModel;

    void Start()
    {
        Color jellyColor;
        ColorUtility.TryParseHtmlString(User.currentUser().playerCustomization.jellyColor, out jellyColor);
        characterModel.materials[1].SetColor("_Color", jellyColor);
        Color donutColor;
        ColorUtility.TryParseHtmlString(User.currentUser().playerCustomization.donutColor, out donutColor);
        characterModel.materials[0].SetColor("_Color", donutColor);
    }

    public void setColorTo(DonutColorable donutColorable, Color color)
    {
        characterModel.materials[(int)donutColorable].SetColor("_Color", color);
    }

    public Color getColor(DonutColorable donutColorable)
    {
        return characterModel.materials[(int)donutColorable].color;
    }

}
