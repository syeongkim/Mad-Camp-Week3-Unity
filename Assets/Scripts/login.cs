using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GitHubLogin : MonoBehaviour
{
    public Button loginButton;
    public Text userInfoText;

    private void Start()
    {
        if (loginButton == null)
        {
            loginButton = GetComponentInChildren<Button>();
        }

        if (userInfoText == null)
        {
            userInfoText = GetComponentInChildren<Text>();
        }

        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    public void OnLoginButtonClick()
    {
        Application.OpenURL("http://localhost:3000/oauth/github");
    }

    public void HandleGitHubCallback(string code)
    {
        StartCoroutine(GetGitHubUser(code));
    }

    private IEnumerator GetGitHubUser(string code)
    {
        UnityWebRequest request = UnityWebRequest.Get($"http://localhost:3000/oauth/github/callback?code={code}");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            string jsonResult = request.downloadHandler.text;
            userInfoText.text = jsonResult;
        }
    }
}