import { Component, EventEmitter, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @Input() cancelRegisterMode = new EventEmitter();
  registerMode = false;
  users: any;
  constructor() { }

  ngOnInit(): void {
    // this.getUsers();
  }

  registerToggel(){
    this.registerMode = !this.registerMode;
  }
  
  
  cancleRegisterMode(event: boolean){
    this.registerMode = event;
  }

  // getUsers(){
  //   //this call API Url
  //   this.http.get('https://localhost:5001/api/users')
  //   //to get data we use subscribe
  //   .subscribe({
  //     //next: x =>    
  //     // it means store data that comes from get request in X
  //     next: response =>
  //     //then that stared data in X we will be able to use it
  //     this.users = response,
  //     //then if error accures , we handle it in this part
  //     error: error=> console.log(error),
  //     //then after request is finished 
  //     complete: () => console.log('Resquest is finished')
  //   })
  //}
}
