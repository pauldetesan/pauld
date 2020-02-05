import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class RequestService {
  baseUrl = '';
  access_token = '';

  constructor(
    private http: HttpClient
  ) {
    this.baseUrl = 'https://localhost:44363/';
  }

  public tryLogin(email, password) {
    const creds = 'grant_type=password&username=' + btoa(email) + '&password=' + btoa(password);
    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    return new Promise<any>(resolve => {
      this.http.post(`${this.baseUrl}token`, creds, { headers }).subscribe(data => {
        debugger;
        this.access_token = data['access_token'];
        resolve(data["Result"]);
      },
        error => {
          debugger
          resolve(error);
        });
    })
  }

  public loginRequest(email: string, password) {
    return new Promise<any>(resolve => {
      this.postData('Auth/Login', {
        username: email,
        password
      }).subscribe(data => {
        resolve(data);
      });
    });
  }


  private postData(url: string, paramData: any): Observable<Object> {

    const headers = new HttpHeaders();
    headers.append('Authorization', 'bearer ' + this.access_token);
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    let params = new HttpParams();
    if (paramData) {
      for (const p in paramData) {
        params = params.set(p, paramData[p]);
      }
    }
    return this.http.post(this.baseUrl + url, paramData, { headers });
  }

}
