using Dll.Base;
using Entity.entity;
using System.Data;

namespace Dll.Sub
{
    public class CodeJudgeDal : BaseDal<CodeJudge>
    {
        public bool JudgeRepetition(string code)
        {
            return SqlHelper.OperateExecuteScalar(GenerateSql.connectionString, CommandType.Text, code);
        }
    }
}
