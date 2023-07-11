# Control a Unity GameObject using ROS2

GetPackage:
download from `https://github.com/RobotecAI/ros2-for-unity/releases`

For ROS2 Foxy:
[Ros2ForUnity_UbuntuFoxy.unitypackage](https://github.com/RobotecAI/ros2-for-unity/releases/download/1.0.0/Ros2ForUnity_UbuntuFoxy.unitypackage)


Build new project:
- Assets>>ImportPackage>>Custom Package>>Ros2ForUnity_UbuntuFoxy.unitypackage


In left top `Hierarchy` panel right click:
- 3D Object>>Plane
- 3D Object>>Cube

Setup Plane:
- Transform >> Position >> Y = -0.5

# Create a red material
In Assets Panel right click:
- Create >> Material
- Click: Albedo -> select red color
- Drag red material to the cube

# Adjust the Camera
- Click Main Camera
- Transform >> Position: Y=6, Z=-5 
- Transform >> Rotation: X=60

# Add Scripts
- Click: Assets >> Ros2ForUnity >> Scripts
- Click: Cube
- Drag `ROS2 Unity Compoenet(Mono Scrip)` into inspector panel in Cube
- Drag `ROS2 Listener Example' to the cube`s inspector

Check `ros-foxy-turtlesim` is installed
```bash
$ sudo apt search ros-foxy-turtlesim

# If not installed
$ sudo apt install ros-foxy-turtlesim

# Once it is intalled try turtle_teleop_key
$ ros2 run turtlesim turtle_teleop_key

# Monitor the movement
$ ros2 topic echo /turtle1/cmd_vel
```

Notice that it only sends 2.0 and -2.0 along x and z axis

Since we know what the topic sends, we will modify `ROS2 Listener Example` to accept it.

In Unity, click vertical 3 dots in `ROS2 Listener Example` >> Edit Script 

```cs
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
    private ISubscription<std_msgs.msg.String> chatter_sub;

    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();
    }

    void Update()
    {
        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityListenerNode");
            chatter_sub = ros2Node.CreateSubscription<std_msgs.msg.String>(
              "chatter", msg => Console.WriteLine("Unity listener heard: [" + msg.Data + "]"));
        }
    }
}

}  // namespace ROS2
```

change chatter to twist:

```cs
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
              "turtle1/cmd_vel", msg => Debug.Log("x: " + msg.Linear.X + 
              "\n z:" + msg.Angular.Z)); //Topic name
        }
    }
}

}  // namespace ROS2

```

## Now Try
- click play

In the `Consol` the values are being displayed which topic received correctly!:


Let's go back to the same scrip to separate the parsing of the recieved data into another function.

Name the function **parse** and pass the received data:

```cs
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
    }

    void parse(geometry_msgs.msg.Twist msg) {
        Debug.Log("x: " + msg.Linear.X);
        Debug.Log("z:" + msg.Angular.Z);
    }
}

}  // namespace ROS2

```

Check see using teleop key and wheter it works in Console.


# Modify the Script to move the Cube upon receiving the data
In the same scrip file we just modifed

```cs
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
        transform.position = new Vector3(transformPosition[0], transformPosition[1], transformPosition[2]);
    }

    void parse(geometry_msgs.msg.Twist msg) {
        // Debug.Log("x: " + msg.Linear.X);
        // Debug.Log("z:" + msg.Angular.Z);
        transformPosition[0] = (float) msg.Linear.X;
        transformPosition[1] = (float) msg.Linear.Y;
        transformPosition[2] = (float) msg.Angular.Z;
        
    }
}

}  // namespace ROS2

```

Now, this allows to telop key to move object.

# Move the Cube in one direction continuously

The idea is set "moving" to tru once a data has been received, update the transform of the Cube and set "moving" to false

```cs
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

```
# Sources
**ros2 useful tutorial:**

[https://github.com/RobotecAI/ros2-for-unity](https://github.com/RobotecAI/ros2-for-unity)

[youtube](https://www.youtube.com/watch?v=FJSs2BsjRRI&ab_channel=UnityROS2Tech)
