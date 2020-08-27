using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;
    public float speedPerSecond = 300f;
    public ForceMode2D fMode;

    public float nextWaypointDistance = 3f;
    private int currentWaypoint = 0;

    [HideInInspector]
    public bool pathIsEnded = false;

    private bool isSearchingForPlayer = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForTarget());
            }
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForTarget());
            }
            return;
        }

        //lookat

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speedPerSecond * Time.fixedDeltaTime;

        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    private IEnumerator SearchForTarget()
    {
        GameObject result = GameObject.FindGameObjectWithTag("Player");
        if (result == null)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(SearchForTarget());
        }
        else
        {
            target = result.transform;
            isSearchingForPlayer = false;
            StartCoroutine(UpdatePath());

            yield return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForTarget());
            }
            yield return false;
        }
        else
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        yield return new WaitForSeconds(1 / updateRate);

        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
