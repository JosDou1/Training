import { parse as parseCSV } from "csv-parse";
import * as fs from 'fs';

const fileStream = fs.createReadStream('./data/Transactions2014.csv');

const parser = parseCSV({
  delimiter: ",",
  from_line: 2
})

fileStream.pipe(parser);

parser.on("readable", () => {
  let transaction;

  while (transaction = parser.read()) {
    console.log(transaction);
  }
})

parser.on("error", (err) => {
  console.error(err);
})

parser.on("end", () => {
  console.log("end")
})