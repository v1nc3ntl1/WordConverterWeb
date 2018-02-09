import { Injectable } from '@angular/core';
import { Profile } from '../app/shared/profile'
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable()
export class WordconverterService {

  constructor(private http: HttpClient,
    private messageService: MessageService) { }

  private converterUrl = 'http://localhost:60615/api/Converter';  

  convertNumber(profile : Profile) : Observable<Profile> {
    const url = `${this.converterUrl}/${profile.no}`;
    
    return this.http.get<Profile>(url)
      .pipe(
        tap(profile => this.log(`fetched number`)),
        catchError(this.handleError('convertNumber', null))
      );
  }

  private log(message: string) {
    this.messageService.add(message);
  }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
 private handleError<T> (operation = 'operation', result?: T) {
  return (error: any): Observable<T> => {

    // TODO: send the error to remote logging infrastructure
    console.error(error); // log to console instead

    // TODO: better job of transforming error for user consumption
    this.log(`${operation} failed: ${error.message}`);

    // Let the app keep running by returning an empty result.
    return of(result as T);
  };
}
}
