using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    public int offsetX = 2;

    public bool hasARightTile = false;
    public bool hasALeftTile = false;

    public bool reverseScale = false;

    private float spriteWidth = 0f;

    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        Tile();
    }

    void Tile()
    {
        if (hasALeftTile == false || hasARightTile == false)
        {
            float camHoriExtend = cam.orthographicSize * Screen.width / Screen.height;
            float edge_VisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHoriExtend;
            float edge_VisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHoriExtend;

            if (cam.transform.position.x >= edge_VisiblePositionRight - offsetX && hasARightTile == false)
            {
                MakeNewTile(1);
                hasARightTile = true;
            }
            else if (cam.transform.position.x <= edge_VisiblePositionLeft + offsetX && hasALeftTile == false)
            {
                MakeNewTile(-1);
                hasALeftTile = true;
            }
        }
    }

    void MakeNewTile(int rightOrLeft)
    {
        Vector3 newPos = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

        Transform newTile = Instantiate(myTransform, newPos, myTransform.rotation) as Transform;

        if (reverseScale == true) // Sprite'ı ters çevirip seamless mış gibi göstermek için
        {
            newTile.localScale = new Vector3(newTile.localScale.x * -1, newTile.localScale.y, newTile.localScale.z);
        }

        newTile.parent = myTransform.parent;

        if (rightOrLeft > 0)
        {
            newTile.GetComponent<Tiling>().hasALeftTile = true;
        }
        else
        {
            newTile.GetComponent<Tiling>().hasARightTile = true;
        }
    }
}
