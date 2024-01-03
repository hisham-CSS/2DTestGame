using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

    //Store current state and the factory that makes the states
    EnemyBaseState currentState;
    EnemyStateFactory states;

    public EnemyBaseState GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(EnemyBaseState value)
    {
        currentState = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
