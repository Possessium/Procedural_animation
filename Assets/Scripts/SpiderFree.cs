using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFree : Spider
{
    [SerializeField] private List<SpiderLeg> allLegs = new List<SpiderLeg>();


    protected override void Update()
    {
        allLegs.ForEach(x => CheckHeight(x));

        allLegs.ForEach(x => CheckInteractbles(x));

        MoveLegs();

        base.Update();
    }

    private void MoveLegs()
    {
        for (int i = 0; i < allLegs.Count; i++)
        {
            if (IsBeyondDistance(allLegs[i]))
                allLegs[i].BufferLegPosition = allLegs[i].ParentedTransform.position;
            allLegs[i].MoveLeg(speed);
            if (Vector3.Distance(allLegs[i].TargetTransform.position, allLegs[i].BufferLegPosition) < lerpThreshold)
            {
                if (allLegs[i].Interactable != null)
                    allLegs[i].Interactable.Activate();
            }
            else if (allLegs[i].Interactable != null)
            {
                RemoveInteractable(allLegs[i].Interactable);
                allLegs[i].Interactable.Deactivate();
                allLegs[i].Interactable = null;
            }
        }
    }
}
