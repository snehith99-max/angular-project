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
  [x: string]: any[];
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
  ContractSummary_List:any;

}

@Component({
  selector: 'app-ims-trn-stockregularization',
  templateUrl: './ims-trn-stockregularization.component.html',
  styleUrls: ['./ims-trn-stockregularization.component.scss']
})
export class ImsTrnStockregularizationComponent {
 
  
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
  CurObj : ContractSummaryList = new ContractSummaryList();
  
  selection = new SelectionModel<ContractSummaryList>(true, []);
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    this.AddForm = new FormGroup({     
      producttype_gid:new FormControl(''),
      producttype_name:new FormControl ([]),
     
    });
  }
  
ngOnInit(): void {
  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options);
  this.GetContractSummary();
}
GetContractSummary(){
  
  var url = 'ImsTrnStockRegularization/GetStockRegularizationSummary'
  this.service.get(url).subscribe((result: any) => {
    $('#contract_summarylist').DataTable().destroy();
    this.responsedata = result;
    this.contract_summarylist = this.responsedata.stockdetails_list;
    setTimeout(() => {
      $('#contract_summarylist').DataTable();
    }, 1);
  });
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

onSubmit() {
  debugger;
  this.pick = this.selection.selected
  this.CurObj.ContractSummary_List = this.pick
  
  if (this.CurObj.ContractSummary_List.length === 0) {
    this.ToastrService.warning("Select atleast one");
    return;
  }
  this.NgxSpinnerService.show();
  var postapi = 'ImsTrnStockRegularization/PostStockRegularization';
  this.service.post(postapi, this.CurObj).subscribe((result: any) => {
    if (result.status == false) {
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
    }
    else {
      this.router.navigate(['/ims/ImsTrnStockRegularization']);
      window.location.reload();
      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
    }
  });
}
redirectToList(){
this.router.navigate(['/ims/ImsTrnStockRegularization']);
}


}
