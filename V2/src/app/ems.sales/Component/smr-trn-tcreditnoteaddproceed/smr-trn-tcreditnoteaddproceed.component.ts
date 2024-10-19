import { Component } from '@angular/core';
import { FormBuilder, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-tcreditnoteaddproceed',
  templateUrl: './smr-trn-tcreditnoteaddproceed.component.html',
  styleUrls: ['./smr-trn-tcreditnoteaddproceed.component.scss']
})
export class SmrTrnTcreditnoteaddproceedComponent {

  creditnoteaddselectsummary_list : any[] =[];
  responsedata: any;


  ngOnInit(): void {
    this.GetCreditNoteAddSelectSummary()
  }

  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }
  //// Summary Grid//////
  GetCreditNoteAddSelectSummary() {
    var url = 'SmrTrnCreditNote/GetCreditNoteAddSelectSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#creditnoteaddselectsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.creditnoteaddselectsummary_list = this.responsedata.creditnotesummary_list;
      setTimeout(() => {
        $('#creditnoteaddselectsummary_list').DataTable();
      }, 1);
    })
  }

  Onclickcredit(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnTcreditnoteaddselect', encryptedParam])
  }
}

