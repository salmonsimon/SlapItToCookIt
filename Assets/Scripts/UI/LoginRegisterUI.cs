using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System.Xml.Serialization;

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
        RegisterNewUser(registerEmailInput, registerPasswordInput);
    }

    private void RegisterNewUser(InputField email, InputField password)
    {
        if (password.text.Length < 6)
        {
            messageText.text = "Password must contain at least 6 characters";
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
        messageText.text = "Registered, logging in...";

        ToMainMenu();
    }

    #endregion

    #region Login

    public void LoginButton()
    {
        Login(loginEmailInput, loginPasswordInput);
    }

    private void Login(InputField email, InputField password)
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
        messageText.text = "logging in...";

        ToMainMenu();
    }

    public void ResetPasswordButton()
    {
        ResetPassword(loginEmailInput);
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
        messageText.text = "Password reset email sent";
    }

    #endregion

    #region Common

    private void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
    }

    private void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    public void ClearMessageText()
    {
        messageText.text = string.Empty;
    }

    #endregion
}
