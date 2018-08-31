import { Component, OnInit, ViewChild, ElementRef  } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';


@Component({
  selector: 'app-actividadpropiedad',
  templateUrl: './actividadpropiedad.component.html',
  styleUrls: ['./actividadpropiedad.component.css']
})
export class ActividadpropiedadComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
