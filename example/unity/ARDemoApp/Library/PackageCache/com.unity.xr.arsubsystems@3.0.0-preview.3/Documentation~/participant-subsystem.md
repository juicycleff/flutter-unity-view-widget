# XR Participant Subsystem

This subsystem provides information about other users in a multi-user collaborative session. It is a type of [tracking subsystem](index.html#tracking-subsystems) and follows the same `GetChanges` pattern to inform the user about changes to the state of tracked participants. The trackable for the participant subsystem is the [`XRParticipant`](../api/UnityEngine.XR.ARSubsystems.XRParticipant.html).

## Use

The participant subsystem surfaces information about participants. Like many other tracking subsystems, you cannot create or destroy participants -- they are detected automatically much like planes or images.
