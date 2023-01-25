using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressPoint : MonoBehaviour
{
    Transform target;
    [SerializeField] float Speed = 100;
    [SerializeField] Transform PreviewTransform;
    float startDist;

    private void Start()
    {
        target = ProgressBar.singleton.TargetPos;
        startDist = Vector3.Distance(transform.position, target.transform.position);
    }

    void Update()
    {
        var dist = Vector3.Distance(transform.position, target.transform.position);
        if (Time.deltaTime * Speed / dist >= 1) {
            ProgressBar.singleton.PointGetted();
            Destroy(gameObject);
        }
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * Speed / dist);

        var part = dist / startDist;
        var height = 1000;
        PreviewTransform.localPosition = new Vector3(0, height / 4f + -height * (part - 0.5f) * (part - 0.5f));
    }
}
