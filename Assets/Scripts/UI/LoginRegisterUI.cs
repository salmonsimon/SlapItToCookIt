using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System.Xml.Serialization;
using PlayFab.PfEditor.Json;

public class LoginRegisterUI : MonoBehaviour
{
    #region Game Object References

    [Header("Common UI")]
    [SerializeField] private Text messageText;

    [Header("Login")]
    [SerializeField] private InputField loginEmailInput;
    [SerializeField] private InputField loginPasswordInput;

    [Header("Register")]
    [SerializeField] private InputField registerEmailInput;
    [SerializeField] private InputField registerPasswordInput;

    #endregion

    #region Register

    public void RegisterNewUserButton()
    {
        ClearMessageText();
        RegisterNewUser(registerEmailInput, registerPasswordInput);
    }

    private void RegisterNewUser(InputField email, InputField password)
    {
        if (password.text.Length < Config.API_MIN_PASSWORD_LENGTH)
        {
            StartCoroutine(ShowMessage(Config.API_PASS_TOO_SHORT_MSG));
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        StartCoroutine(ShowMessage(Config.API_REGISTERED_MSG));

        InitializePlayerProgressIfNeeded();
    }

    #endregion

    #region Login

    #region Buttons
    public void LoginWithEmailButton()
    {
        ClearMessageText();
        LoginWithEmail(loginEmailInput, loginPasswordInput);
    }

    public void LoginWithDeviceButton()
    {
        ClearMessageText();
        LoginWithDevice();
    }

    public void ResetPasswordButton()
    {
        ClearMessageText();
        CheckIfAccountExists(loginEmailInput);
    }

    #endregion

    #region Login

    public void LoginWithDevice()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void LoginWithEmail(InputField email, InputField password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = password.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        StartCoroutine(ShowMessage(Config.API_LOGIN_MSG));

        InitializePlayerProgressIfNeeded();
    }

    private void InitializePlayerProgressIfNeeded()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = Config.API_INITIALIZE_PLAYER_FUNCTION_NAME,
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(initializePlayerRequest, OnInitializationResponse, OnError);
    }

    private void OnInitializationResponse(ExecuteCloudScriptResult result)
    {
        var lastLog = result.Logs[result.Logs.Count - 1];

        if (lastLog.Level == "Error") 
        {
            StartCoroutine(ShowMessage(lastLog.Message));
        }
        else
        {
            ToMainMenu();
        }
    }

    #endregion

    #region Password Reset

    private void CheckIfAccountExists(InputField email)
    {
        var mockPassword = Utils.RandomString(Config.API_RANDOM_PASSWORD_LENGTH);

        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = mockPassword
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, null, OnAccountExistsCheck);
    }

    private void OnAccountExistsCheck(PlayFabError error)
    {
        if (error.ErrorMessage == Config.API_LOGIN_INVALID_ERROR_MSG)
        {
            ResetPassword(loginEmailInput);
        }
        else
        {
            StartCoroutine(ShowMessage(error.ErrorMessage));
        }
    }
 
    private void ResetPassword(InputField email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = Config.API_TITLE_ID
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        StartCoroutine(ShowMessage(Config.API_PASS_RESET_MSG));
    }

    #endregion

    #endregion

    #region Common

    private void OnError(PlayFabError error)
    {
        StartCoroutine(ShowMessage(error.ErrorMessage));
    }

    private void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    public void ClearMessageText()
    {
        messageText.text = string.Empty;
    }

    private IEnumerator ShowMessage(string message)
    {
        messageText.text = message;

        yield return new WaitForSeconds(Config.ERROR_MESSAGE_DURATION);

        ClearMessageText();
    }

    #endregion
}
