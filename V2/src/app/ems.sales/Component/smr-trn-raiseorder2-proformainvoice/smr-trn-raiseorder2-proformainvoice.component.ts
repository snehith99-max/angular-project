import { Component } from '@angular/core';
import { FormBuilder, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-raiseorder2-proformainvoice',
  templateUrl: './smr-trn-raiseorder2-proformainvoice.component.html',
  styleUrls: ['./smr-trn-raiseorder2-proformainvoice.component.scss']
  
})
export class SmrTrnRaiseorder2ProformainvoiceComponent {
  reactiveForm!: FormGroup;
  responsedata: any;
  salesorder_list: any[] = [];
  salesproduct_list: any[] = [];
  salesordertype_list: any[] = [];
  getData: any;
  boolean: any;
  pick: Array<any> = [];
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  salesorder_gid: any;
  
  products: any[] = [];
  company_code: any;
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrTrnSalesordersummary();

  }
  GetSmrTrnSalesordersummary() {
    var url = 'SmrTrnSalesorder/GetSmrTrnSalesorder2invoicesummary'
    this.service.get(url).subscribe((result: any) => {
      $('#salesorder_list').DataTable().destroy();
      this.responsedata = result;
      this.salesorder_list = this.responsedata.salesorder_list;
      setTimeout(() => {
        $('#salesorder_list').DataTable();
      }, 1);


    })


  }
  RaisetoOrder(salesorder_gid: any){

    
    const key = 'storyboard';
    const param = salesorder_gid;
    const salesordergid = AES.encrypt(param,key).toString();
    this.router.navigate(['/smr/SmrTrnOrder2Proformainvoice',salesordergid])
  }

  
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  back(){
    this.router.navigate(['/smr/SmrTrnproformainvoice'])
  }

}