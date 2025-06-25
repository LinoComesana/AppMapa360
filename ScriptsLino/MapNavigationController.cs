using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class MapNavigationController : MonoBehaviour
{
    public AbstractMap map;
    public float baseLatLonStep = 0.0003f;       // Velocidad mínima (grados)
    public float maxLatLonStep = 0.1f;          // Velocidad máxima
    public float accelerationRate = 0.002f;      // Qué tan rápido acelera

    private float currentSpeed = 0f;
    private string lastKeyPressed = "";

    void Update()
    {
        if (map == null) return;

        Vector2d center = map.CenterLatitudeLongitude;
        string keyPressed = "";

        // Detectar teclas y mover
        if (Input.GetKey(KeyCode.W)) { keyPressed = "W"; center.x += currentSpeed; }
        else if (Input.GetKey(KeyCode.S)) { keyPressed = "S"; center.x -= currentSpeed; }
        else if (Input.GetKey(KeyCode.D)) { keyPressed = "D"; center.y += currentSpeed; }
        else if (Input.GetKey(KeyCode.A)) { keyPressed = "A"; center.y -= currentSpeed; }
        else
        {
            currentSpeed = baseLatLonStep;
            lastKeyPressed = "";
        }

        // Aceleración progresiva
        if (keyPressed != "")
        {
            if (keyPressed == lastKeyPressed)
            {
                currentSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxLatLonStep);
            }
            else
            {
                currentSpeed = baseLatLonStep;
                lastKeyPressed = keyPressed;
            }
        }

        // Limitar latitud (no puede exceder ±85 grados por proyección Web Mercator)
        center.x = Mathf.Clamp((float)center.x, -85f, 85f);

        // Reaparecer al otro lado si excede la longitud
        if (center.y > 180) center.y -= 360;
        if (center.y < -180) center.y += 360;

        // Zoom
        if (Input.GetKeyDown(KeyCode.N)) map.UpdateMap(center, map.Zoom + 1);
        if (Input.GetKeyDown(KeyCode.M)) map.UpdateMap(center, map.Zoom - 1);

        map.UpdateMap(center, map.Zoom);
    }
}
