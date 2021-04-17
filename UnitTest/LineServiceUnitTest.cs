using NUnit.Framework;
using ServicesLibrary.LineServices;
using ServicesLibrary.CacheServices;
using ServicesLibrary.Models.Line;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    public class Tests
    {
        private ILineMessageService _lineMessageService;
        private readonly string channel = "yUV7WEF2IRuJ9sYT/UWpzlFfjkv4tVNChXGqseOhPzPe1aHQQ2asCoNhqw7X0cTHPLPrMu+tZMv1g/h/IrOz4CfMZWNgYbtdmVr3TLkcpE2wIltuR4MGeYHgJjLXchZ5jdSXAm4DSXKTFlrZTatL5QdB04t89/1O/w1cDnyilFU=";
        private readonly string userid = "U3e8ed1ca421584c71bf79fe1891c96a3";
        private readonly ICacheService _cacheService = null;

        [SetUp]
        public void Setup()
        {
            _lineMessageService = new LineMessageService(_cacheService);
        }

        [Test]
        public async Task GetUserProfile()
        {
            object result = await _lineMessageService.GetProfile(channel, userid).ConfigureAwait(false);

            Assert.AreNotEqual(null, result);
        }

        [Test]
        public void ValidateLineTextMessageSend()
        {
            LineMessageRequestModel req = new LineMessageRequestModel();
            Assert.IsTrue(ValidateModel(req).Any(
              v => v.MemberNames.Contains("LineChannelAccessToken") &&
                   v.ErrorMessage.Contains("required")

                   ));
            Assert.IsTrue(ValidateModel(req).Any(
             v =>
                  v.MemberNames.Contains("To") &&
                  v.ErrorMessage.Contains("required")

                  ));
            Assert.IsTrue(ValidateModel(req).Any(
             v =>
                v.MemberNames.Contains("Messages") &&
                v.ErrorMessage.Contains("required")

                ));

            req.Messages = new string[] { string.Empty };
            Assert.IsTrue(ValidateModel(req).Any(
             v =>
             v.MemberNames.Contains("Messages") &&
             v.ErrorMessage.Contains("required")

             ));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}