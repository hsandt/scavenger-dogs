using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public class BinaryCardinalProcessor : InputProcessor<Vector2>
{
    /// <summary>
    /// Value at which the lower bound starts.
    /// </summary>
    /// <remarks>
    /// Values in the input below min, in absolute value, will get dropped, and values
    /// at or above will be replaced with -1 or 1 (keeping the same sign as the original value).
    /// This is useful to convert stick input into D-pad-like input.
    /// </remarks>
    [Tooltip("Test")]
    public float min = 0.125f;
    
#if UNITY_EDITOR
    static BinaryCardinalProcessor()
    {
        Initialize();
    }
#endif
    
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<BinaryCardinalProcessor>();
    }
    
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        value.x = GetBinarizedValue(value.x);
        value.y = GetBinarizedValue(value.y);
        return value;
    }

    private float GetBinarizedValue(float value)
    {
        // Process input so that each coordinate is 0/1 as in old school games with D-pad,
        // even when using gamepad left stick. For digital input, there is nothing to do.
        // For analog input (stick), just ceil any sufficient input coordinate to 1, independently from the other.
        return Mathf.Abs(value) > min ? Mathf.Sign(value) : 0f;
    }
}

