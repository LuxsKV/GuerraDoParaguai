using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviour
{

    public int health;
    public bool isLocalPlayer;

    public RectTransform healthBar;
    private float originalHealthBarSize;


    [Header("UI")]
    public TextMeshProUGUI healthText;
    public Image panelDamage;


    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
        AlterarTransparenciaDoPainel(0.1f);
    }
    private void Update()
    {
        //healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

    }

    [PunRPC]
    public void TakeDamege(int _damege)
    {
        health -= _damege;

        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);


        healthText.text = health.ToString();


        if (health <= 0)
        {

            if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();

                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }




            Destroy(gameObject);

        }
        if (health == 75)
        {
            if (isLocalPlayer)
            {
                AlterarTransparenciaDoPainel(0.1f);
            }
        }
        if (health == 50)
        {
            if (isLocalPlayer)
            {
                AlterarTransparenciaDoPainel(0.25f);
            }
        }if (health == 25)
        {
            if (isLocalPlayer)
            {
                AlterarTransparenciaDoPainel(0.5f);
            }
        }
    }
    void AlterarTransparenciaDoPainel(float alpha)
    {
        if (panelDamage != null)
        {
            Color corAtual = panelDamage.color;
            corAtual.a = alpha;
            panelDamage.color = corAtual;
            Debug.Log("Transparência alterada com sucesso!");
        }
        else
        {
            Debug.LogError("O objeto Image não foi atribuído ao script!");
        }
    }
    
}
