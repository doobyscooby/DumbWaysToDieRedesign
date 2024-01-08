using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : SteeringBehaviour
{
	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		Vector3 targetPosition = PlayerController.Instance.transform.position;

        if ((transform.position - targetPosition).magnitude < distanceToTarget)
		{
			targetPosition = transform.position;
        }

        return targetPosition;
	}
}
