using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Tests.Domain
{
    [TestClass]
    public sealed class AccessRecordTests
    {
        private static readonly CredentialCode Code = CredentialCode.Create("CRED-0001");

        private static Employee EmployeeWithCredential()
        {
            Employee employee = new("1037000001", "Ana", "Gómez", Email.Create("ana@logistica.com"), "Auxiliar", OrganizationalArea.Warehouse);
            employee.AssignCredential(Code);
            return employee;
        }

        private static AccessPoint EnabledPoint() => new("Recepción principal", null, AccessPointType.Reception);

        [TestMethod]
        public void Entry_WithValidCredential_IsAuthorized()
        {
            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), EnabledPoint(), MovementType.Entry, null);

            Assert.AreEqual(AccessResult.Authorized, record.Result);
            Assert.IsNull(record.RejectionReason);
        }

        [TestMethod]
        public void Exit_AfterEntry_IsAuthorized()
        {
            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), EnabledPoint(), MovementType.Exit, MovementType.Entry);

            Assert.IsTrue(record.IsAuthorized);
        }

        [TestMethod]
        public void UnknownCredential_IsRejectedAndRecorded()
        {
            AccessRecord record = AccessRecord.Register(Code, null, EnabledPoint(), MovementType.Entry, null);

            Assert.AreEqual(AccessResult.Rejected, record.Result);
            Assert.AreEqual(RejectionReason.CredentialNotFound, record.RejectionReason);
            Assert.AreEqual("CRED-0001", record.CredentialCode);
            Assert.IsNull(record.EmployeeId);
        }

        [TestMethod]
        public void BlockedCredential_IsRejected()
        {
            Employee employee = EmployeeWithCredential();
            employee.BlockCredential(Code);

            AccessRecord record = AccessRecord.Register(Code, employee, EnabledPoint(), MovementType.Entry, null);

            Assert.AreEqual(RejectionReason.CredentialBlocked, record.RejectionReason);
        }

        [TestMethod]
        public void RevokedCredential_IsRejected()
        {
            Employee employee = EmployeeWithCredential();
            employee.RevokeCredential(Code);

            AccessRecord record = AccessRecord.Register(Code, employee, EnabledPoint(), MovementType.Entry, null);

            Assert.AreEqual(RejectionReason.CredentialRevoked, record.RejectionReason);
        }

        [TestMethod]
        public void InactiveEmployee_IsRejected()
        {
            Employee employee = EmployeeWithCredential();
            employee.Deactivate();

            AccessRecord record = AccessRecord.Register(Code, employee, EnabledPoint(), MovementType.Entry, null);

            Assert.AreEqual(RejectionReason.EmployeeInactive, record.RejectionReason);
        }

        [TestMethod]
        public void DisabledAccessPoint_IsRejected()
        {
            AccessPoint point = EnabledPoint();
            point.Disable();

            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), point, MovementType.Entry, null);

            Assert.AreEqual(RejectionReason.AccessPointDisabled, record.RejectionReason);
        }

        [TestMethod]
        public void UnknownAccessPoint_IsRejected()
        {
            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), null, MovementType.Entry, null);

            Assert.AreEqual(RejectionReason.AccessPointNotFound, record.RejectionReason);
            Assert.IsNull(record.AccessPointId);
        }

        [TestMethod]
        public void DoubleEntry_IsRejected()
        {
            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), EnabledPoint(), MovementType.Entry, MovementType.Entry);

            Assert.AreEqual(RejectionReason.EmployeeAlreadyInside, record.RejectionReason);
        }

        [TestMethod]
        public void ExitWithoutEntry_IsRejected()
        {
            AccessRecord record = AccessRecord.Register(Code, EmployeeWithCredential(), EnabledPoint(), MovementType.Exit, null);

            Assert.AreEqual(RejectionReason.EmployeeNotInside, record.RejectionReason);
        }
    }
}
