import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';


export class ContractSummaryList {
  ratecontract_gid: any;
  vendorgid: any;
  producttype: any[]=[]; 
  vendor_companyname: any;
  agreement_date: any;
  expairy_date: any;
  created_date: any;
  assigned_product: any;
  vendor_code: any;
  vendor_details: any;
}

@Component({
  selector: 'app-pmr-trn-createcontract',
  templateUrl: './pmr-trn-createcontract.component.html',
  styleUrls: ['./pmr-trn-createcontract.component.scss']
})
export class PmrTrnCreatecontractComponent {
 
  
  selectedProductType: { [key: string]: any[] } = {};
  contract_summarylist:any[]=[];
  responsedata: any;
  expairy_date: any;
  agreement_date : any;
  AddForm: FormGroup | any;
  cboselectedComponent: any;
  producttype_list:any[]=[];
  producttype_gid:any[]=[];
  pick:Array<any> = [];
  temptable : any []=[];
  curobj : ContractSummaryList = new ContractSummaryList();
  
  selection = new SelectionModel<ContractSummaryList>(true, []);
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    this.AddForm = new FormGroup({
      agreement_date: new FormControl(this.getCurrentDate(), Validators.required),
      expairy_date: new FormControl(this.getCurrentDate(), Validators.required),
     
      producttype_gid:new FormControl(''),
      producttype_name:new FormControl ([]),
     
    });
  }
  
ngOnInit(): void {
  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options);
  this.GetContractSummary()
  debugger;
  var api = 'PmrTrnRateContract/Imsvendorcontract';
    this.service.get(api).subscribe((result: any) => {
       this.responsedata = result;
      this.producttype_list = this.responsedata.Imsvendorrate_list[0].producttype;
    });
}
GetContractSummary(){
  
  var url = 'PmrTrnRateContract/Imsvendorcontract'
  this.service.get(url).subscribe((result: any) => {
    $('#contract_summarylist').DataTable().destroy();
    this.responsedata = result;
    this.contract_summarylist = this.responsedata.Imsvendorrate_list;
    setTimeout(() => {
      $('#contract_summarylist').DataTable();
    }, 1);
  });
}
getCurrentDate(): string {
  
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();
  return dd + '-' + mm + '-' + yyyy;
}
isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.contract_summarylist.length;
  return numSelected===numRows;
}
masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.contract_summarylist.forEach((row: ContractSummaryList) => this.selection.select(row));
}
// onSubmit(){
// debugger
// this.contract_summarylist
// const contract_summarylist=this.AddForm
// this.pick = this.selection.selected  
// this.contract_summarylist = this.pick
// if (this.contract_summarylist.length === 0) {
//   this.ToastrService.warning("Select atleast one product");
//   return;
// } 
// else{
//   this.temptable = [];
//   let j = 0;
//   for(let i=0;i<this.producttype_list.length;i++){
//     if(this.producttype_list[i].producttype_name !=null){
//     this.temptable.push(this.producttype_list[i]);
//     j++;
//     }
//     console.log(this.temptable)
// } 
//   let param = {
//     vendor_gid: this.contract_summarylist[0].vendor_gid,
//     agreement_date: this.AddForm.value.agreement_date,
//     expairy_date: this.AddForm.value.expairy_date,
//     producttype_name: this.temptable
//   };
//   var url = 'PmrTrnRateContract/Postratecontract'
//   this.NgxSpinnerService.show()
//   this.service.postparams(url, param).subscribe((result: any) => {
//     if (result.status == false) {
//       this.ToastrService.warning(result.message)
//       this.GetContractSummary();
//       this.selection.clear();
//       this.NgxSpinnerService.hide()
//     }
//     else {
//       this.ToastrService.success(result.message)
//       this.GetContractSummary();
//       this.NgxSpinnerService.hide()
//     }
//   });
// }










// onSubmit() {
//   debugger;
//   if (this.AddForm.invalid) {
//     this.ToastrService.warning("Please fill in all required fields");
//     return;
//   }

//   const selectedProductGids = this.AddForm.controls['producttype_name'].value;
 
//   const selectedProductTypes = this.producttype_list.filter(productType =>
//     selectedProductGids.includes(productType.producttype_gid)
//   );
  
//   const producttype_name = selectedProductTypes.map((pt, index) => ({
//     index: index,
//     producttype_gid: pt.producttype_gid,
//     producttype_code: pt.producttype_code,
//     producttype_name: pt.producttype_name
//   }));
// debugger;
//   let param = {
//     vendor_gid: this.contract_summarylist[0].vendor_gid,
//     agreement_date: this.AddForm.value.agreement_date,
//     expairy_date: this.AddForm.value.expairy_date,
//     producttype_name: producttype_name 
//   };

//   var url = 'PmrTrnRateContract/Postratecontract';
//   this.NgxSpinnerService.show();
//   this.service.post(url, param).subscribe((result: any) => {
//     if (result.status == false) {
//       this.ToastrService.warning(result.message);
//       this.GetContractSummary();
//       this.selection.clear();
//       this.router.navigate(['/pmr/PmrTrnRatecontract']);
//       this.NgxSpinnerService.hide();
//     } else {
//       this.ToastrService.success(result.message);
//       this.GetContractSummary();
//       this.selection.clear();
//       this.router.navigate(['/pmr/PmrTrnRatecontract']);
//       this.NgxSpinnerService.hide();
//     }
//   });
// }

// onSubmit() {
//   debugger;

//   this.contract_summarylist
  
//   const contract_summarylist = this.AddForm.value.producttype_name;
//   this.pick = this.selection.selected
//   this.contract_summarylist = this.pick
//   debugger;
//   if (this.contract_summarylist.invalid) {
//     this.ToastrService.warning("Atleast select one vendor");
//     return;
//   }
//   Object.keys(this.selectedProductTypes).forEach(vendorCode => {
//     const selectedProductGids = this.selectedProductTypes[vendorCode].map((producttype_gid: string) => {
//     console.log(producttype_gid);
//     return producttype_gid;
//     });
//     const param = {
//       vendor_gid:   this.pick , 
//       agreement_date: this.AddForm.value.agreement_date,
//       expairy_date: this.AddForm.value.expairy_date,
//       producttype: selectedProductGids 
//     };
  
  



//   });
  
//   const url = 'PmrTrnRateContract/Postratecontract';
//   this.NgxSpinnerService.show();

//     this.service.post(url, param).subscribe((result: any) => {
//       if (result.status == false) {
//         this.ToastrService.warning(result.message);
//       } else {
//         this.ToastrService.success(result.message);
//       }
//       this.GetContractSummary();
//       this.selection.clear();
//       this.router.navigate(['/pmr/PmrTrnRatecontract']);
//       this.NgxSpinnerService.hide();
//     });

//   debugger;
 

  
// }

selectedProductTypes(vendor_gid: any){
  debugger
  const selectedProductGids = this.selectedProductType[vendor_gid].map((producttype_gid: string) => {
    return { Producttype_gid: producttype_gid };
  });
  this.curobj.producttype[vendor_gid] = selectedProductGids;
}

onSubmit() {
  debugger; 
 
  this.pick = this.selection.selected;
  this.curobj.vendorgid = this.pick;

  
  this.contract_summarylist = this.pick;
  this.pick.forEach((vendor: any) => {
    vendor.producttype = this.curobj.producttype[vendor.vendor_gid] || [];
  });
  if (!this.pick || this.pick.length === 0) {
    this.ToastrService.warning("At least select one vendor");
    return;
  }
 

 
  console.log('Params:', this.curobj);
  console.log(this.pick)
  const url = 'PmrTrnRateContract/Postratecontract';
 this.NgxSpinnerService.show();
  this.service.post(url,this.curobj).subscribe((result: any) => {
    if (result.status === false) {
      this.ToastrService.warning(result.message);
    } else {
      this.ToastrService.success(result.message);
    }
    this.GetContractSummary();
    this.selection.clear();
    this.router.navigate(['/pmr/PmrTrnRatecontract']);
    this.NgxSpinnerService.hide();
  });
}
redirectToList(){
this.router.navigate(['/pmr/PmrTrnRatecontract']);
}


}
