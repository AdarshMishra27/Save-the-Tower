using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.red;
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Waypoint waypoint;
    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled=false;
        waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled=true;
        }

        SetLabelColor();
        ToggleLabel();
    }

    private void SetLabelColor()
    {
        if (waypoint.IsPlaceable)
        {
            label.color = defaultColor;
        }
        else
        {
            label.color = blockedColor;
        }
    }

    void ToggleLabel() {
        if(Input.GetKeyDown(KeyCode.C)) {
            label.enabled = !label.enabled;
        }
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    private void DisplayCoordinates()
    {
        //CAUTION unityeditor methods cant be used in build so instead add all scripts which use UnityEditor to a folder which is ignored called as Editor
        //Also scripts in Editor cant be attached to GameObjects!!!
        //So be careful!
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z); //because in the 3d y is actually z axis
        label.text = coordinates.x + "," + coordinates.y;
    }
}
