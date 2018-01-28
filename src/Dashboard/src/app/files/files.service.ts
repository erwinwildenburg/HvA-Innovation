import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { ErrorObservable } from 'rxjs/Observable/ErrorObservable';
import { StoredFileInfo } from '../../models/storedfileinfo';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from '../auth.service';

@Injectable()
export class FilesService {

  private filesUrl: string = environment.apiUrl + '/files';
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

  public getFiles(id: string): Observable<StoredFileInfo[]> {
    this.setHeaders();
    const params = id ? new HttpParams().set('id', id) : undefined;

    return this.httpClient.get<StoredFileInfo[]>(this.filesUrl, { headers: this.headers, params: params });
  }
}
