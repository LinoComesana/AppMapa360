using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirestoreManager : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task GuardarMarcador(string nombre, double lat, double lon, string urlImagen360)
    {
        DocumentReference docRef = db.Collection("marcadores").Document();

        var data = new Dictionary<string, object>
        {
            { "nombre", nombre },
            { "lat", lat },
            { "lon", lon },
            { "imagen360", urlImagen360 }
        };

        await docRef.SetAsync(data);
        Debug.Log("✅ Marcador guardado en Firestore.");
    }
}
