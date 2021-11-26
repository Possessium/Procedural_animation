using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFree : Spider
{
    [SerializeField] private List<SpiderLeg> allLegs = new List<SpiderLeg>();

    [SerializeField] private List<SpiderLeg> movingLegs = new List<SpiderLeg>();


    protected override void Update()
    {
        allLegs.ForEach(x => CheckHeight(x));

        for (int i = 0; i < allLegs.Count; i++)
        {
            if (!movingLegs.Contains(allLegs[i]))
            {
                if (IsBeyondDistance(allLegs[i]))
                {
                    allLegs[i].BufferLegPosition = allLegs[i].ParentedTransform.position;
                    movingLegs.Add(allLegs[i]);
                }
            }
        }

        MoveLegs();

        base.Update();
    }

    private void MoveLegs()
    {
        List<SpiderLeg> _bufferRemove = new List<SpiderLeg>();
        for (int i = 0; i < movingLegs.Count; i++)
        {
            movingLegs[i].MoveLeg(speed);
            if(Vector3.Distance(movingLegs[i].TargetTransform.position, movingLegs[i].BufferLegPosition) < lerpThreshold)
                _bufferRemove.Add(movingLegs[i]);
        }
        for (int i = 0; i < _bufferRemove.Count; i++)
        {
            movingLegs.Remove(_bufferRemove[i]);
        }
    }
}
