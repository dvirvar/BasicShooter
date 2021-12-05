using UnityEngine;
using UnityEngine.UI;

public class CredentialsManager : MonoBehaviour
{
    public GameObject loginPrefab;
    public GameObject registerPrefab;

    public void moveToLogin()
    {
        registerPrefab.SetActive(false);
        loginPrefab.SetActive(true);
    }

    public void moveToRegister()
    {
        loginPrefab.SetActive(false);
        registerPrefab.SetActive(true);
    }
}
