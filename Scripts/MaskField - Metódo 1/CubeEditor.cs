using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {
    int maskField;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Cube cube = (Cube)target;	
	string[] layers = new string[32]; //Array que irá armazenar os nomes das layers
	for (int i = 0; i < layers.Length; i++) { //vai percorrer cada índice do array
		foreach (var layer in InternalEditorUtility.layers) { //irá percorrer cada string do array InternalEditorUtility.layers
			if (layer == LayerMask.LayerToName(i)) { //LayerMask.LayerName() é um método que você passa um índice de uma layer e retorna o nome dela
			  layers[i] = layer; //caso a string atual (layer) seja igual a string das layers da Unity, é adicionada no array layers essa string
			}
		}
	}
		
        maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, layers);
        
	if (maskField != cube.maskField) {
		cube.maskField = maskField;
		cube.layer = maskField;
	}

	if (maskField != cube.layer) {
		cube.maskField = cube.layer;
	}

    }
}
    
    
