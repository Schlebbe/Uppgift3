using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uppgift3;

namespace BankTests
{
    [TestClass]
    public class BankAccountTests
    {
        [TestMethod]
        public void WithdrawWithOverdraft()
        {
            // Arrange
            decimal beginningBalance = 5000.76M;
            decimal withdrawAmount = 8000.55M;
            decimal expected = -2999.79M;
            decimal overdraft = 10000;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance, Overdraft = overdraft };
            bank.Accounts.Add(account);

            // Act
            bank.WithdrawMoney(bank.Accounts[bank.Accounts.IndexOf(account)].AccountNumber.ToString(), withdrawAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransferWithOverdraft()
        {
            // Arrange
            decimal beginningBalance = 4732.23M;
            decimal transferAmount = 7312.88M;
            decimal expected = -2580.65M;
            decimal overdraft = 10000;
            Bank bank = new Bank();
            bank.AddCustomer(null, null, null, null, null, null, null, null);
            bank.AddAccount(bank.Customers[0].CustomerNumber.ToString(), "0");
            bank.Accounts[0].Balance = beginningBalance;
            bank.Accounts[0].Overdraft = overdraft;

            // Act
            bank.TransferMoney(bank.Accounts[0].AccountNumber.ToString(), bank.Accounts[1].AccountNumber.ToString(), transferAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[0].Balance;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestInterest()
        {
            // Arrange
            decimal beginningBalance = 7321.123M;
            decimal interest = 75.5M;
            decimal expected = 15.14369M;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance, Interest = interest };
            bank.Accounts.Add(account);

            // Act
            bank.CalculateInterest();

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance-beginningBalance;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WithdrawTooMuch()
        {
            // Arrange
            decimal beginningBalance = 5000;
            decimal withdrawAmount = 7000;
            decimal expected = 5000;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance };
            bank.Accounts.Add(account);

            // Act
            bank.WithdrawMoney(bank.Accounts[bank.Accounts.IndexOf(account)].AccountNumber.ToString(), withdrawAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance;
            Assert.AreEqual(expected, actual);
        }//1

        [TestMethod]
        public void TransferTooMuch()
        {
            // Arrange
            decimal beginningBalance = 82354.72M;
            decimal transferAmount = 129000.26M;
            decimal expected = 82354.72M;
            Bank bank = new Bank();
            bank.AddCustomer(null, null, null, null, null, null, null, null);
            bank.AddAccount(bank.Customers[0].CustomerNumber.ToString(), "0");
            bank.Accounts[0].Balance = beginningBalance;

            // Act
            bank.TransferMoney(bank.Accounts[0].AccountNumber.ToString(), bank.Accounts[1].AccountNumber.ToString(), transferAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[0].Balance;
            Assert.AreEqual(expected, actual);
        }//2

        [TestMethod]
        public void DepositNegative()
        {
            // Arrange
            decimal beginningBalance = 5000;
            decimal depositAmount = -5000;
            decimal expected = 5000;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance };
            bank.Accounts.Add(account);

            // Act
            bank.DepositMoney(bank.Accounts[bank.Accounts.IndexOf(account)].AccountNumber.ToString(), depositAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance;
            Assert.AreEqual(expected, actual);
        }//3

        [TestMethod]
        public void WithdrawNegative()
        {
            // Arrange
            decimal beginningBalance = 5000;
            decimal withdrawAmount = -2000;
            decimal expected = 5000;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance };
            bank.Accounts.Add(account);

            // Act
            bank.WithdrawMoney(bank.Accounts[bank.Accounts.IndexOf(account)].AccountNumber.ToString(), withdrawAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance;
            Assert.AreEqual(expected, actual);
        }//4

        [TestMethod]
        public void TestDebtInterest()
        {
            // Arrange
            decimal beginningBalance = -3453.62M;
            decimal debtInterest = 4.25M;
            decimal expected = -3454.02213M;
            Bank bank = new Bank();
            Account account = new Account() { Balance = beginningBalance, DebtInterest = debtInterest };
            bank.Accounts.Add(account);

            // Act
            bank.CalculateInterest();

            // Assert
            decimal actual = bank.Accounts[bank.Accounts.IndexOf(account)].Balance;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransferNegative()
        {
            // Arrange
            decimal beginningBalance = 82354.72M;
            decimal transferAmount = -1290.26M;
            decimal expected = 82354.72M;
            Bank bank = new Bank();
            bank.AddCustomer(null, null, null, null, null, null, null, null);
            bank.AddAccount(bank.Customers[0].CustomerNumber.ToString(), "0");
            bank.Accounts[0].Balance = beginningBalance;

            // Act
            bank.TransferMoney(bank.Accounts[0].AccountNumber.ToString(), bank.Accounts[1].AccountNumber.ToString(), transferAmount.ToString());

            // Assert
            decimal actual = bank.Accounts[0].Balance;
            Assert.AreEqual(expected, actual);
        }
    }
}
