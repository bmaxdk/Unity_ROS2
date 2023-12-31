// Copyright 2019-2021 Robotec.ai.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using UnityEngine;

namespace ROS2
{

/// <summary>
/// An example class provided for testing of basic ROS2 communication
/// </summary>
[RequireComponent(typeof(ROS2UnityComponent))]
public class ROS2ListenerExample : MonoBehaviour
{
    private ROS2UnityComponent ros2Unity;
    private ROS2Node ros2Node;
    private ISubscription<geometry_msgs.msg.Twist> twist_sub;
    private float[] transformPosition = new float[3];
    float speed = 0.1f;
    bool moving = false;


    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();
    }

    void Update()
    {
        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityListenerNode");
            twist_sub = ros2Node.CreateSubscription<geometry_msgs.msg.Twist>(
              "turtle1/cmd_vel", msg => {
                    parse(msg);
                } 
            ); //Topic name
        }
        
        if (moving)
        {
            transform.position = 
                new Vector3(transform.position.x + (transformPosition[0]*speed), 
                            transformPosition[1], 
                            transform.position.z + (transformPosition[2]*speed));
            moving = false;
        }
    }

    void parse(geometry_msgs.msg.Twist msg) {
        // Debug.Log("x: " + msg.Linear.X);
        // Debug.Log("z:" + msg.Angular.Z);
        transformPosition[0] = (float) msg.Linear.X;
        transformPosition[1] = (float) msg.Linear.Y;
        transformPosition[2] = (float) msg.Angular.Z;
        moving = true;
    }
}

}  // namespace ROS2
