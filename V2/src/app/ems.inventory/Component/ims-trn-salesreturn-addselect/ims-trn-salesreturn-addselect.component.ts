import { SelectionModel } from '@angular/cdk/collections';
import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

export class product_list {
  customer_name: any;
  directorder_gid: any;
  directorder_date: any;
  salesorder_gid: any;
  customer_contactperson: any;
  mobile: any;
  customer_emailid: any;
  customer_address: any;
  return_type: any;
  Productlist: any;
  Remarks: any;
}

@Component({  
  selector: 'app-ims-trn-salesreturn-addselect',
  templateUrl: './ims-trn-salesreturn-addselect.component.html',
})
export class ImsTrnSalesreturnAddselectComponent {

  SalesreturnForm!: FormGroup;
  GetSalesReturnDetails_list: any[] = [];
  selection = new SelectionModel<product_list>(true, []);
  directordergid: any;
  pick: Array<any> = [];
  CurObj: product_list = new product_list();
  return_type_list = [ { return_type : 'Damage',  return_gid : 1},
    { return_type : 'Not Relevant',  return_gid : 2 }
  ];
 
  mdlreturntype: any;

  constructor(private route: Router,
    private router: ActivatedRoute,
    private serivce: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService : NgxSpinnerService
  ) { }

  ngOnInit(): void {

    this.SalesreturnForm = new FormGroup({
      customer_name: new FormControl(''),
      customer_contactperson: new FormControl(''),
      mobile: new FormControl(''),
      customer_emailid: new FormControl(''),
      customer_address: new FormControl(''),
      Remarks: new FormControl(''),
      directorder_gid: new FormControl(''),
      directorder_date: new FormControl(''),
      salesorder_gid: new FormControl(''),
      qty_returnsales: new FormControl(''),
      customer_details: new FormControl(''),
      return_type: new FormControl(''),
    });

    const key = 'storyboard';
    this.directordergid = this.router.snapshot.paramMap.get('directordergid');
    const directorder_gid = AES.decrypt(this.directordergid, key).toString(enc.Utf8);
    this.GetSalesReturnDetails(directorder_gid);
  }



  GetSalesReturnDetails(directorder_gid: any) {
    debugger
    let param = { directorder_gid: directorder_gid }
    this.NgxSpinnerService.show();
    var summaryapi = 'SalesReturn/GetSalesReturnAddselect';
    this.serivce.getparams(summaryapi, param).subscribe((result: any) => {
      this.SalesreturnForm.get('customer_name')?.setValue(result.GetSalesReturnAddSelect_list[0].customer_name);
      this.SalesreturnForm.get('customer_contactperson')?.setValue(result.GetSalesReturnAddSelect_list[0].customer_contactperson);
      this.SalesreturnForm.get('mobile')?.setValue(result.GetSalesReturnAddSelect_list[0].mobile);
      this.SalesreturnForm.get('customer_emailid')?.setValue(result.GetSalesReturnAddSelect_list[0].customer_emailid);
      this.SalesreturnForm.get('customer_address')?.setValue(result.GetSalesReturnAddSelect_list[0].customer_address);
      this.SalesreturnForm.get('directorder_gid')?.setValue(result.GetSalesReturnAddSelect_list[0].directorder_gid);
      this.SalesreturnForm.get('directorder_date')?.setValue(result.GetSalesReturnAddSelect_list[0].directorder_date);
      this.SalesreturnForm.get('salesorder_gid')?.setValue(result.GetSalesReturnAddSelect_list[0].salesorder_gid);
      const customer_mobile = result.GetSalesReturnAddSelect_list[0].mobile;
      const customer_email = result.GetSalesReturnAddSelect_list[0].customer_emailid;
      const customer_contactperson = result.GetSalesReturnAddSelect_list[0].customer_contactperson;
      const customerDetails = `${customer_contactperson}\n${customer_mobile}\n${customer_email}`;
      this.SalesreturnForm.get("customer_details")?.setValue(customerDetails);
      this.GetSalesReturnGridValues(directorder_gid, result.GetSalesReturnAddSelect_list[0].salesorder_gid);
      this.NgxSpinnerService.hide();
    });
  }
  GetSalesReturnGridValues(directorder_gid: any, salesorder_gid: any) {
    this.NgxSpinnerService.show();
    let param = { directorder_gid: directorder_gid, salesorder_gid: salesorder_gid }
    var gridapi = 'SalesReturn/GetSalesReturnAddDetaisls';
    this.serivce.getparams(gridapi, param).subscribe((result: any) => {
      this.GetSalesReturnDetails_list = result.GetSalesReturnAddDetails_list;
      this.NgxSpinnerService.hide();
    })
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetSalesReturnDetails_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.GetSalesReturnDetails_list.forEach((row: product_list) => this.selection.select(row));
  }
  onSubmit() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.Productlist = this.pick
    this.CurObj.customer_name = this.SalesreturnForm.value.customer_name;
    this.CurObj.directorder_gid = this.SalesreturnForm.value.directorder_gid;
    this.CurObj.directorder_date = this.SalesreturnForm.value.directorder_date;
    this.CurObj.salesorder_gid = this.SalesreturnForm.value.salesorder_gid;
    this.CurObj.customer_contactperson = this.SalesreturnForm.value.customer_contactperson;
    this.CurObj.mobile = this.SalesreturnForm.value.mobile;
    this.CurObj.customer_emailid = this.SalesreturnForm.value.customer_emailid;
    this.CurObj.customer_address = this.SalesreturnForm.value.customer_address;
    this.CurObj.return_type = this.SalesreturnForm.value.return_type;
    this.CurObj.Remarks = this.SalesreturnForm.value.Remarks
    if (this.CurObj.Productlist.length === 0) {
      this.ToastrService.warning("Select atleast one Product");
      return;
    }
    this.NgxSpinnerService.show();
    var postapi = 'SalesReturn/PostsalesReturn';
    this.serivce.post(postapi, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      else {
        this.route.navigate(['/ims/ImsTrnSalesReturnSummary']);
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }
}

