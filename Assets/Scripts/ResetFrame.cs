using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ResetFrame : Publisher<Messages.Geometry.PoseStamped>
    {
		public PlaneClick BasePlane;
        public Transform ReferenceTransform; //i.e. Camera position, set the camera on REAL robot and run Update
        public Transform TargetTransform; //i.e. robot in AR(=Unity)  it is miss matched one
        public string FrameId = "Unity";
        private Messages.Geometry.PoseStamped message;
		public Vector3 ResetOffset = Vector3.up * 0.5f;
        protected override void Start()
        {
            base.Start();
            message = new Messages.Geometry.PoseStamped
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };
        }
        //called by some event
        //Set the AR camera on real robot and do ResetFrame(), "Unity"TF will fixed  with the offset of transform as PoseStamped message (in ROS side script)
        public void RsetFrame()
        {
			GameObject go = new GameObject();
			go.transform.parent = ReferenceTransform;
			go.transform.localPosition = -1*ResetOffset;
			go.transform.rotation =ReferenceTransform.rotation;

			Matrix4x4 v=  TargetTransform.localToWorldMatrix  * go.transform.worldToLocalMatrix;
            Vector3 p = v.GetColumn(3);  
            message.header.Update();
            message.pose.position = GetGeometryPoint(p.Unity2Ros());
            message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
            Publish(message);
        }
		public void RsetFramePos()
		{
			GameObject go = new GameObject();
			go.transform.parent = ReferenceTransform;
			if (BasePlane.IsValid())
			{
				Vector3 normal = BasePlane.rotation * Vector3.down;
				float b = Vector3.Dot(normal, BasePlane.position);
				float a = Vector3.Dot(normal, ReferenceTransform.position);
				Debug.Log("a" + a.ToString() + "  b:" + b.ToString());
				go.transform.position = ReferenceTransform.position - (a - b) * normal;
			}
			else
			{
				go.transform.localPosition = Vector3.zero - 1 * ResetOffset;
			}
			go.transform.rotation = TargetTransform.rotation;

			


			Matrix4x4 v = TargetTransform.localToWorldMatrix * go.transform.worldToLocalMatrix;
			Destroy(go);
			Vector3 p = v.GetColumn(3);
			message.header.Update();
			message.pose.position = GetGeometryPoint(p.Unity2Ros());
			message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
			Publish(message);
		}
		public void RsetFrameRot()
		{
			GameObject go = new GameObject();
			go.transform.parent =ReferenceTransform;
			go.transform.position = TargetTransform.position;
			if (BasePlane.IsValid())
			{
				//normal direction must be same as base plane.
				Vector3 normal= BasePlane.rotation * Vector3.up;
				Vector3 rotVec = new Vector3(
					ReferenceTransform.transform.rotation.x,
					ReferenceTransform.transform.rotation.y,
					ReferenceTransform.transform.rotation.z);
				float r = Vector3.Dot(normal, rotVec);
				go.transform.rotation = Quaternion.AxisAngle(normal, r);

			}
			else
			{
				go.transform.localRotation = Quaternion.identity;
			}


			Matrix4x4 v = TargetTransform.localToWorldMatrix * go.transform.worldToLocalMatrix;
			Destroy(go);	
			Vector3 p = v.GetColumn(3);
			message.header.Update();
			message.pose.position = GetGeometryPoint(p.Unity2Ros());
			message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
			Publish(message);
		}
		public void OffsetPos(Vector3 pos)
		{
			GameObject go = new GameObject();
			go.transform.parent = TargetTransform;
			go.transform.localPosition = pos;
			go.transform.localRotation = Quaternion.identity;
			Matrix4x4 v = TargetTransform.localToWorldMatrix * go.transform.worldToLocalMatrix;
			Destroy(go);

			Vector3 p = v.GetColumn(3);
			message.header.Update();
			message.pose.position = GetGeometryPoint(p.Unity2Ros());
			message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
			Publish(message);
		}
		public void OffsetRot(Quaternion q)
		{
			GameObject go = new GameObject();
			go.transform.parent = TargetTransform;
			go.transform.localRotation = q;
			go.transform.localPosition = Vector3.zero;
			Matrix4x4 v = TargetTransform.localToWorldMatrix * go.transform.worldToLocalMatrix;
			Destroy(go);

			Vector3 p = v.GetColumn(3);
			message.header.Update();
			message.pose.position = GetGeometryPoint(p.Unity2Ros());
			message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
			Publish(message);
		}
		public void OffsetPosForward(float l)
		{
			OffsetPos(Vector3.forward * l);
		}
		public void OffsetPosRight(float l)
		{
			OffsetPos(Vector3.right * l);
		}
		public void OffsetRotY(float l)
		{
			OffsetRot(Quaternion.Euler(0, l, 0));
		}
		



		private Messages.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            Messages.Geometry.Point geometryPoint = new Messages.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }

        private Vector3 GetPosition(Messages.Geometry.PoseStamped message)
        {
            return new Vector3(
                message.pose.position.x,
                message.pose.position.y,
                message.pose.position.z);
        }

        private Quaternion GetRotation(Messages.Geometry.PoseStamped message)
        {
            return new Quaternion(
                message.pose.orientation.x,
                message.pose.orientation.y,
                message.pose.orientation.z,
                message.pose.orientation.w);
        }
    }
}
