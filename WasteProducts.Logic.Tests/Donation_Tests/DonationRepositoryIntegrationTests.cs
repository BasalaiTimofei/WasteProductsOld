using Ninject;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.DataAccess.Common.Models.Donations;
using System;

namespace WasteProducts.Logic.Tests.Donation_Tests
{
    [TestFixture]
    class DonationRepositoryIntegrationTests
    {
        private readonly StandardKernel _kernel = new StandardKernel();
        private readonly IDonationRepository _donationRepository;

        public DonationRepositoryIntegrationTests()
        {
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());
            _donationRepository = _kernel.Get<IDonationRepository>();
            ((DonationRepository)_donationRepository).RecreateTestDatabase();
        }

        [Test]
        public void TestCreateNewDonation()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.14M,
                Gross = 13.85M,
                TransactionId = "123567890123",
                Donor = new DonorDB()
                {
                    Email = "test_email@gmail.com",
                    FirstName = "John",
                    LastName = "Newton",
                    Id = "0123456789123",
                    IsVerified = true,
                    Address = new AddressDB()
                    {
                        City = "London",
                        Country = "United Kingdom",
                        IsConfirmed = true,
                        Name = "",
                        State = "London",
                        Street = "Down Town",
                        Zip = "100300"
                    }
                }
            };
            _donationRepository.Add(donation);
        }

        [Test]
        public void TestCreateNewDonationFromSameDonor()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 26.0M,
                TransactionId = "123567890124",
                Donor = new DonorDB()
                {
                    Email = "test_email@gmail.com",
                    FirstName = "John",
                    LastName = "Newton",
                    Id = "0123456789123",
                    IsVerified = true,
                    Address = new AddressDB()
                    {
                        City = "London",
                        Country = "United Kingdom",
                        IsConfirmed = true,
                        Name = "",
                        State = "London",
                        Street = "Down Town",
                        Zip = "100300"
                    }
                }
            };
            _donationRepository.Add(donation);
        }

        [OneTimeTearDown]
        public void LastTearDown()
        {
            _donationRepository?.Dispose();
            _kernel?.Dispose();
        }
    }
}
