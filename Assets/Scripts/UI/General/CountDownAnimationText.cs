using UnityEngine;
using UnityEngine.UI;
using System;

public class CountDownAnimationText : MonoBehaviour
{
    public event Action OnAnimationEnd = delegate { };

    [SerializeField]
    private Text text;

    private Animator animator;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    public void setText(string textIn) {
        this.text.text = textIn;
    }

    public void startAnimation() {
        this.animator.SetTrigger("Play");
    }

    public void stopAnimation() {
        this.animator.Play("CountDown", 0, 0);
    }

    [SerializeField]
    private void onAnimationEnd() {
        OnAnimationEnd();
    }
}
