using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace BizTalkComponents.PipelineComponents.HttpXmlValidator
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("13975C44-8A9D-4AA8-8443-62ACA160C0FC")]
    [ComponentCategory(CategoryTypes.CATID_Validate)]
    public partial class HttpXmlValidator : IComponent, IBaseComponent, IComponentUI, IPersistPropertyBag
    {
        private const string RecoverableInterchangeProcessingPropertyName = "RecoverableInterchangeProcessing";


        [DisplayName("Recoverable Interchange Processing")]
        [Description("http://msdn.microsoft.com/en-us/library/dd224149.aspx")]
        public bool RecoverableInterchangeProcessing { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            object isRequestResponse;
            object epmRRCorrelationToken;
            object correlationToken;
            object reqRespTransmitPipelineID;

            if (!pInMsg.Context.TryRead(new ContextProperty(SystemProperties.IsRequestResponse),
                    out isRequestResponse))
            {
                throw new InvalidOperationException("This component can only be used on request response ports.");
            }

            if (!pInMsg.Context.TryRead(new ContextProperty(SystemProperties.EpmRRCorrelationToken),
                    out epmRRCorrelationToken))
            {
                throw new InvalidOperationException("The request message is missing it's correlation token");
            }

            if (!pInMsg.Context.TryRead(new ContextProperty(SystemProperties.CorrelationToken),
                   out correlationToken))
            {
                throw new InvalidOperationException("The request message is missing it's correlation token");
            }

            if (!pInMsg.Context.TryRead(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID),
                   out reqRespTransmitPipelineID))
            {
                throw new InvalidOperationException("The request is missing the context property ReqRespTransmitPipelineID");
            }

            var validator = new XmlValidator { RecoverableInterchangeProcessing = RecoverableInterchangeProcessing };


            try
            {
                pInMsg = validator.Execute(pContext, pInMsg);
            }
            catch (XmlValidatorException ex)
            {
                pInMsg = GetValidationErrorResponse(pContext, pInMsg, epmRRCorrelationToken, correlationToken, reqRespTransmitPipelineID, ex);
            }

            return pInMsg;
        }

        private IBaseMessage GetValidationErrorResponse(IPipelineContext pContext, IBaseMessage pInMsg, object epmRRCorrelationToken, object correlationToken, object reqRespTransmitPipelineID, XmlValidatorException ex)
        {
            var outMsg = pContext.GetMessageFactory().CreateMessage();
            outMsg.AddPart("Body", pInMsg.BodyPart, true);

            outMsg.Context.Promote(new ContextProperty(SystemProperties.RouteDirectToTP), true);
            outMsg.Context.Write(new ContextProperty(WCFProperties.OutboundHttpStatusCode), "400");
            outMsg.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), true);
            outMsg.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), epmRRCorrelationToken);
            outMsg.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), correlationToken);
            outMsg.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), reqRespTransmitPipelineID);

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(GetExceptionDetails(ex));
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            outMsg.BodyPart.Data = ms;

            return outMsg;
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
