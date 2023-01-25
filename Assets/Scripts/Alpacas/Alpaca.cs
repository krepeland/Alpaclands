using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpaca : MonoBehaviour
{
    [SerializeField] float TimeToJump;
    [SerializeField] float JumpSpeed = 1;
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector3 TargetPos;
    float jumpDistance;
    float JumpT;
    float height;
    bool IsInJump;

    [SerializeField] Transform RendererTransform;
    [SerializeField] MeshFilter RendererMeshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Mesh MeshStay;
    [SerializeField] Mesh MeshJump;


    public AnimationCurve JumpDispersion;
    public float MinJumpDistance = 0.5f;
    public float MaxJumpDistance = 3;
    public Vector3 Normal;

    [SerializeField] private ParticleSystem happyParticleSystem;
    [SerializeField] private Color[] happyParticleSystemColors;
    [SerializeField] private Material[] AlpacaMaterials;

    [SerializeField] private Transform DropCoin;
    private void Start()
    {
        StartPos = transform.position;
        TargetPos = StartPos;
    }

    public void SetAlpacaColor(AlpacaColor alpacaColor)
    {
        var main = happyParticleSystem.main;
        main.startColor = happyParticleSystemColors[(int)alpacaColor];
        meshRenderer.material = AlpacaMaterials[(int)alpacaColor];
        AlpacasManager.singleton.AddAlpacaCount(alpacaColor, this);
    }

    public void CallHappy() {
        happyParticleSystem.Play();

        var rand = Random.Range(1, 101);
        if (rand <= AlpacasManager.Luck) {
            Instantiate(DropCoin, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 0, 0));
        }
    }

    void Update()
    {
        if (IsInJump)
        {
            if (jumpDistance <= 0 || height == 0)
            {
                FinishJump();
                return;
            }
            JumpT += Time.deltaTime * JumpSpeed / jumpDistance;
            Vector3 targetPos = Vector3.Lerp(StartPos, TargetPos, JumpT) +
                new Vector3(0, height / 4f + -height * (JumpT - 0.5f) * (JumpT - 0.5f), 0);

            RendererTransform.LookAt(targetPos);
            transform.position = targetPos;
            if (JumpT >= 1) {
                FinishJump();
            }
        }
        else
        {
            TimeToJump -= Time.deltaTime;
            if (TimeToJump <= 0)
            {
                TimeToJump += Random.Range(0.5f, 2f);
                Jump();
            }
        }
    }

    void Jump() {
        IsInJump = true;
        jumpDistance = Vector3.Distance(StartPos, TargetPos);
        height = jumpDistance * 1.5f;
        JumpT = 0;
        if (jumpDistance > 0.1f)
        {
            RendererMeshFilter.mesh = MeshJump;
        }
    }

    void FinishJump() {
        IsInJump = false;
        transform.position = TargetPos;
        StartPos = TargetPos;
        RendererMeshFilter.mesh = MeshStay;

        if (jumpDistance > 0.1f)
        {
            RendererTransform.rotation = Quaternion.Euler(
                0,
                RendererTransform.localEulerAngles.y,
                RendererTransform.localEulerAngles.z);

            var forward = RendererTransform.forward;

            RendererTransform.LookAt(transform.position + Normal + Vector3.up);
            RendererTransform.localRotation = Quaternion.Euler(RendererTransform.localEulerAngles.x + 90, RendererTransform.localEulerAngles.y, RendererTransform.localEulerAngles.z);

            var delta = Vector3.SignedAngle(RendererTransform.forward, forward, RendererTransform.up);
            RendererTransform.Rotate(Vector3.up, delta, Space.Self);
        }

        if (GetNextJumpPoint(out var hit))
        {
            Normal = hit.normal;
            TargetPos = hit.point;
        }
    }

    bool GetNextJumpPoint(out RaycastHit hit)
    {
        var distance = Mathf.Lerp(MinJumpDistance, MaxJumpDistance, JumpDispersion.Evaluate(Random.Range(0f, 1f)));
        var direction = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized * distance;
        var targetPos = transform.position + direction;
        if(AlpacasManager.singleton.CheckIsPositionSuitable(targetPos, out hit))
        {
            return true;
        }
        return false;
    }
}
