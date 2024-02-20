using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class HttpHandler : MonoBehaviour
{
    public RawImage[] images;
    private string FakeApiUrl = "https://my-json-server.typicode.com/Alejandroguerra4915/sistemas-interactivos";
    private string RickYMortyApiUrl = "https://rickandmortyapi.com/api";
    private Coroutine sendRequest_GetCharacters;
    private int nextImageIndex = 0; 
    public TextMeshProUGUI[] characterTexts;
    public TextMeshProUGUI userText;

    public void SendRequest()
    {
        nextImageIndex = 0; 
        if (sendRequest_GetCharacters == null)
            sendRequest_GetCharacters = StartCoroutine(GetUserData(1));
    }

    public void SendRequest2()
    {
        nextImageIndex = 0; 
        if (sendRequest_GetCharacters == null)
            sendRequest_GetCharacters = StartCoroutine(GetUserData(2));
    }

    public void SendRequest3()
    {
        nextImageIndex = 0; 
        if (sendRequest_GetCharacters == null)
            sendRequest_GetCharacters = StartCoroutine(GetUserData(3));
    }

    IEnumerator GetUserData(int uid)
    {
        UnityWebRequest request = UnityWebRequest.Get(FakeApiUrl + "/users/" + uid);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                Debug.Log(user.username);

                foreach (int cardid in user.deck)
                {
                    StartCoroutine(GetCharacter(cardid));
                }
                userText.text = user.username;
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
        sendRequest_GetCharacters = null;
    }

    IEnumerator GetCharacter(int id)
    {
        UnityWebRequest request = UnityWebRequest.Get(RickYMortyApiUrl + "/character/" + id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
                Debug.Log(character.name + " is a " + character.species);
                Debug.Log(character.image);

                if (nextImageIndex < images.Length)
                {
                    StartCoroutine(DownloadImage(character.image, nextImageIndex));
                    string characterInfo = $"{character.name} is a {character.species}";
                    characterTexts[nextImageIndex].text = characterInfo;
                    nextImageIndex++;
                }
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator DownloadImage(string url, int index)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            images[index].texture = texture; 
        }
    }
}

[System.Serializable]
public class UserData
{
    public int id;
    public string username;
    public int[] deck;
}

[System.Serializable]
public class CharactersList
{
    public charactersInfo info;
    public Character[] results;
}

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}

[System.Serializable]
public class charactersInfo
{
    public int count;
    public int pages;
    public string prev;
    public string next;
}
