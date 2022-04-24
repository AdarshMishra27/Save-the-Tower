using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField][Range(0f, 5f)] float enemySpeed = 1f;
    Enemy enemy;
    private void Start() {
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    void FindPath() {
        GameObject parent=GameObject.FindGameObjectWithTag("Path");

        path.Clear();

        foreach (Transform child in parent.transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if(waypoint!=null)
                path.Add(waypoint);
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (Waypoint item in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = item.transform.position;
            float travelPercent = 0f;

            //old movemnt (not smooth)
            // transform.position = Vector3.Lerp(startPosition,endPosition,travelPercent);
            // yield return new WaitForSeconds(waitTime);

            //new movement (smooth)
            transform.LookAt(endPosition); //enemy is rotating to the path followed

            while (travelPercent < 1f)
            { //smoothining the enemy movement process using LERP->Linear Interpolation
                travelPercent += Time.deltaTime * enemySpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinshPath();
    }

    private void FinshPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
}
