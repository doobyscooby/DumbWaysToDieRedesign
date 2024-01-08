using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBQChicken : Interactable
{
    #region fields
    [SerializeField]
    private TrapBBQ bbq;
    private bool eat;
    #endregion

    #region methods
    private void Awake()
    {
        CanInteract = false;
    }

    public override void Action()
    {
        if (!eat)
        {
            GetComponent<Animator>().SetTrigger("Flip");
            StartCoroutine(EnableEat());
            CanInteract = false;
        }
        else
        {
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make BBQ");
            GetComponent<AudioSource>().Play();
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;

            if (GameManager.Instance.taskManager.GetTask("Make BBQ").stepsComplete == 5)
            {
                bbq.GetComponent<Animator>().SetBool("Light", false);
            }
        }
    }

    IEnumerator EnableEat()
    {
        yield return new WaitForSeconds(0.3f);
        CanInteract = true;
        Text = "Eat";
        eat = true;
    }
    #endregion
}
