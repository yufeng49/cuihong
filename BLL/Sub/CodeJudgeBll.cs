using Dll.Sub;
using Entity.entity;

namespace BLL.Sub
{
    public class CodeJudgeBll : Base.BaseServer<CodeJudge>
    {
        private CodeJudgeDal codeJudgeDal;
        public CodeJudgeBll()
        {
            baseDal = new CodeJudgeDal();
            codeJudgeDal = baseDal as CodeJudgeDal;
        }

        public bool JudgeRepetition(string code, int remark)
        {
            if (remark == 1)
            {
                return false;
            }
            return codeJudgeDal.JudgeRepetition(code);

        }
    }
}
