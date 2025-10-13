using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2DPictrue : MonoBehaviour
{
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            var rotate = transform.localRotation.eulerAngles;
            rotate.z += speed;
            transform.localRotation = Quaternion.Euler(rotate);
        }
    }

}
