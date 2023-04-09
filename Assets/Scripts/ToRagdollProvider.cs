using NaughtyAttributes;
using UnityEngine;

public class ToRagdollProvider : MonoBehaviour
{
    [SerializeField] Rigidbody DamagableRigidbody;

    [ContextMenu(nameof(ToRagdoll))]
    [Button]
    public void ToRagdoll()
    {
        if (TryGetComponent(out Animator animator)) 
        {
            animator.enabled = false;
        }

        if (TryGetComponent(out EnemyAI enemyAI))
        {
            enemyAI.SetState(EnemyAI.EnemyState.Dead);
            enemyAI.enabled = false;
        }

        //if (TryGetComponent(out NavMeshAgent navMeshAgent))
        //{
        //    navMeshAgent.enabled = false;
        //}

        if (TryGetComponent(out Damagable damagable))
        {
            damagable.enabled = false;
        }

        if (TryGetComponent(out Health health))
        {
            health.enabled = false;
        }

        if (DamagableRigidbody) Destroy(DamagableRigidbody);

        var rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        var characterJoints = gameObject.GetComponentsInChildren<CharacterJoint>();
        foreach(var joint in characterJoints)
        {
            joint.enableProjection = true;
            joint.enablePreprocessing = true;
        }

        var skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(var renderer in skinnedMeshRenderers)
        {
            renderer.updateWhenOffscreen = true;
        }
    }
}
