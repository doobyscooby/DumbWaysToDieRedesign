using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
	// Desired Position.
	private Vector3 desiredPosition;

	// Distance to target before stopping or moving to next.
	[SerializeField]
	protected float distanceToTarget = 1;
	/// <summary>
	/// Do steering behaviour code here. At the end of this the desiredVelocity and steeringVelocity variables should be set
	/// </summary>
	/// <param name="steeringAgent">The agent this component is acting on</param>
	/// <returns>The steeringVelocity should always be returned here</returns>
	public abstract Vector3 UpdateBehaviour(SteeringAgent steeringAgent);

	protected virtual void Start()
	{
		// Annoyingly this is needed for the enabled bool to work in Unity. A MonoBehaviour must now have one of the following
		// to activate this: Start(), Update(), FixedUpdate(), LateUpdate(), OnGUI(), OnDisable(), OnEnabled()
	}
}