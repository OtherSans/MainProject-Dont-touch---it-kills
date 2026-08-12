using System.Collections;
using NavMeshPlus.Components;
using UnityEngine;

public class LevelNavMeshBuilder : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface navMeshSurface;

    private IEnumerator Start()
    {
        if (navMeshSurface == null)
        {
            navMeshSurface =
                GetComponent<NavMeshSurface>();
        }

        if (navMeshSurface == null)
        {
            Debug.LogError(
                $"{name}: NavMeshSurface не найден.",
                this
            );

            yield break;
        }

        // Ждём, чтобы все RandomRoomSlot успели выполнить Start()
        // и создать комнаты.
        yield return null;

        navMeshSurface.BuildNavMesh();
    }
}
