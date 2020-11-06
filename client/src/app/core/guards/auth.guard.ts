import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router:Router){}

   isAdmin(){
    
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(auth => {
        if(auth){
          if(route.data.role === "Admin"){
             console.log("ONLY ADMIN CAN ACCESS THAT!");       
              this.accountService.isAdmin().then( res =>{
                if(res)
                {
                  return true;
                }else
                {
                 this.router.navigate(['']);
                 return false;
                }
            });
          }
          return true;
        }
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
      })
    );
  } 
}
