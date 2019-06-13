﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class PhysicsRunner : MonoBehaviour {
    private IEnumerable<ComponentSystemBase> simSystems;
    void Start() {
        World.Active.GetOrCreateSystem<SimulationSystemGroup>().Enabled = false;
        simSystems = World.Active.GetOrCreateSystem<SimulationSystemGroup>().Systems;
    }
    void FixedUpdate() {
        foreach (var sys in simSystems) {
            sys.Update();
        }
    }
}
