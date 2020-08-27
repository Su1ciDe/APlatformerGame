using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] bgs;
    private float[] parallaxScales;
    public float smooth =1f;

    private Transform cam;
    private Vector3 previousCamPos;

    // Called before Start()
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = cam.position;

        parallaxScales = new float[bgs.Length];

        for (int i = 0; i < bgs.Length; i++)
        {
            parallaxScales[i] = bgs[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Parallax();
    }

    void Parallax()
    {
        for (int i = 0; i < bgs.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float bgTargetPosX = bgs[i].position.x + parallax;
            Vector3 bgTargerPos = new Vector3(bgTargetPosX, bgs[i].position.y, bgs[i].position.z);

            bgs[i].position = Vector3.Lerp(bgs[i].position, bgTargerPos, smooth * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
