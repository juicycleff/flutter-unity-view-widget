# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.2-preview.2] - 2019-08-23
* Change legacy input helpers version to 1.*
* Fix documentation validation errors.
* Allow 3.x to work with Unity 2019.2.
  * This package will not work with 2019.3a1 - a11.

## [3.0.2-preview.1] - 2019-08-06
* Remove asset menu creation entry for XR Settings as it is unsupported now.
* Fix an issue with downloading packages that could allow PackMan toget corrupted, forcing the user to reload Unity.

## [3.0.1] - 2019-07-11
* Update base Unity release version after namespace changes.

## [3.0.0] - 2019-07-09
* Update docs to add more information around correct usage.
* add useful names to sub objects of general settings.
* Add Magic Leap to curated packages list.

## [2.99.0-preview.2] - 2019-06-19
* Pick up 2019.2 preview changes that are applicable to 2019.3.
* Fix up the code after Experimental namespace change.

## [2.99.0-preview.1] - 2019-06-14
* Update package to support 2019.3+ only.

## [2.99.0-preview] - 2019-06-14
* Update package to support 2019.3+ only.
* Rev version to almost 3. This is to make space for 2019.2 preview versions and in acknowledgement of the breaking changes that will happen soon.

## [2.0.0-preview.24] - 2019-6-14
* Tie version to 2019.2 exclusively for preview.
* Strip document revision history.
* Remove third party notice as unneeded.

## [2.0.0-preview.23] - 2019-6-10
* Add promotion pipeline yaml file to get promotion to production working again.

## [2.0.0-preview.22] - 2019-6-11
* Revert Legacy Input Helpers dependency to newly pushed 1.3.2 production version.

## [2.0.0-preview.21] - 2019-6-10
* Downgrade Legacy Input Helpers dependency to correct production version.

## [2.0.0-preview.20] - 2019-6-10
* Downgrade Legacy Input Helpers dependency to help get package to production.

## [2.0.0-preview.19] - 2019-6-4
* Fix package name and description.

## [2.0.0-preview.18] - 2019-6-3
* Minor corrections in samples header file.
* Remove Windows from log message.
* Remove tutorial UI and unsupported data.

## [2.0.0-preview.17] - 2019-5-28
* Fix issue where no settings object would cause an error to be logged at build time incorrectly.
* Add helper method to get XRGeneralSettings instance for a specific build target.

## [2.0.0-preview.16] - 2019-5-28
* Move PR template to correct location.

## [2.0.0-preview.15] - 2019-5-23
* Fix the readme help page to only appear once on initial add of package.
* Fix up test namespaces to use correct namespace naming

## [2.0.0-preview.14] - 2019-5-23
* updating number for yamato, adds depednency to com.unity.xr.legacyinputhelpers

## [2.0.0-preview.13] - 2019-5-09
* Fix more output logging for Yamato.

## [2.0.0-preview.12] - 2019-5-09
* Add support for Yamato
* Fix unit tests broken with streamlined workflow changes.

## [2.0.0-preview.10] - 2019-4-19
* Add ability for users to disable auto initialize at start. This should allow for hybrid applications that want to start in non-XR mode and manually switch.
* Fix play mode initialization so that we can guarantee that XR has been initialized (or at least attempted initialization) by the time the Start method is called on MonoBehaviours.
* Documentation updated to cover the above.
* Fixed a bug in the new Readme script code that caused a crash in headless mode. Seems the code was launching an Editor window and causing UIElements to crash on an attempt to repaint. We have a workaround to make sure we don't load the window if in headless mode and a bug is filed with the responsible team to correct the crash.

## [2.0.0-preview.9] - 2019-4-10
* Fix package validation console errors.

## [2.0.0-preview.8] - 2019-4-10
* Fix package validation compilation errors.
* Remove .github folder from npm packaging.

## [2.0.0-preview.7] - 2019-4-10
* Streamlining of the management system. Move XR Manager to a singleton instance on XRGeneralSettings that is populated by an XRManagerSettings instance that the user can switch in and out. __NOTE: This removes the ability to use XRManagement for per scene situations. For hybrid or manual scenes the user will be responsible for instantiating/loading the XRManagerSettings instance they want and dealing with lifecycle themselves.__

## [2.0.0-preview.6] - 2019-3-29
* Fix up package repo information for rel mgmt.

## [2.0.0-preview.5] - 2019-2-5
* Split documentation into separate audience files for End Users and Providers.
* Update package target Unity version to Unity 2019.1.

## [2.0.0-preview.4] - 2019-2-5
* Fix an issue with with an NRE in the build processor.

## [2.0.0-preview.3] - 2019-1-22
* Add missing repo url to package json file
* Fix NRE issue in build processor

## [2.0.0-preview.3] - 2019-1-22
* Fix error in general build processor due to a potential null deref.
* Fix missing check for unity version when referencing UIElements.

## [2.0.0-preview.2] - 2018-12-19
* Fix package validation issues.
* Fix bug due to preinit code that would cause a null ref exception.

## [2.0.0-preview.1] - 2018-12-19
* Upated to support loading integrated and standalone subsystems.
* Add support for pre-init framework to allow for setting handling things like LUID setup pre-gfx setup.
* Add ability for general settings to be set per platform and not just globally.
* Tagged with release preview build. This should be the base on which we move to release for 2019.1

## [0.2.0-preview.9] 2018-11-27
* Fixed some issues with boot time and general setting.

## [0.2.0-preview.8] 2018-10-29
* Fix an API breaking change to UnifiedSettings api
* Fix a NRE in XRGeneralSettings if the user has set an XRManager Component on a scene game object and didn't setup general settings.

## [0.2.0-preview.7] 2018-10-29
* Hopefully all CI issues are resolved now.
  
## [0.2.0-preview.4] 2018-10-24
* Merged in gneral settings support. Initial implentation allows for ability to assign an XR Manager instance for loading XR SDK at boot launch time.

## [0.2.0-preview.3] 2018-10-24
* Merged in Unified Settings dependent changes.

## [0.1.0-preview.9] - 2018-07-30
* Add missing .npmignore file

## [0.1.0-preview.8] - 2018-07-30

* Updated UI for XR Manager to allow for adding, removing and reordering loaders. No more need for CreateAssetMenu attributes on loaders.
* Updated code to match formatting and code standards.

## [0.1.0-preview.7] - 2018-07-25

* Fix issue #3: Add ASMDEFs for sample code to get it to compile. No longer need to keep copy in project.
* Fix Issue #4: Update documentation to reflect API changes and to expand information and API documentation.
* Fix Issue #5: Move boilerplate loader code to a common helper base class that can be used if an implementor wants to.

## [0.1.0-preview.6] - 2018-07-17

### Added runtime tests for XRManager

### Updated code to reflect name changes for XR Subsystem types.

## [0.1.0-preview.5] - 2018-07-17

### Simplified settings for build/runtime

Since we are 2018.3 and later only we can take advantage of the new PlayerSettings Preloaded Assets API. This API allows us to stash assets in PLayerSettings that are preloaded at runtime. Now, instead of figuring out where to write a file for which build target we just use the standard Unity engine and code access to get the settings we need when we need them.

## [0.1.0-preview.4] - 2018-07-17

### Added samples and abiity to load settings

This change adds a full fledged sample base that shows how to work with XR Management from start to finish, across run and build. This includes serializing and de-serializing the settings.

## [0.1.0-preview.3] - 2018-07-17


## [0.1.0-preview.2] - 2018-06-22

### Update build settings management

Changed XRBuildData froma class to an attribute. This allows providers to use simpler SO classes for build data and not forece them to subclass anything.
Added a SettingsProvider subclass that wraps each of these attribute tagged classes. We use the display name from the attribute to populate the path in Unified Settings. The key in the attribute is used to store a single instance of the build settings SO in EditorBuildSettings as a single point to manage the instance.
Added code to auto create the first SO settings instance using a file panel since the Editor build settings container requires stored instances be backed in the Asset DB. There is no UI for creating the settings (unless added by the Provider) so this should allow us to maintain the singleton settings. Even if a user duplicates the settings instance, since it won't be in the Editor build settings container we won't honor it.

## [0.1.0-preview.1] - 2018-06-21

### This is the first release of *Unity Package XR SDK Management*.
