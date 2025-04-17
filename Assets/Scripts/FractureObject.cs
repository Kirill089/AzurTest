using UnityEngine;
using System.Collections;

public class FractureObject : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fractureObject;
    public GameObject explosionVFX;
    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRadius = 10;
    public float fragScaleFactor = 1;

    private GameObject fractObj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    void Explode()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);

            if (fractureObject != null)
            {
                fractObj = Instantiate(fractureObject) as GameObject;

                foreach (Transform t in fractObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);

                    StartCoroutine(Shrink(t, 2)); // запуск плавного уменьшения
                }

                Destroy(fractObj, 5);

                if (explosionVFX != null)
                {
                    GameObject exploVFX = Instantiate(explosionVFX) as GameObject;
                    Destroy(exploVFX, 7);
                }
            }
        }
    }

    private void Reset()
    {
        Destroy(fractObj);
        originalObject.SetActive(true);
    }

    IEnumerator Shrink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (t == null || t.gameObject == null)
            yield break;

        float shrinkDuration = 1.5f;
        float elapsed = 0f;
        Vector3 originalScale = t.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < shrinkDuration)
        {
            if (t == null || t.gameObject == null)
                yield break;

            t.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / shrinkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (t != null)
            t.localScale = targetScale;
    }
}