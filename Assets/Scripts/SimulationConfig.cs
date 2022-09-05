using UnityEngine;
using Unity.Entities;


[GenerateAuthoringComponent]
public struct SimulationConfig : IComponentData
{
  public float Gravity;
  public float VelocityDecay;
  public float AffectRadius;
  public float SpawnRadius;
  public float BoundaryRadius;
  public float BoundaryForce;
}