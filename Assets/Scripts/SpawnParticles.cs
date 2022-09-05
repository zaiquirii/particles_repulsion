using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SpawnParticle: IComponentData
{
  public Random Random;
  public Entity ParticleToSpawn;
  public int Count;
}
