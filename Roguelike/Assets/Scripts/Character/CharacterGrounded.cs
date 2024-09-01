using UnityEngine;

using Sirenix.OdinInspector;

public class CharacterGrounded : MonoBehaviour
{
  [FoldoutGroup("Ground")]
  [SerializeField] private float _groundedOffset = -0.14f;
  [FoldoutGroup("Ground")]
  [SerializeField] private float _groundedRadius = 0.28f;
  [FoldoutGroup("Ground")]
  [SerializeField] private LayerMask _groundLayers;

  [FoldoutGroup("Gravity")]
  [SerializeField] private float _gravity = 15.0f;
  [FoldoutGroup("Gravity")]
  [SerializeField] private float _maxFallSpeed = 10.0f;

  [FoldoutGroup("Sound"), InlineEditor(InlineEditorModes.SmallPreview)]
  [SerializeField] private AudioClip _landingSound;

  //------------------------------------

  private Character character;

  private bool wasGrounded;

  //====================================

  public bool Grounded { get; private set; }

  //====================================

  private void Awake()
  {
    character = GetComponent<Character>();
  }

  private void Update()
  {
    GroundedCheck();

    wasGrounded = Grounded;
  }

  //====================================

  public void Gravity(ref Vector3 parVelocity)
  {
    character.Animator.SetBool(CharacterAnimations.IS_FALL, !Grounded);

    if (!character.Controller.isGrounded)
    {
      if (wasGrounded)
        parVelocity.y = 0.0f;

      parVelocity.y -= _gravity * Time.deltaTime;

      if (parVelocity.y > _maxFallSpeed)
        parVelocity.y = -_maxFallSpeed;
    }
  }

  //====================================

  private void GroundedCheck()
  {
    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
    Grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);

    character.Animator.SetBool(CharacterAnimations.IS_GROUNDED, Grounded);
  }

  //====================================

  private void OnLand(AnimationEvent animationEvent)
  {
    if (animationEvent.animatorClipInfo.weight > 0.5f)
    {
      AudioSource.PlayClipAtPoint(_landingSound, transform.TransformPoint(character.Controller.center), 0.3f);
    }
  }

  //====================================
}