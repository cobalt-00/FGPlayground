using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;



 
public class InputRebindUtil
{
    private static string DataPath = Application.persistentDataPath + "/inputBinds.json";

    public static bool SaveInputs()
    {
        var inputJson = InputSystem.actions.SaveBindingOverridesAsJson();
        byte[] inputBytes = new UTF8Encoding(true).GetBytes(inputJson);
        //clear old inputs first
        if (File.Exists(DataPath))
        {
            File.Delete(DataPath);
        }

        //save new inputs
        using (FileStream file = File.Create(DataPath))
        {
            file.Write(inputBytes, 0, inputBytes.Length);
        }

        return true;
    }

    public static bool LoadInputs()
    {
        if (File.Exists(DataPath))
        {
            string inputString = File.ReadAllText(DataPath);

            InputSystem.actions.LoadBindingOverridesFromJson(inputString);
            return true;
            
        }

        return false;
    }

}
