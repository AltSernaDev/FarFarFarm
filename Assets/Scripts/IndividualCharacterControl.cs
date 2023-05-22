using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IndividualCharacterControl : MonoBehaviour//, ISelectable
{
    bool placed;
    [SerializeField] bool builder;
    public bool busy= false;
    public ParticleSystem partSyst;
    public NavMeshAgent navAgent;

    private void Start()
    {
        navAgent= GetComponent<NavMeshAgent>();

        if(builder)
        {

            // REGISTRO DE BUILDERS
            for (int i = 0; i < FarmersAndBuildersManager.SingletonInstance.builderCtrl.Length; i++)
            {
                if (FarmersAndBuildersManager.SingletonInstance.builderCtrl[i] != this
                    && FarmersAndBuildersManager.SingletonInstance.builderCtrl[i] == null && !placed)
                {
                    placed = true;
                    FarmersAndBuildersManager.SingletonInstance.builderCtrl[i] = this; // ESTE ES EL SUB
                }
            }
        }
        else
        {
            // REGISTRO DE FARMERS
            for (int i = 0; i < FarmersAndBuildersManager.SingletonInstance.farmerCtrl.Length; i++)
            {
                if (FarmersAndBuildersManager.SingletonInstance.farmerCtrl[i] != this
                    && FarmersAndBuildersManager.SingletonInstance.farmerCtrl[i] == null && !placed)
                {
                    placed = true;
                    FarmersAndBuildersManager.SingletonInstance.farmerCtrl[i] = this; // ESTE ES EL SUB
                }
            }
        }
    }

    public void Select()
    {
        Debug.Log("Soy yo, " + gameObject.name);
    }

    private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }

    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }

    void OnTick()
    {

    }

    public void BuildingLevelUp(Structure structure)
    {
        busy = true;
        navAgent.SetDestination(structure.transform.position);
        navAgent.Move(navAgent.destination);
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position,
            2 * 1/TicksTimeManager.Instance.TicksPerSecond, 0.0f);
       
    }

    public void FinishingLevelUp()
    {
        //Builder acaba animación y particulas
        //builder vuelve a casa
        //Builder ya no está BUSY
        //builder = null; // se elimina referencia de su builder
    }
    
}
