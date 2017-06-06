using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(MapBoxScript))]
public class MapBoxCoordinatesDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        if (position != null)
        {
             MapBoxCoordinates coordinates = new MapBoxCoordinates(property.FindPropertyRelative("xPos").intValue, property.FindPropertyRelative("zPos").intValue);
            position = EditorGUI.PrefixLabel(position, label);
            GUI.Label(position, coordinates.ToString());
        }
    }
}
