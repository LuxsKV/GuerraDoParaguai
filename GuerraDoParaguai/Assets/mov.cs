using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mov : MonoBehaviour
{
    public float velocidade = 5.0f;
    public float sensibilidadeMouse = 2.0f;
    public float alturaPulo = 2.0f;
    public float gravidade = 9.8f;

    private CharacterController characterController;
    private Camera playerCamera;
    private Vector3 velocidadeQueda;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Não há um CharacterController associado ao objeto do jogador.");
        }

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("Não há uma câmera associada ao objeto do jogador.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Movimentacao();
        RotacaoMouse();
        AplicarGravidade();
    }

    private void Movimentacao()
    {
        float movimentoFrente = Input.GetAxis("Vertical") * velocidade * Time.deltaTime;
        float movimentoLado = Input.GetAxis("Horizontal") * velocidade * Time.deltaTime;

        Vector3 movimento = transform.TransformDirection(new Vector3(movimentoLado, 0, movimentoFrente));
        characterController.Move(movimento);

        // Pular
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocidadeQueda.y = Mathf.Sqrt(alturaPulo * 2f * gravidade);
        }
    }

    private void RotacaoMouse()
    {
        float rotacaoY = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        transform.Rotate(0, rotacaoY, 0);

        float rotacaoX = -Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        playerCamera.transform.Rotate(rotacaoX, 0, 0);
    }

    private void AplicarGravidade()
    {
        // Aplica gravidade se não estiver no chão
        if (!characterController.isGrounded)
        {
            velocidadeQueda.y -= gravidade * Time.deltaTime;
        }
        else
        {
            // Reseta a velocidade de queda se estiver no chão
            velocidadeQueda.y = -gravidade * 0.5f * Time.deltaTime;
        }

        // Move o jogador para baixo de acordo com a velocidade de queda
        characterController.Move(velocidadeQueda * Time.deltaTime);
    }
}
