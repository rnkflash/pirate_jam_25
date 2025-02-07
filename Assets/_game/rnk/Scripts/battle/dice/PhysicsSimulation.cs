using UnityEngine;

namespace _game.rnk.Scripts.battle.dice
{
    public class PhysicsSimulation : MonoBehaviour
    {
        [SerializeField] float physicsTimeStep = 0.02f;
        [SerializeField] float physicsSpeed = 1.0f;
        
        float timer;

        void Update()
        {
            if (Physics.simulationMode != SimulationMode.Script)
                return;

            timer += Time.deltaTime;

            while (timer >= physicsTimeStep)
            {
                timer -= physicsTimeStep;
                Physics.Simulate(physicsTimeStep * physicsSpeed);
            }
        }
    }
}