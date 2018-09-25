import { Injectable } from '@angular/core';

// environment
import { environment } from '../../../environments/environment';

export enum LogLevel {
  All = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Fatal = 5,
  Off = 6
}

@Injectable({
  providedIn: 'root'
})
export class LoggingService {

  minLevel: LogLevel = environment.logLevel;
  logWithDate = true;

  public constructor() { }

  public debug(msg: string, ...optionalParams: any[]) {
    this.writeToLog(msg, LogLevel.Debug,
      optionalParams);
  }

  public info(msg: string, ...optionalParams: any[]) {
    this.writeToLog(msg, LogLevel.Info,
      optionalParams);
  }

  public warn(msg: string, ...optionalParams: any[]) {
    this.writeToLog(msg, LogLevel.Warn,
      optionalParams);
  }

  public error(msg: string, ...optionalParams: any[]) {
    this.writeToLog(msg, LogLevel.Error,
      optionalParams);
  }

  public fatal(msg: string, ...optionalParams: any[]) {
    this.writeToLog(msg, LogLevel.Fatal,
      optionalParams);
  }

  private shouldLog(level: LogLevel): boolean {
    let ret = false;
    if ((level >= this.minLevel &&
      level !== LogLevel.Off) ||
      this.minLevel === LogLevel.All) {
      ret = true;
    }
    return ret;
  }

  private formatParams(params: any[]): string {
    let ret: string = params.join(',');
    // Is there at least one object in the array?
    if (params.some(p => typeof p === 'object')) {
      ret = '';
      // Build comma-delimited string
      for (const item of params) {
        ret += JSON.stringify(item) + ',';
      }
    }
    return ret;
  }

  private writeToLog(msg: string,
    level: LogLevel,
    params: any[]) {
    if (this.shouldLog(level)) {
      let value = `[${LogLevel[level]}] - ${msg}`;

      if (params.length) {
        value += ' - Extra Info: ' + this.formatParams(params);
      }

      // Log the value
      if (level === LogLevel.Error || level === LogLevel.Fatal) {
        console.error(value);
      } else {
        console.log(value);
      }
    }
  }
}
