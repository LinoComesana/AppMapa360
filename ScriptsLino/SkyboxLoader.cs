using UnityEngine;

public class SkyboxLoader : MonoBehaviour
{
    public Material skyboxPlantilla; // Este es un material base con shader "Skybox/Panoramic"
    
    void Start()
    {
        string nombreImagen = PlayerPrefs.GetString("imagen360", "");
        if (string.IsNullOrEmpty(nombreImagen))
        {
            Debug.LogWarning("No se encontró imagen 360 en PlayerPrefs.");
            return;
        }

        // Carga la imagen desde Resources
        Texture tex = Resources.Load<Texture>("Imagenes360/" + System.IO.Path.GetFileNameWithoutExtension(nombreImagen));
        if (tex == null)
        {
            Debug.LogError("No se encontró la textura: " + nombreImagen);
            return;
        }

        // Creamos una instancia del material base
        Material nuevoSkybox = new Material(skyboxPlantilla);
        nuevoSkybox.SetTexture("_MainTex", tex);

        RenderSettings.skybox = nuevoSkybox;
    }
}