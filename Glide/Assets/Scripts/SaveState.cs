//using UnityEngine;
//using System.Collections;
public class SaveState
{
    public int gold = 0;
    public int completedLevel = 0;

    public int colorOwned = 0;
    public int trailOwned = 0;

    public int activeColor = 0;
    public int activeTrail = 0;

    public bool usingAccelerometer = true;

}
//public struct SafeFloat
//{
//    private float offset;
//    private float value;
//    public SafeFloat(float value = 0)
//    {
//        offset = Random.Range(-1000, +1000);
//        this.value = value + offset;
//    }
//    public float GetValue() //возвращает реальное значение переменной
//    {
//        return value - offset;
//    }
//    public void Dispose()
//    {
//        offset = 0;
//        value = 0;
//    }
//    public override string ToString() //возращает реальное значение переведенное в строковой тип
//    {
//        return GetValue().ToString();
//    }
//    public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
//    {
//        return new SafeFloat(f1.GetValue() + f2.GetValue());
//    }
//    public static SafeFloat operator -(SafeFloat f1, SafeFloat f2)
//    {
//        return new SafeFloat(f1.GetValue() - f2.GetValue());
//    }
//    public static SafeFloat operator *(SafeFloat f1, SafeFloat f2)
//    {
//        return new SafeFloat(f1.GetValue() * f2.GetValue());
//    }
//    public static SafeFloat operator /(SafeFloat f1, SafeFloat f2)
//    {
//        return new SafeFloat(f1.GetValue() / f2.GetValue());
//    }
//    public static SafeFloat operator -(SafeFloat f1, int f2)
//    {
//        return new SafeFloat(f1.GetValue() - f2);
//    }
//    public static SafeFloat operator ++(SafeFloat f1)
//    {
//        return new SafeFloat(f1.GetValue() + 1);
//    }

//    public static implicit operator SafeFloat(float v)
//    {
//        return new SafeFloat { value = v };
//    }
//}

