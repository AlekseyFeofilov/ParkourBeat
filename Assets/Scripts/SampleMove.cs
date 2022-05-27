using System;
using UnityEngine;

public class SampleMove : MonoBehaviour
{

    private Vector3 _startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaY = (float) Math.Sin(Time.time * 20f);
        transform.position = _startPosition + new Vector3(0, deltaY, 0);
        transform.Rotate(Time.deltaTime * 4, Time.deltaTime * 16, 0);
    }
}