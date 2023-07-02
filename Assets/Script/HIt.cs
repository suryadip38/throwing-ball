using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIt : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform launchPoint;
    public float launchForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }

    }
    void LaunchBall()
    {
        GameObject ball = Instantiate(ballPrefab, launchPoint.position, Quaternion.identity);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }
}

