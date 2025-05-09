using UnityEngine;

public class Selection : MonoBehaviour
{
    public RectTransform[] options;
    private RectTransform rect;
    private int currentPosition;
    public AudioClip selectionSound;
    public AudioClip interactSound;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(selectionSound);

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition > options.Length - 1)
        {
            currentPosition = 0;
        }

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }
}
