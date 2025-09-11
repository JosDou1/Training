import { parse as parseCSV } from "csv-parse";
import { format } from 'date-fns';
import { parse } from 'date-fns/parse';
import * as fs from 'fs';
import readlineSync from 'readline-sync';
import Bank from './Bank.js';

const supportBank = new Bank();

const fileStream = fs.createReadStream('./data/Transactions2014.csv');

const parser = parseCSV({
  delimiter: ",",
  from_line: 2
})

fileStream.pipe(parser);

parser.on("readable", () => {
  let transaction;

  while (transaction = parser.read()) {
    const [dateString, from, to, narrative, amountString] = transaction;
    supportBank.addTransaction({ date: parse(dateString, 'dd/MM/yyyy', new Date()), from, to, narrative, amount: parseFloat(amountString) });
  }
})

parser.on("error", (err) => {
  console.error(err);
})

parser.on("end", () => {
  while (true) {
    const input = readlineSync.question('> ').trim();

    handleCommand(input)
  }
})

function handleCommand(input: string) {
  if (input === 'List All') {
    listAllAccounts();

    return;
  }

  if (input.startsWith('List ')) {
    const accountName = input.slice(5).trim();

    listAccountTransactions(accountName);

    return;
  }

  console.log('Unknown Command');
}

function listAllAccounts() {
  supportBank.getAllAccounts().forEach(account =>
    console.log(`${account.name}: ${account.balance.toFixed(2)}`)
  );
}

function listAccountTransactions(accountName: string) {
  const transactions = supportBank.getAccountTransactions(accountName);

  if (transactions.length === 0) {
    console.log(`No transactions found for ${accountName}`);
    return;
  }

  transactions.forEach(({ date, from, to, amount, narrative }) => console.log(
    `${format(date, "dd/MM/yyyy")} | ${from} -> ${to} | ${amount} | ${narrative}`
  ));
}