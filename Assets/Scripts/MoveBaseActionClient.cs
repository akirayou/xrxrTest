using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.Messages;
using RosSharp.RosBridgeClient.Messages.Geometry;

namespace RosSharp.RosBridgeClient
{
    public class MoveBaseActionClient : ActionClient<MoveBaseActionGoal,
                                                      MoveBaseActionFeedback,
                                                      MoveBaseActionResult>
    {
        public PoseStamped target = new PoseStamped();
        public override MoveBaseActionGoal GetGoal()
        {
            return new MoveBaseActionGoal() {
                goal = new MoveBaseGoal {
                    target_pose = new PoseStamped()
                }
            };

        }
        public void Start()
        {
            target.header.Update();
            CancelGoal();
        }


    }
}