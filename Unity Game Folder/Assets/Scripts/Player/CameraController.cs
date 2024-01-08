using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region fields
    [Header("Settings")]
    private float cameraPitch;
    [SerializeField]
    [Range(0f, 0.5f)]
    private float lookSmoothTime;
    [SerializeField]
    [Range(0f, 15f)]
    private float followHeadTime;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private GameObject gameSettingsPrefab;
    private bool freezeRotation;
    #endregion

    #region properties
    public float FollowHeadTime
    {
        get { return followHeadTime; }
        set { followHeadTime = value; }
    }
    public bool FreezeRotation
    {
        get { return freezeRotation; } 
        set { freezeRotation = value; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (GameSettings.Instance == null)
        {
            try
            {
                Instantiate(gameSettingsPrefab, transform.position, Quaternion.identity, null);
            }
            catch
            {
                throw new System.Exception("Missing GameSettings: Please create a GameSettings instance or attach the prefab to the player controller.");
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManager.Instance.EnableCamera)
        {
            CameraLook();
        }

        // Rotate camera towards book when paused
        if (GameManager.Instance.IsPaused)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0.05f, 1.65f, 0.23f), 5f * Time.deltaTime);
            Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(-6, -8, -2), 4f * Time.deltaTime);
        }
    }

    Vector2 currMouseDelta = Vector2.zero;
    Vector2 currMouseDeltaVel = Vector2.zero;
    Vector3 transformVel = Vector3.zero;
    private void CameraLook()
    {
        // Get axis
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X") * GameSettings.Instance.sensitivity, Input.GetAxis("Mouse Y") * GameSettings.Instance.sensitivity);
        // Smoothen rotation
        if (!freezeRotation)
            currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVel, lookSmoothTime * Time.deltaTime);

        cameraPitch -= currMouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -60.0f, 60.0f);

        // Y rotation
        transform.localEulerAngles = Vector3.right * cameraPitch;
        // X rotation
        transform.root.Rotate(Vector3.up * currMouseDelta.x);
        // Y position
        transform.position = Vector3.SmoothDamp(transform.position, head.position, ref transformVel, followHeadTime * Time.deltaTime);
    }
    #endregion
}
