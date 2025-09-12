import log4js from "log4js";

export function configureLogging() {
    log4js.configure({
        appenders: {
            file: {
                type: 'fileSync', filename: 'logs/debug.log'
            }
        },
        categories: {
            default: { appenders: ['file'], level: 'debug' }
        }
    })
}