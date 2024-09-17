using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DynamicMultiAimSource : MonoBehaviour
{
  [SerializeField] private MultiAimConstraint[] _multiAimConstraints;

  //------------------------------------

  private CharacterAnimationRigs characterAnimationRigs;

  //====================================

  private void Awake()
  {
    _multiAimConstraints = GetComponentsInChildren<MultiAimConstraint>(true);

    characterAnimationRigs = GetComponentInParent<CharacterAnimationRigs>();

    UpdateDynamicTarget();
  }

  //====================================

  private void UpdateDynamicTarget()
  {
    GameObject newTarget = new GameObject("DynamicTarget");
    characterAnimationRigs.DynamicPoint = newTarget.transform;

    foreach (var multiAimConstraint in _multiAimConstraints)
    {
      WeightedTransformArray sourceObjects = multiAimConstraint.data.sourceObjects;

      WeightedTransform newSource = new WeightedTransform();
      newSource.transform = newTarget.transform;
      newSource.weight = 1f;

      sourceObjects.Add(newSource);

      multiAimConstraint.data.sourceObjects = sourceObjects;
    }
  }

  //====================================
}