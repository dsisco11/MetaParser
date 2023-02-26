using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal class SetTokenID : IMetaCodeBuilder
    {
        public SetTokenID(IMetaCodeBuilder body)
        {
            Body = body;
        }

        public IMetaCodeBuilder Body { get; }

        public void WriteTo(MetaParserContext context)
        {
            var token = context.Tokens.WorkingSet.Single();
            var wr = context.writer;
            wr.WriteLine($"id = {context.Get_TokenId_Ref(token.IdName)};");
            Body.WriteTo(context);
        }
    }
}
