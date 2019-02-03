using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityStandardAssets.Water;

public class WorldController : MonoBehaviour 
{
    public Gradient TreesGradient;
    public List<Material> TreesMaterials;
    public Gradient WaterGradient;
    public Gradient WaterBaseGradient;
    public Material Water;
	public TOD_Time Time;
    public TOD_Sky Sky;
    public TOD_WeatherManager Weather;
    public WaterBase WaterController;
    public GameManager GameManager;
	public float DayLength;
	public float NightLength;
    public GameObject RainLichtingAndSound;
    public Vector2 RainDelay;
    public Vector2 RainTime;
    public GameObject Grass;

    public bool IsRain { get; private set; }

    void Start () 
    {
        if (RainLichtingAndSound != null)
            RainLichtingAndSound.SetActive(false);
        StartCoroutine(DelayInit());
        StartCoroutine(Rain());
    }

    private IEnumerator DelayInit()
    {
        yield return new WaitForSeconds(2.0f);
        Time.DayLengthInMinutes = TOD_Sky.Instance.IsDay ? DayLength : NightLength;
        Time.OnSunset += SunSet;
        Time.OnSunrise += SunRise;
    }

    private IEnumerator Rain()
    {
        while (true)
        {
            var delay = Random.Range((int)RainDelay.x, (int)RainDelay.y);

            yield return new WaitForSeconds(delay);
            Weather.Atmosphere = TOD_WeatherManager.AtmosphereType.Storm;
            yield return new WaitForSeconds(10.0f);
            GameManager.Player.Rain.SetActive(true);
            if (RainLichtingAndSound != null)
                RainLichtingAndSound.SetActive(true);

            IsRain = true;

            yield return new WaitForSeconds(Random.Range((int)RainTime.x, (int)RainTime.y));
            Weather.Atmosphere = TOD_WeatherManager.AtmosphereType.Clear;
            GameManager.Player.Rain.SetActive(false);
            if (RainLichtingAndSound != null)
                RainLichtingAndSound.SetActive(false);

            IsRain = false;
        }
    }

	private void SunSet()
	{
		Time.DayLengthInMinutes = NightLength;
	}

	private void SunRise()
	{
		Time.DayLengthInMinutes = DayLength;
        GameCenterManager.ReportScore(GameCenterManager.RatingAmountLivedDays, 1);
        GooglePlayServicesController.ReportScore(GPGSIds.leaderboard_amount_lived_days, 1);
	}

	void Update ()
    {
        var curTimePercent = TOD_Sky.Instance.Cycle.Hour / 24.0f;

        foreach (var material in TreesMaterials)
        {
            if (material != null)
                material.SetColor("_Color", TreesGradient.Evaluate(curTimePercent));
        }

        if (Water != null)
        {
            Water.SetColor("_ReflectionColor", WaterGradient.Evaluate(curTimePercent));
            Water.SetColor("_BaseColor", WaterBaseGradient.Evaluate(curTimePercent));
        }
	}

	void OnDestroy()
	{
		Time.OnSunset -= SunSet;
		Time.OnSunrise -= SunRise;
	}
}
