using UnityEngine;
/// <summary>
/// Attach this script to any gameObject for which you want to put a note.
/// </summary>
public class ThisIsComment : MonoBehaviour
{
    [TextArea]
    public string Notes = "Comment Here."; // Do not place your note/comment here. 
                                           // Enter your note in the Unity Editor.
}