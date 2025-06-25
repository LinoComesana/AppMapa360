using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonVolver : MonoBehaviour
{
    public void VolverAlMapa()
    {
        SceneManager.LoadScene("SceneMapa");
    }
}
