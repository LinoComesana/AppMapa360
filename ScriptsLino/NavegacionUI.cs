using UnityEngine;
using UnityEngine.SceneManagement;


public class NavegacionUI : MonoBehaviour
{
    public void IrASceneAgregarUbicacion()
    {
        SceneManager.LoadScene("SceneAgregarUbicacion");
    }

    public void VolverAlMapa()
    {
        SceneManager.LoadScene("SceneMapa");
    }



 

}


