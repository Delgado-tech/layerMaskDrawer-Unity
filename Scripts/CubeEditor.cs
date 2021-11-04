using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using EditorMethods;

[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {

    int PopupsCount = 2;
    int[] maskField;
    int[] convertedValue;
    List<string> layers;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Cube cube = (Cube)target;
		
	if (maskField.Length != PopupsCount) maskField = new int[PopupsCount];
        if (convertedValue.Length != PopupsCount) convertedValue = new int[PopupsCount];
        if (cube.maskField.Length != PopupsCount) cube.maskField = new int[PopupsCount];

        maskField[0] = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField[0], InternalEditorUtility.layers);
        LayerMaskDrawer.Draw(maskField[0], ref cube.maskField[0], ref convertedValue[0],ref cube.layer);

    }
}
    
    
