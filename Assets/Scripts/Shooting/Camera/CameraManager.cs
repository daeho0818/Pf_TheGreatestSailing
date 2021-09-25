using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } = null;

    bool shake = false;

    Vector3 targetPos;
    bool zoom = false;
    bool zoomWait = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    void Update()
    {
        if (shake)
        {
            Camera camera = Camera.main;
            camera.transform.position = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1.5f), -10);
        }

        if (zoom && !zoomWait)
        {
            Camera camera = Camera.main;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, 0.5f);
            camera.orthographicSize -= 0.01f;
        }
    }

    public void ShakeCam(float time)
    {
        shake = true;
        Invoke("ShakeFinish", time);
    }

    public void ZoomCam(Vector3 pos, float time, float wait)
    {
        pos.z = -10;
        zoom = true;
        targetPos = pos;
        Invoke("Zoom", time);
        Invoke("ZoomFinish", time + wait);
    }


    void ShakeFinish()
    {
        shake = false;
        Camera.main.transform.position = new Vector3(0, 1, -10);
    }
    void Zoom()
    {
        zoom = false;
        zoomWait = true;
    }
    void ZoomFinish()
    {
        zoomWait = false;
        Camera camera = Camera.main;
        camera.transform.position = new Vector3(0, 1, -10);
        camera.orthographicSize = 5f;
    }
}
