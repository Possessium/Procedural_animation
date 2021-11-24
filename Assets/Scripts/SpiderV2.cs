using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderV2 : MonoBehaviour
{
    #region Variables
    #region Legs
    [SerializeField] private Transform parentedFrontLeftLeg = null;
    [SerializeField] private Transform targetFrontLeftLeg = null;
    [SerializeField] private Transform parentedFrontRightLeg = null;
    [SerializeField] private Transform targetFrontRightLeg = null;
    [SerializeField] private Transform parentedBackLeftLeg = null;
    [SerializeField] private Transform targetBackLeftLeg = null;
    [SerializeField] private Transform parentedBackRightLeg = null;
    [SerializeField] private Transform targetBackRightLeg = null;
    #endregion

    #region Parameters
    [SerializeField] private float distance = 5;
    [SerializeField, Range(.01f, 1)] private float lerpThreshold = .1f;
    [SerializeField] private float speed = 15;
    #endregion

    #region Private variables
    private Vector3 bufferTargetFrontLeftPosition = Vector3.zero;
    private Vector3 bufferTargetFrontRightPosition = Vector3.zero;
    private Vector3 bufferTargetBackLeftPosition = Vector3.zero;
    private Vector3 bufferTargetBackRightPosition = Vector3.zero;
    private bool heightReachedFront = false;
    private bool heightReachedBack = false;
    private bool isLegMoving = false;
    #endregion
    private event System.Action OnMoveLeg = null;
    public bool IsReady { get { return parentedFrontLeftLeg && parentedFrontRightLeg && parentedBackLeftLeg && parentedBackRightLeg && targetFrontLeftLeg && targetFrontRightLeg && targetBackLeftLeg && targetBackRightLeg; } }
    #endregion

    #region Unity
    private void Start()
    {
        if (!IsReady)
            return;

        // Set correct heights of all the legs parented target position
        CheckHeight();

        // Set the legs to their initial positions
        targetFrontLeftLeg.position = parentedFrontLeftLeg.position;
        targetFrontRightLeg.position = parentedFrontRightLeg.position;
        targetBackLeftLeg.position = parentedBackLeftLeg.position;
        targetBackRightLeg.position = parentedBackRightLeg.position;
    }

    private void Update()
    {
        if (!IsReady)
            return;

        // Set correct heights of all the legs parented target position
        CheckHeight();

        // If no are moving and the distance between front left leg or back right leg is too big
        if (!isLegMoving && (Vector3.Distance(parentedFrontLeftLeg.position, targetFrontLeftLeg.position) > distance || Vector3.Distance(parentedBackRightLeg.position, targetBackRightLeg.position) > distance))
        {
            // Sets the buffer position to have them fixed
            bufferTargetFrontLeftPosition = parentedFrontLeftLeg.position;
            bufferTargetBackRightPosition = parentedBackRightLeg.position;

            // Subscribe the event to the correct method to move the legs and sets the bool
            isLegMoving = true;
            OnMoveLeg += MoveFirstSet;
        }

        // If no legs are moving and the distance between front right leg or back left leg is too big
        if (!isLegMoving && (Vector3.Distance(parentedFrontRightLeg.position, targetFrontRightLeg.position) > distance || Vector3.Distance(parentedBackLeftLeg.position, targetBackLeftLeg.position) > distance))
        {
            // Sets the buffer position to have them fixed
            bufferTargetFrontRightPosition = parentedFrontRightLeg.position;
            bufferTargetBackLeftPosition = parentedBackLeftLeg.position;

            // Subscribe the event to the correct method to move the legs and sets the bool
            isLegMoving = true;
            OnMoveLeg += MoveSecondSet;
        }

        OnMoveLeg?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (!IsReady)
            return;

        // Draw each parented position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(parentedFrontLeftLeg.position, .2f);
        Gizmos.DrawSphere(parentedFrontRightLeg.position, .2f);
        Gizmos.DrawSphere(parentedBackLeftLeg.position, .2f);
        Gizmos.DrawSphere(parentedBackRightLeg.position, .2f);

        // Draw each target position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetFrontLeftLeg.position, .2f);
        Gizmos.DrawSphere(targetFrontRightLeg.position, .2f);
        Gizmos.DrawSphere(targetBackLeftLeg.position, .2f);
        Gizmos.DrawSphere(targetBackRightLeg.position, .2f);

        // Draw each distance from the target
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetFrontLeftLeg.position, distance);
        Gizmos.DrawWireSphere(targetFrontRightLeg.position, distance);
        Gizmos.DrawWireSphere(targetBackLeftLeg.position, distance);
        Gizmos.DrawWireSphere(targetBackRightLeg.position, distance);
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Moves front left and back right legs to their target position
    /// </summary>
    /// <returns></returns>
    private void MoveFirstSet()
    {
        // Front left and Back right legs are in sync, loops until both target position are correct
        if (Vector3.Distance(targetFrontLeftLeg.position, bufferTargetFrontLeftPosition) > lerpThreshold || Vector3.Distance(targetBackRightLeg.position, bufferTargetBackRightPosition) > lerpThreshold)
        {
            //// Front Left section

            // Get position with the correct height
            Vector3 _targetPosition = new Vector3(bufferTargetFrontLeftPosition.x, bufferTargetFrontLeftPosition.y + (heightReachedFront ? 0 : 2), bufferTargetFrontLeftPosition.z);

            // Lerp the leg to the target
            targetFrontLeftLeg.position = Vector3.Lerp(targetFrontLeftLeg.position, _targetPosition, Time.deltaTime * speed);

            // If the height hasn't been reached yet, try to know if it as this time
            if (!heightReachedFront)
                heightReachedFront = targetFrontLeftLeg.position.y >= bufferTargetFrontLeftPosition.y + 1;
            ////


            //// Back Right section
            _targetPosition = new Vector3(bufferTargetBackRightPosition.x, bufferTargetBackRightPosition.y + (heightReachedFront ? 0 : 2), bufferTargetBackRightPosition.z);
            targetBackRightLeg.position = Vector3.Lerp(targetBackRightLeg.position, _targetPosition, Time.deltaTime * speed);

            if (!heightReachedBack)
                heightReachedBack = targetBackRightLeg.position.y >= bufferTargetBackRightPosition.y + 1;
            ////
        }

        // Resets bool and event when both target position are correct
        else
        {
            heightReachedFront = false;
            heightReachedBack = false;
            isLegMoving = false;
            OnMoveLeg = null;
        }
    }


    /// <summary>
    /// Moves front right and back left legs to their target position
    /// </summary>
    /// <returns></returns>
    private void MoveSecondSet()
    {
        // Front right and Back left legs are in sync, loops until both target position are correct
        if (Vector3.Distance(targetFrontRightLeg.position, bufferTargetFrontRightPosition) > lerpThreshold || Vector3.Distance(targetBackLeftLeg.position, bufferTargetBackLeftPosition) > lerpThreshold)
        {
            //// Front Right section
            
            // Get position with the correct height
            Vector3 _targetPosition = new Vector3(bufferTargetFrontRightPosition.x, bufferTargetFrontRightPosition.y + (heightReachedFront ? 0 : 2), bufferTargetFrontRightPosition.z);

            // Lerp the leg to the target
            targetFrontRightLeg.position = Vector3.Lerp(targetFrontRightLeg.position, _targetPosition, Time.deltaTime * speed);

            // If the height hasn't been reached yet, try to know if it as this time
            if (!heightReachedFront)
                heightReachedFront = targetFrontRightLeg.position.y >= bufferTargetFrontRightPosition.y + 1;
            ////
            

            //// Back left section
            _targetPosition = new Vector3(bufferTargetBackLeftPosition.x, bufferTargetBackLeftPosition.y + (heightReachedFront ? 0 : 2), bufferTargetBackLeftPosition.z);
            targetBackLeftLeg.position = Vector3.Lerp(targetBackLeftLeg.position, _targetPosition, Time.deltaTime * speed);

            if (!heightReachedBack)
                heightReachedBack = targetBackLeftLeg.position.y >= bufferTargetBackLeftPosition.y + 1;
            ////
        }

        // Resets bool and event when both target position are correct
        else
        {
            heightReachedFront = false;
            heightReachedBack = false;
            isLegMoving = false;
            OnMoveLeg = null;
        }
    }
    #endregion

    #region My methods
    /// <summary>
    /// Check the height of the parented target points to place them correctly depending on the ground below
    /// </summary>
    private void CheckHeight()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(parentedFrontLeftLeg.position.x, transform.position.y, parentedFrontLeftLeg.position.z), Vector3.down, 20);

        // Get the first object found and set the position of the parent to the hit point
        foreach (RaycastHit _hit in _hits)
        {
            // Safety to not detect itself
            if (_hit.transform != transform)
            {
                parentedFrontLeftLeg.transform.position = _hit.point;
                break;
            }
        }
        _hits = Physics.RaycastAll(new Vector3(parentedFrontRightLeg.position.x, transform.position.y, parentedFrontRightLeg.position.z), Vector3.down, 20);

        foreach (RaycastHit _hit in _hits)
        {
            if (_hit.transform != transform)
            {
                parentedFrontRightLeg.transform.position = _hit.point;
                break;
            }
        }
        _hits = Physics.RaycastAll(new Vector3(parentedBackLeftLeg.position.x, transform.position.y, parentedBackLeftLeg.position.z), Vector3.down, 20);

        foreach (RaycastHit _hit in _hits)
        {
            if (_hit.transform != transform)
            {
                parentedBackLeftLeg.transform.position = _hit.point;
                break;
            }
        }
        _hits = Physics.RaycastAll(new Vector3(parentedBackRightLeg.position.x, transform.position.y, parentedBackRightLeg.position.z), Vector3.down, 20);

        foreach (RaycastHit _hit in _hits)
        {
            if (_hit.transform != transform)
            {
                parentedBackRightLeg.transform.position = _hit.point;
                break;
            }
        }
    }
    #endregion

}
