using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BombPhone : Interactable
{
    enum CodeType
    {
        Number,
        Delete,
        Other
    }

    enum LightColor
    {
        Off,
        Red,
        Green
    }

    #region fields
    private string value;
    [SerializeField]
    private CodeType codeType;
    [SerializeField]
    private TextMeshPro codeText;
    [SerializeField]
    private BombTimer timer;
    [SerializeField]
    private AudioSource wrongSFX, correctSFX;
    [SerializeField]
    private GameObject lightBulb;
    [SerializeField]
    private Material red, green, off;
    [SerializeField]
    private RobotAgent robot;
    [SerializeField]
    NukeButton nukeButton;

    [SerializeField]
    GameObject ladderLight;

    [SerializeField]
    EndLadder endLadder;
    [SerializeField]
    GameObject bombLight;
    #endregion

    #region methods
    private void Awake()
    {
        value = transform.name;
    }

    public override void Action()
    {
        if (timer == null)
            Destroy(this);
        // Input
        switch (codeType)
        {
            case CodeType.Number:
                if (codeText.text.Length < 8)
                {
                    codeText.text = codeText.GetComponent<TextMeshPro>().text + value.ToString() + " ";
                    StartCoroutine(Click());
                    GetComponent<AudioSource>().Play();
                }
                break;
            case CodeType.Delete:
                if (codeText.text.Length >= 2)
                {
                    codeText.text = codeText.text.Substring(0, codeText.text.Length - 2);
                    StartCoroutine(Click());
                    GetComponent<AudioSource>().Play();
                }
                break;
            case CodeType.Other:
                StartCoroutine(Click());
                GetComponent<AudioSource>().Play();
                break;
        }
        // Code complete
        if (codeText.text.Length >= 8)
        {
            if (robot.Dead)
            {
                int num1 = int.Parse(codeText.text.ToString()[0].ToString());
                int num2 = int.Parse(codeText.text.ToString()[2].ToString());
                int num3 = int.Parse(codeText.text.ToString()[4].ToString());
                int num4 = int.Parse(codeText.text.ToString()[6].ToString());
                int[] code = new int[4] { num1, num2, num3, num4 };
                // Check numbers
                for (int i = 0; i < code.Length; i++)
                {
                    // Incorrect
                    if (code[i] != robot.Code[i])
                    {
                        ReduceTime();
                        StartCoroutine(ResetCode());
                        return;
                    }
                }
                // Correct
                StartCoroutine(ChangeLight(LightColor.Green));
                timer.StopTimer();
                correctSFX.Play();
                GameManager.Instance.StopMusic();
                GameManager.Instance.taskManager.UpdateTaskCompletion("Defuse Bomb");
                ladderLight.SetActive(true);
                endLadder.interactable = true;
                bombLight.SetActive(false);
                nukeButton.DisableLights();
                Destroy(this);
            }
            else
            {
                ReduceTime();
                StartCoroutine(ResetCode());
            }
        }
    }

    private void ReduceTime()
    {
        timer.CurrentTime -= 30.0f;
        if (timer.CurrentTime < 0.0f)
            timer.CurrentTime = 0.0f;
    }

    IEnumerator ResetCode()
    {
        codeText.text = "* * * * ";
        yield return new WaitForSeconds(0.25f);
        codeText.text = "";
        wrongSFX.Play();
        StartCoroutine(ChangeLight(LightColor.Red));
    }

    IEnumerator Click()
    {
        float startingY = transform.localPosition.y;
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, -0.02f, transform.localPosition.z), 50.0f * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, startingY, transform.localPosition.z), 50.0f * Time.deltaTime);
    }

    IEnumerator ChangeLight(LightColor color)
    {
        switch (color)
        {
            case LightColor.Green:
                lightBulb.GetComponent<Renderer>().material = green;
                break;
            case LightColor.Red:
                lightBulb.GetComponent<Renderer>().material = red;
                break;
            case LightColor.Off:
                lightBulb.GetComponent<Renderer>().material = off;
                break;
        }

        yield return new WaitForSeconds(1.0f);

        lightBulb.GetComponent<Renderer>().material = off;
    }
    #endregion
}
