import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, Event as NavigationEvent } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { MainmenuComponent } from './components/mainmenu/mainmenu.component';
import { AuthService } from './auth.service';
import { UtilsService } from './utils.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

export class AppComponent {
    title = 'app';
    @ViewChild(MainmenuComponent) mainmenu :  MainmenuComponent;
    @ViewChild(FooterComponent) footer : FooterComponent;

    constructor(private router:Router, private auth: AuthService, private utils: UtilsService){
        router.events.forEach((event: NavigationEvent) => {

            //Before Navigation
            if (event instanceof NavigationStart) {
                switch (event.url) {
                    case "/login":
                    {
                        this.mainmenu.isMasterPage = false;
                        this.footer.isMasterPage = false;
                        this.auth.logoff();
                        this.utils.setIsMasterPage(false);
                        break;
                    }
                    default:{
                         
                    }
                }
            }

            //After Navigation
            /*if (event instanceof NavigationEnd) {
                switch (event.url) {
                case "/app/home":
                {
                    //Do Work
                    break;
                }
                case "/app/About":
                {
                    //Do Work
                    break;
                }
                }
            }*/
        });
    }

    ngOnInit(){

    }

    ngOnDestroy(){
        this.utils.cleanStorage();
    }
}
