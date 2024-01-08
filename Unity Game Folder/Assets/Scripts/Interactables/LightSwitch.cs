using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightSwitch : Interactable
{
    #region fields
    [SerializeField]
    private GameObject[] lights;
    [SerializeField]
    private MeshRenderer[] bulbs;
    [SerializeField]
    private Material emissionOn, emissionOff;
    [SerializeField]
    private bool flip = true;
    private bool awake;

    private bool isOff;
    #endregion

    #region methods
    private void Start()
    {
        Action();
        awake = true;
    }

    public override void Action()
    {
        // Switch
        isOff = !isOff;

        // Set lights
        foreach (GameObject light in lights)
            light.SetActive(isOff);
        // Set bulbs
        foreach (MeshRenderer bulb in bulbs)
            bulb.material = (isOff) ? emissionOn : emissionOff;

        // Switch scale
        if (flip)
            transform.parent.localScale = (lights[0].activeSelf) ? new Vector3(1.0f, -1.0f, 1.0f) : Vector3.one;

        // Player audio
        if (awake)
            AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}
