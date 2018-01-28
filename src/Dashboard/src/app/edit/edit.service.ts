import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { ErrorObservable } from 'rxjs/Observable/ErrorObservable';
import { StoredFile, StoredFileInfo } from '../../models/storedfileinfo';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from '../auth.service';

@Injectable()
export class EditService {

  private filesUrl: string = environment.apiUrl;
  private headers: HttpHeaders = new HttpHeaders();

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) { }

  private setHeaders() {
    const token = this.authService.currentUser.access_token;
    if (token !== '') {
        const tokenValue = 'Bearer ' + token;
        this.headers = this.headers.set('Authorization', tokenValue);
    }
  }

  public getFile(params): Observable<StoredFile[]>  {
    this.setHeaders();
    return this.httpClient.post<StoredFile[]>(this.filesUrl + '/getFile', {
      key: params.key,
      title: params.title
    });
  }

  public getContent(params) {
    this.setHeaders();
    return this.httpClient.post(this.filesUrl + '/getFileContent', {
      key: params.key
    });
  }

  public storeFile(file) {
    this.setHeaders();
    return this.httpClient.post(this.filesUrl + '/storeFile', file);
  }
}
