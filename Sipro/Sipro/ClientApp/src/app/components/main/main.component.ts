import { Component, OnInit } from '@angular/core';
import { utils } from 'protractor';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

    isLoggedIn : boolean;
    isMasterPage : boolean;

  constructor(private auth: AuthService, private utils: UtilsService) { }

    ngOnInit() {
        this.isMasterPage = this.auth.isLoggedIn();
        this.utils.setIsMasterPage(this.isMasterPage);
    }


}
