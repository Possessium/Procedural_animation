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
        firstSet.ForEach(x => CheckInteractbles(x));
        secondSet.ForEach(x => CheckInteractbles(x));

        // Check to see if any leg is too far for any set
        if (!isFirstStepMoving && !isSecondStepMoving)
        {
            for (int i = 0; i < firstSet.Count; i++)
            {
                if (IsBeyondDistance(firstSet[i]))
                {
                    isFirstStepMoving = true;
                    SpiderLeg _bufferLeg = null;
                    for (int ii = 0; ii < firstSet.Count; ii++)
                    {
                        _bufferLeg = firstSet[ii];
                        _bufferLeg.BufferLegPosition = _bufferLeg.ParentedTransform.position;
                        if(_bufferLeg.Interactable != null)
                        {
                            RemoveInteractable(_bufferLeg.Interactable);
                            _bufferLeg.Interactable.Deactivate();
                            _bufferLeg.Interactable = null;
                        }
                    }
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
                        SpiderLeg _bufferLeg = null;
                        for (int ii = 0; ii < secondSet.Count; ii++)
                        {
                            _bufferLeg = secondSet[ii];
                            _bufferLeg.BufferLegPosition = _bufferLeg.ParentedTransform.position;
                            if (_bufferLeg.Interactable != null)
                            {
                                RemoveInteractable(_bufferLeg.Interactable);
                                _bufferLeg.Interactable.Deactivate();
                                _bufferLeg.Interactable = null;
                            }
                        }
                        break;
                    }
                }
            }
        }

        if (isFirstStepMoving)
        {
            // Moves the leg
            firstSet.ForEach(x => x.MoveLeg(speed));

            SpiderLeg _bufferLeg = null;
            int _trues = 0;
            for (int i = 0; i < firstSet.Count; i++)
            {
                _bufferLeg = firstSet[i];
                if(Vector3.Distance(_bufferLeg.TargetTransform.position, _bufferLeg.BufferLegPosition) < lerpThreshold)
                {
                    _trues++;
                    if (_bufferLeg.Interactable != null)
                    {
                        _bufferLeg.Interactable.Activate();
                    }
                }
            }
            if(_trues == firstSet.Count)
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

            SpiderLeg _bufferLeg = null;
            int _trues = 0;
            for (int i = 0; i < secondSet.Count; i++)
            {
                _bufferLeg = secondSet[i];
                if (Vector3.Distance(_bufferLeg.TargetTransform.position, _bufferLeg.BufferLegPosition) < lerpThreshold)
                {
                    _trues++;
                    if (_bufferLeg.Interactable != null)
                    {
                        _bufferLeg.Interactable.Activate();
                    }
                }

            }
            if (_trues == secondSet.Count)
            {
                // Resets bool and height bool
                isSecondStepMoving = false;
                secondSet.ForEach(x => x.ResetHeight());
            }
        }

        base.Update();
    }

}
