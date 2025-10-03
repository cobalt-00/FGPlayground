using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputProcessor inputProcesser;
    void Start()
    {
        Init();
    }

    //ref: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.Users.InputUser.html
    private void Init(/*eventually want to initalize this per specific device with an input user*/)
    {
        var input = GetComponent<PlayerInput>();
        InputRebindUtil.SaveInputs();
        inputProcesser = GetComponent<InputProcessor>();
    }

    public void OnMove(InputValue input)
    {
        inputProcesser.ProcessMove(input);
    }

    public void OnPunch(InputValue input)
    {
        inputProcesser.ProcessInput("Punch", input);
    }
    public void OnKick(InputValue input)
    {
        inputProcesser.ProcessInput("Kick", input);
    }
    public void OnWeapon(InputValue input)
    {
        inputProcesser.ProcessInput("Weapon", input);
    }
    public void OnBreak(InputValue input)
    {
        inputProcesser.ProcessInput("Break", input);
    }

}

