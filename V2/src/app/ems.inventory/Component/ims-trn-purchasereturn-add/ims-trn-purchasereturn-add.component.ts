import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

export class product_list {
  branch_gid: any;
  grn_gid: any;
  purchasereturn_date: any;
  remarks: any;
  vendor_gid: any;
  purchaseQTY: any;
  product_gid: any;
  cost_price: number = 0;
}

@Component({
  selector: 'app-ims-trn-purchasereturn-add',
  templateUrl: './ims-trn-purchasereturn-add.component.html',
  styleUrls: ['./ims-trn-purchasereturn-add.component.scss']
})
export class ImsTrnPurchasereturnAddComponent {


  GRNform!: FormGroup;
  grngid: any;
  vendorgid: any;
  vendor_gid: any;
  grn_gid: any;
  vendor_name: any;
  branch_name: any;
  branch_gid: any;
  GetPurchaseDetails_list: any[] = [];
  selection = new SelectionModel<product_list>(true, []);
  pick: Array<any> = [];
  CurObj: product_list = new product_list();
  qty_purchasereturn: number = 0;
  product_remarks: any;
  lspage : any;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService
  ) { }

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const key = 'storyboard';
    this.grngid = this.route.snapshot.paramMap.get('grngid');
    this.grngid = this.grngid;
    const deencryptedParam = AES.decrypt(this.grngid, key).toString(enc.Utf8);
    this.grn_gid = deencryptedParam;
    this.vendorgid = this.route.snapshot.paramMap.get('vendorgid');
    this.vendorgid = this.vendorgid;
    const deencryptedParam1 = AES.decrypt(this.vendorgid, key).toString(enc.Utf8);
    this.vendor_gid = deencryptedParam1;
    this.lspage = this.route.snapshot.paramMap.get('lspage');
    this.lspage = this.lspage;
    const deencryptedParam2 = AES.decrypt(this.lspage, key).toString(enc.Utf8);
    this.lspage = deencryptedParam2;



    this.GetPurchaseReturnDetails(this.vendor_gid, this.grn_gid);

    
   

    this.GRNform = new FormGroup({
      branch_name: new FormControl(''),
      purchasereturn_refno: new FormControl(''),
      vendor_name: new FormControl(''),
      remarks: new FormControl(''),
      purchasereturn_date: new FormControl(this.getCurrentDate()),
      qty_purchasereturn: new FormControl(''),
      product_remarks: new FormControl(''),
    })
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  GetPurchaseReturnDetails(vendor_gid: any, grn_gid: any) {

    let param = { vendor_gid: vendor_gid, grn_gid: grn_gid }
    var summaryapi = 'PurchaseReturn/GetPurchaseReturnDetailsSummary';
    this.service.getparams(summaryapi, param).subscribe((result: any) => {
      this.GetPurchaseDetails_list = result.GetGRNDetailsSummary_list;
      this.GRNform.get('branch_name')?.setValue(result.branch_name);
      this.branch_gid = result.branch_gid;
      this.GRNform.get('vendor_name')?.setValue(result.vendor_companyname);

    });
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetPurchaseDetails_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.GetPurchaseDetails_list.forEach((row: product_list) => this.selection.select(row));
  }

  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }
  onSubmit() {
    debugger
    for (let data of this.GetPurchaseDetails_list) {
      if(data.qty_rejected!=0)
      {
      let warning = this.qtycal(data);
      
      if (warning != '') {
        this.ToastrService.warning(warning);
        return;
      }
    }
    }
    this.pick = this.selection.selected
    this.CurObj.purchaseQTY = this.pick
    this.CurObj.branch_gid = this.branch_gid
    this.CurObj.vendor_gid = this.vendor_gid
    this.CurObj.grn_gid = this.grn_gid

    this.CurObj.purchasereturn_date = this.GRNform.value.purchasereturn_date
    this.CurObj.remarks = this.GRNform.value.remarks
    if (this.CurObj.purchaseQTY.length === 0) {
      this.ToastrService.warning("Select atleast one GRN");
      return;
    }
    this.NgxSpinnerService.show();
    var postapi = 'PurchaseReturn/PostPurchaseReturn';
    this.service.post(postapi, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      else {
        this.NgxSpinnerService.hide();
        this.router.navigate(['/ims/ImsTrnPurchaseReturns']);
        this.ToastrService.success(result.message);
      }
    })

  }
  qtycal(data: any) {
    debugger;
    const grndtl_gid = data.grndtl_gid;

    const qty_delivery = this.GetPurchaseDetails_list
      .filter((item: { grndtl_gid: any; qty_delivered: number }) => item.grndtl_gid === grndtl_gid)
      .map(item => item.qty_delivered)[0];

    const qty_returned = this.GetPurchaseDetails_list
      .filter((item: { grndtl_gid: any; qty_returned: number }) => item.grndtl_gid === grndtl_gid)
      .map(item => item.qty_returned)[0];

    const qty_rejected = this.GetPurchaseDetails_list
      .filter((item: { grndtl_gid: any; qty_rejected: number }) => item.grndtl_gid === grndtl_gid)
      .map(item => item.qty_rejected)[0];

    if (data.qty_purchasereturn < qty_rejected) {
      return ('Return has greater than rejected qty');
    }


    let qty_returned_trimmed = parseFloat(qty_returned.toString().replace(/\.00$/, ''));


    let qty_purchasereturn_number = parseFloat(data.qty_purchasereturn);


    let qty_rejected_number = parseFloat(qty_rejected);


    let qty_return_new = qty_purchasereturn_number + qty_returned_trimmed;


    if (qty_return_new > qty_rejected_number) {
      return ('Amend qty cannot be greater than outstanding qty');
    }
    return '';
  }

  onback(){
    if(this.lspage == 'GRN QC'){
     this.router.navigate(['/ims/ImsTrnPurchaseReturns']);
    }
    else{
      this.router.navigate(['/pmr/PmrTrnGrninward']);
    }
}

}
