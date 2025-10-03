using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private const int INPUT_STORE_BUFFER_LENGTH = 80;

    private InputUser user; //serves as our unique id
    private Dictionary<string, int> CurrentInputs = new Dictionary<string, int>();
    private Vector2 stickPosition;

    private List<Frame> frames = new List<Frame>();


    public InputDefinitions InputDefinitions;
    public bool Mirror = false;

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
        foreach (var definition in InputDefinitions.inputs)
        {
            if (CheckMotion(definition))
            {
                //call the motion
                Debug.Log("#### Motion achieved!~ " + definition.name);
                break;
            }
        }
    }

    private bool CheckMotion(InputDefinition inputDefinition)
    {
        //first, we see how long we have to check for this input 
        var total = inputDefinition.steps.Sum(_ => _.nextStepMaximumFrames);
        //and gather that many frames from the end of the buffer
        var searchableFrames = frames.TakeLast(total).ToList();

        if (searchableFrames.Any(frame => frame.inputs.TryGetValue(inputDefinition.button, out var duration) && duration == 1))
        { 
            //we have the button, now check for the motion
            foreach (var step in inputDefinition.steps)
            {
                if (searchableFrames.Count() == 0)
                {
                    //this means we ran out of frames to search and still have steps remaining
                    return false;
                }

                //remove from the front until we run out of non-matching frames
                var nonMatching = searchableFrames.TakeWhile(frame => Vec2ToNumpad(frame.stickPosition) != step.input);
                var recursiveSearchable = searchableFrames.TakeLast(searchableFrames.Count() - nonMatching.Count()).ToList();
                searchableFrames = recursiveSearchable;
            }

            //if we reach this before running out of frames, we've made the input
            return true;
        }

        return false;


    }

    private string Vec2ToNumpad(Vector2 input)
    {
        var mirrorMult = Mirror ? -1 : 1;

        if (input.x * mirrorMult == -1 && input.y == -1) return "1";
        if (input.x * mirrorMult == 0 * mirrorMult && input.y == -1) return "2";
        if (input.x * mirrorMult == 1 && input.y == -1) return "3";
        if (input.x * mirrorMult == -1 * mirrorMult && input.y == 0) return "4";
        if (input.x * mirrorMult == 0 && input.y == 0) return "5";
        if (input.x * mirrorMult == 1 && input.y == 0) return "6";
        if (input.x * mirrorMult == -1 && input.y == 1) return "7";
        if (input.x == 0 * mirrorMult && input.y == 1) return "8";
        if (input.x == 1 * mirrorMult && input.y == 1) return "9";

        return "5";

    }

    public InputProcessor(InputUser user)
    {
        this.user = user;
    }
    public void ProcessMove(InputValue input)
    {
        if (input.Get() == null)
        {
            stickPosition = Vector2.zero;
        }
        else
        {
            stickPosition = (Vector2)input.Get();
        }
        if (stickPosition.x > 0) stickPosition.x = 1;
        if (stickPosition.x < 0) stickPosition.x = -1;
        if (stickPosition.y > 0) stickPosition.y = 1;
        if (stickPosition.y < 0) stickPosition.y = -1;

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
