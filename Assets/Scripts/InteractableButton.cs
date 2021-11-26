using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour, ISpiderInteractable
{
    public void Activate()
    {
        Debug.Log("Active");
    }

    public void Deactivate()
    {
        Debug.Log("Disabled");
    }
}
