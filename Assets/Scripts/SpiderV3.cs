using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderV3 : MonoBehaviour
{
    [SerializeField] List<SpiderLeg> firstSet = new List<SpiderLeg>();
    [SerializeField] List<SpiderLeg> secondSet = new List<SpiderLeg>();

    [SerializeField] private float speed = 15;
    [SerializeField] private float distance = 2;
    [SerializeField, Range(.01f, 1)] private float lerpThreshold = .1f;

    [SerializeField] private SpiderLeg frontLeftLeg = null;
    [SerializeField] private SpiderLeg frontRightLeg = null;
    [SerializeField] private SpiderLeg backLeftLeg = null;
    [SerializeField] private SpiderLeg backRightLeg = null;
    [SerializeField] private Transform parentedTransformParent = null;

    private bool isFirstStepMoving = false;
    private bool isSecondStepMoving = false;

    private void Start()
    {
        // Set correct heights of all the legs parented target position
        firstSet.ForEach(x => CheckHeight(x));
        secondSet.ForEach(x => CheckHeight(x));

        // Set the legs to their initial positions
        firstSet.ForEach(x => x.SetLeg(x.ParentedTransform.position));
        secondSet.ForEach(x => x.SetLeg(x.ParentedTransform.position));
    }
    private void Update()
    {
        // Set correct heights of all the legs parented target position
        firstSet.ForEach(x => CheckHeight(x));
        secondSet.ForEach(x => CheckHeight(x));

        // Check to see if any leg is too far for any set
        if (!isFirstStepMoving && !isSecondStepMoving)
        {
            CheckDistance(firstSet);
            CheckDistance(secondSet);
        }

        if (isFirstStepMoving)
        {
            // Moves the leg
            firstSet.ForEach(x => x.MoveLeg(speed));

            // Seek if all the legs have arrives to their target position
            if (firstSet.TrueForAll(x => Vector3.Distance(x.TargetTransform.position, x.BufferLegPosition) < lerpThreshold))
            {
                // Resets bool and height bool
                isFirstStepMoving = false;
                firstSet.ForEach(x => x.ResetHeight());
            }
        }

        if(isSecondStepMoving)
        {
            // Moves the leg
            secondSet.ForEach(x => x.MoveLeg(speed));

            // Seek if all the legs have arrives to their target position
            if (secondSet.TrueForAll(x => Vector3.Distance(x.TargetTransform.position, x.BufferLegPosition) < lerpThreshold))
            {
                // Resets bool and height bool
                isSecondStepMoving = false;
                secondSet.ForEach(x => x.ResetHeight());
            }
        }

        // Sets the position of the parentedtransform because they can't be parented due to rotation of the body
        parentedTransformParent.position = transform.position;

        CheckAngle();
    }


    /// <summary>
    /// Check the height of the parented target point to place it correctly depending on the ground below
    /// </summary>
    private void CheckHeight(SpiderLeg _l)
    {
        // Raycast from the leg position and transform height towards down
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(_l.ParentedTransform.position.x, transform.position.y, _l.ParentedTransform.position.z), Vector3.down, 20);

        if(_hits.Length > 0)
        {
            Vector3 _position = Vector3.zero;
            float _height = -99;

            // Find the higher point found that is not the spider itself
            foreach (RaycastHit _hit in _hits)
            {
                if(_hit.point.y > _height && _hit.transform != transform)
                {
                    _position = _hit.point;
                    _height = _hit.point.y;
                }
            }
            // If found any changes the height of the ParentedTransform of the leg
            if(_height > -99)
                _l.ParentedTransform.position = _position;
        }
    }

    /// <summary>
    /// Check the distance between the parented point and the target point of each legs in the given list
    /// </summary>
    /// <param name="_l">The list of SpiderLeg checked by the method</param>
    private void CheckDistance(List<SpiderLeg> _l)
    {
        for (int i = 0; i < _l.Count; i++)
        {
            // If the distance between the parented position and the current position is above the distance
            if(Vector3.Distance(_l[i].ParentedTransform.position, _l[i].TargetTransform.position) > distance)
            {
                // Set correctly both step moving bool
                isFirstStepMoving = firstSet.Contains(_l[i]);
                isSecondStepMoving = !isFirstStepMoving;

                // Set each leg buffer position of the correct set to the parented position
                if (isFirstStepMoving)
                    firstSet.ForEach(x => x.BufferLegPosition = x.ParentedTransform.position);

                if (isSecondStepMoving)
                    secondSet.ForEach(x => x.BufferLegPosition = x.ParentedTransform.position);

                break;
            }
        }
    }

    /// <summary>
    /// Moves body rotation based on the position of the legs in the four corners
    /// </summary>
    private void CheckAngle()
    {
        Vector3 _bufferAngle = Vector3.zero;

        // Add value depending on the Y position of each leg and it neighbors
        _bufferAngle.x += frontLeftLeg.BufferLegPosition.y - frontRightLeg.BufferLegPosition.y;
        _bufferAngle.x += backLeftLeg.BufferLegPosition.y - backRightLeg.BufferLegPosition.y;
        _bufferAngle.z -= frontLeftLeg.BufferLegPosition.y - backLeftLeg.BufferLegPosition.y;
        _bufferAngle.z -= frontRightLeg.BufferLegPosition.y - backRightLeg.BufferLegPosition.y;

        // Move the rotation value to the corrected one
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_bufferAngle * 5), Time.deltaTime * (speed / 2));
    }
}
