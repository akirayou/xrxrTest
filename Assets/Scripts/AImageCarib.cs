namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    public class AImageCarib : MonoBehaviour
    {
        public int TargetImageIndex = 0;
        public GameObject MarkerObj=null;
        /// <summary>
        /// From the marker to the robot position offset.
        /// </summary>
        public Vector3 MarkerOffset = new Vector3(0, 0.075f / 2, 0);
        /// <summary>
        /// In many case the robot marker is laied flat. You have to rotate the marker for robot real rotaton.
        /// </summary>
        public Vector3 MarkerRotate = new Vector3(0,-90,0);
        private Vector3 m_OriginalScale;
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();
        public RosSharp.RosBridgeClient.ResetFrame ResetFrame;
        // Start is called before the first frame update
        void Start()
        {
            if (MarkerObj == null)
            {
                MarkerObj=GameObject.CreatePrimitive(PrimitiveType.Cube);
                MarkerObj.transform.localScale = new Vector3(0.115f, 0.075f, 0.01f);
                MarkerObj.transform.localRotation = Quaternion.Euler(MarkerRotate);
            }
            m_OriginalScale = MarkerObj.transform.localScale;
            MarkerObj.SetActive(false);
        }



        // Update is called once per frame
        void Update()
        {
            Session.GetTrackables<AugmentedImage>(
                m_TempAugmentedImages, TrackableQueryFilter.Updated);
            foreach (var image in m_TempAugmentedImages)
            {
                if (image.DatabaseIndex != TargetImageIndex) continue;

                if (image.TrackingState == TrackingState.Tracking && MarkerObj.activeSelf == false)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    MarkerObj.transform.parent = anchor.transform;
                    MarkerObj.transform.localPosition=Vector3.zero;
                    MarkerObj.transform.localRotation = Quaternion.identity;
                    MarkerObj.SetActive(true);
                    ResetFrame.ResetFrameBoth(MarkerObj.transform);
                    ToastUtil.Toast(this, "Set Robot pose by the marker.");

                }
                else if (image.TrackingState == TrackingState.Stopped && MarkerObj.activeSelf == true)
                {
                    if(MarkerObj.transform.parent!=null)
                        Destroy(MarkerObj.transform.parent.gameObject);
                    MarkerObj.transform.parent = null;
                }
            }



        }
    }

}
