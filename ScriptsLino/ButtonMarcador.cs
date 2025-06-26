using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMarcador : MonoBehaviour
{
    // Nombre del archivo de imagen a usar en el visor
    public string nombreImagen360;

    void OnMouseDown()
    {
        Debug.Log("Marcador tocado: " + gameObject.name);

        // Guardamos el nombre de la imagen para que lo use la otra escena
        PlayerPrefs.SetString("imagen360", nombreImagen360);
        PlayerPrefs.Save();

        // Cargamos la escena del visor
        SceneManager.LoadScene("Visor360Scene");
    }
}