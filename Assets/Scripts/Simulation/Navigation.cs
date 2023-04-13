using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{


    int index = 0;
    public float moveSpeed = 10f;
    public float rotateSpeed = 100f;
    float angleEps = 0.1f;
    float distanceEps = 0.1f;
    int increment = 1;
    [HideInInspector]
    public Vector3[] wayPoints;
    int pointsTravelled = 0;
    bool move = false;

    void Start()
    {

        
    }

    void Update()
    {

        if (move)
        {
            float distance = Vector3.Distance(transform.position, wayPoints[index]);
            if (distance > distanceEps)
            {
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[index], moveSpeed * Time.deltaTime);
            }
            else
            {
                if (index == wayPoints.Length - 1)
                {
                    move = false;
                    increment = -1;
                }
                else if (index == 0)
                {
                    move = false;
                    increment = 1;
                }

                index += increment;

            }
        }

        else
        {
            Debug.Log("rotate");
            Vector3 targetDirection = wayPoints[index] - transform.position;
            if (Vector3.Angle(transform.forward, targetDirection) <= angleEps)
            {
                move = true;
                pointsTravelled = 0;

            }
            else
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                transform.rotation = newRotation;
            }
        }




        //// Move and rotate together
        //float distance = Vector3.Distance(transform.position, wayPoints[index]);
        //if (distance > distanceEps)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, wayPoints[index], moveSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    index = (index + 1) % wayPoints.Length;
        //    state = State.Rotate;
        //}
        //Vector3 targetDirection = wayPoints[index] - transform.position;
        //if (Vector3.Angle(transform.forward, targetDirection) >= angleEps)
        //{
        //    Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        //    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
        //    transform.rotation = newRotation;
        //}


    }
}
