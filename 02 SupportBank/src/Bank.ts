import { format } from 'date-fns';
import log4js from "log4js";

const logger = log4js.getLogger("Bank.ts")

type Accounts = Map<string, number>;

export interface Transaction {
  date: Date;
  from: string;
  to: string;
  narrative: string;
  amount: number;
}

export default class Bank {
  private accounts: Accounts = new Map();
  private transactions: Transaction[] = [];

  private createAccount(name: string, balance: number = 0) {
    this.accounts.set(name, balance);

    logger.info(`ACCOUNT-CREATED | ${name}`)
  }

  getAllAccounts() {
    const accountsInPounds = new Map<string, number>();

    for (const [name, balanceInPence] of this.accounts) {
      accountsInPounds.set(name, balanceInPence / 100);
    }

    return accountsInPounds;
  }

  getAccountBalanceInPence(name: string) {
    return this.accounts.get(name) ?? 0;
  }

  private setBalance(name: string, newBalance: number) {
    const accountExists = name in this.accounts;

    if (!accountExists)
      this.createAccount(name, newBalance);

    else
      this.accounts.set(name, newBalance);
  }

  private adjustBalance(name: string, amount: number) {
    logger.info(`ADJUST-BALANCE--START | ${name}`)
    const balance = this.getAccountBalanceInPence(name);
    const newBalance = balance + amount;

    this.setBalance(name, newBalance);

    logger.info(`ADJUST-BALANCE--END | ${name} / ${balance} -> ${newBalance}`)
  }

  addTransaction(transaction: Transaction) {
    logger.info(`ADD-TRANSACTION--START | ${format(transaction.date, "dd/MM/yyyy")} / ${transaction.from} -> ${transaction.to} / £${transaction.amount} / ${transaction.narrative}`)

    const amountInPence = transaction.amount * 100

    this.adjustBalance(transaction.from, -amountInPence);
    this.adjustBalance(transaction.to, amountInPence);

    this.transactions.push(transaction);
    logger.info(`ADD-TRANSACTION--END`)
  }

  getAccountTransactions(accountName: string) {
    return this.transactions.filter(transaction => transaction.from === accountName || transaction.to === accountName);
  }
}