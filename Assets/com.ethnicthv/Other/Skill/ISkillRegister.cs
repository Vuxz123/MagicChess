using com.ethnicthv.Other.Skill.Params;
using UnityEngine;

namespace com.ethnicthv.Other.Skill
{
    public interface ISkillRegister
    {
        public void RegisterSkill(int skillId, SkillParamsSchema skillParamsSchema, GameObject skillPrefab);
        void UnregisterSkill(int skillId);
    }
}