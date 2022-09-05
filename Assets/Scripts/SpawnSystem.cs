using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpawnSystem : SystemBase
{
  protected override void OnUpdate()
  {
    var config = GetSingleton<SimulationConfig>();
    EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
    Entities.ForEach((Entity e, ref SpawnParticle spawnParticle) =>
    {
      for (var i = 0; i < spawnParticle.Count; i++)
      {
        Entity particle = ecb.Instantiate(spawnParticle.ParticleToSpawn);
        var spawnPoint = spawnParticle.Random.NextFloat2Direction() *
                         spawnParticle.Random.NextFloat(config.SpawnRadius);
        ecb.SetComponent(particle, new Translation { Value = new float3(spawnPoint, 0) });
        ecb.DestroyEntity(e);
      }
    }).Schedule();
    this.Dependency.Complete();
    ecb.Playback(this.EntityManager);
    ecb.Dispose();
  }
}
