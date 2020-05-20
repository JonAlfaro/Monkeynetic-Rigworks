using UnityEngine;
using UnityEngine.Serialization;

public class EnvriomentController : MonoBehaviour
{
    private static readonly int SunDirection = Shader.PropertyToID("_SunDirection");
    private float _dayHourTick;
    private float _nightHourTick;


    private float _timeTracker;

    [FormerlySerializedAs("ColorDawnHorizon")]
    public Color colorDawnHorizon;

    [FormerlySerializedAs("ColorDawnSky")] public Color colorDawnSky;

    [FormerlySerializedAs("ColorDayHorizon")]
    public Color colorDayHorizon;

    [FormerlySerializedAs("ColorDaySky")] public Color colorDaySky;

    [FormerlySerializedAs("ColorDuskHorizon")]
    public Color colorDuskHorizon;

    [FormerlySerializedAs("ColorDuskSky")] public Color colorDuskSky;

    [FormerlySerializedAs("ColorNightHorizon")]
    public Color colorNightHorizon;

    [FormerlySerializedAs("ColorNightSky")]
    public Color colorNightSky;

    [FormerlySerializedAs("HoursInADay")] public float hoursInADay = 16;

    [FormerlySerializedAs("HoursInANight")]
    public float hoursInANight = 8;

    [FormerlySerializedAs("PlayerCamera")] public Camera playerCamera;

    [FormerlySerializedAs("SecondsInHour")]
    public float secondsInHour = 60;

    [FormerlySerializedAs("StageLight")] public Light stageLight;
    [FormerlySerializedAs("StageTerrain")] public Terrain stageTerrain;
    [Range(1.0f, 24.0f)] public float startTime = 12f;
    [Range(1.0f, 24.0f)] public float sunriseTime = 5f;


    // Start is called before the first frame update
    private void Start()
    {
        const int degreesInDay = 200;
        _timeTracker = startTime;
        _dayHourTick = degreesInDay / hoursInADay / secondsInHour;
        _nightHourTick = (360 - degreesInDay) / hoursInANight / secondsInHour;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        // Skybox Controller
        Transform stageLightTransform;
        _timeTracker += 1 / secondsInHour * Time.fixedDeltaTime;
        if (_timeTracker >= 24) _timeTracker = 0;

        if (_timeTracker < 1)
            Debug.Log((int) (_timeTracker + 12) + ":" + (int) (_timeTracker % 1 * 60) + "AM");
        else if (_timeTracker < 12)
            Debug.Log((int) _timeTracker + ":" + (int) (_timeTracker % 1 * 60) + "AM");
        else if (_timeTracker < 13)
            Debug.Log((int) _timeTracker + ":" + (int) (_timeTracker % 1 * 60) + "PM");
        else
            Debug.Log((int) _timeTracker - 12 + ":" + (int) ((_timeTracker - 12) % 1 * 60) + "PM");

        if (_timeTracker >= sunriseTime && _timeTracker <= sunriseTime + hoursInADay)
        {
            Debug.Log("DAY TICK: " + _dayHourTick);
            (stageLightTransform = stageLight.transform).localRotation *=
                Quaternion.AngleAxis(_dayHourTick * Time.fixedDeltaTime, Vector3.right);
            Shader.SetGlobalVector("_SunDirection", stageLightTransform.forward);
            
            Shader.SetGlobalColor("_ColorSky", colorDaySky);
            Shader.SetGlobalColor("_ColorHorizon", colorDayHorizon);
        }
        else
        {
            Debug.Log("NIGHT TICK: " + _nightHourTick);
            (stageLightTransform = stageLight.transform).localRotation *=
                Quaternion.AngleAxis(_nightHourTick * Time.fixedDeltaTime, Vector3.right);
            Shader.SetGlobalVector("_SunDirection", stageLightTransform.forward);
            Shader.SetGlobalColor("_ColorSky", colorNightSky);
            Shader.SetGlobalColor("_ColorHorizon", colorNightHorizon);
        }

        // Shader.SetGlobalVector("_SkyAngle", new Vector4(GetXDegree(stageLight.transform), 0));
    }

    private float GetXDegree(Transform gObjectTransform)
    {
        var x = gObjectTransform.eulerAngles.x;

        if (Vector3.Dot(gObjectTransform.up, Vector3.up) < 0f)
        {
            if (x >= 0f && x <= 90f)
                x = 180 - x;
            else if (x >= 270f && x <= 360f) x = 540 - x;
        }

        return x;
    }
}