using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExtensionMethods;

public class Login : MonoBehaviour
{
    #region Values
    public Button regisBtn;
    public Button loginBtn;
    public InputField usernameInput;
    public InputField passWordInput;
    public Text errorTxt;
    public Image loading;
    #endregion
    private UsersService usersService;
    private AuthSocketService authSocketService;
    private SocketHandler socketHandler;
    // Start is called before the first frame update

    private void Awake()
    {
        usersService = new UsersService();
    }

    private void Start()
    {
        SocketHandler.OnSocketOpen += getSocketPermission;
        socketHandler = FindObjectOfType<SocketHandler>();
        authSocketService = new AuthSocketService(socketHandler);
        errorTxt.Hide();
        loginBtn.onClick.AddListener(BtnLoginClick);
    }

    void BtnLoginClick()
    {
        string userName = usernameInput.text;
        string password = passWordInput.text;
        if (userName != "" || password != "")
        {
            errorTxt.Hide();
            startLoading();
            //Request the server
            StartCoroutine(usersService.login(userName, password, loginCallback));
        }
        else {
            errorTxt.ShowTextWith("Please enter Username and Password");
        }
    }

    private void loginCallback(LoginResponse response)
    {
        if (!response.isSuccess)
        {
            stopLoading();
            errorTxt.ShowTextWith(response.rawResponse);
        }
        else 
        {
            socketHandler.Connect();
        }
    }

    private void Update()
    {
        //Checks if the user presses Enter
        if (Input.GetButtonDown(StaticStrings.Input.submit))
        {
            BtnLoginClick();
        }else if(Input.GetButtonDown(StaticStrings.Input.tab))
        {
            handleInputFields();
        }
    }

    private void getSocketPermission()
    {
        authSocketService.getPermission(delegate(SocketPermissionStatusResponse response)
        {
            stopLoading();
            if (response.parsedResponse.permission)
            {
                SceneManager.LoadSceneAsync("MainMenu");
            }
            else
            {
                errorTxt.ShowTextWith(response.parsedResponse.reason);
                enableView(true);
                socketHandler.Close();
            }
        });
    }

    private void enableView(bool enable)
    {
        loginBtn.enabled = enable;
        regisBtn.enabled = enable;
    }

    #region Start And Stop Loading
    private void startLoading()
    {
        loading.Show();
        enableView(false);
    }

    private void stopLoading()
    {
        loading.Hide();
        enableView(true);
    }
    #endregion

    private void handleInputFields()
    {
        if (usernameInput.isFocused)
        {
            passWordInput.ActivateInputField();
        }
        else if (passWordInput.isFocused)
        {
            passWordInput.DeactivateInputField();
        }
        else
        {
            if (usernameInput.text == "")
            {
                usernameInput.ActivateInputField();
            }
            else if (passWordInput.text == "")
            {
                passWordInput.ActivateInputField();
            }
        }
    }

    private void OnDestroy()
    {
        SocketHandler.OnSocketOpen -= getSocketPermission;
        loginBtn.onClick.RemoveAllListeners();
    }
}
