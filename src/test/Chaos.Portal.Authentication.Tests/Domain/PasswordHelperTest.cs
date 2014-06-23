namespace Chaos.Portal.Authentication.Tests.Domain
{
    using Authentication.Domain;
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class PasswordHelperTest : TestBase
    {
        [Test]
        public void GenerateHash_OneIterationWithoutSalt_DifferentSaltsYieldSameResult()
        {
            var helper = Make_PasswordHelper(1, false);
            var password = "password";

            var r1 = helper.GenerateHash(password, salt: "1");
            var r2 = helper.GenerateHash(password, salt: "2");
            var r3 = helper.GenerateHash(password, salt: "3");

            
            Assert.That(r1, Is.EqualTo(r2));
            Assert.That(r2, Is.EqualTo(r3));
        }
        
        [Test]
        public void GenerateHash_OneIterationWithoutSalt_ShouldBeCompatibleWithV5()
        {
            var helper = Make_PasswordHelper(1, false);
            var password = "1234";

            var r1 = helper.GenerateHash(password, salt: "1");

            Assert.That(r1, Is.EqualTo("139f69c93c042496a8e958ec5930662c6cccafbf"));
        }
        
        [Test]
        public void GenerateHash_OneIterationWithSalt_ShouldGenerateDifferentHashesForSamePassword()
        {
            var helper = Make_PasswordHelper(1, true);
            var password = "1234";

            var r1 = helper.GenerateHash(password, salt: "12345678");
            var r2 = helper.GenerateHash(password, salt: "23456789");
            var r3 = helper.GenerateHash(password, salt: "34567890");

            Assert.That(r1, Is.Not.EqualTo(r2));
            Assert.That(r1, Is.Not.EqualTo(r3));
            Assert.That(r2, Is.Not.EqualTo(r3));
        }
        
        [Test]
        public void GenerateHash_MultipleIterationWithoutSalt_ShouldGenerateDifferentHashesForSamePassword()
        {
            var password = "1234";

            var r1 = Make_PasswordHelper(10, false).GenerateHash(password, salt: "1");
            var r2 = Make_PasswordHelper(100, false).GenerateHash(password, salt: "1");
            var r3 = Make_PasswordHelper(100000, false).GenerateHash(password, salt: "1");

            Assert.That(r1, Is.Not.EqualTo(r2));
            Assert.That(r1, Is.Not.EqualTo(r3));
            Assert.That(r2, Is.Not.EqualTo(r3));
        }

        private static PasswordHelper Make_PasswordHelper(uint iterations, bool useSalt)
        {
            return new PasswordHelper(new PasswordSettings
                {
                    Iterations = iterations,
                    UseSalt = useSalt
                });
        }
    }
}