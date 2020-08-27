using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public int rotationOffset=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rotationZ = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg ;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ+ rotationOffset);
    }
}
