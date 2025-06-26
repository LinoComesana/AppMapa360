using UnityEngine;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SubidaImagenFirebase : MonoBehaviour
{

    public string rutaLocalImagen; // Este valor se llenará al seleccionar una imagen local
    public string nombreArchivoFirebase = "imagen_" + Guid.NewGuid().ToString() + ".jpg";
    public string carpetaFirebase = "imagenes360";

    public async Task<string> SubirImagenYObtenerURL()
    {
        if (string.IsNullOrEmpty(rutaLocalImagen) || !File.Exists(rutaLocalImagen))
        {
            Debug.LogError("Ruta inválida o archivo inexistente.");
            return null;
        }

        // Leer la imagen como bytes
        byte[] bytes = File.ReadAllBytes(rutaLocalImagen);

        // Crear referencia en Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference imagenRef = storage.GetReference(carpetaFirebase).Child(nombreArchivoFirebase);

        // Subir la imagen
        var metadata = new MetadataChange();
        metadata.ContentType = "image/jpeg";

        try
        {
            await imagenRef.PutBytesAsync(bytes, metadata);
            Uri uri = await imagenRef.GetDownloadUrlAsync();
            string url = uri.ToString(); // <- conversión correcta
            Debug.Log("✅ Imagen subida. URL: " + url);
            return url;
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Error al subir imagen: " + e.Message);
            return null;
        }
    }



    public void SeleccionarImagen()
    {
    #if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Seleccionar imagen 360", "", "jpg,png");

        if (!string.IsNullOrEmpty(path))
        {
            rutaLocalImagen = path;
            Debug.Log("🖼 Imagen seleccionada: " + rutaLocalImagen);
        }
        else
        {
            Debug.LogWarning("⚠ No se seleccionó ninguna imagen.");
        }
    #else
        Debug.LogWarning("⚠ Selección de imagen sólo disponible en el editor de Unity por ahora.");
    #endif
    }



}
