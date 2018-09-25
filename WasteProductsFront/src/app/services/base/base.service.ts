import { LoggingService } from '../logging/logging.service';

export class BaseService {

  constructor(private loggingService: LoggingService) { }


  protected logDebug(msg: any) {
    this.loggingService.debug(this.getLoggingMsg(msg));
  }

  protected logInfo(msg: any) {
    this.loggingService.info(this.getLoggingMsg(msg));
  }

  protected logWarn(msg: any) {
    this.loggingService.warn(this.getLoggingMsg(msg));
  }

  protected logError(msg: any) {
    this.loggingService.error(this.getLoggingMsg(msg));
  }

  protected logFatal(msg: any) {
    this.loggingService.fatal(this.getLoggingMsg(msg));
  }

  private getLoggingMsg(msg: string) {
    return `${this.constructor.name}: ${msg}`;
  }
}
