import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-smr-trn-salesorderhistory',
  templateUrl: './smr-trn-salesorderhistory.component.html',
  styleUrls: ['./smr-trn-salesorderhistory.component.scss']
})
export class SmrTrnSalesorderhistoryComponent {
  salesorderhistoryform: FormGroup | any;
  salesorderhistorylist: any;
  responsedata: any;
  salesorder_gid: any;
  salesorderhistorysummarylist: any;
  salesorderproduct_list: any[] = [];
  parameterValue1: any;
  salesorderdata: any;

  ngOnInit() {


    const salesorder_gid = this.route.snapshot.paramMap.get('salesorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      salesorder_gid: deencryptedParam
    }
    var api = 'SmrTrnSalesorder/Getsalesorderhistorysummarydata';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorderhistorylist = this.responsedata.salesorderhistorysummarylist;
    });
    this.Getquotationhistorydata();
  }

  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {

    this.salesorderhistoryform = new FormGroup({
      so_referenceno1: new FormControl(''),
      salesorder_date: new FormControl('', Validators.required),
      customer_name: new FormControl('', Validators.required),
      salesorder_remarks: new FormControl(''),
      salesorder_gid: new FormControl(''),
      quote_refno: new FormControl(''),
      quote_date: new FormControl(''),
      salesorder_customer_name: new FormControl(''),
      customer_contact_person: new FormControl(''),
      user_firstname: new FormControl(''),
      Grandtotal: new FormControl(''),
      salesorder_status: new FormControl(''),
      so_remarks : new FormControl(''),
      leadbank_gid: new FormControl('')

    })
  }

  Getquotationhistorydata() {
    debugger
    const salesorder_gid = this.route.snapshot.paramMap.get('salesorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      salesorder_gid: deencryptedParam
    }

    var api = 'SmrTrnSalesorder/Getsalesorderhistorydata';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.salesorderhistorylist = result.salesorderhistorylist;
      this.salesorderhistoryform.get("salesorder_gid")?.setValue(this.salesorderhistorylist[0].salesorder_gid);
      this.salesorderhistoryform.get("so_referenceno1")?.setValue(this.salesorderhistorylist[0].so_referenceno1);
      this.salesorderhistoryform.get("salesorder_date")?.setValue(this.salesorderhistorylist[0].salesorder_date);
      this.salesorderhistoryform.get("customer_name")?.setValue(this.salesorderhistorylist[0].customer_name);
      this.salesorderhistoryform.get("leadbank_gid")?.setValue(this.salesorderhistorylist[0].leadbank_gid);
      //this.salesorderhistoryform.get("quote_refno")?.setValue(this.salesorderhistorylist[0].quote_refno);
      this.salesorderhistoryform.get("salesorder_date")?.setValue(this.salesorderhistorylist[0].salesorder_date);
     // this.salesorderhistoryform.get("customer_name")?.setValue(this.salesorderhistorylist[0].customer_name);
      this.salesorderhistoryform.get("customer_contact_person")?.setValue(this.salesorderhistorylist[0].customer_contact_person);
      this.salesorderhistoryform.get("user_firstname")?.setValue(this.salesorderhistorylist[0].user_firstname);
      this.salesorderhistoryform.get("Grandtotal")?.setValue(this.salesorderhistorylist[0].Grandtotal);
      this.salesorderhistoryform.get("salesorder_status")?.setValue(this.salesorderhistorylist[0].salesorder_status);
    });
  }

  Details(parameter: string, salesorder_gid: string) {
    this.parameterValue1 = parameter;
    this.salesorder_gid = parameter;

    var url = 'SmrTrnSalesorder/Getsalesorderproductdetails'
    let param = {
      salesorder_gid: salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorderproduct_list = result.salesorderproduct_list;
    });
  }

  onview(params: any,leadbank_gid :any) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = "/smr/SmrTrnSalesOrderHistory"
    this.router.navigate(['/smr/SmrTrnSalesorderview', encryptedParam,leadbank_gid, lspage])
  }

  back() {
    this.router.navigate(['/smr/SmrTrnSalesorderSummary'])
  }





}