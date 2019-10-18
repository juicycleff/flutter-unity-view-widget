# AR Foundation

Use the AR Foundation package to add high-level functionality for working with augmented reality. Unity 2018.1 includes built-in multi-platform support for AR. These APIs are in the `UnityEngine.XR.ARSubsystems` namespace, and consist of a number of `Subsystem`s, e.g., `XRPlaneSubsystem`. Several XR Subsystems comprise the low-level API for interacting with AR. The **AR Foundation** package wraps this low-level API into a cohesive whole and enhances it with additional utilities, such as AR session lifecycle management and the creation of `GameObject`s to represent detected features in the environment.

## Branch Guidelines
* `master` ==> Target this branch for changes that need to be in the most recent preview package. Then cherry-pick changes from this branch into older, non-staging branches. `master` will eventually branch to `2.1.x` once work on a newer release begins. 

* `2.0.x` - Target this branch for changes that should be in the 2.0.x package versions.

* `2.0.x-staging` - Only merge to this branch from `2.0.x` in preparation for a release. This package should ideally contain the exact contents of the latest release on [staging Bintray](https://bintray.com/unity/unity-staging/com.unity.xr.arfoundation). So before a new release QA would validate against `2.0.x` and then a merge would take place into `2.0.x-staging`.

* `1.1.x` - Target this branch for changes that should be in the 1.1.x package versions.

* `1.1.x-staging` - Only merge to this branch from `1.1.x` in preparation for a release. This package should ideally contain the exact contents of the latest release on [staging Bintray](https://bintray.com/unity/unity-staging/com.unity.xr.arfoundation). So before a new release QA would validate against `1.1.x` and then a merge would take place into `1.1.x-staging`.

* `1.0.x` - Target this branch for changes that should be in the 1.0.x package versions.

* `1.0.x-staging` - Only merge to this branch from `1.0.x` in preparation for a release. This package should ideally contain the exact contents of the latest release on [staging Bintray](https://bintray.com/unity/unity-staging/com.unity.xr.arfoundation). So before a new release QA would validate against `1.0.x` and then a merge would take place into `1.0.x-staging`.
