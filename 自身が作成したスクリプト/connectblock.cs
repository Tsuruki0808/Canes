using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBlock : MonoBehaviour
{
    [SerializeField] private AudioSource Sound; // AudioSource
    [SerializeField] private AudioClip ConnectSE;
    public GameObject connectorObj;
    private static GameObject clickedGameObject = null; // 連結する最初のブロック（全インスタンス共通）
    private static bool connectflg = false;

    public float maxRange = 5.0f; // 範囲の半径

    // デバッグ用の円
    private GameObject circleMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    void Start()
    {
        CreatePlacementCircle();
    }

    private void CreatePlacementCircle()
    {
        circleMesh = new GameObject("PlacementCircle");
        meshFilter = circleMesh.AddComponent<MeshFilter>();
        meshRenderer = circleMesh.AddComponent<MeshRenderer>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = new Color(1f, 1f, 0f, 0.1f); // 透明な黄色

        meshRenderer.sortingLayerName = "Background"; // 背景レイヤー
        meshRenderer.sortingOrder = -4;

        circleMesh.SetActive(false);
    }

    private void Update()
    {
        //空白をクリックしたとき用のif分
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null || hit.collider.gameObject.tag != "ConnectBlock")
            {
                // コネクトブロック以外をクリックしたら選択を解除
                Debug.Log("コネクトブロック以外がタッチされたので選択解除");
                connectflg = false;
                clickedGameObject = null;
            }
        }

        if (connectflg && clickedGameObject == gameObject)
        {
            DrawPlacementRange(transform.position, maxRange);
        }
        else
        {
            HidePlacementRange();
        }
    }

    private void OnMouseDown()
    {
        if (clickedGameObject == gameObject)
        {
            Debug.Log("同じブロックを選択したのでキャンセル");
            connectflg = false;
            clickedGameObject = null;
            return;
        }

        if (connectflg)
        {
            Debug.Log("連結するよー");
            Vector3 startPos = clickedGameObject.transform.position;
            Vector3 endPos = transform.position;
            Vector3 midPoint = (startPos + endPos) / 2f;
            float distance = Vector3.Distance(startPos, endPos);
            Debug.Log("block間の距離は" + distance);

            if (distance <= maxRange)
            {
                Sound.PlayOneShot(ConnectSE);
                GameObject bridge = Instantiate(connectorObj, midPoint, Quaternion.identity);
                bridge.transform.localScale = new Vector3(distance, bridge.transform.localScale.y, bridge.transform.localScale.z);
                Vector3 direction = endPos - startPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bridge.transform.rotation = Quaternion.Euler(0, 0, angle);
                connectflg = false;
            }
        }
        else
        {
            Debug.Log("連結ブロックを選択");
            connectflg = true;
            clickedGameObject = gameObject;
        }
    }

    void DrawPlacementRange(Vector2 centerPosition, float radius)
    {
        GenerateCircleMesh(radius);
        circleMesh.transform.position = centerPosition;
        circleMesh.SetActive(true);
    }

    void HidePlacementRange()
    {
        circleMesh.SetActive(false);
    }

    void GenerateCircleMesh(float radius)
    {
        int segments = 64;
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 中心点

        for (int i = 0; i < segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            if (i < segments - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 色をオレンジに変更(RGB,透明度)
        Gizmos.DrawWireSphere(transform.position, maxRange);// 座標に球体を描画


        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f); // 色をオレンジに変更(RGB,透明度)
        Gizmos.DrawWireSphere(transform.position, maxRange);


    }
}
