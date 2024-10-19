import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';

interface IPaymentaddProceed {
  branch_name: string;
  department_name: string;
  month: string;
  year: string;
  branch_gid: string;
  department_gid: string;
  salary_gid: string;
}

@Component({
  selector: 'app-acc-trn-add-payment',
  templateUrl: './acc-trn-add-payment.component.html',
  styleUrls: ['./acc-trn-add-payment.component.scss']
})

export class AccTrnAddPaymentComponent {
  PaymentaddProceed!: IPaymentaddProceed;
  Payment_addlist: any[] = [];
  responsedata: any;
  Payment_expand: any[] = [];
  product_list: any;
  reactiveForm: any;
  Companyname: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.PaymentaddProceed = {} as IPaymentaddProceed;
  }

  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      Companyname: new FormControl(''),
    });

    this.GetpaymentSummary();

    var url = 'AccTrnPayment/GetPaymentAddproceed'
    const params = {
      vendorname: "",
    };

    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Payment_addlist = this.responsedata.paymentadd;
      setTimeout(() => {
        $('#Payment_addlist').DataTable();
      },);
    });
  }

  GetpaymentSummary() {
    const selectedCompanyname = this.reactiveForm.value.Companyname;
    const params = {
      vendorname: selectedCompanyname,
    };
    const url = 'AccTrnPayment/GetPaymentAddproceed'
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Payment_addlist = this.responsedata.paymentadd;
      // setTimeout(() => {
      //   $('#Payment_addlist').DataTable();
      // }, );
    });
  }

  ondetail(vendor_gid: any) {
    var url = 'AccTrnPayment/GetMakepaymentExpand'
    let param = {
      vendor_gid: vendor_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      debugger;
      this.Payment_expand = result.paymentExpand;
    });
  }

  Productdetails(data1: any): void {
    debugger;
    var api1 = 'AccTrnPayment/Productdetails';
    let params = {
      expense_gid: data1.expense_gid,
      // salarygradetype :"Deduction"
    };
    this.service.getparams(api1, params).subscribe((result: any) => {
      this.responsedata = result;
      this.product_list = this.responsedata.productdetail_list;
    });
  }

  addpayment(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/finance/AccTrnMultipleExpense2singlepayment', encryptedParam])
  }

  onback() {
    this.router.navigate(['/finance/AccTrnPaymentSummary'])
  }

  addsinglepayment(expense_gid: any, vendor_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param1 = (expense_gid);
    const param2 = (vendor_gid);
    const expense_gid1 = AES.encrypt(param1, secretKey).toString();
    const vendor_gid1 = AES.encrypt(param2, secretKey).toString();
    this.router.navigate(['/finance/AccTrnPaymentaddconfirm', expense_gid1, vendor_gid1])
  }
}
