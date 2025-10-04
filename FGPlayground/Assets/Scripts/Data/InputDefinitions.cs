using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct InputDefinition
{
    public string name;
    public List<InputStep> steps;
    public string button;
}

[System.Serializable]
public struct InputStep
{
    public string input;
    public int nextStepMaximumFrames;
    public int requiredHeldFrames;
}

[CreateAssetMenu(fileName = "InputDefinitions", menuName = "Scriptable Objects/InputDefinitions")]
public class InputDefinitions : ScriptableObject
{
    //higher on the list = higher input priority
    public List<InputDefinition> inputs;
}
