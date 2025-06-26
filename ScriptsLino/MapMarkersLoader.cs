using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Firebase.Firestore;

[System.Serializable]
public class MarcadorData
{
    public string nombre;
    public double lat;
    public double lon;
    public string imagen360;
}

public class MapMarkersLoader : MonoBehaviour
{
    public AbstractMap map;
    public GameObject marcadorPrefab;

    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        StartCoroutine(EsperarYDescargarMarcadores());
    }

    private IEnumerator EsperarYDescargarMarcadores()
    {
        yield return new WaitForSeconds(2f);
        _ = DescargarYCrearMarcadores(); // Llama al método async sin bloquear
    }

    private async System.Threading.Tasks.Task DescargarYCrearMarcadores()
    {
        QuerySnapshot snapshot = await db.Collection("marcadores").GetSnapshotAsync();

        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> data = doc.ToDictionary();

            string nombre = data["nombre"].ToString();
            double lat = double.Parse(data["lat"].ToString());
            double lon = double.Parse(data["lon"].ToString());
            string imagen360 = data["imagen360"].ToString();

            Debug.Log($"📌 Marcador cargado: {nombre} ({lat}, {lon})");

            Vector2d location = new Vector2d(lat, lon);
            Vector3 worldPos = map.GeoToWorldPosition(location, true);

            GameObject nuevoMarcador = Instantiate(marcadorPrefab, worldPos, Quaternion.identity);
            nuevoMarcador.name = nombre;
            nuevoMarcador.transform.SetParent(map.transform, true);

            // Fijar posición real
            MarkerUpdater updater = nuevoMarcador.AddComponent<MarkerUpdater>();
            updater.map = map;
            updater.location = location;

            // Añadir script para abrir visor
            ButtonMarcador boton = nuevoMarcador.AddComponent<ButtonMarcador>();
            boton.nombreImagen360 = imagen360;
        }
    }
}
