using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Tests.Domain
{
    [TestClass]
    public sealed class EmployeeTests
    {
        private static Employee NewEmployee() =>
            new("1037000001", "Ana", "Gómez", Email.Create("ana@logistica.com"), "Auxiliar", OrganizationalArea.Warehouse);

        [TestMethod]
        public void NewEmployee_IsActive()
        {
            Employee employee = NewEmployee();

            Assert.AreEqual(EmploymentStatus.Active, employee.Status);
            Assert.AreNotEqual(Guid.Empty, employee.Id);
        }

        [TestMethod]
        public void Email_InvalidFormat_Throws()
        {
            Assert.ThrowsExactly<BusinessRuleException>(() => Email.Create("no-es-un-correo"));
        }

        [TestMethod]
        public void Email_IsNormalizedToLowerCase()
        {
            Assert.AreEqual("ana@logistica.com", Email.Create("  ANA@Logistica.com ").Value);
        }

        [TestMethod]
        public void Area_UnknownCode_Throws()
        {
            Assert.ThrowsExactly<BusinessRuleException>(() => OrganizationalArea.FromCode("MARKETING"));
        }

        [TestMethod]
        public void Deactivate_Twice_Throws()
        {
            Employee employee = NewEmployee();
            employee.Deactivate();

            Assert.AreEqual(EmploymentStatus.Inactive, employee.Status);
            Assert.ThrowsExactly<BusinessRuleException>(employee.Deactivate);
        }

        [TestMethod]
        public void AssignCredential_ToInactiveEmployee_Throws()
        {
            Employee employee = NewEmployee();
            employee.Deactivate();

            Assert.ThrowsExactly<BusinessRuleException>(() => employee.AssignCredential(CredentialCode.Create("CRED-0001")));
        }

        [TestMethod]
        public void AssignCredential_WhenAnotherIsActive_Throws()
        {
            Employee employee = NewEmployee();
            employee.AssignCredential(CredentialCode.Create("CRED-0001"));

            Assert.ThrowsExactly<BusinessRuleException>(() => employee.AssignCredential(CredentialCode.Create("CRED-0002")));
        }

        [TestMethod]
        public void AssignCredential_AfterRevokingPrevious_Succeeds()
        {
            Employee employee = NewEmployee();
            employee.AssignCredential(CredentialCode.Create("CRED-0001"));
            employee.RevokeCredential(CredentialCode.Create("CRED-0001"));

            Credential credential = employee.AssignCredential(CredentialCode.Create("CRED-0002"));

            Assert.AreEqual(CredentialStatus.Active, credential.Status);
            Assert.AreEqual(2, employee.Credentials.Count);
        }

        [TestMethod]
        public void CredentialCode_IsNormalizedToUpperCase()
        {
            Assert.AreEqual("CRED-0001", CredentialCode.Create(" cred-0001 ").Value);
        }

        [TestMethod]
        public void BlockAndUnblock_ChangesStatus()
        {
            Employee employee = NewEmployee();
            CredentialCode code = CredentialCode.Create("CRED-0001");
            employee.AssignCredential(code);

            employee.BlockCredential(code);
            Assert.AreEqual(CredentialStatus.Blocked, employee.FindCredential(code)!.Status);

            employee.UnblockCredential(code);
            Assert.AreEqual(CredentialStatus.Active, employee.FindCredential(code)!.Status);
        }

        [TestMethod]
        public void RevokedCredential_CannotBeBlockedOrUnblocked()
        {
            Employee employee = NewEmployee();
            CredentialCode code = CredentialCode.Create("CRED-0001");
            employee.AssignCredential(code);
            employee.RevokeCredential(code);

            Assert.ThrowsExactly<BusinessRuleException>(() => employee.BlockCredential(code));
            Assert.ThrowsExactly<BusinessRuleException>(() => employee.UnblockCredential(code));
            Assert.ThrowsExactly<BusinessRuleException>(() => employee.RevokeCredential(code));
        }
    }
}
