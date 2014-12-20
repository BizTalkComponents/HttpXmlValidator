using Microsoft.XLANGs.BaseTypes;

namespace BizTalkComponents.HttpXmlValidator.Tests.UnitTests
{
    using Microsoft.XLANGs.BaseTypes;


    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BizTalk_Server_Project3.TestSchema", @"Test")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] { @"Test" })]
    public sealed class TestSchema : Microsoft.BizTalk.TestTools.Schema.TestableSchemaBase
    {

        [System.NonSerializedAttribute()]
        private static object _rawSchema;

        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BizTalk_Server_Project3.TestSchema"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://BizTalk_Server_Project3.TestSchema"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Test"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" name=""RequiredElement"" type=""xs:string"" />
        <xs:element name=""IntegerElement"" type=""xs:int"" />
        <xs:element minOccurs=""0"" name=""OptionalElement"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

        public TestSchema()
        {
        }

        public override string XmlContent
        {
            get
            {
                return _strSchema;
            }
        }

        public override string[] RootNodes
        {
            get
            {
                string[] _RootElements = new string[1];
                _RootElements[0] = "Test";
                return _RootElements;
            }
        }

        protected override object RawSchema
        {
            get
            {
                return _rawSchema;
            }
            set
            {
                _rawSchema = value;
            }
        }
    }
}
