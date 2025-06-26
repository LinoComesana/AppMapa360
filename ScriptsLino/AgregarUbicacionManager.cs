using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class AgregarUbicacionManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_InputField inputNombre;
    public TMP_InputField inputLat;
    public TMP_InputField inputLon;

    [Header("Referencias de Lógica")]
    public FirestoreManager firestoreManager;
    public SubidaImagenFirebase subidaImagen;

    public async void GuardarMarcador()
    {
        string nombre = inputNombre.text;

        if (string.IsNullOrWhiteSpace(nombre) || 
            string.IsNullOrWhiteSpace(inputLat.text) || 
            string.IsNullOrWhiteSpace(inputLon.text))
        {
            Debug.LogWarning("❗ Todos los campos deben estar completos.");
            return;
        }

        if (!double.TryParse(inputLat.text, out double lat) || 
            !double.TryParse(inputLon.text, out double lon))
        {
            Debug.LogWarning("❗ Latitud y longitud deben ser números válidos.");
            return;
        }

        // Subir imagen y obtener su URL
        string urlImagen = await subidaImagen.SubirImagenYObtenerURL();

        if (!string.IsNullOrEmpty(urlImagen))
        {
            await firestoreManager.GuardarMarcador(nombre, lat, lon, urlImagen);
            Debug.Log("✅ Marcador subido correctamente a Firestore.");
        }
        else
        {
            Debug.LogWarning("❌ No se pudo subir la imagen o recuperar su URL.");
        }
    }
}
