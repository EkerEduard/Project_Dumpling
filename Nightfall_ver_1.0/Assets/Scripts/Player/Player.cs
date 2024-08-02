using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControlls controlls {  get; private set; }
    public Player_Aim aim {  get; private set; }
    public Player_Movement movement { get; private set; }
    public Player_WeaponController weapon { get; private set; }

    private void Awake()
    {
        controlls = new PlayerControlls();
        aim = GetComponent<Player_Aim>();
        movement = GetComponent<Player_Movement>();
        weapon = GetComponent<Player_WeaponController>();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }
}
