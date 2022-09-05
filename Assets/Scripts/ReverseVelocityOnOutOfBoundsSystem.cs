using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ReverseVelocityOnOutOfBoundsSystem : SystemBase
{
  protected override void OnUpdate()
  {
    var config = GetSingleton<SimulationConfig>();
    
    var delta = Time.DeltaTime;
    Entities.ForEach((ref Velocity velocity, ref Translation translation) =>
    {
      var distance = math.length(translation.Value.xy);
      if (distance > config.BoundaryRadius)
      {
        var toCenter = -translation.Value.xy;
        velocity.Value += toCenter * config.BoundaryForce * delta;
      }
    }).ScheduleParallel();
  }
}
