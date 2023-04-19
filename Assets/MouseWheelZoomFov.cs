using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWheelZoomFov : MonoBehaviour
{
    // Start is called before the first frame update

    public float minFOV;
    public float maxFOV;
    public float sensitivity;
    public float FOV;

    public float rotation;
    public float sensitivityRotation;


    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse2))
        {
            rotation = Camera.main.transform.localEulerAngles.z;
            rotation += (Input.GetAxis("Mouse ScrollWheel") * sensitivityRotation) * -1;
            Camera.main.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));

        }
        else
        {
            FOV = Camera.main.fieldOfView;
            FOV += (Input.GetAxis("Mouse ScrollWheel") * sensitivity) * -1;
            FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
            Camera.main.fieldOfView = FOV;


        }
        
    }

}
