using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0f;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform hitParticlePrefab;
    public Transform muzzleflashPrefab;

    float timeToFire = 0;
    Transform firePoint;

    public float camShakeAmt = 0.1f;
    public float camShakeDuration = 0.1f;
    CameraShake camShake;

    AudioManager audioManager;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
    }

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();

        audioManager = AudioManager.am;
    }

    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.Log("Hit " + damage + " damage to " + hit.collider.name);
            Debug.DrawLine(firePointPosition, hit.point, Color.red);

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
            }
        }

        Vector3 hitPos = Vector3.zero;
        Vector3 hitNormal;
        if (hit.collider == null)
        {
            hitPos = (mousePosition - firePointPosition) * 30;
            hitNormal = new Vector3(9999, 9999, 9999);
        }
        else
        {
            hitPos = hit.point;
            hitNormal = hit.normal;
        }
        ShootEffect(hitPos, hitNormal);
    }

    void ShootEffect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.05f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform _hitParticle = Instantiate(hitParticlePrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(_hitParticle.gameObject, 0.04f);
        }

        Transform muzzleflash = Instantiate(muzzleflashPrefab, firePoint.position, firePoint.rotation) as Transform;
        muzzleflash.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleflash.localScale = new Vector3(size, size, size);
        Destroy(muzzleflash.gameObject, 0.02f);

        // Camera Shake
        camShake.Shake(camShakeAmt, camShakeDuration);

        // Shoot sound
        audioManager.PlaySound("PistolSound");
    }
}
