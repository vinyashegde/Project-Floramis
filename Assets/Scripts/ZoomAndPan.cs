using UnityEngine;

public class ZoomAndPan : MonoBehaviour
{
    private Vector3 touchStart;
    private Camera cam;

    public float zoomSpeed = 0.01f;
    public float panSpeed = 0.1f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Zoom
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, 10f);
        }

        // Pan
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += direction * panSpeed;
        }
    }
}
