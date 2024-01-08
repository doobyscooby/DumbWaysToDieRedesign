using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringAgent : MonoBehaviour
{
	[SerializeField]
	protected NavMeshAgent agent;

	protected const float DefaultUpdateTimeInSecondsForAI = 0.1f;

	/// <summary>
	/// Adjusts the frequency time in seconds of when the AI will  updates its logic
	/// </summary>
	[SerializeField]
	[Range(0.005f, 5.0f)]
	protected float maxUpdateTimeInSecondsForAI = DefaultUpdateTimeInSecondsForAI;

	/// <summary>
	/// Returns the Target Position of the Agent
	/// </summary>
	public Vector3 TargetPosition { get; protected set; }

	/// <summary>
	/// Stores a list of all steering behaviours that are on a SteeringAgent GameObject, regardless if they are enabled or not
	/// </summary>
	[SerializeField]
	protected List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();


	private float updateTimeInSecondsForAI = DefaultUpdateTimeInSecondsForAI;

	/// <summary>
	/// Called once per frame
	/// </summary>
	private void Update()
	{
		updateTimeInSecondsForAI += Time.deltaTime;
		if (updateTimeInSecondsForAI >= maxUpdateTimeInSecondsForAI)
		{
			updateTimeInSecondsForAI %= maxUpdateTimeInSecondsForAI;
			CooperativeArbitration();
		}

		UpdatePosition();
	}

	/// <summary>
	/// This is responsible for how to deal with multiple behaviours and selecting which ones to use. Please see this link for some decent descriptions of below:
	/// https://alastaira.wordpress.com/2013/03/13/methods-for-combining-autonomous-steering-behaviours/
	/// Remember some options for choosing are:
	/// 1 Finite state machines which can be part of the steering behaviours or not (Not the best approach but quick)
	/// 2 Weighted Truncated Sum
	/// 3 Prioritised Weighted Truncated Sum
	/// 4 Prioritised Dithering
	/// 5 Context Behaviours: https://andrewfray.wordpress.com/2013/03/26/context-behaviours-know-how-to-share/
	/// 6 Any other approach you come up with
	/// </summary>
	protected virtual void CooperativeArbitration()
	{
		TargetPosition = Vector3.zero;

		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
		{
			if (currentBehaviour.enabled)
			{
				TargetPosition += currentBehaviour.UpdateBehaviour(this);
			}
		}
	}

	/// <summary>
	/// Updates the position of the GAmeObject via Teleportation. In Craig Reynolds architecture this would the Locomotion layer
	/// </summary>
	protected virtual void UpdatePosition()
	{
		if (agent.enabled)
		{
			agent.SetDestination(TargetPosition);
		}
	}

	/// <summary>
	/// Only enables a single steering behaviour
	/// </summary>
	/// <param name="behaviourToEnable"></param>
	protected virtual void EnableSteeringBehaviour(SteeringBehaviour behaviourToEnable)
	{
		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
		{
			currentBehaviour.enabled = false;
		}
		if (behaviourToEnable != null)
		{
			behaviourToEnable.enabled = true;
		}
	}


#region UI code that should not be in starter example

public float MaxUpdateTimeInSecondsForAI { get => maxUpdateTimeInSecondsForAI; }

	public void SetAITime(float time)
	{
		maxUpdateTimeInSecondsForAI = time;
	}
	#endregion
}
