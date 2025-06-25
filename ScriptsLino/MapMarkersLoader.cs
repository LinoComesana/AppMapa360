using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;

[System.Serializable]
public class MarcadorData
{
    public string nombre;
    public double lat;
    public double lon;
    public string imagen360;
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
    public string archivoJSON = "marcadores"; // nombre sin extensión

    void Start()
    {
        StartCoroutine(EsperarYCrearMarcadores());
    }

    IEnumerator EsperarYCrearMarcadores()
    {
        yield return new WaitForSeconds(2f); // espera 2 segundos
        CargarMarcadoresDesdeJSON();
    }

    void CargarMarcadoresDesdeJSON()
    {
        TextAsset archivo = Resources.Load<TextAsset>(archivoJSON);
        if (archivo == null)
        {
            Debug.LogError("Archivo JSON no encontrado en Resources.");
            return;
        }

        string jsonEnvuelto = "{\"marcadores\":" + archivo.text + "}";
        MarcadorListWrapper data = JsonUtility.FromJson<MarcadorListWrapper>(jsonEnvuelto);

        if (data == null || data.marcadores == null)
        {
            Debug.LogError("No se pudieron cargar los datos de marcadores.");
            return;
        }

        foreach (MarcadorData marcador in data.marcadores)
        {
            Debug.Log($"Instanciando marcador: {marcador.nombre} en {marcador.lat}, {marcador.lon}");

            Vector2d location = new Vector2d(marcador.lat, marcador.lon);
            Vector3 worldPos = map.GeoToWorldPosition(location, true);

            GameObject nuevoMarcador = Instantiate(marcadorPrefab, worldPos, Quaternion.identity);
            nuevoMarcador.name = marcador.nombre;
            nuevoMarcador.transform.SetParent(map.transform, true);

            MarkerUpdater updater = nuevoMarcador.AddComponent<MarkerUpdater>();
            updater.map = map;
            updater.location = new Vector2d(marcador.lat, marcador.lon);


            // Añadir el script que abrirá la escena al hacer clic
            if (nuevoMarcador.GetComponent<ButtonMarcador>() == null)
            {
                nuevoMarcador.AddComponent<ButtonMarcador>();
            }

            // Añadir el script que abrirá la escena al hacer clic
            ButtonMarcador boton = nuevoMarcador.AddComponent<ButtonMarcador>();

            boton.nombreImagen360 = marcador.imagen360; // Cargamos directamente desde el JSON



        }
    }
}
