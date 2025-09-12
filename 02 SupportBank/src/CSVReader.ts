import { parse as parseCSV } from "csv-parse";
import { createReadStream } from 'fs';
import log4js from "log4js";

const logger = log4js.getLogger("CSVFileReader.ts")

export default class CSVReader {
  static readFile(
    filePath: string,
    onRow: (row: any, rowNumber: number) => void,
  ): Promise<void> {
    return new Promise((resolve, reject) => {
      const parser = parseCSV({
        delimiter: ",",
        from_line: 2
      })

      const fileStream = createReadStream(filePath);

      fileStream.pipe(parser);

      let currentRowNumber = 2

      parser.on("readable", () => {
        logger.info("READ-CSV--START")

        let row: any;
        while (row = parser.read()) {
          onRow(row, currentRowNumber)
          currentRowNumber++
        }

        logger.info("READ-CSV--END")
      })

      parser.on("error", (error) => {
        reject(error)
      })

      parser.on("end", () => {
        resolve()
      })
    })
  }
}