using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputKey", menuName = "Scriptable Objects/PlayerInputKey")]
public class PlayerInputKey : ScriptableObject
{
    [Header("Player Movement Keys")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    public KeyCode MoveUpKey { get => moveUpKey; set => moveUpKey = value; }

    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    public KeyCode MoveDownKey { get => moveDownKey; set => moveDownKey = value; }

    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    public KeyCode MoveLeftKey { get => moveLeftKey; set => moveLeftKey = value; }

    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    public KeyCode MoveRightKey { get => moveRightKey; set => moveRightKey = value; }
    
    [Header("Player Interaction Key")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    public KeyCode InteractionKey { get => interactionKey; set => interactionKey = value; }
    
    [Header("UI Interaction Key")]
    [SerializeField] private KeyCode uiConfirmKey = KeyCode.Mouse0;
    public KeyCode UiConfirmKey { get => uiConfirmKey; set => uiConfirmKey = value; }

    [SerializeField] private KeyCode uiCancelKey = KeyCode.Mouse1;
    public KeyCode UiCancelKey { get => uiCancelKey; set => uiCancelKey = value; }

    [SerializeField] private KeyCode uiStopMenuKey = KeyCode.Escape;
    public KeyCode UiStopMenuKey { get => uiStopMenuKey; set => uiStopMenuKey = value; }
}
