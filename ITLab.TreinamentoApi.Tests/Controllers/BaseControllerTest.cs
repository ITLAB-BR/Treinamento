using ITLab.Treinamento.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace ITLab.Treinamento.Api.Tests.Controllers
{
    [TestClass]
    public class BaseControllerTest : BaseController
    {
        [TestMethod]
        public void SanitizeResultTest()
        {
            //Arrange
            var mensagensErro = new string[] { "Mensagem de teste 1. Mensagem de teste 2.", "Mensagem de teste 3." };

            IdentityResult identityResult = new IdentityResult(mensagensErro);

            //Expected
            var mensagensErroExpected = new string[] { "alerts:error.mensagem_de_teste_1", "alerts:error.mensagem_de_teste_2", "alerts:error.mensagem_de_teste_3" };
            IdentityResult identityResultExpected = new IdentityResult(mensagensErroExpected);

            //Act
            var result = BaseController.SanitizeResult(identityResult);


            //Assert
            Assert.IsFalse(identityResult.Succeeded);

            foreach (var item in result.Errors)
            {
                Assert.IsTrue(identityResultExpected.Errors.Single(x => x == item).Any());
            }
        }
    }
}
