using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Material activatedMaterial;
    public Material defaultMaterial;
    public MeshRenderer checkpointRenderer;

    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isActivated)
            {
                isActivated = true;

                if (checkpointRenderer != null && activatedMaterial != null)
                {
                    checkpointRenderer.material = activatedMaterial;
                }

                GameManager.Instance.SetCurrentCheckpoint(this);
            }
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        return transform.position;
    }

    public void Reset()
    {
        isActivated = false;
        if (checkpointRenderer != null && defaultMaterial != null)
        {
            checkpointRenderer.material = defaultMaterial;
        }
    }
}