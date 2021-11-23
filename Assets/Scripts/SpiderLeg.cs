using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    [SerializeField] private Transform frontLeftParented = null;
    [SerializeField] private Transform frontLeftActual = null;
    [SerializeField] private Transform frontRightParented = null;
    [SerializeField] private Transform frontRightActual = null;
    [SerializeField] private float distance = 5;
    [SerializeField] private float speed = 5;

    private bool moveFrontLeft = false;
    private bool moveFrontRight = false;

    Vector3 frontLeftTargetPosition = Vector3.zero;
    Vector3 frontRightTargetPosition = Vector3.zero;

    Coroutine frontLeft = null;
    Coroutine frontRight = null;

    /*
     * 
     * Changer en faisant en sorte que quand la distance est trop grande, la position du target est stockée et c'est là où le leg doit aller.
     * 
     */


    private void Update()
    {
        FrontLeft();
        FrontRight();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(frontLeftActual.position, .25f);
        Gizmos.DrawSphere(frontRightActual.position, .25f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(frontLeftParented.position, .25f);
        Gizmos.DrawSphere(frontRightParented.position, .25f);
    }

    private void FrontLeft()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(frontLeftActual.transform.position.x, transform.position.y, frontLeftActual.transform.position.z), Vector3.down, 50);
        if (_hits.Length > 0)
        {
            foreach (RaycastHit _hit in _hits)
            {
                if (_hit.transform != transform)
                {
                    frontLeftActual.transform.position = _hit.point;
                    break;
                }
            }
        }

        if (frontRight == null && frontLeft == null && Vector3.Distance(frontLeftActual.position, frontLeftParented.position) > distance)
        {
            frontLeftTargetPosition = frontLeftParented.position;
            frontLeft = StartCoroutine(MoveFrontLeft());
        }
    }

    private void FrontRight()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(frontRightActual.transform.position.x, transform.position.y, frontRightActual.transform.position.z), Vector3.down, 50);
        if (_hits.Length > 0)
        {
            foreach (RaycastHit _hit in _hits)
            {
                if (_hit.transform != transform)
                {
                    frontRightActual.transform.position = _hit.point;
                    break;
                }
            }
        }

        if (frontRight == null && frontLeft == null && Vector3.Distance(frontRightActual.position, frontRightParented.position) > distance)
        {
            frontRightTargetPosition = frontRightParented.position;
            frontRight = StartCoroutine(MoveFrontRight());
        }
    }

    private bool heightReached = false;

    IEnumerator MoveFrontLeft()
    {
        while(Vector3.Distance(frontLeftTargetPosition, frontLeftActual.position) > .1f)
        {
            frontLeftActual.position = Vector3.MoveTowards(frontLeftActual.position, new Vector3(frontLeftTargetPosition.x, heightReached ? frontLeftTargetPosition.y : transform.position.y, frontLeftTargetPosition.z), Time.deltaTime * speed);

            heightReached = frontLeftActual.position.y >= transform.position.y;

            yield return new WaitForEndOfFrame();
        }
        heightReached = false;
        frontLeft = null;
    }

    IEnumerator MoveFrontRight()
    {
        while (Vector3.Distance(frontRightTargetPosition, frontRightActual.position) > .1f)
        {
            frontRightActual.position = Vector3.MoveTowards(frontRightActual.position, new Vector3(frontRightTargetPosition.x, heightReached ? frontRightTargetPosition.y : transform.position.y, frontRightTargetPosition.z), Time.deltaTime * speed);

            heightReached = frontRightActual.position.y >= transform.position.y;

            yield return new WaitForEndOfFrame();
        }
        heightReached = false;
        frontRight = null;
    }


}

