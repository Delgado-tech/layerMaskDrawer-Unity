using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {
	
    SerializedProperty layer;
	
    void OnEnable() {
      layer = serializedObject.FindProperty("layer");
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

	EditorGUILayout.PropertyField(layer, new GUIContent("Layer","Escolha uma opção"));
		
	serializedObject.ApplyModifiedProperties();
    }
}
    
    
