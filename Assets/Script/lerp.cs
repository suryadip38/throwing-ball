using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerp : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float movementDistance = 10f;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f)
        {
            float targetX = Mathf.Clamp(transform.position.x + (horizontalInput * movementDistance), -movementDistance, movementDistance);
            targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
    
