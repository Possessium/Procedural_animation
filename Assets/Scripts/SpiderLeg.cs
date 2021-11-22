using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    [SerializeField] private Transform frontLeftCurrent = null;
    [SerializeField] private Transform frontLeftTarget = null;
    [SerializeField] private Transform frontRightCurrent = null;
    [SerializeField] private Transform frontRightTarget = null;
    [SerializeField] private float distance = 5;
    [SerializeField] private float speed = 5;

    private bool moveFrontLeft = false;
    private bool moveFrontRight = false;

    Coroutine frontLeft = null;
    Coroutine frontRight = null;

    /*
     * 
     * Changer en faisant en sorte que quand la distance est trop grande, la position du target est stockée et c'est là où le leg doit aller.
     * Virer le lerp aussi
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
        Gizmos.DrawSphere(frontLeftTarget.position, .25f);
        Gizmos.DrawSphere(frontRightTarget.position, .25f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(frontLeftCurrent.position, .25f);
        Gizmos.DrawSphere(frontRightCurrent.position, .25f);
    }

    private void FrontLeft()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(frontLeftTarget.transform.position.x, transform.position.y, frontLeftTarget.transform.position.z), Vector3.down, 50);
        if (_hits.Length > 0)
        {
            foreach (RaycastHit _hit in _hits)
            {
                if (_hit.transform != transform)
                {
                    frontLeftTarget.transform.position = _hit.point;
                    break;
                }
            }
        }

        if (frontRight == null && frontLeft == null && Vector3.Distance(frontLeftTarget.position, frontLeftCurrent.position) > distance)
        {
            frontLeft = StartCoroutine(MoveFrontLeft());
        }
    }

    private void FrontRight()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(frontRightTarget.transform.position.x, transform.position.y, frontRightTarget.transform.position.z), Vector3.down, 50);
        if (_hits.Length > 0)
        {
            foreach (RaycastHit _hit in _hits)
            {
                if (_hit.transform != transform)
                {
                    frontRightTarget.transform.position = _hit.point;
                    break;
                }
            }
        }

        if (frontRight == null && frontLeft == null && Vector3.Distance(frontRightTarget.position, frontRightCurrent.position) > distance)
        {
            frontRight = StartCoroutine(MoveFrontRight());
        }
    }

    private bool heightReached = false;

    IEnumerator MoveFrontLeft()
    {
        while(Vector3.Distance(frontLeftCurrent.position, frontLeftTarget.position) > .1f)
        {
            frontLeftTarget.position = Vector3.Lerp(frontLeftTarget.position, new Vector3(frontLeftCurrent.position.x, heightReached ? transform.position.y : frontLeftCurrent.position.y, frontLeftCurrent.position.z), Time.deltaTime * speed);
            Debug.Log(heightReached);
            heightReached = frontLeftTarget.position.y >= transform.position.y;

            yield return new WaitForEndOfFrame();
        }
        heightReached = false;
        frontLeft = null;
    }

    IEnumerator MoveFrontRight()
    {
        while (Vector3.Distance(frontRightCurrent.position, frontRightTarget.position) > .1f)
        {
            frontRightTarget.position = Vector3.Lerp(frontRightTarget.position, new Vector3(frontRightCurrent.position.x, heightReached ? transform.position.y : frontRightCurrent.position.y, frontRightCurrent.position.z), Time.deltaTime * speed);

            heightReached = frontRightTarget.position.y >= transform.position.y;

            yield return new WaitForEndOfFrame();
        }
        heightReached = false;
        frontRight = null;
    }


}

