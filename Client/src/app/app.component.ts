import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  users: any;
  //this how to enject the modules in constructor
  constructor(private http:HttpClient){}
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    
    //this call API Url
    this.http.get('https://localhost:5001/api/users')
    //to get data we use subscribe
    .subscribe({
      //next: x =>    
      // it means store data that comes from get request in X
      next: response =>
      //then that stared data in X we will be able to use it
      this.users = response,
      //then if error accures , we handle it in this part
      error: error=> console.log(error),
      //then after request is finished 
      complete: () => console.log('Resquest is finished')
    })
  }
}
