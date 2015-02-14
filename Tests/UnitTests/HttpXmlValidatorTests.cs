using System;
using System.IO;
using BizTalkComponents.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.PipelineComponents.HttpXmlValidator.Tests.UnitTests
{
    [TestClass]
    public class HttpXmlValidatorTests
    {
        [TestMethod]
        public void TestOk()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <RequiredElement>RequiredElement_0</RequiredElement>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), "true");
            message.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), "token");
            message.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), "token2");
            message.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), "id");
            

            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);

            Assert.AreEqual(1, result.Count);
            object status;
            Assert.IsFalse(result[0].Context.TryRead(new ContextProperty(WCFProperties.OutboundHttpStatusCode),out status));
        }

        [TestMethod]
        public void TestMissingRequiredField()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), "true");
            message.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), "token");
            message.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), "token2");
            message.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), "id");


            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].Context.IsPromoted(new ContextProperty(SystemProperties.IsRequestResponse)));
            Assert.IsTrue(result[0].Context.IsPromoted(new ContextProperty(SystemProperties.RouteDirectToTP)));
            Assert.IsTrue(result[0].Context.IsPromoted(new ContextProperty(SystemProperties.EpmRRCorrelationToken)));
            Assert.IsTrue(result[0].Context.IsPromoted(new ContextProperty(SystemProperties.CorrelationToken)));
            Assert.IsTrue(result[0].Context.IsPromoted(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID)));
            Assert.AreEqual("400", result[0].Context.Read(new ContextProperty(WCFProperties.OutboundHttpStatusCode)));

            string msg;

            using (var sr = new StreamReader(result[0].BodyPart.GetOriginalDataStream()))
            {
                msg = sr.ReadToEnd();
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFailedPreReqRequestResponse()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <RequiredElement>RequiredElement_0</RequiredElement>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), "token");
            message.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), "token2");
            message.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), "id");


            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFailedPreReqEpmCorrelationToken()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <RequiredElement>RequiredElement_0</RequiredElement>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), "true");
            message.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), "token2");
            message.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), "id");


            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFailedPreReqCorrelationToken()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <RequiredElement>RequiredElement_0</RequiredElement>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), "true");
            message.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), "token");
            message.Context.Promote(new ContextProperty(SystemProperties.ReqRespTransmitPipelineID), "id");


            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFailedPreReqPipelineId()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            pipeline.AddDocSpec(typeof(TestSchema));

            var validator = new PipelineComponents.HttpXmlValidator.HttpXmlValidator();

            const string msgStr = @"<ns0:Test xmlns:ns0='http://BizTalk_Server_Project3.TestSchema'>
  <RequiredElement>RequiredElement_0</RequiredElement>
  <IntegerElement>10</IntegerElement>
  <OptionalElement>OptionalElement_0</OptionalElement>
</ns0:Test>";

            var message = MessageHelper.CreateFromString(msgStr);

            message.Context.Promote(new ContextProperty(SystemProperties.IsRequestResponse), "true");
            message.Context.Promote(new ContextProperty(SystemProperties.EpmRRCorrelationToken), "token");
            message.Context.Promote(new ContextProperty(SystemProperties.CorrelationToken), "token2");


            pipeline.AddComponent(validator, PipelineStage.Validate);

            var result = pipeline.Execute(message);
        }
    }
}
