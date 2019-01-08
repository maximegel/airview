using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AirView.Api.Core
{
    public class HalOutputFormatter : TextOutputFormatter
    {
        private readonly TextOutputFormatter _innerFormatter;

        private HalOutputFormatter(TextOutputFormatter innerFormatter)
        {
            _innerFormatter = innerFormatter;
            foreach (var encoding in innerFormatter.SupportedEncodings) SupportedEncodings.Add(encoding);
        }

        public static HalOutputFormatter ForJson(JsonOutputFormatter jsonFormatter)
        {
            var instance = new HalOutputFormatter(jsonFormatter);
            instance.SupportedMediaTypes.Add("application/hal+json");
            return instance;
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context) =>
            context.ObjectType.HasLinks();

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) =>
            _innerFormatter.WriteResponseBodyAsync(
                new OutputFormatterWriteContext(
                    context.HttpContext, context.WriterFactory, context.ObjectType, context.Object.ToLinkedDto()),
                selectedEncoding);
    }
}