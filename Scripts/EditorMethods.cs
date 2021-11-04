using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

public static class EditorMethods
{
    static List<string> layers;
    public static void LayerMaskDrawer(int maskField, ref int lateMaskField, ref int convertedValue, ref LayerMask mask) {

        if (maskField == lateMaskField && mask != convertedValue) {
            LayerMaskDrawerInternalChange(ref convertedValue, ref lateMaskField, ref mask);
            return;
        }

        if (maskField != 0 && maskField != -1) {
            convertedValue = 0;
            layers = new List<string>();

            int tempVal = maskField;
            int x = 1;
            int l = 0;

            while (tempVal != 0) {

                if (tempVal > 0 && x * 2 > tempVal) {
                    layers.Add(InternalEditorUtility.layers[l]);
                    tempVal -= x;
                    x = 1;
                    l = 0;
                    continue;
                }


                if (tempVal < 0 && x * 2 < tempVal) {
                    layers.Add(InternalEditorUtility.layers[l]);
                    tempVal += x;
                    x = 1;
                    l = 0;
                    continue;
                }

                x *= 2;
                l++;
            }

            convertedValue = LayerMask.GetMask(layers.ToArray());

        } else {
            convertedValue = maskField;
        }

        lateMaskField = maskField;
        mask = convertedValue;

    }

    static void LayerMaskDrawerInternalChange(ref int convertedValue, ref int lateMaskField, ref LayerMask mask) {
        if (mask != 0 && mask != -1) {
            layers = new List<string>();

            int tempVal = mask;
            int x = 1;
            int l = 0;
            while (tempVal != 0) {

                if (tempVal > 0 && x * 2 > tempVal) {
                    layers.Add(LayerMask.LayerToName(l));
                    tempVal -= x;
                    x = 1;
                    l = 0;
                    continue;
                }


                if (tempVal < 0 && x * 2 < tempVal) {
                    layers.Add(LayerMask.LayerToName(l));
                    tempVal += x;
                    x = 1;
                    l = 0;
                    continue;
                }

                x *= 2;
                l++;
            }

            lateMaskField = 0;
            for (int i = 0; i < InternalEditorUtility.layers.Length; i++) {
                foreach (var layer in layers) {
                    if (InternalEditorUtility.layers[i] == layer) {
                        lateMaskField += (int)Mathf.Pow(2, i);
                    }
                }
            }

        } else {
            lateMaskField = mask;
        }

        convertedValue = mask;
    }
}
