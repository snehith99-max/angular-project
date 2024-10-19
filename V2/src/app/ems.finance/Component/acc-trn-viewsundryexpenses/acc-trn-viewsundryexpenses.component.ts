import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-viewsundryexpenses',
  templateUrl: './acc-trn-viewsundryexpenses.component.html',
  styleUrls: ['./acc-trn-viewsundryexpenses.component.scss']
})
export class AccTrnViewsundryexpensesComponent {


  sundryexpense_list : any[]=[];
  productsummary_list : any[]=[];
  expenseview : any;
  expense_gid : any;
  responsedata: any;
  address1 : any;
  address2: any;
  city: any;
  state : any;
  pincode : any;
  vendor_address:any;
  contact_telephonenumber :any;
  email_id : any;
  contactperson_name : any;
  vendor_address2 : any
  sundryproexpense_list : any[]=[]

  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService) {
  }
  ngOnInit(): void {
  const expenseview =this.route.snapshot.paramMap.get('expense_gid');
  this.expenseview= expenseview;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.expenseview,secretKey).toString(enc.Utf8);
  this.expense_gid = deencryptedParam;
  this.GetViewexpensesSummary(deencryptedParam);
  this.GetViewexpensesProductSummary(deencryptedParam);
  }
  
  GetViewexpensesSummary(expense_gid  : any){
    var url='AccTrnSundryExpenses/GetSundryExpenseView'
    let param = {
      expense_gid : expense_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.sundryexpense_list = result.expense_list; 
    this.address1 = this.sundryexpense_list[0].address1;  
    this.address2 = this.sundryexpense_list[0].address2;  
    this.city = this.sundryexpense_list[0].city;
    this.state = this.sundryexpense_list[0].state;
    this.pincode = this.sundryexpense_list[0].postal_code;

    this.contactperson_name = this.sundryexpense_list[0].contactperson_name;
    this.email_id = this.sundryexpense_list[0].email_id;
    this.contact_telephonenumber = this.sundryexpense_list[0].contact_telephonenumber;

    const address = this.address1 + '\n' + this.address2 + '\n' +this.city +'\n' + this.state +'\n'+this.pincode;
    this.vendor_address = address

    const address2 = this.contactperson_name + '\n' + this.email_id + '\n' +this.contact_telephonenumber ;
    this.vendor_address2 = address2
    });
  }


  GetViewexpensesProductSummary(expense_gid  : any){
    var url='AccTrnSundryExpenses/GetSundryExpenseViewProducts'
    let param = {
      expense_gid : expense_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.sundryproexpense_list = result.sundryledgerview_list; 
    console.log(this.sundryproexpense_list,'view_list')
    });
  }
}
