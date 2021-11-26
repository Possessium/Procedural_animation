using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSets : Spider
{
    [SerializeField] List<SpiderLeg> firstSet = new List<SpiderLeg>();
    [SerializeField] List<SpiderLeg> secondSet = new List<SpiderLeg>();

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

    protected override void Update()
    {
        // Set correct heights of all the legs parented target position
        firstSet.ForEach(x => CheckHeight(x));
        secondSet.ForEach(x => CheckHeight(x));

        // Check to see if any leg is too far for any set
        if (!isFirstStepMoving && !isSecondStepMoving)
        {
            for (int i = 0; i < firstSet.Count; i++)
            {
                if (IsBeyondDistance(firstSet[i]))
                {
                    isFirstStepMoving = true;
                    firstSet.ForEach(x => x.BufferLegPosition = x.ParentedTransform.position);
                    break;
                }
            }
            if(!isFirstStepMoving)
            {
                for (int i = 0; i < secondSet.Count; i++)
                {
                    if (IsBeyondDistance(secondSet[i]))
                    {
                        isSecondStepMoving = true;
                        secondSet.ForEach(x => x.BufferLegPosition = x.ParentedTransform.position);
                        break;
                    }
                }
            }
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

        if (isSecondStepMoving)
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

        base.Update();
    }

}
