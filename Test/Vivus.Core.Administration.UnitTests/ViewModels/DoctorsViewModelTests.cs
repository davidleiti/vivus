
namespace Vivus.Core.Administration.UnitTests.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Vivus.Core.Administration.ViewModels;
    using VivusDataModels = Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.UnitTests.Dependencies;
    using Vivus.Core.UnitTests.Dependencies.UoW;
    using Vivus.Core.ViewModels.Base;
    /// <summary>
    /// test class for doctors viewmodel
    /// </summary>
    [TestClass]
    public class DoctorsViewModelTests
    {
        private UnitOfWork unitOfWork;
        private Mock<IApllicationViewModel<Administrator>> appViewModel;
        private DispatcherWrapper dispatcherWrapper;
        private Security security;
        private ParentPage parentPage;
        private DoctorsViewModel viewModel;


        /// <summary>
        /// Called before each test. Puts the viewmodel in a clean state.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            appViewModel = new Mock<IApllicationViewModel<Administrator>>();
            dispatcherWrapper = new DispatcherWrapper();
            parentPage = new ParentPage();
            security = new Security();
            viewModel = new DoctorsViewModel(unitOfWork, appViewModel.Object, dispatcherWrapper, security)
            {
                ParentPage = parentPage
            };
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void AddModify_OnAddEmpty_ReturnsZero()
        {
            viewModel.AddModifyAsync().Wait();
            

            Assert.AreEqual(0, viewModel.Doctors.Count);
        }

        // Convention: MethodsName_Scenario_ExpectedBehaviour
        // Generic scenario: WhenCalled
        [TestMethod]
        public void AddModify_OnAdd_ReturnsOne()
        {
            viewModel.Email = "you@email.com";
            viewModel.Person.FirstName = "FirstName";
            viewModel.Person.LastName = "LastName";
            viewModel.Person.BirthDate = "1/1/1";
            viewModel.Person.NationalIdentificationNumber = "1234567890123";
            viewModel.Person.PhoneNumber = "012345678";
            viewModel.Person.Gender = new VivusDataModels.BasicEntity<string>(0, "Gender");
            viewModel.HomeAddress.County = new VivusDataModels.BasicEntity<string>(0, "County");
            viewModel.HomeAddress.City = "City";
            viewModel.HomeAddress.StreetName = "Streetname";
            viewModel.HomeAddress.StreetNumber = "StreetNumber";

            viewModel.WorkAddress.County = new VivusDataModels.BasicEntity<string>(0, "County");
            viewModel.WorkAddress.City = "City";
            viewModel.WorkAddress.StreetName = "Streetname";
            viewModel.WorkAddress.StreetNumber = "StreetNumber";

            viewModel.AddModifyAsync().Wait();

            Assert.AreEqual(1, viewModel.Doctors.Count);
        }
    }
}

