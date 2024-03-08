using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Linq;

public class handler : MonoBehaviour
{
   
    public TMP_Text puntajesText;
    string url = "https://sid-restapi.onrender.com";
    public string Token { get; private set; }
    public string Username { get; private set; }

    void Start()
    {
        Token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay Token");
            //PanelAuth.SetActive(true);
        }
        else
        {
            Username = PlayerPrefs.GetString("username");
            StartCoroutine("GetProfile");
        }
    }

    public void enviarRegistro()
    {
        string username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine(Registro(JsonUtility.ToJson(new AuthenticationData { username = username, password = password })));
    }

    public void enviarLogin()
    {
        string username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine(Login(JsonUtility.ToJson(new AuthenticationData { username = username, password = password })));
    }

    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("Registro Exitoso!");
                StartCoroutine(Login(json));
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/auth/login", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                AuthenticationData data = JsonUtility.FromJson<AuthenticationData>(request.downloadHandler.text);
                Token = data.token;
                Username = data.username;
                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("username", Username);

                Debug.Log(data.token);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator GetProfile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios/" + Username);
        request.SetRequestHeader("x-token", Token);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);

                string puntajesInfo = "";

                foreach (var usuario in response.usuarios)
                {
                    puntajesInfo += "El usuario " + usuario.username + " tiene un puntaje de " + usuario.data.score + "\n";
                }

                
                puntajesText.text = puntajesInfo;
            }
            else
            {
                Debug.Log("El usuario no está autenticado");
            }
        }
    }

    IEnumerator Highscore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", Token);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
    }

    [System.Serializable]
    public class AuthResponse
    {
        public List<UsuarioJson> usuarios;
    }

    [System.Serializable]
    public class AuthenticationData
    {
        public string username;
        public string password;
        public UsuarioJson usuario;
        public string token;
        public bool estado;
        public DataUser data;
    }

    [System.Serializable]
    public class UsuarioJson
    {
        public string _id;
        public string username;
        public DataUser data;
    }

    [System.Serializable]
    public class DataUser
    {
        public int score;
    }
}
