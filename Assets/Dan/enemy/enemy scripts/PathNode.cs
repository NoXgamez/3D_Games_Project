using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode NextNode;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.25f);

        if(NextNode != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, NextNode.transform.position);

            Gizmos.color = Color.red;
            Vector3 direction = (NextNode.transform.position - transform.position).normalized;
            Gizmos.DrawLine(transform.position, transform.position + (direction * 2));
        }
    }
}
