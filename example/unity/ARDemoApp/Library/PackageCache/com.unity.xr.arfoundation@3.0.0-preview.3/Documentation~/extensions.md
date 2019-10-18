# Extending AR Foundation

In many cases, AR Foundation (or rather, each subsystem) wraps some platform-specific SDK, such as ARCore or ARKit. If you know you are on a particular platform, you may want to access specific features of that SDK that are not accessible via AR Foundation.

For many objects, AR Foundation provides a native pointer to some platform-specific data. For instance, the `XRSessionSubsystem` has a `nativePtr` property.

Each provider package defines what each native pointer points to. However, the general pattern is that it points to a struct whose first member is an `int` containing a version number followed by the raw pointer. This allows us to add additional data to the struct in the future.

In C, the `XRSessionSubsystem.nativePtr` might point to a struct like this:

```c
typedef struct UnityXRNativeSessionPtr
{
    int version;
    void* session;
} UnityXRNativeSessionPtr;
```

Note that structure packing and alignment rules vary by platform, so the `void* session` pointer is not necessarily at a 4 byte offset. On a 64-bit platform, for instance, the pointer might be offset by 8 bytes to ensure the pointer is on an 8 byte boundary.

All trackables (e.g., planes, tracked images, faces) provide a native pointer. This lets you access things like the native frame, session, plane, anchor, and so on.
