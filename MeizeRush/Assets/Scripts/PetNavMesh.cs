using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Unity.AI.Navigation;
using NavMeshPlus.Components;

public class PetNavMesh : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private GameObject agent;
    private Transform player;
    public GameObject petPrefab;
    public Sprite slime;
    public Sprite lula;


    IEnumerator WaitAndGenerateProceduralNavMesh()
    {
        yield return new WaitForSeconds(3f);
        GenerateProceduralNavMesh();
    }

    IEnumerator Wait4Seconds() { yield return new WaitForSeconds(4f); }

    void GenerateProceduralNavMesh()
    {
        // yield return new WaitForSeconds(2f);
        // Gere o NavMesh
        navMeshSurface.BuildNavMeshAsync();
    }

    void Start()
    {
        // Chame este método depois de gerar o seu cenário procedural
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
        // GenerateProceduralNavMesh();
        StartCoroutine(WaitAndGenerateProceduralNavMesh());
    }

    void Update()
    {
        // Atualize o destino do agente para seguir o player
        if (agent != null && player != null)
        {
            UnityEngine.AI.NavMeshAgent navMeshAgent =
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>();
            Vector3 position = new Vector3(player.position.x, player.position.y, 0);
            navMeshAgent.SetDestination(position);
        }
    }

    public void comecar()
    {
        int num = Random.Range(0, 10);

        if (agent == null)
        {
            Vector3 position = new Vector3(player.position.x + 1, player.position.y, 0);
            Instantiate(petPrefab, position, Quaternion.identity);
            agent = GameObject.FindGameObjectWithTag("Pet");
            if (num % 2 == 0)
            {
                agent.GetComponent<SpriteRenderer>().sprite = lula;

            }
            else agent.GetComponent<SpriteRenderer>().sprite = slime;
            var navAgent = agent.GetComponent<UnityEngine.AI.NavMeshAgent>();
            navAgent.updateRotation = false;
            navAgent.updateUpAxis = false;
            navAgent.stoppingDistance = 2f;
        }
    }
}
