using System.Collections.Generic;
// using MoreMountains.Feedbacks;
// using TimohaGTA.Player.Cameras;
using UnityEngine;
using UnityEngine.Serialization;

namespace CMF
{
	//This script handles and plays audio cues like footsteps, jump and land audio clips based on character movement speed and events; 
	public class AudioControl : MonoBehaviour
	{
        private Controller _controller;
		private Animator _animator;
		private Mover _mover;
		private Transform _thisTransform;
		public AudioSource audioSource;
		public AudioSource additionalAuduoSource;
		public AudioSource restartAuduoSource;

		//Whether footsteps will be based on the currently playing animation or calculated based on walked distance (see further below);
		public bool useAnimationBasedFootsteps = true;

		//Velocity threshold for landing sound effect;
		//Sound effect will only be played if downward velocity exceeds this threshold;
		public float landVelocityThreshold = 5f;

		//Footsteps will be played every time the traveled distance reaches this value (if 'useAnimationBasedFootsteps' is set to 'true');
		public float footstepDistance = 0.2f;
        private float _currentFootstepDistance = 0f;

		private float _currentFootStepValue = 0f;

		//Volume of all audio clips;
		[Range(0f, 1f)]
		public float audioClipVolume = 0.1f;

		//Range of random volume deviation used for footsteps;
		//Footstep audio clips will be played at different volumes for a more "natural sounding" result;
		public float relativeRandomizedVolumeRange = 0.2f;

		//Audio clips;
		[SerializeField] private List<FeedbackData> _footstepFeedbacks;
		[SerializeField] private List<FeedbackData> _jumpFeedbacks;
		[SerializeField] private List<FeedbackData> _landFeedbacks;
		[SerializeField] private List<FeedbackData> _shakeFeedbacks;
		[SerializeField] private List<FeedbackData> _deathFeedbacks;
		[SerializeField] private List<FeedbackData> _damageFeedbacks;
		[SerializeField] private List<FeedbackData> _restartFeedbacks;

		//Setup;
		void Start () {
			//Get component references;
			_controller = GetComponent<Controller>();
			_animator = GetComponentInChildren<Animator>();
			_mover = GetComponent<Mover>();
			_thisTransform = transform;

			//Connecting events to controller events;
			_controller.OnLand += OnLand;
			_controller.OnJump += OnJump;

			if(!_animator)
				useAnimationBasedFootsteps = false;
		}
		
		//Update;
		void Update () 
        {
            Vector3 velocity = _controller.GetVelocity();
            Vector3 horizontalVelocity = VectorMath.RemoveDotVector(velocity, _thisTransform.up);
            Vector3 verticalVelocity = VectorMath.RemoveDotVector(velocity, _thisTransform.forward);

            FootStepUpdate(horizontalVelocity.magnitude);
		}


        void FootStepUpdate(float _movementSpeed)
		{
			float _speedThreshold = 0.05f;

			if(useAnimationBasedFootsteps)
			{
				//Get current foot step value from animator;
				float _newFootStepValue = _animator.GetFloat("FootStep");

				//Play a foot step audio clip whenever the foot step value changes its sign;
				if((_currentFootStepValue <= 0f && _newFootStepValue > 0f) || (_currentFootStepValue >= 0f && _newFootStepValue < 0f))
				{
					//Only play footstep sound if mover is grounded and movement speed is above the threshold;
					if(_mover.IsGrounded() && _movementSpeed > _speedThreshold)
						PlayFootstepSound(_movementSpeed);
				}
                
				_currentFootStepValue = _newFootStepValue;
			}
			else
			{
				_currentFootstepDistance += Time.deltaTime * _movementSpeed;

				//Play foot step audio clip if a certain distance has been traveled;
				if(_currentFootstepDistance > footstepDistance)
				{
					//Only play footstep sound if mover is grounded and movement speed is above the threshold;
					if(_mover.IsGrounded() && _movementSpeed > _speedThreshold)
						PlayFootstepSound(_movementSpeed);
                    
					_currentFootstepDistance = 0f;
				}
			}
		}

        void PlayFootstepSound(float movementSpeed)
		{
           PlayFeedbacks(_footstepFeedbacks);
		}

		void OnLand(Vector3 vector)
        {
            //Only trigger sound if downward velocity exceeds threshold;
			if(VectorMath.GetDotProduct(vector, _thisTransform.up) > -landVelocityThreshold)
				return;
			
			PlayFeedbacks(_landFeedbacks);
        }

		public void OnShake()
		{
			PlayFeedbacks(_shakeFeedbacks, additionalAuduoSource);
		}
		
		public void OnDeathTake()
		{
			PlayFeedbacks(_deathFeedbacks, additionalAuduoSource);
		}
		
		public void OnTakeDamage()
		{
			PlayFeedbacks(_damageFeedbacks, additionalAuduoSource);
		}		
		
		public void OnRestart()
		{
			PlayFeedbacks(_restartFeedbacks, restartAuduoSource);
		}

        void OnJump(Vector3 vector)
		{
			PlayFeedbacks(_jumpFeedbacks);
		}

		private void PlayFeedbacks(List<FeedbackData> feedbacks, AudioSource newAudioSource = null)
		{
			AudioSource defaultSource = audioSource;
			
			if (newAudioSource != null)
			{
				defaultSource = newAudioSource;
			}
			
			foreach (var feedback in feedbacks)
			{
				feedback.Play(defaultSource);
			}
		}
		
        // private void PlayFeedbacks(List<MMF_Player> feedbacks)
        // {
        //     if (feedbacks.Count > 0)
        //     {
        //         foreach (MMF_Player feedback in feedbacks)
        //         {
        //             if(feedback != null)
        //                 feedback.PlayFeedbacks();
        //         }
        //     }
        // }
    }
}

