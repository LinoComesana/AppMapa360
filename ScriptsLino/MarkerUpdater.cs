using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class MarkerUpdater : MonoBehaviour
{
    public AbstractMap map;
    public Vector2d location;

    void LateUpdate()
    {
        if (map != null)
        {
            transform.position = map.GeoToWorldPosition(location, true);
        }
    }
}
