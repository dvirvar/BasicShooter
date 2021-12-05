using UnityEngine;
using UnityEngine.UI;

public class Popup: MonoBehaviour
{
    public Text text;
    public Button closeBtn;
    public bool showCloseBtnOnEnabled = true;

    public bool isActive
    {
        get
        {
           return gameObject.activeInHierarchy;
        }

        set
        {
            gameObject.SetActive(value);
        }
    }

    private void OnEnable()
    {
        closeBtn.gameObject.SetActive(showCloseBtnOnEnabled);
    }

    public void closePressed()
    {
        gameObject.SetActive(false);
    }

    public void setText(string text)
    {
        this.text.text = text;
    }

    public void showWith(string text)
    {
        this.text.text = text;
        gameObject.SetActive(true);
    }

    public void showCloseBtn()
    {
        closeBtn.gameObject.SetActive(true);
    }

    public void hideCloseBtn()
    {
        closeBtn.gameObject.SetActive(false);
    }
}
