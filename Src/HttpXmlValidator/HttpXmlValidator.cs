using System;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.PipelineComponents.HttpXmlValidator
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("13975C44-8A9D-4AA8-8443-62ACA160C0FC")]
    [ComponentCategory(CategoryTypes.CATID_Validate)]
    public partial class HttpXmlValidator : IComponent, IBaseComponent, IComponentUI, IPersistPropertyBag
    {
        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            throw new NotImplementedException();
        }


        

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            throw new NotImplementedException();
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            throw new NotImplementedException();
        }
    }
}
