# XR Session Subsystem

A "session" refers to an instance of AR. While the other AR subsystems provide specific pieces of functionality, like plane detection, the session controls the lifecycle of all AR-related subsystems. Thus, if you `Stop` (or fail to `Create`) an `XRSessionSubsystem`, the other AR subsystems may not work.

`Start` and `Stop` will start/resume and pause the session, respectively.

## Determining Availability

On some platforms, AR capabilities are built into the device's operating system. However, on others, AR software may be installable on demand. The question "is AR available on this device" may require checking a remote server for software availability. Therefore,  `XRSessionSubsystem.GetAvailabilityAsync` as a method that returns a `Promise<SessionAvailability>`. `Promise` is a `CustomYieldInstruction`, so it can be used in a coroutine.

Once availability is determined, the device might be unsupported, supported but only with an update or install, or supported and ready.

## Installing Additional AR Software

If `SessionAvailability` is `SessionAvailability.Supported` but not `SessionAvailability.Installed`, you should call `InstallAsync` to install the AR software. This returns another type of `Promise`: `Promise<SessionInstallationStatus>`. If the installation is successful, then it is safe to `Start` the subsystem.
