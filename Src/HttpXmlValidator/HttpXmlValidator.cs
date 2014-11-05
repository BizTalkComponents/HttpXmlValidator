using System.IO;
using System.Text;
using BizTalkComponents.Utils.ContextPropertyHelpers;
using BizTalkComponents.Utils.PropertyBagHelpers;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component.Utilities;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.PipelineComponents.HttpXmlValidator
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("13975C44-8A9D-4AA8-8443-62ACA160C0FC")]
    [ComponentCategory(CategoryTypes.CATID_Validate)]
    public partial class HttpXmlValidator : IComponent, IBaseComponent, IComponentUI, IPersistPropertyBag
    {
        private const string RecoverableInterchangeProcessingPropertyName = "RecoverableInterchangeProcessing";

        public bool RecoverableInterchangeProcessing { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            var validator = new XmlValidator {RecoverableInterchangeProcessing = RecoverableInterchangeProcessing};

            try
            {
                validator.Execute(pContext, pInMsg);
            }
            catch (XmlValidatorException ex)
            {
                pInMsg.Context.Write(new ContextProperty(SystemProperties.RouteDirectToTP), "true");
                pInMsg.Context.Promote(new ContextProperty(WCFProperties.OutboundHttpStatusCode),"400");

                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);
                sw.Write(GetExceptionDetails(ex));
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                pInMsg.BodyPart.Data = ms;
            }

            return pInMsg;
        }

        private string GetExceptionDetails(XmlValidatorException ex)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < ex.ArgumentCount; i++)
            {
                sb.AppendLine(ex.GetArgument(i));
            }

            return sb.ToString();
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            var recoverableInterchangeProcessing = PropertyBagHelper.ReadPropertyBag(propertyBag, RecoverableInterchangeProcessingPropertyName);

            if (recoverableInterchangeProcessing != null)
            {
                RecoverableInterchangeProcessing = (bool)recoverableInterchangeProcessing;
            }
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PropertyBagHelper.WritePropertyBag(propertyBag, RecoverableInterchangeProcessingPropertyName, RecoverableInterchangeProcessing);
        }
    }
}
