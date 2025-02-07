using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _game.rnk.Scripts.battle.dice
{
    public class Dice3D : MonoBehaviour
    {
        public enum State
        {
            FREE, FOLLOW
        }

        [SerializeField] Vector3 minTorque, maxTorque;
        [SerializeField] Vector3 minImpulse, maxImpulse;
        [SerializeField] Vector3 minInitialRotation, maxInitialRotation;
        [SerializeField] AudioClip[] floorHitSounds;
        [SerializeField] AudioClip[] diceHitSounds;
        [SerializeField] Transform[] faceDetectors;
        
        [NonSerialized] public int rollValue;
        [NonSerialized] public Vector3 targetPosition;
        [NonSerialized] public bool isRolled;

        Rigidbody body;
        AudioSource audioSource;
        MoveableSmoothDamp movable;

        State state;
        bool isMoving;
        float rolledTimer;
        float rolledTimerMax = 0.25f;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            body = GetComponent<Rigidbody>();
            movable = GetComponent<MoveableSmoothDamp>();
            
            SetState(State.FREE);
            
        }

        void Update()
        {
            if (state == State.FREE)
            {
                isMoving = body.linearVelocity.sqrMagnitude > 0.01f;
                if (isMoving == false)
                {
                    rolledTimer -= Time.deltaTime;
                    if (rolledTimer <= 0)
                    {
                        isRolled = true;
                        FindFaceResult();
                    }
                }
            } else if (state == State.FOLLOW)
            {
                movable.targetPosition = targetPosition;
            }
        }

        public void SetState(State newState)
        {
            state = newState;
            body.isKinematic = state == State.FOLLOW;
            
            if (state == State.FOLLOW)
            {
                isRolled = true;
                FindFaceResult();
                isMoving = false;
                movable.enabled = true;
                movable.targetPosition = targetPosition;
            }
            else
            {
                movable.enabled = false;
            }
        }

        public void RollDice()
        {
            rolledTimer = rolledTimerMax;
            isMoving = true;
            isRolled = false;
            var initialRotation = transform.rotation;
            transform.rotation = new Quaternion(
                initialRotation.x + Random.Range(minInitialRotation.x, maxInitialRotation.x),
                initialRotation.y + Random.Range(minInitialRotation.y, maxInitialRotation.y),
                initialRotation.z + Random.Range(minInitialRotation.z, maxInitialRotation.z),
                initialRotation.w
            );
            Vector3 torque = new Vector3(
                Random.Range(minTorque.x, maxTorque.x),
                Random.Range(minTorque.y, maxTorque.y),
                Random.Range(minTorque.z, maxTorque.z)
            );
            body.AddTorque(torque);
            
            Vector3 impulse = new Vector3(
                Random.Range(minImpulse.x, maxImpulse.x),
                Random.Range(minImpulse.y, maxImpulse.y),
                Random.Range(minImpulse.z, maxImpulse.z)
            );
            body.AddForce(impulse, ForceMode.Impulse);
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if (state != State.FREE)
                return;
            var volume = collision.relativeVelocity.magnitude;
            if (collision.transform.CompareTag("DiceFloor"))
            {
                PlaySound(floorHitSounds, Mathf.Clamp(volume, 0, 6));
            }

            if (collision.transform.CompareTag("Dice"))
            {
                PlaySound(diceHitSounds, volume);
            }
        }

        void PlaySound(AudioClip[] clips, float volumeScale = 1.0f)
        {
            if (clips.Length == 0) return;
            audioSource.PlayOneShot(clips.GetRandom(), volumeScale);
        }
        
        public int FindFaceResult()
        {
            int maxIndex = 0;
            for (int i = 1; i < faceDetectors.Length; i++)
            {
                if (faceDetectors[maxIndex].transform.position.y <
                    faceDetectors[i].transform.position.y)
                {
                    maxIndex = i;
                }
            }
            rollValue = maxIndex;
            return maxIndex;
        }

        public void SetFaceAnimated(int faceIndex)
        {
            var faceDetector = faceDetectors[faceIndex];
            //Quaternion upAlignment = Quaternion.FromToRotation(faceDetector.up, Vector3.up);

            //Vector3 directionToCamera = mainCamera.transform.position - faceDetector.position;
            //directionToCamera.y = 0; // Optional: Keep the rotation in the horizontal plane
            Vector3 directionToCamera = Vector3.forward;

            Quaternion desiredChildRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
            Quaternion parentRotation = desiredChildRotation * Quaternion.Inverse(faceDetector.localRotation);

            //transform.rotation = parentRotation;
            transform.DORotate(parentRotation.eulerAngles, 0.25f);
        }
    }
}