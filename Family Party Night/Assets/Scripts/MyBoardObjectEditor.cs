using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(BoardObject))]
public class MyBoardObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the target MonoBehaviour object
        BoardObject myBoardObject = (BoardObject)target;

        // Display the base class inspector
        DrawDefaultInspector();

        // If the ScriptableObject is assigned
        if (myBoardObject.boardInfo != null)
        {
            // Display the fields of the ScriptableObject
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("MyScriptableObject Fields", EditorStyles.boldLabel);
            
            SerializedObject serializedObject = new SerializedObject(myBoardObject.boardInfo);
            SerializedProperty serializedProperty = serializedObject.GetIterator();

            // Show all fields of the ScriptableObject
            serializedObject.Update();
            while (serializedProperty.NextVisible(true))
            {
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
            serializedObject.ApplyModifiedProperties();

            // Display the fields of each BoardSpot in the spaces list
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Board Spots", EditorStyles.boldLabel);

            SerializedProperty spacesProperty = serializedObject.FindProperty("spaces");

            if (spacesProperty != null && spacesProperty.isArray)
            {
                // Iterate through each BoardSpot in the spaces list
                for (int i = 0; i < spacesProperty.arraySize; i++)
                {
                    SerializedProperty boardSpotProperty = spacesProperty.GetArrayElementAtIndex(i);

                    // If the BoardSpot is not null, show its fields
                    if (boardSpotProperty.objectReferenceValue != null)
                    {
                        // Create a SerializedObject for each BoardSpot
                        SerializedObject boardSpotSerializedObject = new SerializedObject(boardSpotProperty.objectReferenceValue);
                        SerializedProperty boardSpotSerializedProperty = boardSpotSerializedObject.GetIterator();

                        // Display the fields of the BoardSpot
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"Board Spot {i + 1}", EditorStyles.boldLabel);
                        
                        boardSpotSerializedObject.Update();
                        while (boardSpotSerializedProperty.NextVisible(true))
                        {
                            EditorGUILayout.PropertyField(boardSpotSerializedProperty, true);
                        }
                        boardSpotSerializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox($"Board Spot {i + 1} is missing", MessageType.Warning);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No Board Spots found.", MessageType.Info);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No ScriptableObject assigned", MessageType.Warning);
        }
    }
}
