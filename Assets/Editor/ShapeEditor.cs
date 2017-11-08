using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
//taken from https://www.youtube.com/watch?v=bPO7_JNWNmI
//part 2 https://www.youtube.com/watch?v=ew4NtzkXj8U
public class ShapeEditor : Editor
{

    ShapeCreator shapeCreator;
    SelectionInfo selectionInfo;
    bool needsRepaint;

    //Called when any input on the scene
    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(guiEvent);
            if (needsRepaint)
            {
                HandleUtility.Repaint();
                needsRepaint = false;
            }
        }

    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float distToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(distToDrawPlane);

        if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDown(mousePosition);
        }

        if (guiEvent.type == EventType.mouseUp && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseUp(mousePosition);
        }

        if (guiEvent.type == EventType.mouseDrag && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDrag(mousePosition);
        }

        if (!selectionInfo.pointIsSelected)
        {
            UpdateMouseOverInfo(mousePosition);
        }


    }

    void HandleLeftMouseDown(Vector3 mousePosition)
    {
        if (!selectionInfo.mouseIsOverPoint)
        {
            Undo.RecordObject(shapeCreator, "Add point");
            shapeCreator.points.Add(mousePosition);
            selectionInfo.pointIndex = shapeCreator.points.Count - 1;
        }

        selectionInfo.pointIsSelected = true;
        selectionInfo.positionAtStartOfDrag = mousePosition;
        needsRepaint = true;
    }

    void HandleLeftMouseUp(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            //use the mouse pos at the start of the move to enable undo of moving a point.
            shapeCreator.points[selectionInfo.pointIndex] = selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(shapeCreator, "Move point");
            shapeCreator.points[selectionInfo.pointIndex] = mousePosition;

            selectionInfo.pointIsSelected = false;
            selectionInfo.pointIndex = -1;
            needsRepaint = true;
        }
    }

    void HandleLeftMouseDrag(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            shapeCreator.points[selectionInfo.pointIndex] = mousePosition;
            needsRepaint = true;
        }
    }

    void UpdateMouseOverInfo(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            if (Vector3.Distance(mousePosition, shapeCreator.points[i]) < shapeCreator.handleRadius)
            {
                mouseOverPointIndex = i;
                break;
            }
        }
        if (mouseOverPointIndex != selectionInfo.pointIndex)
        {
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            needsRepaint = true;
        }
    }

    void Draw()
    {
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            Vector3 nextPoint = shapeCreator.points[(i + 1) % shapeCreator.points.Count];
            Handles.color = Color.black;
            Handles.DrawDottedLine(shapeCreator.points[i], nextPoint, 4);

            if (i == selectionInfo.pointIndex)
            {
                Handles.color = (selectionInfo.pointIsSelected) ? Color.black : Color.red;
            }
            else
            {
                Handles.color = Color.white;
            }
            Handles.DrawSolidDisc(shapeCreator.points[i], Vector3.up, shapeCreator.handleRadius);
        }
    }

    private void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
        selectionInfo = new SelectionInfo();
    }

    public class SelectionInfo
    {
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;
    }
}
