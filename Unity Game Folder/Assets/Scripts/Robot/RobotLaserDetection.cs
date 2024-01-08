using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotLaserDetection : MonoBehaviour
{
    #region fields
    [SerializeField]
    private float speed;
    [SerializeField]
    [Range(0.0f, 90.0f)]
    private float degreesToRotate;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float range;
    private bool reached;
    [SerializeField]
    private LayerMask playerMask;

    [SerializeField]
    private GameObject laser, alertLight;
    [SerializeField]
    private Material greenLight, redLight;
    private Material metal;

    private RaycastHit hit;
    private bool detected;
    #endregion

    #region methods
    private void OnValidate()
    {
        GetComponent<LineRenderer>().SetPosition(1, new Vector3(0.0f, range / 1000.0f, 0.0f));
    }

    private void Start()
    {
        metal = alertLight.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        // Laser movement
        float max = 0.00028f;
        float min = -0.00028f;
        if (!reached)
        {
            transform.localPosition -= new Vector3(speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.x <= min)
                reached = true;
        }
        else if (reached)
        {
            transform.localPosition += new Vector3(speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.x >= max)
                reached = false;
        }

        // Laser rotation
        float normalised = (transform.localPosition.x - 0.0f) / (max - 0.0f);
        transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -normalised * degreesToRotate));

        // Laser detection
        if (Physics.Raycast(transform.position, transform.up, out hit, range, playerMask))
        {
            if (hit.transform.tag == "Player")
            {
                if (!detected)
                {
                    if (transform.root.GetComponent<RobotAgent>().FryingPan)
                    {
                        transform.root.GetChild(0).GetComponent<Animator>().SetBool("Defend Back", true);
                        transform.root.GetChild(0).GetComponent<Animator>().SetBool("Defend Front", false);
                    }

                    ChangeRed();
                    GetComponent<AudioSource>().Play();

                    detected = true;
                }
                else
                {
                    StopAllCoroutines();
                }
            }
        }
        else if (detected)
        {
            StartCoroutine(ResetDetected());
        }
    }

    IEnumerator ResetDetected()
    {
        yield return new WaitForSeconds(3.0f);

        if (transform.root.GetComponent<RobotAgent>().FryingPan)
        {
            transform.root.GetChild(0).GetComponent<Animator>().SetBool("Defend Back", false);
        }
        ChangeGreen();

        detected = false;
    }

    public void ChangeGreen()
    {
        Material[] newMats = new Material[2];
        newMats[0] = metal;
        newMats[1] = greenLight;
        alertLight.GetComponent<Renderer>().materials = newMats;
    }

    public void ChangeRed()
    {
        Material[] newMats = new Material[2];
        newMats[0] = metal;
        newMats[1] = redLight;
        alertLight.GetComponent<Renderer>().materials = newMats;
    }
    #endregion
}
