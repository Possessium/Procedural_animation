using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{

    [SerializeField] private protected float speed = 15;
    [SerializeField] private float distance = 2;
    [SerializeField, Range(.01f, 1)] private protected float lerpThreshold = .1f;
    [SerializeField] private Transform parentedTransformParent = null;


    /*
     * Method to detect interactables               TO REDACT
     * Method to check Height                       DONE
     * Method to check Angle                        NO IDEA HOW TO SCALE IT
     * Method to check Distance ? (list & single)   DONE
     * Method to Place bones properly               ABORTED FOR THE MOMENT
     */

    protected virtual void Update()
    {
        // Sets the position of the parentedtransform because they can't be parented due to rotation of the body
        parentedTransformParent.position = transform.position;
        // CheckAngle();
    }

    /// <summary>
    /// Check the height of the parented target point to place it correctly depending on the ground below
    /// </summary>
    private protected void CheckHeight(SpiderLeg _l)
    {
        // Raycast from the leg position and transform height towards down
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(_l.ParentedTransform.position.x, transform.position.y, _l.ParentedTransform.position.z), Vector3.down, 20);

        if (_hits.Length > 0)
        {
            Vector3 _position = Vector3.zero;
            float _height = -99;

            // Find the higher point found that is not the spider itself
            foreach (RaycastHit _hit in _hits)
            {
                if (_hit.point.y > _height && _hit.transform != transform)
                {
                    _position = _hit.point;
                    _height = _hit.point.y;
                }
            }
            // If found any changes the height of the ParentedTransform of the leg
            if (_height > -99)
                _l.ParentedTransform.position = _position;
        }
    }

    /// <summary>
    /// Check the distance between the parented point and the target point of each legs in the given list
    /// </summary>
    /// <param name="_l">The list of SpiderLeg checked by the method</param>
    private protected bool IsBeyondDistance(SpiderLeg _l)
    {
        // Return true if the distance between the parented position and the current position is above the distance
        return Vector3.Distance(_l.ParentedTransform.position, _l.TargetTransform.position) > distance;
    }

    /// <summary>
    /// Moves body rotation based on the position of the legs in the four corners
    /// </summary>
    //private void CheckAngle()
    //{
    //    Vector3 _bufferAngle = Vector3.zero;

    //    // Add value depending on the Y position of each leg and it neighbors
    //    _bufferAngle.x += frontLeftLeg.BufferLegPosition.y - frontRightLeg.BufferLegPosition.y;
    //    _bufferAngle.x += backLeftLeg.BufferLegPosition.y - backRightLeg.BufferLegPosition.y;
    //    _bufferAngle.z -= frontLeftLeg.BufferLegPosition.y - backLeftLeg.BufferLegPosition.y;
    //    _bufferAngle.z -= frontRightLeg.BufferLegPosition.y - backRightLeg.BufferLegPosition.y;

    //    // Move the rotation value to the corrected one
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_bufferAngle * 5), Time.deltaTime * (speed / 2));
    //}

    private protected void CheckInteractbles(SpiderLeg _l)
    {
        // Spherecast du bone start sur la longueur de la patte
        // Si ISpiderInteracble --> poggers
    }

    //private protected void CheckBones(SpiderLeg _l)
    //{
        // à voir quand j'aurais plus d'xp sur l'IK
    //}
}
