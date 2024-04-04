Instructions:

1. Open the project with the Unity version corresponding to the one indicated in the ProjectSettings > ProjectVersion.txt file.
2. Navigate to Assets > Unity Technologies > SpaceRobotKyle > Models and drag and drop "KyleRobot" into the Hierarchy.
3. Drag and drop "Main Camera" under the "KyleRobot" object as the first element in the hierarchy.
4. In the Inspector window for "KyleRobot", find the "Transform" section and paste the following as "World Transform":
   UnityEditor.TransformWorldPlacementJSON:{"position":{"x":0.0,"y":0.0,"z":0.0},"rotation":{"x":0.0,"y":1.0,"z":0.0,"w":0.0},"scale":{"x":1.0,"y":1.0,"z":1.0}}
5. In the Inspector window for "Main Camera", find the "Transform" section and paste the following as "World Transform":
   UnityEditor.TransformWorldPlacementJSON:{"position":{"x":1.24,"y":0.769,"z":-1.47},"rotation":{"x":0.0,"y":0.383,"z":0.0,"w":-0.924},"scale":{"x":0.01,"y":0.01,"z":0.01}}
6. Select "AvatarController" in the Animator > Controller section of the inspector for "KyleRobot".
7. Add IK Control (Script) as component to "KyleRobot", check "Ik Active", and specify "Assets/JSON Data/soldatino" as the "Json File Path".
8. Assign respective GameObjects to all the body parts.
9. Run the project. An error may initially appear, but it will disappear upon rerunning. Press "L" to see the avatar move.
