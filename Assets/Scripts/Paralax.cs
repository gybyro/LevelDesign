using System;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    Vector2 startPos;
    float startZ;
    
    
    Vector2 travel => (Vector2)cam.transform.position - startPos;

    // technically the parallax factor
    // 0.99 < far away to -0.99 < in yo face
    public float hey;

    float distanceFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    public void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }

    public void Update()
    {
        // Vector2 newPos = startPos + travel * parallaxFactor;
        Vector2 newPos = startPos + travel * hey;

        transform.position = new Vector3(newPos.x, newPos.y, startZ);


    }


}
