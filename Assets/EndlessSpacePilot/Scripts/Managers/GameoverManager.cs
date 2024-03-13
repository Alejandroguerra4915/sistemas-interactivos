using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace EndlessSpacePilot
{
    public class GameoverManager : MonoBehaviour
    {
        public static GameoverManager instance;
        public Text scoreText;           
        public string url = "https://sid-restapi.onrender.com/api/usuarios"; 

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
            if (!PlayerPrefs.HasKey("Token") || !PlayerPrefs.HasKey("username"))
            {
                Debug.LogWarning("Token o nombre de usuario no encontrados en PlayerPrefs.");
                return;
            }
        }

        void Update()
        {
           
            scoreText.text = PlayerManager.playerScore.ToString();
        }

        /// <summary>
     
        /// </summary>
        public void SaveScore()
        {
            StartCoroutine(SendScore(PlayerPrefs.GetString("username"), PlayerManager.playerScore));
        }

       

		IEnumerator SendScore(string username, int score)
			{
				
				string fullUrl = url + "/api/usuarios/" + username + "/score";

				DataUser newData = new DataUser();
				newData.username = username;
				newData.score = score;

				string json = JsonUtility.ToJson(newData);

				using (UnityWebRequest request = UnityWebRequest.Put(fullUrl, json))
            {
                request.method = "PATCH";
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));

                
                yield return request.SendWebRequest();

               
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("Error de conexión al enviar el puntaje: " + request.error);
                }
                else
                {
                    if (request.responseCode == 200)
                    {
                        Debug.Log("Puntaje actualizado con éxito");
                    }
                    else
                    {
                        Debug.LogError("Error al actualizar puntaje - Código de respuesta: " + request.responseCode);
                        Debug.LogError("Mensaje de error: " + request.downloadHandler.text);
                    }
                }
            }
        }
    }

    
    [System.Serializable]
	public class DataUser
	{
		public string username;
		public int score;
	}

}
