using UnityEngine;

public class VibrationCollision 
{
    /// Вибрация на Android с указанием времени и амплитуды.
    /// <param name="milliseconds">Длительность вибрации в миллисекундах</param>
    /// <param name="amplitude">Амплитуда от 1 до 255 (0 = default)</param>
    public void VibrationMethod(long milliseconds, int amplitude)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (vibrator != null)
            {
                // Проверка версии Android (v26+ для амплитуды)
                using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    int sdkInt = version.GetStatic<int>("SDK_INT");

                    if (sdkInt >= 26)
                    {
                        // Создаем VibrationEffect
                        using (AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                        {
                            AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                                "createOneShot", milliseconds, amplitude
                            );
                            vibrator.Call("vibrate", effect);
                        }
                    }
                    else
                    {
                        // Для старых устройств просто vibrate(milliseconds)
                        vibrator.Call("vibrate", milliseconds);
                    }
                }
            }
        }
#else
        Debug.Log("Vibration: " + milliseconds + "ms, amplitude: " + amplitude);
#endif
    }
}