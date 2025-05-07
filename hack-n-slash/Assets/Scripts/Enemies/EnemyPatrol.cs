using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform leftEdge;
    public Transform rightEdge;

    public Transform enemy;

    public float speed;
    private Vector3 initScale;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        MoveInDirection(1);
    }

    private void MoveInDirection(int _direction)
    {
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * -_direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
