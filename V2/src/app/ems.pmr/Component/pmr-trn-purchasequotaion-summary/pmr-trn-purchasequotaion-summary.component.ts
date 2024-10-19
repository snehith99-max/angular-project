import { Component } from '@angular/core';
import { FormBuilder, FormGroup,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-pmr-trn-purchasequotaion-summary',
  templateUrl: './pmr-trn-purchasequotaion-summary.component.html',
})
export class PmrTrnPurchasequotaionSummaryComponent {

  quotation_list :any;
  responsedata :any

  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {

    
  }
  ngOnInit(): void {
    
      const options: Options = {
        dateFormat: 'd-m-Y' ,    
      };
      flatpickr('.date-picker', options); 
    this.GetPmrTrnPurchaseQuotation();
}

GetPmrTrnPurchaseQuotation() {
  debugger
  var url = 'PmrTrnPurchaseQuotation/GetPmrTrnPurchaseQuotation'
  // this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#quotation_list').DataTable().destroy();
    this.responsedata = result;
    this.quotation_list = this.responsedata.quotation_list;
    setTimeout(() => {
      $('#quotation_list').DataTable();
      // this.NgxSpinnerService.hide()
    }, 1);


  })
  
  
}

Details(){}
  onview(){}
  openModaledit(){}
  openModalmail(){}
  openModalprint(){}
  openModalamend(){}
  onaddinfo(){}
}
