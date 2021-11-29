using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour, ISpiderInteractable
{
    public Renderer Rend = null;
    public Material ActiveMat = null;
    public Material DisableMat = null;

    public void Activate()
    {
        Rend.material = ActiveMat;
    }

    public void Deactivate()
    {
        Rend.material = DisableMat;
    }
}
