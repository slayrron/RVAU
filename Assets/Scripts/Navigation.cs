using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float walkingSpeed = 3.0f;
    public float rotationSpeed = 3.0f;
    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up") || Input.GetKey(KeyCode.Z))
        {
            transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);
        }
        if (Input.GetKey("down"))
        {
            transform.Translate(Vector3.back * walkingSpeed * Time.deltaTime);
        }
        if (Input.GetKey("left"))
        {
            transform.Translate(Vector3.left * walkingSpeed * Time.deltaTime);
        }
        if (Input.GetKey("right"))
        {
            transform.Translate(Vector3.right * walkingSpeed * Time.deltaTime);
        }
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
        }
        {
            //camera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * rotationSpeed);
        }
    }
}
