import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';
import { Group } from 'src/app/models/groups/group';

import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class GroupService extends BaseHttpService{

  private apiUrl = `${environment.apiHostUrl}/api/groups`;

  constructor(httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
  }

  create(group:Group){
    return this.httpService.post(this.apiUrl, group); 
  }
  
  update(group:Group, groupId:string){
    return this.httpService.put(this.apiUrl+`/`+groupId, group); 
  }
  
  delete(group:Group, groupId:string){

    this.httpService.delete(this.apiUrl+`/`+groupId, new RequestOptions({
      headers: headers,
      body: group
   }))
  }
}
