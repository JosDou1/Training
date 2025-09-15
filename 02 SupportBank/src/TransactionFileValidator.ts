import { parse, isValid } from 'date-fns';
import CSVReader from './CSVReader.js';
import log4js from "log4js";

const logger = log4js.getLogger("TransactionFileValidator.ts")

export type FileErrors = Map<number, string[]>;

export default class TransactionFileValidator {
  private fileErrors: FileErrors = new Map();

  validate(filePath: string): Promise<FileErrors> {
    return new Promise(async (resolve, reject) => {
      await CSVReader.readFile(
        filePath,
        this.rowHandler,
      );

      console.log(this.fileErrors.size > 0 ? "Invalid File\n" : "Valid File\n")

      resolve(this.fileErrors)

    });
  }

  private rowHandler = (row: any, rowNumber: number) => {
    const rowErrors = TransactionFileValidator.validateRow(row)

    if (rowErrors.length > 0) this.fileErrors.set(rowNumber, rowErrors)
  }

  private static errorHandler(error: Error) {
    logger.error(error.message);
  }

  private static validateRow(row: any) {
    const rowErrors = []

    if (!Array.isArray(row) || row.length !== 5) {
      rowErrors.push("Invalid Row: Expected 5 columns")
    }

    const [rawDate, rawFrom, rawTo, rawNarrative, rawAmount] = row;

    if (!TransactionFileValidator.validateDate(rawDate))
      rowErrors.push("Invalid Date: Expected date in format dd/MM/yyyy")

    if (!TransactionFileValidator.validateString(rawFrom))
      rowErrors.push("Invalid From: Expected text")

    if (!TransactionFileValidator.validateString(rawTo))
      rowErrors.push("Invalid To: Expected text")

    if (!TransactionFileValidator.validateString(rawNarrative))
      rowErrors.push("Invalid Narrative: Expected text")

    if (!TransactionFileValidator.validateNumber(rawAmount))
      rowErrors.push("Invalid Amount: Expected number")

    return rowErrors;
  }

  private static validateDate(rawDate: any) {
    const parsedDate = parse(rawDate, "dd/MM/yyyy", new Date());

    return isValid(parsedDate);
  }

  private static validateString(rawString: any) {
    return typeof rawString === "string" && rawString.trim().length > 0
  }

  private static validateNumber(rawNumber: any) {
    const parsedNumber = Number(rawNumber)
    return Number.isFinite(parsedNumber)
  }
}