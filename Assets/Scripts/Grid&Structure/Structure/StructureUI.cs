using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StructureUI : MonoBehaviour
{
    Structure structure;

    int level;

    float progress;
    TimeSpan progressTime;
    DateTime levelUpDate;

    [SerializeField] Slider levelUpSlider;
    [SerializeField] GameObject levelUpButton, levelText, levelTimeText, yesButton, noButton;

    private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }
    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }

    private void Start()
    {
        structure = gameObject.GetComponentInParent<Structure>();
    }

    private void OnTick()
    {
        if (structure.LevelUpDate != null)
        {
            if (levelUpSlider.gameObject.activeSelf == true && 
                levelTimeText.activeSelf == true &&
                levelUpButton.activeSelf == false &&
                levelText.activeSelf == false)
            {
                levelUpDate = structure.LevelUpDate.Value;
                progressTime = DateTime.Now - levelUpDate;
                progress = (float)(progressTime.TotalSeconds);
                progress = progress / structure.LevelUpTime;
                levelTimeText.GetComponent<TMP_Text>().text = (((levelUpDate + new TimeSpan(0, 0, structure.LevelUpTime)) - DateTime.Now).Seconds + 1).ToString();
                levelUpSlider.value = progress;

            }
            else
            {
                levelUpSlider.gameObject.SetActive(true);
                levelTimeText.gameObject.SetActive(true);
                levelUpButton.SetActive(false);
                levelText.SetActive(false);
            }
        }
        else
        {
            levelUpSlider.gameObject.SetActive(false);
            levelTimeText.gameObject.SetActive(false);
            //levelUpButton.SetActive(true);
            levelText.SetActive(true);
        }
        
        if (ConstructionManager.Instance.constructionMode && structure.LevelUpDate == null)
        {
            if (ConstructionManager.Instance.currentStructure == structure)
            {
                yesButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(true);
                levelUpButton.SetActive(true);
            }
            else
            {
                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);
                levelUpButton.SetActive(false);
            }
        }
        else
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            levelUpButton.SetActive(false);
        }

        if (structure.Level > 0)
        {
            level = structure.Level;
            levelText.GetComponent<TMP_Text>().text = "Level " + level;
        }
        else
            levelUpButton.SetActive(false);
    }
}
