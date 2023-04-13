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
    [SerializeField] GameObject levelUpButton, levelText;

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
                levelUpButton.activeSelf == false &&
                levelText.activeSelf == false)
            {
                levelUpDate = structure.LevelUpDate.Value;
                progressTime = DateTime.Now - levelUpDate;
                progress = (float)(progressTime.TotalSeconds);
                progress = progress / structure.LevelUpTime;
                levelUpSlider.value = progress;
            }
            else
            {
                levelUpSlider.gameObject.SetActive(true);
                levelUpButton.SetActive(false);
                levelText.SetActive(false);
            }
        }
        else
        {
            levelUpSlider.gameObject.SetActive(false);
            levelUpButton.SetActive(true);
            levelText.SetActive(true);
        }

        if (structure.Level > 0)
        {
            level = structure.Level;
            levelText.GetComponent<TMP_Text>().text = "Level " + level;
        }
    }
}
