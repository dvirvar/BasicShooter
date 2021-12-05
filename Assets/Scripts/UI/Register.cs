using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExtensionMethods;

public class Register : MonoBehaviour
{
    
    [SerializeField] private Image loading;
    [SerializeField] private Text errorTxt;
    [SerializeField] private Button registerBtn, loginBtn;
    [SerializeField] private InputField userIF, passwordIF, confirmIF;
    
    private UsersService usersService;

    private void Start()
    {
        errorTxt.Hide();
        usersService = new UsersService();
        registerBtn.onClick.AddListener(delegate {
            errorTxt.Hide();
            if (userIF.text == "" || passwordIF.text == "" || confirmIF.text == "")
            {
                errorTxt.ShowTextWith("Fields must not be empty");
            }
            else if (confirmIF.text != passwordIF.text)
            {
                errorTxt.ShowTextWith("Passwords don't match");
            }
            else {
                registerBtn.enabled = false;
                loginBtn.enabled = false;
                loading.Show();
                StartCoroutine(usersService.register(userIF.text, passwordIF.text, registerCallback));
            }
        });
    }

    private void registerCallback(RegisterResponse response) {
        loading.Hide();
        registerBtn.enabled = true;
        loginBtn.enabled = true;
        if (!response.isSuccess)
        {
            errorTxt.ShowTextWith(response.rawResponse);
        }
        else
        {
            loginBtn.onClick.Invoke();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown(StaticStrings.Input.tab))
        {
            handleInputFields();
        }
    }

    private void handleInputFields()
    {
        if (userIF.isFocused)
        {
            passwordIF.ActivateInputField();
        }
        else if (passwordIF.isFocused)
        {
            confirmIF.ActivateInputField();
        }
        else if (confirmIF.isFocused)
        {
            confirmIF.DeactivateInputField();
        }
        else
        {
            if (userIF.text == "")
            {
                userIF.ActivateInputField();
            }
            else if (passwordIF.text == "")
            {
                passwordIF.ActivateInputField();
            } else if (confirmIF.text == "")
            {
                confirmIF.ActivateInputField();
            }
        }
    }
    
    private void OnDestroy() {
        registerBtn.onClick.RemoveAllListeners();
    }
    
}
