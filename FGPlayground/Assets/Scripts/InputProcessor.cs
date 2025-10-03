using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Windows;

public struct Frame
{
    public Frame(Dictionary<string, int> inputs, Vector2 stickPosition)
    { 
        this.inputs = inputs;
        this.stickPosition = stickPosition;
    }
    public Dictionary<string, int> inputs;
    public Vector2 stickPosition;
}

public class InputProcessor : MonoBehaviour
{
    private const int INPUT_STORE_BUFFER_LENGTH = 20;

    private InputUser user; //serves as our unique id
    private Dictionary<string, int> CurrentInputs = new Dictionary<string, int>();
    private Vector2 stickPosition;

    private List<Frame> frames = new List<Frame>();

    private void Start()
    {
        frames = new List<Frame>();
        CurrentInputs = new Dictionary<string, int>();
        stickPosition = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (frames.Count == INPUT_STORE_BUFFER_LENGTH)
        {
            //hopefully this isnt a ridiculous performance hit; might want do something funkier with overwriting f0 and reindexing to f20
            frames.RemoveAt(0);
        }
        //store most recent frame in our buffer
        frames.Add(new Frame(CurrentInputs, stickPosition));

        //then increment the number of frames each input has been held for (important for tracking charge inputs)
        foreach (var input in CurrentInputs.Keys.ToList())
        {
            CurrentInputs[input]+= 1;
        }

        CheckMotions();
    }

    private void CheckMotions()
    {
        //TODO: this is going to be complicated
    }

    public InputProcessor(InputUser user)
    {
        this.user = user;
    }
    public void ProcessMove(InputValue input)
    {
        stickPosition = (Vector2)input.Get();
        Debug.Log("Move" + input.Get());
    }
    public void ProcessInput(string InputName, InputValue input)
    {
        if (input.Get() != null)
        {
            //this means we pressed
            CurrentInputs.Add(InputName, 0);
            Debug.Log("Pressed " +  InputName);
        }
        else
        {
            //otherwise, we released
            CurrentInputs.Remove(InputName);
            Debug.Log("Released " + InputName);
        }
    }
}
