using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vivus.Core.UnitTests.Dependencies;
using Vivus.Core.UnitTests.Dependencies.UoW;
using Vivus.Core.Donor.ViewModels;
using Vivus.Core.Model;

namespace Vivus.Core.Donor.UnitTests.ViewModels
{
    /// <summary>
    /// Summary description for ApplyViewModelTests
    /// </summary>
    [TestClass]
    public class ApplyViewModelTests
    {
        private UnitOfWork unitOfWork;
        private ApplicationViewModel<Model.Donor> appViewModel;
        private Core.UnitTests.Dependencies.DispatcherWrapper dispatcherWrapper;
        private ParentPage parentPage;
        private ApplyViewModel viewModel; 
        Model.Donor donor;
        /// <summary>
        /// Called before each test. Puts the viewmodel in a clean state.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {

            unitOfWork = new UnitOfWork();
            appViewModel = new ApplicationViewModel<Model.Donor>();
            Address homeAddress = new Address();
            homeAddress.City = "Cluj-Napoca";
            homeAddress.Street = "DeathIsSweet";
            homeAddress.StreetNo = "123";
            homeAddress.AddressID = 7000;
            Person person = new Person();
            person.FirstName = "Alex";
            person.LastName = "Alex";
            person.PersonID = 7000;
            appViewModel.User = new Model.Donor { PersonID = 7000, Person = person, ResidenceAddress = homeAddress, ResidenceID = 7000};

            dispatcherWrapper = new Core.UnitTests.Dependencies.DispatcherWrapper();
            parentPage = new ParentPage();
            viewModel = new ApplyViewModel(unitOfWork, appViewModel, dispatcherWrapper)
            {
                ParentPage = parentPage
            };
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Tests the apply funtionality when no errors occur, but an exception is thrown
        /// </summary>
        [TestMethod]
        public void Add_OnApply_ApplyForDonationFormScenario1()
        {
            //Scenario 1
            viewModel.Weight = 40;
            viewModel.HeartRate = 60;
            viewModel.SystolicBloodPressure = null;
            viewModel.DiastolicBloodPressure = null;
            viewModel.ConsumedAlcohol = false;
            viewModel.ConsumedHighFatFood = false;
            viewModel.UnderMedicalTreatment = false;
            viewModel.HadTravelled = "Test1";
            viewModel.IsPregnant = false;

            int? initialValue = appViewModel.User.DonationCenterID;
            appViewModel.User.DonationCenterID = null;

            List<DonationForm> donationForms = unitOfWork.DonationForms.Entities.ToList();

            Assert.AreEqual(viewModel.Errors, 0);

            int count = donationForms.Count;
            Assert.AreEqual(donationForms.Count, count);

            try
            {
                viewModel.ApplyAsync().Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(viewModel.Errors, 0);
                Assert.AreEqual(donationForms.Count, count);
            }
            
            appViewModel.User.ResidenceID = initialValue;


        }


        /// <summary>
        /// Tests the apply funtionality when error coccur
        /// </summary>
        [TestMethod]
        public void Add_OnApply_ApplyForDonationFormScenario2()
        {
            //Scenario 1
            viewModel.Weight = 40;
            viewModel.HeartRate = 60;
            viewModel.SystolicBloodPressure = null;
            viewModel.DiastolicBloodPressure = null;
            viewModel.ConsumedAlcohol = false;
            viewModel.ConsumedHighFatFood = false;
            viewModel.UnderMedicalTreatment = false;
            viewModel.HadTravelled = "Test2";
            viewModel.IsPregnant = true;

            List<DonationForm> donationForms = unitOfWork.DonationForms.Entities.ToList();
            Assert.AreEqual(viewModel.Errors, 0);

            int count = donationForms.Count;
            Assert.AreEqual(donationForms.Count, count);

            viewModel.ApplyAsync().Wait();

            Assert.AreEqual(viewModel.Errors, 0);
            Assert.AreEqual(donationForms.Count, count);
        }
    }
}
