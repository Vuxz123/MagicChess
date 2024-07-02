using com.ethnicthv.Other.Skill.Params;
using UnityEngine;

namespace com.ethnicthv.Other.Skill
{
    public interface ISkillEmitter
    {
        public void EmitSkill(int skillId, Vector3 position, Quaternion rotation, SkillParams skillParams);
    }
}