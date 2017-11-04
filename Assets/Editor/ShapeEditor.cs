using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
//taken from https://www.youtube.com/watch?v=bPO7_JNWNmI
public class ShapeEditor : Editor {

    ShapeCreator shapeCreator;

    //Called when any input on the scene
    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float distToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(distToDrawPlane);

        if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
        {
            shapeCreator.points.Add(mousePosition);
            Debug.Log("add:" + mousePosition);
        }
    }

    private void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
    }
}
