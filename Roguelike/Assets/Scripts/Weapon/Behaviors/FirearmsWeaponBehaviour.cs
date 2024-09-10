using System.Collections;
using UnityEngine;
using Zenject;

using Sirenix.OdinInspector;
using UnityEngine.TextCore.Text;

public class FirearmsWeaponBehaviour : Weapon
{
  [FoldoutGroup("Shooting")]
  [SerializeField, MinValue(0)] private int _shotCount = 1;
  [FoldoutGroup("Shooting")]
  [SerializeField, MinValue(0)] private int _shotsPerMinutes = 200;
  [FoldoutGroup("Shooting")]
  [SerializeField, MinValue(0)] private int _shotSpeed = 8;
  [FoldoutGroup("Shooting")]
  [SerializeField] private Transform[] _startPoints;

  [FoldoutGroup("Spread")]
  [SerializeField] private bool _useSpread = true;
  [FoldoutGroup("Spread")]
  [SerializeField, MinValue(0)] private float _spreadFactor = 1.0f;

  [FoldoutGroup("Ammo")]
  [SerializeField, MinValue(0)] private int _maxAmountAmmo = 0;
  [FoldoutGroup("Ammo")]
  [SerializeField, MinValue(0)] private int _maxAmountAmmoInMagazine = 0;

  [FoldoutGroup("Recharge")]
  [SerializeField] private bool _autoRecharge = false;
  [FoldoutGroup("Recharge")]
  [SerializeField, MinValue(0)] private float _rechargeTime = 1.0f;

  [FoldoutGroup("Effects")]
  [SerializeField] private Transform _muzzleEffectPrefab;
  [FoldoutGroup("Effects")]
  [SerializeField, MinValue(0)] private float _hitEffectDestroyDelay = 2.0f;

  [FoldoutGroup("Sounds"), InlineEditor(InlineEditorModes.SmallPreview)]
  [SerializeField] private AudioClip _soundFire;
  [FoldoutGroup("Sounds"), InlineEditor(InlineEditorModes.SmallPreview)]
  [SerializeField] private AudioClip _soundEmptyAmmo;
  [FoldoutGroup("Sounds"), InlineEditor(InlineEditorModes.SmallPreview)]
  [SerializeField] private AudioClip _soundRecharge;

  [FoldoutGroup("Projectile")]
  [SerializeField] private BaseProjectile _projectilePrefab;

  //------------------------------------

  protected Character character;

  protected Coroutine coroutineRecharge;

  protected int currentAmountAmmo;
  protected int currentAmountAmmoInMagazine;

  protected float lastShotTime;

  //====================================

  public bool IsRecharge { get; private set; }
  public bool IsRechargeNotCompleted { get; private set; }

  //====================================

  [Inject]
  private void Construct(Character parCharacter)
  {
    character = parCharacter;
  }

  //====================================

  public virtual void Start()
  {
    currentAmountAmmo = _maxAmountAmmo;
    currentAmountAmmoInMagazine = _maxAmountAmmoInMagazine;
  }

  private void OnDisable()
  {
    character.OnRechargeAnim();

    coroutineRecharge = null;
  }

  public virtual void Update()
  {
    if (CanShoot)
      Attack();

    TryShoot(false);
  }

  //====================================

  public override void Attack()
  {
    if (coroutineRecharge != null)
      return;

    if (currentAmountAmmoInMagazine == 0)
      return;

    if (!(Time.time - lastShotTime > 60.0f / _shotsPerMinutes))
      return;

    Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
    //Ray ray = new Ray(character.CameraController.MainCamera.transform.position, character.CameraController.MainCamera.transform.forward);
    RaycastHit hit;

    Vector3 targetPoint;

    if (Physics.Raycast(ray, out hit))
      targetPoint = hit.point;
    else
      targetPoint = ray.GetPoint(100);

    for (int i = 0; i < _shotCount; i++)
    {
      Vector3 direction = ((_useSpread ? (targetPoint + CalculateSpread()) : targetPoint) - _startPoints[0].position).normalized;

      Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
      BaseProjectile projectile = Instantiate(_projectilePrefab, _startPoints[0].position, rotation);
      projectile.Initialize(0, character.gameObject);

      projectile.BodyRB.velocity = direction * _shotSpeed;
    }

    PerformEffects();

    lastShotTime = Time.time;

    currentAmountAmmoInMagazine--;
    PlaySound(_soundFire, 0.3f);

    if (_autoRecharge && currentAmountAmmoInMagazine == 0)
    {
      Recharge();
    }
  }

  public void Recharge(int parValue)
  {
    if (coroutineRecharge != null)
    {
      Debug.LogWarning("ѕерезарадка уже запущена");
      return;
    }

    if (parValue < 0)
    {
      Debug.LogError("«начение перезар€дки не может быть меньше 0");
      return;
    }

    if (currentAmountAmmo == 0)
    {
      Debug.LogWarning("“екущее количество патронов равно 0, перезар€дка невозможна");
      return;
    }

    if (currentAmountAmmoInMagazine >= _maxAmountAmmoInMagazine)
    {
      Debug.Log("“екущее количество патронов в магазине >= максимального значени€");
      return;
    }

    coroutineRecharge = StartCoroutine(CoroutineRecharge(parValue));
  }

  public override void Recharge()
  {
    Recharge(_maxAmountAmmoInMagazine);
  }

  //====================================

  private IEnumerator CoroutineRecharge(int parValue)
  {
    int amountAmmoBefore = currentAmountAmmo;
    int amountAmmoInMagazineBefore = currentAmountAmmoInMagazine;

    if (parValue + amountAmmoInMagazineBefore > _maxAmountAmmoInMagazine)
      parValue = _maxAmountAmmoInMagazine - amountAmmoInMagazineBefore;

    if (amountAmmoBefore - parValue <= 0)
      parValue = amountAmmoBefore;

    character.Animator.SetBool("IsReloading", true);

    PlaySound(_soundRecharge, 0.3f);

    while (character.IsRecharge)
      yield return null;

    currentAmountAmmo -= parValue;
    currentAmountAmmoInMagazine += parValue;

    coroutineRecharge = null;
  }

  private void PerformEffects()
  {
    if (_muzzleEffectPrefab == null)
      return;

    foreach (var point in _startPoints)
    {
      var muzzleEffect = Instantiate(_muzzleEffectPrefab, point);

      Destroy(muzzleEffect.gameObject, _hitEffectDestroyDelay);
    }
  }

  private Vector3 CalculateSpread()
  {
    return new Vector3
    {
      x = Random.Range(-_spreadFactor, _spreadFactor),
      y = Random.Range(-_spreadFactor, _spreadFactor),
      z = Random.Range(-_spreadFactor, _spreadFactor)
    };
  }

  private void PlaySound(AudioClip parAudioClip, float parVolume)
  {
    if (parAudioClip == null)
    {
      Debug.LogError($"«вук {parAudioClip} не найден!");
      return;
    }

    AudioSource.PlayClipAtPoint(parAudioClip, transform.position, parVolume);
  }

  //====================================
}