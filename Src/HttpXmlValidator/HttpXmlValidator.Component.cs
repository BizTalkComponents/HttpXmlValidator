using System;
using System.Collections;

namespace BizTalkComponents.PipelineComponents.HttpXmlValidator
{
    public partial class HttpXmlValidator
    {
        public string Name { get { return "HttpXmlValidator"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get
        {
            return
                "Validates incomming XML messages and route invalid messages back to the client with HTTP 400 bad request.";
        } }

        public IEnumerator Validate(object projectSystem)
        {
            //Nothing to validate at design time.
            return null;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("310651A9-4963-4F41-A82F-30B2F1A9613D");
        }

        public void InitNew()
        {
            
        }
    }
}