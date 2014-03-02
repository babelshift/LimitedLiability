using System;

namespace LimitedLiability
{
	public class BankAccount
	{
		public int Balance { get; private set; }

		public event EventHandler<BankAccountTransactionEventArgs> AmountDeposited;

		public event EventHandler<BankAccountTransactionEventArgs> AmountWithdrawn;

		public BankAccount(int startingBalance)
		{
			Balance = startingBalance;
		}

		public void Withdraw(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException("amount", "Cannot withdraw negative amounts.");

			if (Balance - amount < 0)
				throw new Exception(String.Format("Balance is {0}. Withdrawing ${1} would result in an overdraft.", Balance, amount));

			Balance -= amount;

			if (AmountWithdrawn != null)
				AmountWithdrawn(this, new BankAccountTransactionEventArgs(Balance, amount));
		}

		public void Deposit(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException("amount", "Cannot deposit negative amounts.");

			Balance += amount;

			if (AmountDeposited != null)
				AmountDeposited(this, new BankAccountTransactionEventArgs(Balance, amount));
		}
	}

	public class BankAccountTransactionEventArgs : EventArgs
	{
		public int NewBalance { get; private set; }

		public int TransactionAmount { get; private set; }

		public BankAccountTransactionEventArgs(int newBalance, int transactionAmount)
		{
			NewBalance = newBalance;
			TransactionAmount = transactionAmount;
		}
	}
}