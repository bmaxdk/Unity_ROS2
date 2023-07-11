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
- Drag 'ROS2 Unity Compoenet(Mono Scrip)' into inspector panel in Cube
- Drag 'ROS2 Listener Example' to the cube's inspector