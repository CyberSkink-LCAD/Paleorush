using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public GameObject WaypointA;
    public GameObject WaypointB;
    public float speed = 5f;
    public float delay = 1f;
    public Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform.transform.position = WaypointA.transform.position;
        targetPosition = WaypointB.transform.position;
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        while (true)
        {
            while ((targetPosition - platform.transform.position).sqrMagnitude > 0.01f)
            {
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            targetPosition = targetPosition == WaypointA.transform.position ? WaypointB.transform.position : WaypointA.transform.position;
            yield return new WaitForSeconds(delay);
        }
        

    }
}
