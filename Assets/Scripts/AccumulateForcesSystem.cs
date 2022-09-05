using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class AccumlateForcesSystem : SystemBase
{
  private EntityQuery particleQuery;

  protected override void OnCreate()
  {
    particleQuery = GetEntityQuery(ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<ParticleTag>());
  }

  protected override void OnUpdate()
  {
    var simulationConfig = GetSingleton<SimulationConfig>();
    
    var particles = particleQuery.ToEntityArray(Allocator.TempJob);
    var delta = Time.DeltaTime;

    Entities
      .ForEach((Entity entity, ref Velocity velocity, in Translation translation, in ParticleTag particleTag) =>
      {
        var force = float2.zero;
        foreach (var particle in particles)
        {
          if (entity == particle) continue;
          var otherTranslation = GetComponent<Translation>(particle);
          var distance = math.distance(translation.Value, otherTranslation.Value);
          if (distance > 0 && distance < simulationConfig.AffectRadius)
          {
            var scalarForce = simulationConfig.Gravity / distance;
            force += math.normalizesafe(otherTranslation.Value.xy - translation.Value.xy) * scalarForce;
          }
        }

        var velocityDelta = force * delta;
        var newVelocity = velocity.Value + velocityDelta;
        var decay = newVelocity * simulationConfig.VelocityDecay * -delta;
        velocity.Value = newVelocity + decay;
      })
      .Schedule();

    this.Dependency.Complete();

    particles.Dispose();
  }
}