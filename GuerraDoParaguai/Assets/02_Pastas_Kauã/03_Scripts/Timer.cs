using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Timer : MonoBehaviourPunCallbacks, IPunObservable
{
    public float timeRemaining = 5f; //900f = 15 minutes in seconds
    private bool isTimerRunning = true;

    public TextMeshProUGUI timerText; // Referência ao objeto de texto

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InitializeTimer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void InitializeTimer()
    {
        // Inicializar o texto do temporizador
        timerText.text = FormatTime(timeRemaining);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                float updatedTime = timeRemaining - Time.deltaTime;

                // Atualizar o texto do temporizador
                timerText.text = FormatTime(updatedTime);

                // Sincronizar o tempo através da rede usando Photon
                photonView.RPC("UpdateTimer", RpcTarget.OthersBuffered, updatedTime);

                timeRemaining = updatedTime;

                if (timeRemaining < 1)
                {
                    isTimerRunning = false;
                    photonView.RPC("CloseServerAndLoadMenu", RpcTarget.All, isTimerRunning);
                }
            }
        }
    }

    [PunRPC]
    void UpdateTimer(float updatedTime)
    {
        // Atualizar o tempo para outros jogadores
        timeRemaining = updatedTime;
        timerText.text = FormatTime(updatedTime);
    }

    [PunRPC]
    void CloseServerAndLoadMenu(bool isClose)
    {
        // Adicione aqui a lógica para fechar o servidor
        // (por exemplo, chame uma função personalizada de gerenciamento do Photon)

        if (!isClose)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // Carregar a cena "Menu"
                SceneManager.LoadScene("Menu");
                PhotonNetwork.Disconnect();
            }
            MouseLook.instance.UnLockCursor();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Envie o tempo pela rede
            stream.SendNext(timeRemaining);
        }
        else
        {
            // Receba o tempo pela rede
            timeRemaining = (float)stream.ReceiveNext();
            // Atualizar o texto do temporizador para outros jogadores
            timerText.text = FormatTime(timeRemaining);
        }
    }

    string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secondsInt = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, secondsInt);
    }
}
