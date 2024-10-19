import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

interface Iarbitration {
  arbitration_date: string;
  arbitration_no: string;
  arbit_type:string;
  title:string;
  arbitrator:string;
  institute: string;
  created_date: string;
  Status:string;
  arbitration_gid:string; 
}
@Component({
  selector: 'app-law-trn-arbitrationadd',
  templateUrl: './law-trn-arbitrationadd.component.html',
  styleUrls: ['./law-trn-arbitrationadd.component.scss']
})
export class LawTrnArbitrationaddComponent {
  reactiveForm!: FormGroup;
  arbitList: any;
  arbitration!: Iarbitration;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router) {
    this.arbitration = {} as Iarbitration;
  }


  get arbitration_no() {
    return this.reactiveForm.get('arbitration_no')!;
  }
  get arbitration_date() {
    return this.reactiveForm.get('arbitration_date')!;
  }






}
