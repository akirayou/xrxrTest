using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
public class PlaneClick : MonoBehaviour
{
	public GameObject FlagObj;
	private TrackableHit _hit;
    public Anchor PlaneAnchor=null;
	public TrackableHit TrackableHit()
	{
		return _hit;
	}
	public class PlaneClickEvent : UnityEngine.Events.UnityEvent<PlaneClick>
	{
	}

	public PlaneClickEvent SetFlagEvent = new PlaneClickEvent();

	public Vector3 position {
		get{return transform.position;}
	}
	public Quaternion rotation
	{
		get { return transform.rotation; }
	}
	public bool IsValid()
	{
		return FlagObj.activeSelf;
	}
	public void SetActive(bool active)
	{
		FlagObj.SetActive(true);
	}
	
	
	// Start is called before the first frame update
	void Start()
    {
        FlagObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount==1 && Input.GetTouch(0).phase==TouchPhase.Began))
		{
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.touchCount == 1 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;


            Debug.Log("Click");

			Vector3 p=Input.mousePosition;
			GoogleARCore.TrackableHit hit;
            
			if(GoogleARCore.Frame.Raycast(p.x, p.y, GoogleARCore.TrackableHitFlags.PlaneWithinPolygon, out hit))
			{
				FlagObj.SetActive(false);
				_hit = hit;
				Pose planePose = hit.Pose;
                PlaneAnchor=hit.Trackable.CreateAnchor(hit.Pose);

                transform.position = planePose.position;
				transform.rotation = planePose.rotation;
				FlagObj.SetActive(true);
				SetFlagEvent.Invoke(this);
			}
		}
	}
}
