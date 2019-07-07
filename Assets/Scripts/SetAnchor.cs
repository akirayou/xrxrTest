using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnchor : MonoBehaviour
{
    public UnityEngine.UI.Dropdown Mode;
    public PlaneClick planeClick;
    public Transform AnchoredFrame;
    public Transform AnchorMarker;

    void OnPlaneClick(PlaneClick plane)
    {
        if (Mode.value != 0) return;//Anchor Mode
        AnchoredFrame.transform.parent = plane.PlaneAnchor.transform;
        AnchorMarker.transform.parent = plane.PlaneAnchor.transform;
        AnchorMarker.transform.position = plane.position;
        AnchorMarker.transform.rotation=plane.rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        planeClick.SetFlagEvent.AddListener(OnPlaneClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
