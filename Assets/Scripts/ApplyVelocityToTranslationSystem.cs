using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ApplyVelocityToTranslationSystem : SystemBase
{
  protected override void OnUpdate()
  {
    var delta = Time.DeltaTime;
    Entities.ForEach((ref Translation translation, in Velocity velocity) =>
    {
      translation.Value += new float3(velocity.Value * delta, 0);
    }).ScheduleParallel();
  }
}
