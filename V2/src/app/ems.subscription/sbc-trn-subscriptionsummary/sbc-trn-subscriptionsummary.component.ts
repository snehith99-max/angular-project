

import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup,FormControl } from '@angular/forms';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-sbc-trn-subscriptionsummary',
  templateUrl: './sbc-trn-subscriptionsummary.component.html',
  styleUrls: ['./sbc-trn-subscriptionsummary.component.scss']
})
export class SbcTrnSubscriptionsummaryComponent {
  subscription_list:any;
  response_data :any;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  invoice:any;
  showOptionsDivId:any;
  rows:any[]=[];


  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,) {} 
  ngOnInit(): void {
    this.GetsubscriptionSummary();
    this.reactiveForm = new FormGroup({
    file: new FormControl(''),
    });
  }
  GetsubscriptionSummary() {
   debugger
    const api = 'SubTrnSubscrition/GetsubscriptionSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.subscription_list = this.response_data.subscription_list;
      setTimeout(() => {  
        $('#subscription_list').DataTable({
          order: [] // Disable initial sorting
        });
      }, 1);
 
    });
  }

}

