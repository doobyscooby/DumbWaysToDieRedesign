using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicSky : MonoBehaviour
{
    #region fields
    [SerializeField]
    [Range(0.0f, 24.0f)]
    private float timeOfDay;
    private float targetTime;
    [SerializeField]
    private float orbitSpeed, cloudSpeed;
    private float cloudRotation;

    [SerializeField]
    private Light sun, moon;
    [SerializeField]
    private Volume skyVolume;
    private PhysicallyBasedSky sky;
    [SerializeField]
    private AnimationCurve dayNightCurve;
    [SerializeField]
    private Cubemap daySkybox, nightSkybox;

    [SerializeField]
    private bool morning;
    private bool isNight;

    [SerializeField]
    private GameObject[] outsideLights;
    [SerializeField]
    private Material emission;

    public static DynamicSky Instance;
    #endregion

    #region properties

    #endregion

    #region methods
    private void Awake()
    {
        // Get sky
        skyVolume.sharedProfile.TryGet<PhysicallyBasedSky>(out sky);
        // Set starting time
        if (morning)
        {
            timeOfDay = 5.0f;
            targetTime = 7.0f;
        }

        UpdateTime();

        Instance = this;
    }

    private void OnValidate()
    {
        // Get sky
        skyVolume.sharedProfile.TryGet<PhysicallyBasedSky>(out sky);
        // Set starting time
        targetTime = timeOfDay;

        UpdateTime();
    }

    private void Update()
    {
        if (timeOfDay < targetTime)
        {
            timeOfDay += orbitSpeed * Time.deltaTime;
        }

        // Increase time of day
        //timeOfDay += orbitSpeed * Time.deltaTime;
        // Reset time of day
        if (timeOfDay > 24.0f)
            timeOfDay = 0.0f;

        UpdateTime();
    }

    private void UpdateTime()
    {
        // Calculate time of day
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90.0f, 270.0f, alpha);
        float moonRotation = sunRotation - 180.0f;

        // Rotate sun & moon
        sun.transform.rotation = Quaternion.Euler(sunRotation, 270.0f, 0.0f);
        moon.transform.rotation = Quaternion.Euler(moonRotation, 270.0f, 0.0f);

        // Rotate clouds
        cloudRotation += cloudSpeed * Time.deltaTime;
        //sky.spaceRotation.value = new Vector3(0.0f, cloudRotation, 0.0f);

        // Set skybox emission
        sky.spaceEmissionMultiplier.value = dayNightCurve.Evaluate(alpha) * 500.0f;
        // Set light intesity
        sun.transform.GetChild(0).GetComponent<Light>().intensity = dayNightCurve.Evaluate(alpha) * 4000.0f;
        moon.transform.GetChild(0).GetComponent<Light>().intensity = dayNightCurve.Evaluate(alpha) * 50.0f;

        CheckNightDayTransition();
    }

    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else if (!isNight)
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartDay()
    {
        // Set shadows
        //sun.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.Soft;
        //moon.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.None;

        sky.spaceEmissionTexture.value = daySkybox;

        isNight = false;
    }

    private void StartNight()
    {
        // Set shadows
        //sun.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.None;
        //moon.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.Soft;

        sky.spaceEmissionTexture.value = nightSkybox;

        isNight = true;
    }
    public void AdvanceTime()
    {
        timeOfDay = 19.0f;
        UpdateTime();

        foreach(GameObject light in outsideLights)
        {
            light.transform.GetChild(0).GetComponent<MeshRenderer>().material = emission;
            light.transform.GetChild(4).gameObject.SetActive(true);
        }
    }
    #endregion
}
