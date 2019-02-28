using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnHexagon : MonoBehaviour {

    Vector3 V2zero;
    Transform t_transform;

	private void Start () {
        V2zero = Vector3.zero;
        t_transform = GetComponent<Transform>();

    }

    const float PI = 3.141618f;

	private Vector2 flat_hex_corner(Vector2 center, int size, int i)
    {
        int angle_deg = 60 * i;
        float angle_rad = PI / 180 * angle_deg;

        return new Vector2
            (
            center.x + size * Mathf.Cos(angle_rad),
            center.y + size *  Mathf.Sin(angle_rad)
            );
    }
    [ContextMenu("create Hexagon")]
    public void flat_hex_corner()
    {
        int size = 5;
        
        for (int i = 0; i < 6; i ++)
        {
            int angle_deg = 60 * i;
            float angle_rad = PI / 180 * angle_deg;

            Vector3 CornerPosition = new Vector3(
               V2zero.x + size * Mathf.Cos(angle_rad),
               V2zero.y,
               V2zero.z + size * Mathf.Sin(angle_rad));
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = i.ToString();
            sphere.transform.parent = t_transform;
            sphere.transform.position = CornerPosition;

        }
    }

}
