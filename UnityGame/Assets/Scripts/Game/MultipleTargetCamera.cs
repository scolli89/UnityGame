using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public GameObject[] ptargets;

    public Vector3 offset;
    public float smoothTime = .5f;

    public float minZoom = 25f;
    public float maxZoom = 5f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        ptargets = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ptarget in ptargets)
        {
            targets.Add(ptarget.transform);
        }
        cam = GetComponent<Camera>();
        //targets = Get;
        offset = new Vector3(0, 0, -1);
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        centerCamera();
        zoomCamera();
    }

    void centerCamera()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void zoomCamera()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / 50f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return Mathf.Max(bounds.size.x, bounds.size.y);
        //return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector2.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
