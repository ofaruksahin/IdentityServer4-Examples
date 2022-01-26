import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  status : any;

  constructor(
    private authService : AuthService) { 
    }


  ngOnInit(): void {
    this.authService.userManager.getUser()
      .then((user)=>{
        if(user){
          this.status = "Hoşgeldiniz"
        }else{
          this.status = "Giriş Yapılmadı"
        }
      })
  }

  login(){
    this.authService.userManager.signinRedirect();
  }

  logout(){
    this.authService.userManager.signoutRedirect();
  }

}
