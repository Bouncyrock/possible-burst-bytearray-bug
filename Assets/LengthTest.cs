using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Confirmed the bug in Burst version 1.4.1, 1.4.3, and 1.5.0-pre.3

public class LengthTest : MonoBehaviour
{
    NativeArray<byte> _testArray;
    
    void Start()
    {
        _testArray = new NativeArray<byte>(128, Allocator.Persistent);
        Debug.Log("Running");
    }

    void OnDestroy()
    {
        _testArray.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<byte> hmm;

        hmm = _testArray.GetSubArray(1, 1);
        Foo.Create(hmm).Run();
        Debug.Log("Starting at index 1 works");
        
        hmm = _testArray.GetSubArray(2, 1);
        Foo.Create(hmm).Run();
        Debug.Log("Starting at index 2 works");

        hmm = _testArray.GetSubArray(3, 1);
        Foo.Create(hmm).Run();
        Debug.Log("Starting at index 3 fails");
        
        hmm = _testArray.GetSubArray(0, 1);
        Foo.Create(hmm).Run();
        Debug.Log("Starting at index 0 fails (sometimes)");
    }

    [BurstCompile(CompileSynchronously = true)]
    struct Foo : IJob
    {
        NativeArray<byte> _bytes;
        
        public static Foo Create(NativeArray<byte> bytes)
        {
            return new Foo()
            {
                _bytes = bytes,
            };
        }

        public void Execute()
        {
            _bytes[0] = 1; // will give an IndexOutOfRangeException here 
        }
    }
}
