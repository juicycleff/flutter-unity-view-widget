# Quality Report
`ARSubsystems` is a support package (does not appear in the package manager UI) that cannot
be manually tested in isolation. The following QA report describes the end-to-end testing done
for `ARCore XR Plugin` which has a dependency `ARSubsystems`.

## Version tested: [*ARSubsystems 2.0.0*]

## QA Owner: [*Dianna Ireland*]
## UX Owner: [*Sean Low*]

## Test strategy
* [ARFoundation testrail](https://qatestrail.hq.unity3d.com/index.php?/suites/view/2755&group_by=cases:section_id&group_order=asc)
* [Tests compelted in Google Doc](https://docs.google.com/document/d/1kblEP6o9gpZ3b5nk7HpZ55G3aJSGzyJhETf_tJec36Q/edit)
* [Jenkins Automation](http://xrtest.hq.unity3d.com:8080/job/Test_Jobs/job/2019.1/job/2019.1-Win2-Run_All_XR_Functional_Tests/) - Only confirms activating ARCore does not result in failures.

## Package Status
Use this section to describe:
* Test Suite scenes confirming basical functionality.
  * ✅ Simple AR - Scene rendered asset tracked accurately on plane.
  * ✅ Motion Tracking - Tracks position trail as expected.
  * ✅ Camera to Meshed background switching - Camera switches between rendered background and camera background, keeping assets accurately in place.
  * ✅ Plane Detection - Able to track vertical and horizonal planes.
  * ✅ ARParticles - Particles fall from correct position and orient correctly when rotating camera.
  * ✅ Navigational Meshing - User is able to trace planes and use simple plane asset to recoginize where it is able to travel.
  * ✅ Reference Point Testing - User can place Pose and Plane reference points on planes.
  * ✅ AR Video Player - Video is rendered correctly inside of a plane created by user.

ARFoundation with ARsubsystems is stable and all test scenes worked as expected.
