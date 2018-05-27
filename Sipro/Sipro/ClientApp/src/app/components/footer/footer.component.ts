import { Component, OnInit, OnDestroy } from '@angular/core';
import { UtilsService } from '../../utils.service';

@Component({
  selector: 'main-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

    isMasterPage : boolean;

    constructor(private utils:UtilsService) { 
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

}
