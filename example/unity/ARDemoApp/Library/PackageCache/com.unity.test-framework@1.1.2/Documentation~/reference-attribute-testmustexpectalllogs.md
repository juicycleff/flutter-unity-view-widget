# TestMustExpectAllLogs attribute

The presence of this attribute causes the **Test Runner** to expect every single log. By default, the Test Runner only fails on error logs, but `TestMustExpectAllLogs` fails on warnings and info level messages as well. It is the same as calling the method [LogAssert.NoUnexpectedReceived](./reference-custom-assertion.md#static-methods) at the bottom of every affected test.

## Assembly-wide usage

You can apply this attribute to test assemblies (that affects every test in the assembly), fixtures (affects every test in the fixture), or on individual test methods. It is also inherited from base fixtures.

The `MustExpect` property (`true` by default) lets you enable or disable the higher level value. 

For example when migrating an assembly to this more strict checking method, you might attach `[assembly:TestMustExpectAllLogs]` to the assembly itself, but then whitelist failing fixtures and test methods with `[TestMustExpectAllLogs(MustExpect=false)]` until you have migrated them. This also means new tests in that assembly would have the more strict checking.