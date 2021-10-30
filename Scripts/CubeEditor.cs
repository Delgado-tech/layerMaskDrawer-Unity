using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine;


[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor {

    int maskField;
    int convertedValue;
    List<int> layers;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Cube cube = (Cube)target;

        maskField = EditorGUILayout.MaskField(new GUIContent("Layer", "escolha uma layer"), cube.maskField, InternalEditorUtility.layers);
        LayerMaskDrawer(maskField, ref cube.maskField, ref cube.layer);


    }

    void LayerMaskDrawer(int maskField, ref int lateMaskField, ref LayerMask mask) {

        if (maskField != lateMaskField) {
            if (maskField > 0) {
                convertedValue = 0;
                layers = new List<int>();

                int tempVal = maskField;
                int x = 1;
                int l = 0;

                while (tempVal > 0) {
                    if (x * 2 > tempVal) {
                        layers.Add(l);
                        tempVal -= x;
                        x = 1;
                        l = 0;
                        continue;
                    }
                    x *= 2;
                    l++;
                }

                foreach (var layer in layers) {
                    convertedValue += (int)Mathf.Pow(2, LayerMask.NameToLayer(InternalEditorUtility.layers[layer]));
                }

            } else {
                convertedValue = maskField;
            }
            lateMaskField = maskField;
        }

        mask = convertedValue;

    }
    
    
