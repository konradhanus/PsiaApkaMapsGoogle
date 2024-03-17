using System.Collections;
using System.Collections.Generic;

namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;
	using UnityEngine;

	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{

		bool _isInitialized;
		// public Rigidbody rigidbody;

		Vector3 _targetPosition;
		public float smoothSpeed = 0.5f; // Adjust the speed of smooth movement here.

		 private Transform playerTransform; // Transformacja gracza

		[Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;


		ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}

		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
			playerTransform = GetComponent<Transform>();
		}

		void Update()
		{
			
		}

		void FixedUpdate()
		{
 			UpdatePosition();
		}

		// void LateUpdate()
		void UpdatePosition()
		{
			// if (_isInitialized)
			// {
			// 	var map = LocationProviderFactory.Instance.mapManager;
			// 	Vector3 newPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
			// 	var localPosition = transform.localPosition;
			// 	// transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude)+shift;
	
			// 	Debug.Log("LocalPOsition"+localPosition);
			// 	Debug.Log("NewPosition"+newPosition);
			// 	// rigidbody.MovePosition(newPosition);
			// 	rigidbody.MovePosition(newPosition);
				
			// }
			if (_isInitialized)
			{
				var map = LocationProviderFactory.Instance.mapManager;
				// Debug.Log("GROUNDED");
				Vector3 newPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
				newPosition.y = 0.1f;
				Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
				transform.localPosition = smoothedPosition;
			}
			//GroundedCheck();
		}

		// private void GroundedCheck()
        // {
        //     // set sphere position, with offset
        //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
        //         transform.position.z);
        //     Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
        //         QueryTriggerInteraction.Ignore);

           
        // }
	}
}