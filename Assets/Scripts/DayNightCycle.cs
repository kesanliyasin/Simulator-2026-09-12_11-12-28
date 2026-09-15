using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Zaman Ayarlari")]
    [Tooltip("Oyun icindeki bir saatin kac gercek saniye surececegi.")]
    public float realSecondsPerGameHour = 10f;
    public float startHour = 8f;
    public float dayStartHour = 6f;
    public float nightStartHour = 20f;

    [Header("Isik Ayarlari")]
    public Light mainLight;
    public Color dayLightColor = Color.white;
    public Color nightLightColor = new Color(0.2f, 0.3f, 0.6f);
    public float dayLightIntensity = 1.2f;
    public float nightLightIntensity = 0.2f;
    public float transitionDurationHours = 2f;

    [Header("Test ve Arayuz")]
    public bool showDebugClock = true;

    public float CurrentHour { get; private set; }
    public bool IsNight { get; private set; }

    private void Start()
    {
        CurrentHour = Mathf.Repeat(startHour, 24f);
        UpdateNightState();
        UpdateLighting();

        if (mainLight == null)
        {
            Debug.LogWarning("DayNightCycle: Main Light atanmamis.");
        }
    }

    private void Update()
    {
        if (realSecondsPerGameHour <= 0f)
        {
            Debug.LogWarning("DayNightCycle: realSecondsPerGameHour 0'dan buyuk olmali.");
            return;
        }

        CurrentHour = Mathf.Repeat(
            CurrentHour + Time.deltaTime / realSecondsPerGameHour,
            24f);

        UpdateNightState();
        UpdateLighting();
    }

    private void UpdateNightState()
    {
        bool newNightState = CurrentHour >= nightStartHour || CurrentHour < dayStartHour;

        if (newNightState != IsNight)
        {
            IsNight = newNightState;
            Debug.Log(IsNight ? "Gece basladi." : "Gunduz basladi.");
        }
    }

    private void UpdateLighting()
    {
        if (mainLight == null) return;

        float daylight = GetDaylightFactor();
        mainLight.color = Color.Lerp(nightLightColor, dayLightColor, daylight);
        mainLight.intensity = Mathf.Lerp(nightLightIntensity, dayLightIntensity, daylight);

        RenderSettings.ambientLight = Color.Lerp(
            nightLightColor * 0.5f,
            Color.white,
            daylight);
    }

    private float GetDaylightFactor()
    {
        if (transitionDurationHours <= 0f)
        {
            return IsNight ? 0f : 1f;
        }

        float sunriseEnd = dayStartHour + transitionDurationHours;
        float sunsetStart = nightStartHour - transitionDurationHours;

        if (CurrentHour >= dayStartHour && CurrentHour < sunriseEnd)
        {
            float progress = Mathf.InverseLerp(dayStartHour, sunriseEnd, CurrentHour);
            return Mathf.SmoothStep(0f, 1f, progress);
        }

        if (CurrentHour >= sunsetStart && CurrentHour < nightStartHour)
        {
            float progress = Mathf.InverseLerp(sunsetStart, nightStartHour, CurrentHour);
            return Mathf.SmoothStep(1f, 0f, progress);
        }

        return IsNight ? 0f : 1f;
    }

    private void OnGUI()
    {
        if (!showDebugClock) return;

        int hour = Mathf.FloorToInt(CurrentHour);
        int minute = Mathf.FloorToInt((CurrentHour - hour) * 60f);
        string timeText = string.Format("{0:00}:{1:00}", hour, minute);
        string stateText = IsNight ? "Gece" : "Gunduz";

        GUI.Label(new Rect(20f, 20f, 220f, 30f), "Saat: " + timeText);
        GUI.Label(new Rect(20f, 45f, 220f, 30f), "Durum: " + stateText);
    }
}
