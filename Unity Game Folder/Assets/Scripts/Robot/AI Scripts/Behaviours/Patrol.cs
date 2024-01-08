using System.Collections.Generic;
using UnityEngine;

public class Patrol : SteeringBehaviour
{
	[SerializeField]
	protected LineRenderer patrolLineRenderer;

	protected int patrolPointIndex = 0;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		// Reached Target?
		Vector3 targetPosition = patrolLineRenderer.GetPosition(patrolPointIndex);
		if ((transform.position - targetPosition).magnitude < distanceToTarget)
		{
			++patrolPointIndex;
			if(patrolPointIndex >= patrolLineRenderer.positionCount)
			{
				patrolPointIndex = 0;
			}

			// Update the target position as patrol point has changed due to index update
			targetPosition = patrolLineRenderer.GetPosition(patrolPointIndex);
		}
		return targetPosition;
	}
}
