using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCereal : Interactable
{
    #region fields
    [SerializeField]
    private GameObject milk, cereal;
    private int completion;
    #endregion

    #region methods
    public override void Action()
    {
        if (completion == 2)
        {
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            // Play effects
            GetComponent<AudioSource>().Play();
            Camera.main.transform.Find("VFX").transform.Find("Cereal Eating Effect").GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Add milk
        if (collision.gameObject == milk)
        {
            // Enable milk
            transform.GetChild(0).gameObject.SetActive(true);

            // Have already added cereal
            if (completion == 1)
            {
                // Change cereal
                transform.GetChild(1).localPosition = Vector3.zero;
                transform.GetChild(1).localScale = new Vector3(1.1f, 1.1f, 1.1f);
                Text = "Eat";
            }

            // Update
            Destroy(collision.gameObject);
            AudioManager.Instance.PlayAudio("Cloth");
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            completion++;
        }
        // Add cereal
        else if (collision.gameObject == cereal)
        {
            // Enable cereal
            GameObject obj = transform.GetChild(1).gameObject;
            obj.SetActive(true);

            // Have not added milk
            if (completion == 0)
            {
                obj.transform.localPosition = new Vector3(0.0f, 0.0002f, -0.00135f);
                obj.transform.localScale = Vector3.one;
            }
            // Have added milk
            else if (completion == 1)
            {
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                Text = "Eat";
            }

            // Update
            Destroy(collision.gameObject);
            AudioManager.Instance.PlayAudio("Cloth");
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            completion++;
        }
    }
    #endregion
}
