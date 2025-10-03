using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.Rendering.DebugUI;



 
public class InputRebindUtil
{
    private static string DataPath(InputUser user) => Application.persistentDataPath + "/inputBinds_" + user.id + ".json";

    public static bool SaveInputs(InputUser user)
    {
        var inputJson = InputSystem.actions.SaveBindingOverridesAsJson();
        byte[] inputBytes = new UTF8Encoding(true).GetBytes(inputJson);
        //clear old inputs first
        if (File.Exists(DataPath(user)))
        {
            File.Delete(DataPath(user));
        }

        //save new inputs
        using (FileStream file = File.Create(DataPath(user)))
        {
            file.Write(inputBytes, 0, inputBytes.Length);
        }

        return true;
    }

    public static bool LoadInputs(InputUser user)
    {
        if (File.Exists(DataPath(user)))
        {
            string inputString = File.ReadAllText(DataPath(user));

            InputSystem.actions.LoadBindingOverridesFromJson(inputString);
            return true;
            
        }

        return false;
    }

}
