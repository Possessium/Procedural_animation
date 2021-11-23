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
    [SerializeField] private float speed = 15;
    #endregion

    #region Private variables
    private Vector3 bufferTargetFrontLeftPosition = Vector3.zero;
    private Vector3 bufferTargetFrontRightPosition = Vector3.zero;
    private Vector3 bufferTargetBackLeftPosition = Vector3.zero;
    private Vector3 bufferTargetBackRightPosition = Vector3.zero;
    private Coroutine moveFrontLeft = null;
    private Coroutine moveFrontRight = null;
    private bool heightReachedFront = false;
    private bool heightReachedBack = false;
    #endregion
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

        // If no coroutine is already playing and the distance between front left leg or back right leg is too big
        if (moveFrontLeft == null && moveFrontRight == null && (Vector3.Distance(parentedFrontLeftLeg.position, targetFrontLeftLeg.position) > distance || Vector3.Distance(parentedBackRightLeg.position, targetBackRightLeg.position) > distance))
        {
            // Sets the buffer position to have them fixed
            bufferTargetFrontLeftPosition = parentedFrontLeftLeg.position;
            bufferTargetBackRightPosition = parentedBackRightLeg.position;

            // Start the coroutine to move the legs
            moveFrontLeft = StartCoroutine(MoveFrontLeft());
        }

        // If no coroutine is already playing and the distance between front right leg or back left leg is too big
        if (moveFrontLeft == null && moveFrontRight == null && (Vector3.Distance(parentedFrontRightLeg.position, targetFrontRightLeg.position) > distance || Vector3.Distance(parentedBackLeftLeg.position, targetBackLeftLeg.position) > distance))
        {
            // Sets the buffer position to have them fixed
            bufferTargetFrontRightPosition = parentedFrontRightLeg.position;
            bufferTargetBackLeftPosition = parentedBackLeftLeg.position;

            // Start the coroutine to move the legs
            moveFrontRight = StartCoroutine(MoveFrontRight());
        }

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
    /// Coroutine that moves front left and back right legs to their target position
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveFrontLeft()
    {
        // Front letf and Back right legs are in sync, so the wile loops until both target position are correct
        while (targetFrontLeftLeg.position != bufferTargetFrontLeftPosition && targetBackRightLeg.position != bufferTargetBackRightPosition)
        {
            //// Front Left section

            // Get position with the correct height
            Vector3 _targetPosition = new Vector3(bufferTargetFrontLeftPosition.x, bufferTargetFrontLeftPosition.y + (heightReachedFront ? 0 : 2), bufferTargetFrontLeftPosition.z);

            // MoveTowards the leg to the target
            targetFrontLeftLeg.position = Vector3.MoveTowards(targetFrontLeftLeg.position, _targetPosition, Time.deltaTime * speed);

            // If the height hasn't been reached yet, try to know if it as this time
            if (!heightReachedFront)
                heightReachedFront = targetFrontLeftLeg.position.y >= bufferTargetFrontLeftPosition.y + 1;
            ////


            //// Back Right section
            _targetPosition = new Vector3(bufferTargetBackRightPosition.x, bufferTargetBackRightPosition.y + (heightReachedFront ? 0 : 2), bufferTargetBackRightPosition.z);
            targetBackRightLeg.position = Vector3.MoveTowards(targetBackRightLeg.position, _targetPosition, Time.deltaTime * speed);

            if (!heightReachedBack)
                heightReachedBack = targetBackRightLeg.position.y >= bufferTargetBackRightPosition.y + 1;
            ////

            yield return new WaitForEndOfFrame();
        }
        heightReachedFront = false;
        heightReachedBack = false;
        moveFrontLeft = null;
    }


    /// <summary>
    /// Coroutine that moves front right and back left legs to their target position
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveFrontRight()
    {
        // Front right and Back left legs are in sync, so the wile loops until both target position are correct
        while (targetFrontRightLeg.position != bufferTargetFrontRightPosition && targetBackLeftLeg.position != bufferTargetBackLeftPosition)
        {
            //// Front Right section
            
            // Get position with the correct height
            Vector3 _targetPosition = new Vector3(bufferTargetFrontRightPosition.x, bufferTargetFrontRightPosition.y + (heightReachedFront ? 0 : 2), bufferTargetFrontRightPosition.z);

            // MoveTowards the leg to the target
            targetFrontRightLeg.position = Vector3.MoveTowards(targetFrontRightLeg.position, _targetPosition, Time.deltaTime * speed);

            // If the height hasn't been reached yet, try to know if it as this time
            if (!heightReachedFront)
                heightReachedFront = targetFrontRightLeg.position.y >= bufferTargetFrontRightPosition.y + 1;
            ////
            

            //// Back left section
            _targetPosition = new Vector3(bufferTargetBackLeftPosition.x, bufferTargetBackLeftPosition.y + (heightReachedFront ? 0 : 2), bufferTargetBackLeftPosition.z);
            targetBackLeftLeg.position = Vector3.MoveTowards(targetBackLeftLeg.position, _targetPosition, Time.deltaTime * speed);

            if (!heightReachedBack)
                heightReachedBack = targetBackLeftLeg.position.y >= bufferTargetBackLeftPosition.y + 1;
            ////
            
            yield return new WaitForEndOfFrame();
        }

        // Resets bool and Coroutine values
        heightReachedFront = false;
        heightReachedBack = false;
        moveFrontRight = null;
    }
    #endregion

    #region My methods
    /// <summary>
    /// Check the height of the parented target points to place them correctly depending on the ground below
    /// </summary>
    private void CheckHeight()
    {
        RaycastHit[] _hits = Physics.RaycastAll(new Vector3(parentedFrontLeftLeg.position.x, transform.position.y, parentedFrontLeftLeg.position.z), Vector3.down, 20);

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
