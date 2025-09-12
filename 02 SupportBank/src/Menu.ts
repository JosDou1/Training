import type Bank from "./Bank.js";
import { format } from 'date-fns';
import { formatPounds, getMaxKeyLength } from "./Utils.js";
import readlineSync from 'readline-sync';
import type { FileErrors } from "./TransactionFileValidator.js";

export default class Menu {
    private bank: Bank;

    constructor(bank: Bank) {
        this.bank = bank;
    }

    start() {
        while (true) {
            console.log("  - Commands -")
            console.log("  • List All")
            console.log("  • List <Account Name>")
            console.log("  • Exit")
            console.log("")

            const input = readlineSync.question('> ').trim();

            console.log("")

            this.handleCommand(input)

            console.log("")
        }
    }

    private handleCommand(input: string) {
        if (input.toLowerCase() === 'list all') {
            this.listAllAccounts();

            return;
        }

        if (input.toLowerCase().startsWith('list ')) {
            const accountName = input.slice(5).trim();

            this.listAccountTransactions(accountName);

            return;
        }

        if (input.toLowerCase() === 'exit') {
            process.exit(0);
        }

        console.log('Unknown Command');
    }

    private listAllAccounts() {
        const accounts = this.bank.getAllAccounts()

        for (let [name, balance] of accounts) {
            console.log(`${balance < 0 ? '🟥' : '🟩'}  ${name.padStart(getMaxKeyLength(accounts))} ${formatPounds(balance)}`);
        }
    }


    private listAccountTransactions(accountName: string) {
        const transactions = this.bank.getAccountTransactions(accountName);

        if (transactions.length === 0) {
            console.log(`No transactions found for ${accountName}`);
            return;
        }

        const maxKeyLength = getMaxKeyLength(this.bank.getAllAccounts());
        transactions.forEach(({ date, from, to, amount, narrative }) => console.log(
            `${from === accountName ? '🟥' : '🟩'} ${format(date, "dd/MM/yyyy")}\t${from.padStart(maxKeyLength)} -> ${to.padEnd(maxKeyLength)}\t${formatPounds(amount).padStart(7)}\t ${narrative}`
        ));
    }

    logValidationErrors(fileErrors: FileErrors) {  
      for (let [row, errors] of fileErrors) {
        console.log(`Row ${row} has errors:`)
        errors.forEach(error => console.log(`  • ${error}`))
        console.log('')
      }
    }  
}