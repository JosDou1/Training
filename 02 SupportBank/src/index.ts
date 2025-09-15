
import { parse } from 'date-fns';
import Bank from './Bank.js';
import CSVReader from './CSVReader.js';
import { configureLogging } from './logging.js';
import Menu from './Menu.js';
import TransactionFileValidator, { type FileErrors } from './TransactionFileValidator.js';

configureLogging()

const supportBank = new Bank();

const menu = new Menu(supportBank);

const inputFilePath = "./data/Transactions2014.csv";

const fileValidator = new TransactionFileValidator()

const fileErrors = await fileValidator.validate(inputFilePath)

if (fileErrors.size > 0) {
  menu.logValidationErrors(fileErrors)
  process.exit()
}

await CSVReader.readFile(inputFilePath, transactionHandler)

menu.start()

function transactionHandler(row: any) {
  const [dateString, from, to, narrative, amountString] = row;

  supportBank.addTransaction({ date: parse(dateString, 'dd/MM/yyyy', new Date()), from, to, narrative, amount: parseFloat(amountString) });
}

