using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isOnGround;
    public bool isGroundSlamming;
    public bool isInvincible;
    public bool isCollectingItem;
    public bool isDead;

    public System.Action<string, bool> OnStateChanged;

    public void SetState(string state, bool value)
    {
        bool oldValue = false;

        switch (state.ToLower())
        {
            case "isonground":
                oldValue = isOnGround;
                isOnGround = value;
                break;
            case "isgroundslamming":
                oldValue = isGroundSlamming;
                isGroundSlamming = value;
                if (value) SetState("isinvincible", true);
                break;
            case "isinvincible":
                oldValue = isInvincible;
                isInvincible = value;
                break;
            case "iscollectingitem":
                oldValue = isCollectingItem;
                isCollectingItem = value;
                break;
            case "isdead":
                oldValue = isDead;
                isDead = value;
                if (value)
                {
                    SetState("isgroundslamming", false);
                    SetState("iscollectingitem", false);
                }
                break;
            default:
                Debug.LogWarning($"Unknown state: {state}");
                return;
        }

        if (oldValue != value)
        {
            OnStateChanged?.Invoke(state, value);
        }
    }

    public bool GetValue(string stateName)
    {
        switch (stateName.ToLower())
        {
            case "isonground":
                return isOnGround;
            case "isgroundslamming":
                return isGroundSlamming;
            case "isinvincible":
                return isInvincible;
            case "iscollectingitem":
                return isCollectingItem;
            case "isdead":
                return isDead;
            default:
                Debug.LogWarning($"Unknown state requested: {stateName}");
                return false;
        }
    }



    public bool CanMove()
    {
        return !isDead && !isCollectingItem && !isGroundSlamming;
    }

    public bool CanJump()
    {
        return isOnGround && !isDead && !isCollectingItem && !isGroundSlamming;
    }

    public bool CanAct()
    {
        return !isDead && !isCollectingItem;
    }
}