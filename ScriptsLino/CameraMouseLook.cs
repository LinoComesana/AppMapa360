using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMouseLook : MonoBehaviour
{
    public float sensibilidad = 3.0f;
    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Visor360Scene")
        {
            enabled = false; // Desactiva este script en otras escenas
            return;
        }

        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.y;
        rotY = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        rotX += mouseX;
        rotY -= mouseY;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0f);
        transform.rotation = localRotation;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
