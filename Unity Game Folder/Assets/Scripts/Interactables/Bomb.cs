using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Bomb : MonoBehaviour
{
    public VisualEffect explosionVFX;
    public AudioSource explosionSFX;
    public AudioSource clickSFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;
        clickSFX.Play();
        StartCoroutine(KillPlayer(other.gameObject));
    }

    IEnumerator KillPlayer(GameObject other)
    {
        yield return new WaitForSeconds(0.2f);
        explosionSFX.Play();
        explosionVFX.Play();

        // Disable both player controllers.
        if (other.name == "Character")
        {
            if (PlayerController.Instance)
            {
                TopdownPlayerController tdPC = PlayerController.Instance.GetComponent<TopdownPlayerController>();
                if (tdPC)
                    tdPC.enabled = false;
                GameManager.Instance.EnableControls = false;
                GameManager.Instance.EnableCamera = false;
                Camera.main.GetComponent<CameraController>().FreezeRotation = true;
            }
        }
        else
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (!otherRb)
                otherRb = other.AddComponent<Rigidbody>();
            otherRb.AddForce(new Vector3(0.0f, 1000.0f, 0.0f));
            Destroy(gameObject);
            yield break;
        }

        // Unchild lawnmower.
        transform.parent = null;
        Vector3 dir = new Vector3(100, 200, 0);
        float delay = 0.1f;
        PlayerController.Instance.ThrowPlayerInDirection(dir, delay);
        Destroy(gameObject);
    }
}
