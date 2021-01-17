using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

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
        var hmm = _testArray.GetSubArray(59, 1);
        Debug.Log($"Here the length of hmm is {hmm.Length}");
        Foo.Create(hmm).Schedule().Complete();
        Debug.Log("Will never reach here");
    }

    [BurstCompile]
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
            _bytes[0] = 1;
        }
    }
}
