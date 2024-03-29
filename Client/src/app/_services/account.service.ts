import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  //currentUserSource is the User object
  //currentUser is converted from currentUserSourceto observable variable.
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model)
    .pipe( //pipe handle the response & has function to map the response into an object post type <user>
      map((response: User)=>{
        const user = response;
        if(user){
          //here we are storing the user & token in LocalStorage
          this.setCurrentUser(user);          
        }
      })
    );
  }
  
  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }


  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe( //pipe handle the response & has function to map the response into an object post type <user>
    map(user=>{
      if(user){
        //here we are storing the user & token in LocalStorage
        this.setCurrentUser(user);
      }
      return user;
    })
  );
  }
}
