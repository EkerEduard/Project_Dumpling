using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationsEvents : MonoBehaviour
{
    private Player_WeaponVisuals visualController;

    private void Start()
    {
        visualController = GetComponentInParent<Player_WeaponVisuals>();
    }

    public void ReloadIsOver()
    {
        visualController.MaximazeRigWeight();
    }

    public void ReturnRig()
    {
        visualController.MaximazeRigWeight();
        visualController.MaximazeLeftHandWeight();
    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }
}
