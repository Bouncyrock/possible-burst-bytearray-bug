# Possible NativeArray\<byte\> bug

Writing to a subarray of a NativeArray\<byte\> in certain positions can fail.

## Approach

We take a subarray of length 1 from a NativeArray\<byte\>, pass that subarray to a job and try to write the position zero.

## Result

> IndexOutOfRangeException: Index 0 is out of range of '0' Length.

```
IndexOutOfRangeException: Index 0 is out of range of '0' Length.
Unity.Collections.NativeArray`1[T].FailOutOfRangeError (System.Int32 index) (at <e414e10bfe5f45729ff122f3359de21b>:0)
Unity.Collections.NativeArray`1[T].CheckElementWriteAccess (System.Int32 index) (at <e414e10bfe5f45729ff122f3359de21b>:0)
Unity.Collections.NativeArray`1[T].set_Item (System.Int32 index, T value) (at <e414e10bfe5f45729ff122f3359de21b>:0)
LengthTest+Foo.Execute () (at Assets/LengthTest.cs:60)
Unity.Jobs.IJobExtensions+JobStruct`1[T].Execute (T& data, System.IntPtr additionalPtr, System.IntPtr bufferRangePatchData, Unity.Jobs.LowLevel.Unsafe.JobRanges& ranges, System.Int32 jobIndex) (at <e414e10bfe5f45729ff122f3359de21b>:0)
Unity.Jobs.LowLevel.Unsafe.JobsUtility:Schedule_Injected(JobScheduleParameters&, JobHandle&)
Unity.Jobs.LowLevel.Unsafe.JobsUtility:Schedule(JobScheduleParameters&)
Unity.Jobs.IJobExtensions:Run(Foo)
LengthTest:Update() (at Assets/LengthTest.cs:29)
```

## Versions and Parameters Affected

Confirmed the bug in Burst version 1.4.1, 1.4.3, and 1.5.0-pre.3

Tested with burst compilation on and off. Same issue both times.

## Notes

It only affects certain positions. My guess was that it would be around multiples of four as my original issue occured with `GetSubArray(59, 1);` however this doesnt seem to be the case.
It doesn't occur on `GetSubArray(1, 1);` or `GetSubArray(2, 1);`, but it does occur on `GetSubArray(5, 1);`.

Best of luck with this one!
