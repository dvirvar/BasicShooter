using System.Collections;

/// <summary>
/// Handles logic of user requests
/// </summary>
public class UsersService: BasicService
{
    
    public UsersService(int timeout = 10): base(timeout)
    {
        
    }

    public IEnumerator login(string username, string password, System.Action<LoginResponse> callBack)
    {
        string encyptedUsername = EncryptionTool.encryptString(username.ToLower());
        string encyptedPassword = EncryptionTool.encryptString(password.ToLower());
        LoginData loginData = new LoginData(encyptedUsername, encyptedPassword);
        yield return serverRequests.login(loginData, (response) =>
          {
              if (response.isSuccess)
              {
                  User.currentUser().id = response.parsedResponse.id;
                  User.currentUser().token = response.parsedResponse.token;
                  User.currentUser().name = username;
              }
              callBack(response);
          });
    }

    public IEnumerator register(string username, string password, System.Action<RegisterResponse> callBack)
    {
        string encyptedUsername = EncryptionTool.encryptString(username.ToLower());
        string encyptedPassword = EncryptionTool.encryptString(password.ToLower());
        RegisterData registerData = new RegisterData(encyptedUsername, encyptedPassword);
        yield return serverRequests.register(registerData, (response) =>
        {
            
            callBack(response);
        });
    }
}

