#pragma once

struct Vector3f
{
    float x, y, z;
};

inline float VecDotProduct(const Vector3f& v1, const Vector3f& v2)
{
    return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
}

inline float VecMagnitude(const Vector3f& v)
{
    return sqrtf(VecDotProduct(v, v));
}

inline Vector3f VecMake(float x, float y, float z)
{
    Vector3f v = {x, y, z};
    return v;
}

inline Vector3f VecCrossProduct(const Vector3f& v1, const Vector3f& v2)
{
    return VecMake(
        v1.y * v2.z - v1.z * v2.y,
        v1.z * v2.x - v1.x * v2.z,
        v1.x * v2.y - v1.y * v2.x);
}

inline Vector3f VecScale(float s, const Vector3f& v)
{
    return VecMake(s * v.x, s * v.y, s * v.z);
}

inline Vector3f VecNormalize(const Vector3f& v)
{
    return VecScale(1.0f / VecMagnitude(v), v);
}
