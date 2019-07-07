using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using RosBridgeClient.Messages;
  
public class GenMsg : EditorWindow
{
    private const string MENU_PATH = "/THIS_PROJECT/";
    private const string OUT_PATH = "Assets/GeneratedScripts/";
    
    [MenuItem(MENU_PATH+"MsgGen")]
    private static void CreateBasicScript()
    {
        ShowWindow("MsgGen");
    }
    private static void ShowWindow(string templateScriptName)
    {

        EditorWindow.GetWindow<GenMsg>("Create Script");
    }

    private void OnGUI()
    {

        GUILayout.Space(10);
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.wordWrap = true;
        GUILayout.Label("This will generate all ROS Messaages which this project needs.Out put directory is "+OUT_PATH,style);
        if (GUILayout.Button("Generate"))
        {
            if (CreateScript())
            {
                this.Close();
            }
        }

    }
    //=================================================================================
    //Generate ROS Messages
    //=================================================================================
    private static bool CreateScript()
    {
        ActionMessageGenerator.Generate("MoveBase", "move_base_msgs",
            new MessageElement[] { new MessageElement("PoseStamped", "target_pose", false) },
            new MessageElement[] { },
            new MessageElement[] { new MessageElement("PoseStamped", "base_position", false) },
            OUT_PATH);
        Debug.Log("Create Message Done");

        return true;
    }

}