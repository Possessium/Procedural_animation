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
            {
                if (allLegs[i].Interactable != null)
                {
                    allLegs[i].Interactable.Deactivate();
                    RemoveInteractable(allLegs[i].Interactable);
                    allLegs[i].Interactable = null;
                }
                allLegs[i].BufferLegPosition = allLegs[i].ParentedTransform.position;
            }

            allLegs[i].MoveLeg(speed);
        }
    }
}
