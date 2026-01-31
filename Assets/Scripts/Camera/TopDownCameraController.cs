using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 15f;

    [Header("Zoom (Orthographic)")]
    public float zoomSpeed = 5f;
    public float minOrthoSize = 5f;
    public float maxOrthoSize = 25f;

    [Header("Bounds (optional)")]
    public bool useBounds = false;
    public Vector2 minXZ = new Vector2(-50, -50);
    public Vector2 maxXZ = new Vector2(50, 50);

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Move
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0f, v).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Zoom
        if (cam.orthographic)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
            }
        }

        // Clamp
        if (useBounds)
        {
            Vector3 p = transform.position;
            p.x = Mathf.Clamp(p.x, minXZ.x, maxXZ.x);
            p.z = Mathf.Clamp(p.z, minXZ.y, maxXZ.y);
            transform.position = p;
        }
    }
}
