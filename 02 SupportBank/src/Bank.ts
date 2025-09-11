export interface Account {
  name: string;
  balance: number;
}

export interface Transaction {
  date: Date;
  from: string;
  to: string;
  narrative: string;
  amount: number;
}

export default class Bank {
  private accounts: Account[] = [];
  private transactions: Transaction[] = [];

  private createAccount(name: string, balance: number = 0) {
    this.accounts.push({
      name,
      balance
    })
  }

  getAccount(name: string) {
    return this.accounts.find(account => account.name === name);
  }

  getAllAccounts() {
    return this.accounts;
  }

  getAccountBalance(name: string) {
    return this.getAccount(name)?.balance;
  }

  private setBalance(name: string, newBalance: number) {
    const accountIndex = this.accounts.findIndex(account => account.name === name);

    if (accountIndex === -1)
      this.createAccount(name, newBalance);

    else if (this.accounts[accountIndex])
      this.accounts[accountIndex].balance = newBalance;
  }

  private adjustBalance(name: string, amount: number) {
    const balance = this.getAccountBalance(name);

    this.setBalance(name, (balance ?? 0) + amount);
  }

  addTransaction(transaction: Transaction) {
    this.adjustBalance(transaction.from, -transaction.amount);
    this.adjustBalance(transaction.to, transaction.amount);

    this.transactions.push(transaction);
  }

  getAccountTransactions(accountName: string) {
    return this.transactions.filter(transaction => transaction.from === accountName || transaction.to === accountName);
  }
}