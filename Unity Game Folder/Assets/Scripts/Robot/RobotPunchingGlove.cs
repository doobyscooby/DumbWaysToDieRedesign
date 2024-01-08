using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class RobotPunchingGlove : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject constraint;
    [SerializeField]
    private Transform endBone;
    private RaycastHit hit;

    [SerializeField]
    private VisualEffect whamVFX;

    private Vector3 startingPos;
    private bool attack;
    #endregion

    #region methods
    private void Start()
    {
        startingPos = constraint.transform.localPosition;
    }

    private void Update()
    {
        if (attack)
        {
            // Rotate towards player
            Vector3 dir = (PlayerController.Instance.transform.position - transform.root.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, lookRot, 3.5f * Time.deltaTime);

            // Move boxing glove towards player
            constraint.transform.position = Vector3.Lerp(constraint.transform.position, PlayerController.Instance.transform.position, 8.5f * Time.deltaTime);

            // Collide with objects infront
            if (Physics.SphereCast(endBone.position, 0.3f, endBone.forward, out hit, 0.5f))
            {
                if (hit.transform.name != "Boxing Glove Rig" && hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    attack = false;
                    whamVFX.Play();
                    GetComponent<AudioSource>().Play();
                }
            }

            // Hit player
            if (Vector3.Distance(constraint.transform.position, PlayerController.Instance.transform.position) <= 0.5f)
            {
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
                attack = false;
                whamVFX.Play();
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            // Retract boxing glove towards initial position
            constraint.transform.localPosition = Vector3.Lerp(constraint.transform.localPosition, startingPos, 2f * Time.deltaTime);
        }
    }

    public void Action()
    {
        attack = true;
    }
    #endregion
}