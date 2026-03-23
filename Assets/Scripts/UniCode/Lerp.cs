using UnityEngine;

public enum LerpCurve
{
    LINEAR, //!Unimplemented
    EASE_IN,
    EASE_OUT,
    EASE_IN_OUT,
    EASE_IN_EXPO,
    EASE_OUT_EXPO,
    SPIKE,
}

public class Lerp
{
    public static float Flip(float x) => 1 - x;

    //Start slow, speed up.
    public static float EaseIn(float t, float pow = 2) => 
        Mathf.Pow(t, pow);

    //Start fast, slow down.
    public static float EaseOut(float t, float pow = 2) => 
        1f - Mathf.Pow((1f - t), pow);

    //Start slow, speed up, end slow.
    public static float EaseInOut(float t, float pow = 2) => 
        (t < 0.5f)? 2f * Mathf.Pow(t, pow) : 1f - Mathf.Pow((-2f * t +2f), pow) / 2f;

    //Start fast snap.
    public static float EaseInExpo(float t) => 
        (t == 0f)? 0f : Mathf.Pow(2f, 10f * t - 10f);

    //End fast snap.
    public static float EaseOutExpo(float t) => 
        (t == 1f)? 1f : 1f - Mathf.Pow(2f, -10f * t);

    //Fast snap in middle.
    public static float Spike(float t, float pow = 2)
    {
        if (t <= 0.5f) return EaseIn(t / 0.5f, pow);

        return EaseIn(Flip(t) / 0.5f, pow);
    }

}
