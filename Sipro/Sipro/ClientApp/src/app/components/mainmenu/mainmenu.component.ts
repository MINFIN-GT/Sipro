import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../../utils.service';
import { AuthService } from '../../auth.service';
import { MatToolbar } from '@angular/material';

@Component({
  selector: 'main-menu',
  templateUrl: './mainmenu.component.html',
  styleUrls: ['./mainmenu.component.css']
})
export class MainmenuComponent implements OnInit {

    isMasterPage : boolean;

    constructor(private utils : UtilsService, private auth : AuthService) { 
        this.isMasterPage = utils.isMasterPage();
    } 

    ngOnInit() {
        this.utils.changeMasterPage.subscribe(
            _isMasterPage=>{
                this.isMasterPage = _isMasterPage;
            });
    }

    ngOnDestroy(){
        this.utils.changeMasterPage.unsubscribe();
    }

    hasClaim(val:string){
        return this.auth.hasClaim(val);
    }

}
