using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.RegisterProcessor<InputReader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnMove(InputValue input)
    {
        Debug.Log("Move" + input.Get());
    }
}
