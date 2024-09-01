using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorHealth : Health
{
  private int maxArmor;

  //===================================

  public int CurrentArmor { get; private set; }

  //===================================

  public ArmorHealth(int parArmor, int parHealth)
  {
    maxArmor = parArmor;
  }

  //===================================

  public event Action<int> OnChangeArmor;

  //===================================

  public void AddArmor(int parArmor)
  {
    if (parArmor < 0)
      throw new ArgumentOutOfRangeException(nameof(parArmor));

    int armorBefore = CurrentArmor;
    CurrentArmor += parArmor;

    if (CurrentArmor > maxArmor)
      CurrentArmor = maxArmor;

    OnChangeArmor?.Invoke(CurrentArmor);
  }
  
  public override void Reduce(int parDamage)
  {
    if (parDamage < 0)
      throw new ArgumentOutOfRangeException(nameof(parDamage));

    int armorBefore = CurrentArmor;
    CurrentArmor -= parDamage;

    parDamage -= armorBefore;

    if (parDamage <= 0)
      parDamage = 0;

    if (CurrentArmor < 0)
      CurrentArmor = 0;

    OnChangeArmor?.Invoke(CurrentArmor);

    base.Reduce(parDamage);
  }

  //===================================
}