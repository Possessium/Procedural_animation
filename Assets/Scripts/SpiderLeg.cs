using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    public Transform ParentedTransform = null;
    public Transform TargetTransform = null;
    [HideInInspector] public Vector3 BufferLegPosition = Vector3.zero;

    private bool heightReached = false;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(ParentedTransform.position, .2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(TargetTransform.position, .2f);
    }

    /// <summary>
    /// Moves the target position to the buffer position
    /// </summary>
    /// <param name="_speed">The speed used in the Lerp method</param>
    public void MoveLeg(float _speed)
    {
        // Lerp the leg to the target
        TargetTransform.position = Vector3.Lerp(TargetTransform.position, new Vector3(BufferLegPosition.x, BufferLegPosition.y + (heightReached ? 0 : 2), BufferLegPosition.z), Time.deltaTime * _speed);

        // If the height hasn't been reached yet, try to know if it as this time
        if (!heightReached)
            heightReached = TargetTransform.position.y >= BufferLegPosition.y + 1;
    }

    /// <summary>
    /// Instantly set the leg to the given position
    /// </summary>
    /// <param name="_position">The position the target will be set</param>
    public void SetLeg(Vector3 _position)
    {
        TargetTransform.position = _position;
    }

    /// <summary>
    /// Resets the heightReached bool
    /// </summary>
    public void ResetHeight()
    {
        heightReached = false;
    }
}
