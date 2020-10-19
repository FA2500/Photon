using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabTest : MonoBehaviour
{
    private const string UserNamePlayerPref = "NamePickUserName";

    private string userEmail;
    private string userPassword;
    private string userDisplayName;

    public GameObject loginPanel;
    public GameObject registerPanel;

    public GameObject guestbutton;
    public GameObject emailbutton;

    public ChatGui chatNewComponent;



    void Start()
    {
        this.chatNewComponent = FindObjectOfType<ChatGui>();
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "1BAC8";
        }
    }

    public void loginMobile()
    {

#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
        Debug.Log("3");
        //chatNewComponent.Connect();
#endif
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
        //chatNewComponent.Connect();
#endif

    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        loginPanel.SetActive(false);
        guestbutton.SetActive(false);
        emailbutton.SetActive(false);
        
        Debug.Log("5");

    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
       
        Debug.Log("9");
    }



    public void loginEmail()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void UILogin()
    {
        emailbutton.SetActive(false);
        guestbutton.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void UIRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { RequireBothUsernameAndEmail = false, Email = userEmail, Password = userPassword, DisplayName = userDisplayName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        loginPanel.SetActive(false);
        Debug.Log("login success");

        
        
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetUsernameResult, OnGetUsernameError);

        ChatGui chatNewComponent = FindObjectOfType<ChatGui>();
        chatNewComponent.UserName = userDisplayName;
        chatNewComponent.Connect();
        enabled = false;


        

        PlayerPrefs.SetString(PlayFabTest.UserNamePlayerPref, chatNewComponent.UserName);
    }

    private void OnLoginFailure(PlayFabError error)
    {

        Debug.LogError(error.GenerateErrorReport());
    }


    private void OnGetUsernameResult(GetAccountInfoResult result)
    {
        ChatGui chatNewComponent = FindObjectOfType<ChatGui>();
        userDisplayName = result.AccountInfo.TitleInfo.DisplayName;
        chatNewComponent.UserName = result.AccountInfo.TitleInfo.DisplayName;
        //userDisplayName = result.AccountInfo.TitleInfo.DisplayName;
        print(userDisplayName);
    }

    private void OnGetUsernameError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn; 
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUserDisplayName(string DisplayNameIn)
    {
        userDisplayName = DisplayNameIn;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);

    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("15");
        return deviceID;

    }


}
