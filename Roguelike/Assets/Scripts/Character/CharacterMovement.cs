using System;
using UnityEngine;
using Random = UnityEngine.Random;

using Sirenix.OdinInspector;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
  [FoldoutGroup("Speed")]
  [SerializeField] private float _speedWalking = 2.0f;
  [FoldoutGroup("Speed")]
  [SerializeField] private float _speedRunning = 5.0f;
  [FoldoutGroup("Speed")]
  [SerializeField] private float _speedAiming = 3.0f;

  [FoldoutGroup("Boost")]
  [SerializeField] private float _acceleration = 9.0f;
  [FoldoutGroup("Boost")]
  [SerializeField] private float _deceleration = 11.0f;

  [FoldoutGroup("Gravity")]
  [SerializeField] private float _gravity = 15.0f;

  [FoldoutGroup("Camera")]
  [SerializeField] private float _rotationSpeed;

  [FoldoutGroup("Sound")]
  [SerializeField] private AudioClip[] _footstepSounds;

  //------------------------------------

  private Character character;
  private CharacterGrounded grounded;

  private float animationBlend;

  private Vector3 velocity;

  //====================================

  public float SpeedWalking => _speedWalking;
  public float SprintSpeed => _speedRunning;

  public float RotationSpeed => _rotationSpeed;

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
    grounded = GetComponent<CharacterGrounded>();
  }

  //====================================

  /*public void Move(IInput parIInput)
  {
    float targetSpeed = isSprint ? _speedRunning : _speedWalking;

    // Если нет ввода, устанавливаем целевую скорость в 0 для плавного замедления
    if (parIInput.Move() == Vector2.zero)
      targetSpeed = 0;

    // Получаем текущую горизонтальную скорость
    float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

    // Плавно интерполируем текущую скорость к целевой скорости
    speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * _acceleration);
    speed = Mathf.Round(speed * 1000f) / 1000f; // Округление скорости

    // Получаем направление ввода
    Vector3 inputDirection = new Vector3(parIInput.Move().x, 0.0f, parIInput.Move().y).normalized;

    // Если есть ввод, корректируем направление и вращение игрока
    if (parIInput.Move() != Vector2.zero)
    {
      targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

      float rotationVelocity = 0;
      float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, 0.12f);

      transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    // Рассчитываем целевое направление движения
    Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

    // Двигаем игрока в целевом направлении с учетом скорости
    controller.Move(targetDirection.normalized * (speed * Time.deltaTime));
  }*/

  public void Move(IInput parIInput)
  {
    Vector2 frameInput = Vector3.ClampMagnitude(parIInput.Move(), 1.0f);
    var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

    desiredDirection *= character.IsRunning ? _speedRunning : _speedWalking;

    if (desiredDirection != Vector3.zero)
      animationBlend = Mathf.Lerp(animationBlend, character.IsRunning ? _speedRunning : _speedWalking, Time.deltaTime * _acceleration);
    else
      animationBlend = Mathf.Lerp(animationBlend, 0f, Time.deltaTime * _deceleration);

    //desiredDirection = transform.TransformDirection(desiredDirection);
    desiredDirection = character.CameraController.MainCamera.transform.TransformDirection(desiredDirection);
    desiredDirection.y = 0;

    if (desiredDirection.sqrMagnitude > 0.1f)
    {
      Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }

    velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? _acceleration : _deceleration));

    grounded.Gravity(ref velocity);

    Vector3 applied = velocity * Time.deltaTime;
    if (character.Controller.isGrounded)
      applied.y -= 0.03f;

    character.Controller.Move(applied);

    character.Animator.SetFloat(CharacterAnimations.SPEED, animationBlend);
  }

  //====================================

  private void OnFootstep(AnimationEvent animationEvent)
  {
    if (animationEvent.animatorClipInfo.weight > 0.5f)
    {
      if (_footstepSounds.Length > 0)
      {
        var index = Random.Range(0, _footstepSounds.Length);
        AudioSource.PlayClipAtPoint(_footstepSounds[index], transform.TransformPoint(character.Controller.center), 0.3f);
      }
    }
  }

  //====================================
}