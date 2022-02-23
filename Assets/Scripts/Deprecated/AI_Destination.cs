using UnityEngine;
using System.Collections;

namespace Pathfinding
{

	public class AI_Destination : VersionedMonoBehaviour
	{
		/// <summary>The object that the AI should move to</summary>
		private GameObject Enemy;
		private Transform target;
		IAstarAI ai;

        private void Start()
        {
			Enemy = GameObject.FindGameObjectWithTag("Enemy");
		}

		void OnEnable()
		{
			ai = GetComponent<IAstarAI>();

			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable()
		{
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update()
		{
			Vector3 targetLocation = new Vector3(Enemy.transform.position.x - 1, Enemy.transform.position.y);
			target.position = targetLocation;
			if (target != null && ai != null) ai.destination = target.position;
		}
	}
}
