using UnityEngine;

public class PanAndZoom : MonoBehaviour //Attach this to your camera
{
    public float smoothSpeed = 10; //The speed with which to interpolate to destination: lower = faster, higher = slower
    public float zoomTimeOut = 0.2f; //Time in seconds until zoom damping can occur if there is only one finger on the screen
    public float minDelta = 0.01f; //The minimum viewport distance to move until touch moves are tracked
    public float minY = 2; //Minimum Y position of the camera
    public float maxY = 100; //Maximum Y position of the camera

    float delta0;
    bool origin0Set;
    Vector3 origin0;

    float delta1;
    bool origin1Set;
    Vector3 origin1;

    bool middleSet;
    Vector3 middle;
    float radius;
    
    bool velocitySet;
    Vector3 velocity;

    Vector3 destination;

    float zoomTimer = -1;

    void Start()
    {
        destination = transform.position;
    }

    void Update()
    {
        int screenSize = Mathf.Min(Screen.width, Screen.height);

        if (Input.touchCount > 0 && Input.GetTouch(0).fingerId < 2)
        {
            Touch touch0 = Input.GetTouch(0);

            Ray ray;
            float t;

            if (touch0.phase == TouchPhase.Began)
            {
                delta0 = 0;
                origin0Set = false;
                middleSet = false;
                destination = transform.position;
                velocitySet = false;

                ray = GetComponent<Camera>().ScreenPointToRay(touch0.position); //Setting origin in case minDelta was not reached yet, otherwise zooming won't work
                t = Vector3.Dot(-ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
                origin0 = ray.GetPoint(t);
            }

            if (Input.touchCount > 1 && Input.GetTouch(1).fingerId == 1)
            {
                Touch touch1 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began)
                {
                    delta1 = 0;
                    origin1Set = false;
                    middleSet = false;
                    destination = transform.position;
                    velocitySet = false;

                    ray = GetComponent<Camera>().ScreenPointToRay(touch1.position);
                    t = Vector3.Dot(-ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up); //This represents the distance to the plane at Y = 0, normal = up
                    origin1 = ray.GetPoint(t);
                }

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    delta0 += touch0.deltaPosition.magnitude / screenSize;
                    delta1 += touch1.deltaPosition.magnitude / screenSize;
                    if (delta0 >= minDelta || delta1 >= minDelta) //Check if this touch moves should be tracked
                    {
                        if (!origin0Set)
                        {
                            ray = GetComponent<Camera>().ScreenPointToRay(touch0.position);
                            t = Vector3.Dot(-ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
                            origin0 = ray.GetPoint(t);
                            origin0Set = true; //Origin0 is set and ready for use
                            velocitySet = false;
                            middleSet = false;
                        }

                        if (!origin1Set)
                        {
                            ray = GetComponent<Camera>().ScreenPointToRay(touch1.position);
                            t = Vector3.Dot(-ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
                            origin1 = ray.GetPoint(t);
                            origin1Set = true;
                            velocitySet = false;
                            middleSet = false;
                        }

                        if (!middleSet)
                        {
                            middle = (origin0 + origin1) * 0.5f;
                            radius = (origin0 - origin1).magnitude * 0.5f;
                            middleSet = true;

                            origin0 = middle - transform.right * radius; //Overriding origins, they must be on the same axis
                            origin1 = middle + transform.right * radius;
                        }

                        Ray ray0 = GetComponent<Camera>().ScreenPointToRay(touch0.position);
                        float t0 = Vector3.Dot(-ray0.origin, Vector3.up) / Vector3.Dot(ray0.direction, Vector3.up);

                        Ray ray1 = GetComponent<Camera>().ScreenPointToRay(touch1.position);
                        float t1 = Vector3.Dot(-ray1.origin, Vector3.up) / Vector3.Dot(ray1.direction, Vector3.up);

                        float currentRadius = (ray0.GetPoint(t0) - ray1.GetPoint(t1)).magnitude * 0.5f;

                        Vector3 currentVector0 = middle - transform.right * currentRadius - transform.position; //Get the point on plane and subtract camera's position, used in zoom computation
                        Vector3 currentVector1 = middle + transform.right * currentRadius - transform.position;

                        Vector3 destination0 = origin0 - currentVector0; //The destination relative to the origin
                        Vector3 destination1 = origin1 - currentVector1;

                        Vector3 direction0 = currentVector0.normalized;
                        Vector3 direction1 = currentVector1.normalized;

                        float denominator0 = Vector3.Dot(direction0, Vector3.up);
                        float denominator1 = Vector3.Dot(direction1, Vector3.up);

                        Vector3 a0 = Vector3.Dot(new Vector3(0, minY, 0) - destination0, Vector3.up) / denominator0 * direction0 + destination0; //Setting the segment points
                        Vector3 a1 = Vector3.Dot(new Vector3(0, maxY, 0) - destination0, Vector3.up) / denominator0 * direction0 + destination0;

                        Vector3 b0 = Vector3.Dot(new Vector3(0, minY, 0) - destination1, Vector3.up) / denominator1 * direction1 + destination1;
                        Vector3 b1 = Vector3.Dot(new Vector3(0, maxY, 0) - destination1, Vector3.up) / denominator1 * direction1 + destination1;

                        Vector3 vec0 = a1 - a0; //Intersection calculation for two 3d vectors
                        Vector3 vec1 = b1 - b0;
                        Vector3 vec2 = b0 - a0;

                        Vector3 cross0 = Vector3.Cross(vec0, vec1);
                        Vector3 cross1 = Vector3.Cross(vec2, vec1);

                        float s = Vector3.Dot(cross1, cross0) / Mathf.Max(cross0.sqrMagnitude, 0.0000001f);

                        if (s < 0)
                        {
                            destination = (a0 + b0) * 0.5f;
                        }
                        else if (s > 1)
                        {
                            destination = (a1 + b1) * 0.5f;
                        }
                        else
                        {
                            destination = a0 + (vec0 * s); //Get the point of intersection of the segments
                        }

                        t0 = Vector3.Dot(-destination, Vector3.up) / Vector3.Dot(ray0.direction, Vector3.up); //Overriding the distance to plane using the new destiantion
                        t1 = Vector3.Dot(-destination, Vector3.up) / Vector3.Dot(ray1.direction, Vector3.up);

                        destination0 = origin0 - ray0.direction * t0;
                        destination1 = origin1 - ray1.direction * t1;

                        destination = (destination0 + destination1) * 0.5f; //Get the middle of these destinations
                    }
                }

                if (touch0.phase == TouchPhase.Ended || touch0.phase == TouchPhase.Canceled) //Reset touches
                {
                    origin0Set = false;
                    origin1Set = false;
                    zoomTimer = Time.time;
                }

                if (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled)
                {
                    origin0Set = false;
                    origin1Set = false;
                    zoomTimer = Time.time;
                }

                if (origin0Set || origin1Set)
                {
                    transform.position = Damp(transform.position, destination, smoothSpeed); //Smoothly interpolate to destination
                }
            }
            else
            {
                if (Time.time - zoomTimer < zoomTimeOut)
                {
                    if (!velocitySet)
                    {
                        velocity = Damp(transform.position, destination, smoothSpeed) - transform.position;
                        velocitySet = true;
                    }

                    transform.position += velocity;
                    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
                    velocity = Damp(velocity, Vector3.zero, smoothSpeed * 0.5f);
                }

                if (touch0.phase == TouchPhase.Moved)
                {
                    delta0 += touch0.deltaPosition.magnitude / screenSize;
                    if (delta0 >= minDelta)
                    {
                        if (!origin0Set)
                        {
                            ray = GetComponent<Camera>().ScreenPointToRay(touch0.position);
                            t = Vector3.Dot(-ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
                            origin0 = ray.origin + ray.direction * t;
                            origin0Set = true;
                        }

                        ray = GetComponent<Camera>().ScreenPointToRay(touch0.position);
                        t = Vector3.Dot(-transform.position, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
                        destination = origin0 - ray.direction * t; //Where the camera should go
                    }
                }

                if (touch0.phase == TouchPhase.Ended || touch0.phase == TouchPhase.Canceled)
                {
                    if (Time.time - zoomTimer >= zoomTimeOut) //Reset velocity if zoomTimeOut ended
                    {
                        velocitySet = false;
                    }
                }

                if (origin0Set)
                {
                    transform.position = Damp(transform.position, destination, smoothSpeed);
                }
            }
        }
        else //Some inertia
        {
            if (!velocitySet)
            {
                velocity = Damp(transform.position, destination, smoothSpeed) - transform.position;
                velocitySet = true;
            }

            transform.position += velocity;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
            velocity = Damp(velocity, Vector3.zero, smoothSpeed * 0.5f);
        }
    }

    Vector3 Damp(Vector3 a, Vector3 b, float lambda)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * Time.deltaTime));
    }
}