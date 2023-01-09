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
    [Header("Common UI")]
    [SerializeField] private Text messageText;

    [Header("Login")]
    [SerializeField] private InputField loginEmailInput;
    [SerializeField] private InputField loginPasswordInput;

    [Header("Register")]
    [SerializeField] private InputField registerEmailInput;
    [SerializeField] private InputField registerPasswordInput;

    #region Register

    public void RegisterNewUserButton()
    {
        ClearMessageText();
        RegisterNewUser(registerEmailInput, registerPasswordInput);
    }

    private void RegisterNewUser(InputField email, InputField password)
    {
        if (password.text.Length < 6)
        {
            StartCoroutine(ShowMessage("Password must contain at least 6 characters"));
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
        StartCoroutine(ShowMessage("Registered, logging in..."));

        ToMainMenu();
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
        StartCoroutine(ShowMessage("logging in..."));

        InitializePlayerProgressIfNeeded();
    }

    private void InitializePlayerProgressIfNeeded()
    {
        var initializePlayerRequest = new ExecuteCloudScriptRequest()
        {
            FunctionName = "InitializePlayerIfNeeded",
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
        var mockPassword = Utils.RandomString(8);

        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = mockPassword
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, null, OnAccountExistsCheck);
    }

    private void OnAccountExistsCheck(PlayFabError error)
    {
        if (error.ErrorMessage == "Invalid email address or password")
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
        StartCoroutine(ShowMessage("Password reset email sent"));
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

        yield return new WaitForSeconds(3f);

        ClearMessageText();
    }

    #endregion
}
