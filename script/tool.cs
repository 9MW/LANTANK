using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
[CustomEditor(typeof(T))]//T is gameobject component,muet be full else if the extension can't work
public class tool : Editor {
    Color Transparency;//The color will be instead by 0,0,0,0,
    Texture2D outTex;
    Texture2D inTex;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Transparency Color", GUILayout.Width(130));
        Transparency = EditorGUILayout.ColorField(Transparency);
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.DropdownButton(new GUIContent("Generate"), FocusType.Keyboard))
        {
            outTex = new Texture2D(inTex.width, inTex.height);
            Color[] c = inTex.GetPixels();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == Transparency)
                {
                    c[i] = new Color(0, 0, 0, 0);
                }
            }
            outTex.SetPixels(c);
            outTex.Apply();
            File.WriteAllBytes(Application.dataPath + "/SavedScreen.png",
            outTex.EncodeToPNG());
            Debug.Log("already accomplish");
        }
    }
}
