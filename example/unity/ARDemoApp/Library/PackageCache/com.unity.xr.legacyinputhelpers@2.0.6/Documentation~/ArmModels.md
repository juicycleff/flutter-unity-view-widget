# Arm Models

The com.unity.xr.legacyinputhelpers package contains three arm models. These are:
- [Base Arm Model](#Base-Arm-Model)
- [Swing Arm Model](#Swing-Arm-Model)
- [Transition Arm Model](#Transition-Arm-Model)

These arm models are based on the arm models from the Google daydream package. The original code can be found [here](https://developers.google.com/vr/develop/unity/get-started-android).

# Using the Arm Models

The arm model pose providers are intended to be used as a pose provider plugin to a tracked pose driver.

Pose provider plugins are used to allow custom logic to be performed while still gaining the update and transform application utility of the tracked pose driver. For the arm models,we read rotation and acceleration data from the controller pose node, the Head position from the Head node, and calculate a position in session space based on a mathematical arm model.

The following image shows how to use the arm model pose provider with a tracked pose driver:

![Arm Model TPD Examples](images/ArmModelImages/TrackedPoseDriverArmModelExample.png)

The above image shows that the arm model monobehaviour has been linked to the use pose provider field of the tracked pose driver

The following section outlines the parameters that are found on the inspector of the arm model.

## Base Arm Model 

This section outlines how to configure and use the Base Arm Model
The base arm model can be used for most situations that require pointing or throwing.

The image below shows the base arm model component:

![Base Arm Model](images/ArmModelImages/armmodelbase.png)

The following table details the Arm Model inspector controls:

| Control Name | Description |
|---|---|
|  Input Pose Source | The Input pose source defines which XR Node will be used as controller tracking input source. The pose data, and gyro/angular acceleration data for this XR Node will be used to drive the arm model calculations.|
| Head Position Source | The head position source defines which XR Node will be used to drive the Head Position when calculating the arm model. |
| Arm Extension Offset | Offset applied to the elbow position as the controller is rotated upwards. |
| Elbow Bend Ratio | The ratio of the controllers rotation ato apply to the rotation of the elbow. The remaining rotation is applied to the wrist's rotation. |
| Lock To Neck | The Lock To Neck checkbox controls whether the Head Position Source is used to determined the model's neck position, or if the neck position is assumed to be Vector3's zero. |
| Rest Position | The rest position settings specify the joint positions of each part of the mathematical model of the arm relative to the head position, before the arm model is applied. |

The positions defined here are multiplied by -1 or 1 depending on if they are used as the left, or right arm. The positions are therefore relative to the center of the head.
The table below details the Rest Position controls:

| Control Name | Description |
| ------------ | ----------- | 
| Elbow Rest Position | Position of the elbow joint relative to the head before the arm model is applied. |
| Wrist Rest Position | Position of the wrist joint relative to the head before the arm model is applied. |
| Controller Rest Position | Position of the controller joint relative to the head before the arm model is applied. |

## Swing Arm Model

The swing arm model has the same base settings as the [Base Arm Model](#Base-Arm-Model) but also contains some additional extra parameters to deal with swinging motion.

The swing arm model is intended to be used as a pose provider to a tracked pose driver in the same way as the base arm model. 

In the swing arm model, there is a second set of rotation ratio parameters that are applied when the controller is pointing towards the player (ie: backwards).

the image below shows the swing arm model component:

![Swing Model](images/ArmModelImages/swingarmmodel.png)

| Control Name | Description |
| ------------ | ----------- | 
| Joint Shift Angle | The joint shift angle property specifies the min/max angle where the model will lerp from using the normal rotation ratio for the joint to the shifted rotation ratio.Below the min vale, the normal rotation ratio setting will be used, above the max value, the shifted rotation ratio will be used. The Shifted rotation ratio is intended to be used when the controller is facing backwards to the normal orientation, or towards the user. |
| Joint Shift Exponent | Exponent applied to the blend between the rotation ratio, and the shited rotation ratio. |
| Rotation Ratio | The rotation ratio section of the swing arm model allows the user to configure how much of the controller rotation is applied to different joints in the simulation. 
| Shifted Rotation Ratio | The shifted rotation ratio section of the swing arm model allows the user to configure how much of the controller rotation is applied to different joints in the simulation when the controller is backwards, or facing the user. |

The table blelow further explains the rotation ratio Inspector Controls:

| Control Name | Description |
| ------------ | ----------- | 
| Shoulder Rotation Ratio | Portion of the controller rotation applied to the shoulder joint. |
| Elbow Rotation Ratio | Portion of the controller rotation applied to the elbow joint. |
| Wrist Rotation Ratio | Portion of the controller rotation applied to the wrist joint. |

The table blelow further explains the shifted rotation ratio Inspector Controls:

| Control Name | Description |
| ------------ | ----------- | 
| Shifted Shoulder Rotation Ratio | Portion of the controller rotation applied to the shoulder joint when the controller is backwards. |
| Shifted Elbow Rotation Ratio | Portion of the controller rotation applied to the elbow joint when the controller is backwards. |
| Shifted Wrist Rotation Ratio | Portion of the controller rotation applied to the wrist joint when the controller is backwards. |


# Transition Arm Model

The transition arm model is used to transition between arm models at runtime.

Changing the arm model at runtime will cause the newly requested arm model to be transitioned in using the angular acceleration of the transition Pose Source to control the blend rate.

The image below shows the transition arm model component.

![Transition Arm Model](images/ArmModelImages/Transitionarmmodel.png)

## Current Arm Model Component

This field contains the current active arm model that will be used as the input to the tracked pose driver which is using the Transition arm model.

The current arm model field is used during edit mode to indicate which arm model will be applied when entering play mode. Setting the current arm model field when in play mode will directly set the current arm model being applied, but will not override any arm model transitioning that is currently occuring. 

## Transitions

The Transitions field is a list of key/arm model pairs. The _Queue_ function allows the passing of an arm model, or a string. If the string matches any of the transitions listed in the Transitions list, that arm model will begin transitioning in.

### Key

A string key which will be used to identify the arm model to transition to.

### Arm Model

The arm model that will be transitioned into if the transition arm model is queued using a string Key value.

## Using the Transition Arm Model

The transition Arm Model, like the other arm models, is intended to be used as a pose provider plugin attached to a tracked pose driver. 

The transition arm model is intended to allow the user to transition between two, or more, arm models. When the application wishes to begin transitioning, the arm model that the application wishes to transition to is set via the _Queue_ function.

The transition arm model will then transition from the currently selected arm mode, to the arm model that has been queued.

The transitioning between arm models is driven by the angular velocity of the transition pose source XR node. The more angular velocity expressed by this controller, the faster the transition arm model will transition towards to the queued arm model.

For Example:
- Application starts with the _current arm model_ field referencing a simple pointing arm model
- The user interacts with a ball, and the application scripts requests that the Transition Arm Model queue a swing arm model to allow the user to throw the ball.
- The user then throws the ball, as the ball is released, the application scripts request that the transtional arm model queue the original starting pointing arm model.

 The following image shows how the transition arm model would be configured to work with a number of arm models, and our example script found below:

![Example Configuration For Transition Arm Model Usage](images/ArmModelImages/ExampleTransitionArmModelSetup.png)


Here, the tracked pose driver is set to be driven by the transition arm model on this game object. 
The transition arm model has been configured to have a "Current" arm model that will be applied at startup. This is the same as the pointer arm model configured in the Transitions.

The transitions section has two elements configured. One being the Swing Arm Model for swinging behaviours, and the other is the Pointing Arm model used for simple pointing.

Our example monobehaviour has a reference to the transition arm model that it will control, as well as the names of the Arm Models that it will transition between.

The code below shows the implementation of the example transition arm model monobehaviour

```csharp
public class ExampleTransitionArmModel : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.XR.LegacyInputHelpers.TransitionArmModel transitionArmModel;

    [SerializeField]
    public string swingArmModelName = "SwingArmModel";

    [SerializeField]
    public string pointerArmModelName = "PointerArmModel";

    float timeToNextButtonPress = 0.0f;
    int currentArmModel = 0;

    // Update is called once per frame
    void Update()
    {
        // this uses the Right Trigger on the controller. to seed the input asset with this action, please
        // consult the XR Input Seeding documentation
        if (timeToNextButtonPress <= 0.0f && Input.GetButton("XRI_Right_TriggerButton"))
        {
            if(currentArmModel == 0)
            {
                transitionArmModel.Queue(swingArmModelName);
            }
            else
            {
                transitionArmModel.Queue(pointerArmModelName);
            }
            // flip which arm we're using
            currentArmModel = currentArmModel == 0 ? 1 : 0;
            timeToNextButtonPress = 1.0f; // wait a second before allowing another arm model to be queued
        }
        else
        {
            timeToNextButtonPress -= Time.deltaTime;
        }
    }
}

```