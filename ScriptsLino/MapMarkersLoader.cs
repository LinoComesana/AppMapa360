using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Mapbox.Unity.Map;
using Mapbox.Utils;

[System.Serializable]
public class MarcadorData
{
    public string nombre;
    public double lat;
    public double lon;
    public string imagen360; // ahora usamos este campo, como en el JSON original
}

[System.Serializable]
public class MarcadorListWrapper
{
    public List<MarcadorData> marcadores;
}

public class MapMarkersLoader : MonoBehaviour
{
    public AbstractMap map;
    public GameObject marcadorPrefab;

    [Header("Firebase Settings")]
    public string jsonURL = "https://mapa360app.web.app/marcadores.json";

    void Start()
    {
        StartCoroutine(DescargarYCrearMarcadores());
    }

    IEnumerator DescargarYCrearMarcadores()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(jsonURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al descargar JSON: " + request.error);
                yield break;
            }

            string jsonOriginal = request.downloadHandler.text;
            string jsonConWrapper = "{\"marcadores\":" + jsonOriginal + "}";
            MarcadorListWrapper data = JsonUtility.FromJson<MarcadorListWrapper>(jsonConWrapper);

            if (data == null || data.marcadores == null)
            {
                Debug.LogError("Error al parsear JSON.");
                yield break;
            }

            foreach (MarcadorData marcador in data.marcadores)
            {
                Debug.Log($"Instanciando marcador: {marcador.nombre} en {marcador.lat}, {marcador.lon}");

                Vector2d location = new Vector2d(marcador.lat, marcador.lon);
                Vector3 worldPos = map.GeoToWorldPosition(location, true);

                GameObject nuevoMarcador = Instantiate(marcadorPrefab, worldPos, Quaternion.identity);
                nuevoMarcador.name = marcador.nombre;
                nuevoMarcador.transform.SetParent(map.transform, true);

                // Asegura posición vinculada al mapa (importante)
                MarkerUpdater updater = nuevoMarcador.AddComponent<MarkerUpdater>();
                updater.map = map;
                updater.location = location;

                // Asignar comportamiento de click para cambiar de escena
                ButtonMarcador boton = nuevoMarcador.AddComponent<ButtonMarcador>();
                boton.nombreImagen360 = marcador.imagen360;
            }
        }
    }
}
