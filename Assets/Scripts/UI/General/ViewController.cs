using UnityEngine;

public class ViewController : MonoBehaviour
{
    [HideInInspector] public ViewController presentingViewController, presentedViewController;
    [HideInInspector] public NavigationController navigationController;
    [HideInInspector] public TabbarController tabbarController;

    public void present(ViewController viewController)
    {
        viewController.SetActive(true);
        presentedViewController = viewController;
        viewController.presentingViewController = this;
    }

    public void dismiss()
    {
        if (presentingViewController != null)
        {
            presentingViewController.presentedViewController = null;
            presentingViewController = null;
            SetActive(false);
        }
    }

    public void pushToNavigationController(ViewController viewController)
    {
        navigationController.push(viewController);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void backButtonPressed()
    {
        navigationController.pop();
    }
}
