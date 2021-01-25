using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite damageSprite;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;
    public float tempHealth;
    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
        tempHealth = playerCurrentHealth.RuntimeValue / 2;
        StartUpdateHearts();
    }
    void Update()
    {

    }
    public void InitHearts()
    {
        for(int i = 0; i < heartContainers.RuntimeValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
        for(int i = (int)heartContainers.RuntimeValue; i < 5; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }
    public void StartUpdateHearts()
    {
        StartCoroutine(UpdateHearts());
    }
    IEnumerator UpdateHearts()
    {

        tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < heartContainers.RuntimeValue; i++)
        {
            if(i == tempHealth  || i  == tempHealth -0.5)
            {
                Debug.Log(tempHealth);
                Debug.Log(i);
                hearts[i].sprite = damageSprite;
                yield return new WaitForSeconds(0.20f);
            }
            if (i <= tempHealth - 1)
            {
                hearts[i].sprite = fullHeart;
            }else if(i >= tempHealth)
            {
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                hearts[i].sprite = halfHeart;
            }
        }
    }
}
