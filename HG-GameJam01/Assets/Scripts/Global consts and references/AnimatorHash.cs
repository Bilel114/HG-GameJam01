using UnityEngine;

public static class AnimatorHash
{
    public static readonly int Player_MoveDirectionX = Animator.StringToHash("MoveDirectionX");
    public static readonly int Player_MoveDirectionY = Animator.StringToHash("MoveDirectionY");
    public static readonly int Player_Dodge = Animator.StringToHash("Dodge");

    public static readonly int Boss_Move = Animator.StringToHash("BossMove");
    public static readonly int Boss_Attack1Anticipation = Animator.StringToHash("BossAttack1_Anticipation");
    public static readonly int Boss_Attack1Charge = Animator.StringToHash("BossAttack1_Charge");
    public static readonly int Boss_Attack1End = Animator.StringToHash("BossAttack1_End");
}
