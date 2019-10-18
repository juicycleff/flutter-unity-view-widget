# ARCore XR SDK Package

The purpose of this package is to provide ARCore XR Support. This package provides the necessary sdk libraries for users to build Applications that work with ARCore on Android.

# Building From Source

:warning: Be sure you have installed [Git Large File Support](https://git-lfs.github.com/) before cloning this repository.

:warning: The first time you build requires an internet connection and you must be on Unity's network (or VPN).

From a terminal window:

1. Clone this repo:
```
git clone git@github.com:Unity-Technologies/upm-xr-arcore.git
```
2. Change directory to `upm-xr-arcore`:
```
cd upm-xr-arcore
```
3. Get the submodules:
```
git submodule update --init --recursive
```
4. Change directory to `Source~`:
```
cd Source~
```
5. Build the source using `bee`.
  - On Windows: ```bee.exe```
  - On Mac: ```mono bee.exe```
