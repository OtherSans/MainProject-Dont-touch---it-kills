using System.Collections;
using UnityEngine;

public class FsmEnemyStateSkewered : FsmState
{
    private readonly EnemyController enemy;

    private SwordController sword;
    private PlayerController player;
    private SkeweredEnemyInteractable interactable;

    private Quaternion originalRootRotation;
    private Quaternion originalVisualRotation;

    public FsmEnemyStateSkewered(
        Fsm fsm,
        EnemyController enemy)
        : base(fsm)
    {
        this.enemy = enemy;
    }

    public override void Enter(FsmContext context)
    {
        Debug.Log("Skewer State [ENTER]");

        if (context is not FsmSkewerContext skewer)
        {
            Debug.LogError("SkewerContext expected.");
            return;
        }

        sword = skewer.Sword;

        player =
            sword.GetComponentInParent<PlayerController>();

        interactable =
            enemy.GetComponent<SkeweredEnemyInteractable>();

        // Сохраняем исходный поворот.
        originalRootRotation =
            enemy.transform.rotation;

        if (enemy.Visual != null)
        {
            originalVisualRotation =
                enemy.Visual.localRotation;
        }

        // Отключаем AI.
        if (enemy.Agent != null)
            enemy.Agent.enabled = false;

        enemy.OnSkewered();

        // Отключаем физику врага.
        if (enemy.Rigidbody != null)
        {
            enemy.Rigidbody.linearVelocity =
                Vector2.zero;

            enemy.Rigidbody.angularVelocity = 0f;

            enemy.Rigidbody.simulated = false;
        }

        if (enemy.Collider != null)
            enemy.Collider.enabled = false;



        // Прикрепляем КОРЕНЬ врага к мечу.
        enemy.transform.SetParent(
            skewer.SkewerPoint
        );

        enemy.transform.localPosition =
            enemy.SkeweredLocalPosition;

        /*
         * Сам корень не наклоняем.
         * Он просто повторяет ориентацию SkewerPoint.
         */
        enemy.transform.localRotation =
            Quaternion.identity;

        /*
         * А визуальную часть можно повернуть
         * отдельно как угодно.
         */
        if (enemy.Visual != null)
        {
            enemy.Visual.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    enemy.SkeweredVisualRotation
                );
        }

        if (player == null)
        {
            Debug.LogError(
                "PlayerController не найден среди родителей SwordController.",
                sword
            );

            return;
        }

        if (interactable == null)
        {
            Debug.LogError(
                "На враге отсутствует SkeweredEnemyInteractable.",
                enemy
            );

            return;
        }

        interactable.EnableInteraction(
            player,
            sword
        );
    }

    public override void Exit()
    {
        if (interactable != null)
        {
            interactable.DisableInteraction();
        }

        enemy.OnUnskewered();

        /*
         * Сначала отвязываем от меча.
         */
        enemy.transform.SetParent(null);

        /*
         * Возвращаем исходный поворот корня.
         */
        enemy.transform.rotation =
            originalRootRotation;

        /*
         * Возвращаем исходный поворот визуала.
         */
        if (enemy.Visual != null)
        {
            enemy.Visual.localRotation =
                originalVisualRotation;
        }

        if (enemy.Collider != null)
            enemy.Collider.enabled = true;

        if (enemy.Rigidbody != null)
        {
            enemy.Rigidbody.simulated = true;
            enemy.Rigidbody.linearVelocity =
                Vector2.zero;
        }

        if (enemy.Agent != null)
            enemy.Agent.enabled = true;

        interactable = null;
        player = null;
        sword = null;

        Debug.Log("Skewer State [EXIT]");
    }

    public override void Update()
    {
    }
}
