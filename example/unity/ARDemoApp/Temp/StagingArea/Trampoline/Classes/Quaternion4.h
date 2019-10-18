#pragma once

struct Quaternion4f
{
    float x, y, z, w;
};

static Quaternion4f gQuatRot[4] =
{   // { x*sin(theta/2), y*sin(theta/2), z*sin(theta/2), cos(theta/2) }
    // => { 0, 0, sin(theta/2), cos(theta/2) } (since <vec> = { 0, 0, +/-1})
    { 0.f, 0.f, 0.f /*sin(0)*/, 1.f /*cos(0)*/},    // ROTATION_0, theta = 0 rad
    { 0.f, 0.f, (float)sqrt(2) * 0.5f /*sin(pi/4)*/, -(float)sqrt(2) * 0.5f /*cos(pi/4)*/}, // ROTATION_90, theta = pi/4 rad
    { 0.f, 0.f, 1.f /*sin(pi/2)*/, 0.f /*cos(pi/2)*/},  // ROTATION_180, theta = pi rad
    { 0.f, 0.f, -(float)sqrt(2) * 0.5f /*sin(3pi/4)*/, -(float)sqrt(2) * 0.5f /*cos(3pi/4)*/}    // ROTATION_270, theta = 3pi/2 rad
};

inline void QuatMultiply(Quaternion4f& result, const Quaternion4f& lhs, const Quaternion4f& rhs)
{
    result.x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y;
    result.y = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z;
    result.z = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x;
    result.w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z;
}

inline Quaternion4f QuatMultiply(const Quaternion4f& lhs, const Quaternion4f& rhs)
{
    Quaternion4f output;
    QuatMultiply(output, lhs, rhs);
    return output;
}

inline Quaternion4f QuatMake(float x, float y, float z, float w)
{
    Quaternion4f q = {x, y, z, w};
    return q;
}

inline Quaternion4f QuatIdentity()
{
    return gQuatRot[0];
}

inline Quaternion4f QuatScale(const Quaternion4f& q, float s)
{
    return QuatMake(s * q.x, s * q.y, s * q.z, s * q.w);
}

inline float QuatNormSquared(const Quaternion4f& q)
{
    return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
}

inline Quaternion4f QuatConjugate(const Quaternion4f& q)
{
    return QuatMake(-q.x, -q.y, -q.z, q.w);
}

inline Quaternion4f QuatInverse(const Quaternion4f& q)
{
    return QuatScale(QuatConjugate(q), 1.0f / QuatNormSquared(q));
}

inline Vector3f QuatToEuler(const Quaternion4f& q)
{
    return VecMake(
        atan2f(2.0f * (q.w * q.y + q.x * q.z),
            1.0f - 2.0f * (q.y * q.y + q.x * q.x)),
        asinf(2.0f * (q.w * q.x - q.z * q.y)),
        atan2f(2.0f * (q.w * q.z + q.y * q.x),
            1.0f - 2.0f * (q.x * q.x + q.z * q.z)));
}

inline float QuatNorm(const Quaternion4f& q)
{
    return sqrtf(QuatNormSquared(q));
}

inline Quaternion4f QuatNormalize(const Quaternion4f& q)
{
    return QuatScale(q, 1.0f / QuatNorm(q));
}

inline Quaternion4f QuatDifference(const Quaternion4f& a, const Quaternion4f& b)
{
    return QuatMultiply(QuatInverse(b), a);
}

inline Quaternion4f QuatRotationFromTo(const Vector3f& src, const Vector3f& dest)
{
    // Based on Stan Melax's article in Game Programming Gems
    float mag0 = VecMagnitude(src);
    if (mag0 < FLT_EPSILON)
        return QuatIdentity();

    float mag1 = VecMagnitude(dest);
    if (mag1 < FLT_EPSILON)
        return QuatIdentity();

    Vector3f v0 = VecScale(1.0f / mag0, src);
    Vector3f v1 = VecScale(1.0f / mag1, dest);

    float d = VecDotProduct(v0, v1);

    // If dot == 1, vectors are the same
    if (d >= (1.0f - 1e-6f))
        return QuatIdentity();

    if (d < (1e-6f - 1.0f))
        return gQuatRot[2];

    float s = sqrtf((1.0f + d) * 2.0f);
    float i = 1.0f / s;

    Vector3f c = VecCrossProduct(v0, v1);

    return QuatNormalize(QuatMake(
        c.x * i, c.y * i, c.z * i, s * 0.5f));
}
