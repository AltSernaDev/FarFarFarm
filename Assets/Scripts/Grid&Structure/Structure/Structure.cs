using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable][RequireComponent(typeof(BoxCollider))]
public class Structure : MonoBehaviour
{
    public StructureSo structureSo; //on change SetValues()
    public StructureType? type;

    int level = 0;
    int levelUpPrice;
    int levelUpTime;

    GameObject building;

    //IndividualCharacterControl buildFarm;

    uint? levelUpEventKey = null;
    DateTime? levelUpDate = null;


    public int LevelUpPrice { get => levelUpPrice; }
    public int Level { get => level; }
    public int LevelUpTime { get => levelUpTime; }
    public uint? LevelUpEventKey { get => levelUpEventKey; }
    public DateTime? LevelUpDate { get => levelUpDate; }

    private void Start() //de pronto es un start. att: serna // culpa de serna
    {
        SetValues();
    }

    private void OnEnable()
    {
        TimeEventsManager.OnDoneEvent += FinishLevelUp;
    }
    private void OnDisable()
    {
        TimeEventsManager.OnDoneEvent -= FinishLevelUp;
    }

    public void SetValues()
    {
        print(type);
        print(structureSo);

        if (structureSo == null && type == null)
            return;
        else if (structureSo != null && type == null)
            type = GridManager.Instance.FromSoToEnum(structureSo);
        else if (structureSo == null && type != null)
            structureSo = GridManager.Instance.FromEnumToSO((StructureType)type);

        gameObject.name = structureSo.name;
        if (building != null)
            Destroy(building);
        building = Instantiate(structureSo.building, transform); // look for evaluate the level for the model 

        levelUpTime = (int)Mathf.Round(structureSo.initialLevelUpTime * ((1 + structureSo.levelUpTimeMultiplier) * level - structureSo.levelUpTimeMultiplier));
        levelUpPrice = (int)Mathf.Round(structureSo.initialLevelUpPrice * ((1 + structureSo.levelUpPriceMultiplier) * level - structureSo.levelUpPriceMultiplier));

        gameObject.GetComponent<BoxCollider>().size = new Vector3(structureSo.size[0], 1, structureSo.size[1]);
        gameObject.GetComponent<BoxCollider>().center = new Vector3(structureSo.size[0] / 2, 0.5f, structureSo.size[1] / 2);

        if (building.GetComponent<IAction>() != null)
            building.GetComponent<IAction>().SetValues(Level);

        //gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    public void LoadValues(int level_, uint? levelUpEventKey_, DateTime? levelUpDate_)
    {
        level = level_;
        levelUpEventKey = levelUpEventKey_;
        levelUpDate = levelUpDate_;

        SetValues();
    }

    public void Demolish()
    {
        StartCoroutine(Demolish_());
    }
    private IEnumerator Demolish_()
    {
        //ParticleSystem .Play();
        //yield return new WaitWhile(() => ParticleSystem .isPlaying)

        yield return null;
        Destroy(gameObject);
    }

    public void Cancel()
    {
        ConstructionZone.Instance.Cancel();
    }

    public void LevelUp()
    {
        if (level == 0)
        {
            BuildUp();
            return;
        }

        if (ResoucesManager.Instance.PayCash(levelUpPrice) == false)
            return;


        if (levelUpEventKey == null)
        {
            ConstructionZone.Instance.Cancel();

            /*
            buildFarm = FarmersAndBuildersManager.SingletonInstance.FindBuilder();
            buildFarm.BuildingLevelUp(this);
            */

            levelUpEventKey = TimeEventsManager.Instance.CreateNewKey();
            TimeEventsManager.Instance.CreateTimeEvent((uint)levelUpEventKey, new TimeSpan(0, 0, levelUpTime));
            levelUpDate = DateTime.Now;
        }

        //working particles or mesh, feedback, something to know it's leveling up
    }
    public void BuildUp() // we don't use costructionTime :D
    {
        if (transform.GetComponentInParent<ConstructionZone>().Construct()) // need to clean old cells
        {
            if (level == 0)
            {
                level = 1;
                SetValues();
            }
        }
    }
    void FinishLevelUp(uint key_)
    {
        if (levelUpEventKey != null)
        {
            if (levelUpEventKey == key_)
            {
                level++;
                levelUpEventKey = null;
                levelUpDate = null;
                SetValues();

                /*buildFarm.FinishingLevelUp();
                buildFarm = null;*/
            }
        }
    }
    /// <summary>
    /*private IEnumerator LevelUp_()
    {
        //lock production
        //set level up vfx

        while (levelUpeTime > timer)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //ParticleSystem .Play();
        //change bulding mesh;
        //yield return new WaitWhile(() => ParticleSystem .isPlaying)

        yield return null;

        level++;
        levelUpePrice = (int)(levelUpePrice * 1.4f);
        levelUpeTime = levelUpeTime * 1.4f;
    }*/
    /// </summary>


    public void Action()
    {
         building.GetComponent<IAction>().Action();
    }
}
[System.Serializable]
public class StructureData
{
    public StructureType? type;
    public int level;
    public uint? levelUpEventKey;
    public DateTime? levelUpDate;
}
