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
        public void _00CreateNewDonation()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.14M,
                Gross = 1.0M,
                TransactionId = "1",
                Donor = CreateDonorWithLondonAddress()
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _01CreateNewDonationFromSameDonor()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 2.0M,
                TransactionId = "2",
                Donor = CreateDonorWithLondonAddress()
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _02CreateNewDonationFromChangedDonorWithUnmodifiedAddress()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            donor.IsVerified = true;
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 3.0M,
                TransactionId = "3",
                Donor = donor
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _03CreateNewDonationFromDonorWithNewAddress_OldAddressIsNotUsed()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            donor.Address = CreateAmsterdamAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 4.0M,
                TransactionId = "4",
                Donor = donor
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _04CreateNewDonationFromOtherDonorWithSameAddress()
        {
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 5.0M,
                TransactionId = "5",
                Donor = CreateDonorWithAmsterdamAddress()
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _05CreateNewDonationFromDonorWithNewAddress_OldAddressIsUsed()
        {
            DonorDB donor = CreateDonorWithLondonAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 6.0M,
                TransactionId = "6",
                Donor = donor
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [Test]
        public void _06CreateNewDonationFromDonorWithChangedButExistAddress_OldAddressIsNotUsed()
        {
            DonorDB donor = CreateDonorWithAmsterdamAddress();
            donor.Address = CreateLondonAddress();
            DonationDB donation = new DonationDB()
            {
                Currency = "USD",
                Date = DateTime.Now,
                Fee = 0.15M,
                Gross = 7.0M,
                TransactionId = "7",
                Donor = donor
            };
            _donationRepository.Add(donation);
            Assert.IsTrue(_donationRepository.Contains(donation.TransactionId));
        }

        [OneTimeTearDown]
        public void LastTearDown()
        {
            _donationRepository?.Dispose();
            _kernel?.Dispose();
        }

        private AddressDB CreateAmsterdamAddress()
        {
            const string AMSTERDAM = "Amsterdam";
            return new AddressDB()
            {
                City = AMSTERDAM,
                State = AMSTERDAM,
                Country = "Netherlands",
                IsConfirmed = false,
                Name = "Inc.",
                Street = "Avenue",
                Zip = "600100"
            };
        }

        private AddressDB CreateLondonAddress()
        {
            const string LONDON = "London";
            return new AddressDB()
            {
                City = LONDON,
                Country = "United Kingdom",
                IsConfirmed = true,
                Name = "Test name",
                State = LONDON,
                Street = "Down Town",
                Zip = "100300"
            };
        }

        private DonorDB CreateDonorWithLondonAddress()
        {
            
            return new DonorDB()
            {
                Email = "JohnNewton@gmail.com",
                FirstName = "John",
                LastName = "Newton",
                Id = "1",
                IsVerified = false,
                Address = CreateLondonAddress()
            };
        }

        private DonorDB CreateDonorWithAmsterdamAddress()
        {
            return new DonorDB()
            {
                Email = "TomSnow@gmail.com",
                FirstName = "Tom",
                LastName = "Snow",
                Id = "2",
                IsVerified = false,
                Address = CreateAmsterdamAddress()
            };
        }
    }
}