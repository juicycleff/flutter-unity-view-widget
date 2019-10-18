# Custom equality comparers

To enable easier verification of custom Unity type values in your tests we provide you with some custom equality comparers:

* [ColorEqualityComparer](./reference-comparer-color.md)
* [FloatEqualityComparer](./reference-comparer-float.md)
* [QuaternionEqualityComparer](./reference-comparer-quaternion.md)
* [Vector2EqualityComparer](./reference-comparer-vector2.md)
* [Vector3EqualityComparer](./reference-comparer-vector3.md)
* [Vector4EqualityComparer](./reference-comparer-vector4.md)

Use these classes to compare two objects of the same type for equality within the range of a given tolerance using [NUnit ](https://github.com/nunit/docs/wiki/Constraints)or [custom constraints](./reference-custom-constraints.md) . Call Instance to apply the default calculation error value to the comparison. To set a specific error value, instantiate a new comparer object using a one argument constructor `ctor(float error)`.

## Static properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `Instance` | A singleton instance of the comparer with a predefined default error value. |

## Constructors

| Syntax              | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `ctor(float error)` | Creates an instance of comparer with a custom error `value.allowedError`. The relative error to be considered while comparing two values. |

## Public methods

| Syntax                               | Description                                                  |
| ------------------------------------ | ------------------------------------------------------------ |
| `bool Equals(T expected, T actual);` | Compares the actual and expected objects for equality using a custom comparison mechanism. Returns `true` if expected and actual objects are equal, otherwise it returns `false`. |

 
