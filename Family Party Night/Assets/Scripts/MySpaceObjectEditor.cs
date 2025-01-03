using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(SpaceObject))]
public class MySpaceObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the target MonoBehaviour object
        SpaceObject mySpaceObject = (SpaceObject)target;

        // Display the base class inspector
        DrawDefaultInspector();

        // If the ScriptableObject is assigned
        if (mySpaceObject.spaceInfo != null)
        {
            // Display the fields of the ScriptableObject in the same Inspector
            EditorGUILayout.Space(); // Adds space between sections
            EditorGUILayout.LabelField("MyScriptableObject Fields", EditorStyles.boldLabel);
            
            // Create a SerializedObject for the ScriptableObject to display its fields
            SerializedObject serializedObject = new SerializedObject(mySpaceObject.spaceInfo);
            SerializedProperty serializedProperty = serializedObject.GetIterator();

            // Show all fields of the ScriptableObject
            serializedObject.Update();
            while (serializedProperty.NextVisible(true))
            {
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.HelpBox("No ScriptableObject assigned", MessageType.Warning);
        }
    }
}
