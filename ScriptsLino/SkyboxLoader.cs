using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SkyboxLoader : MonoBehaviour
{
    void Start()
    {
        string url = PlayerPrefs.GetString("imagen360");

        Debug.Log("🛰️ Cargando imagen desde URL: " + url);

        if (!string.IsNullOrEmpty(url))
        {
            StartCoroutine(CargarImagenSkybox(url));
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró URL en PlayerPrefs.");
        }
    }

    IEnumerator CargarImagenSkybox(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al cargar imagen desde URL: " + request.error);
        }
        else
        {
            Texture tex = DownloadHandlerTexture.GetContent(request);
            Material skyboxMat = RenderSettings.skybox;

            if (skyboxMat != null && skyboxMat.HasProperty("_MainTex"))
            {
                skyboxMat.SetTexture("_MainTex", tex);
            }
            else
            {
                Debug.LogError("⚠️ El material del Skybox no tiene la propiedad _MainTex.");
            }
        }
    }
}
