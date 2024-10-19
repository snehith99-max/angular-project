import { Component } from '@angular/core';
import { FormBuilder,FormGroup,FormControl,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
interface purchaseorderadd{

}

@Component({
  selector: 'app-pmr-trn-contractpo',
  templateUrl: './pmr-trn-contractpo.component.html',
})
export class PmrTrnContractpoComponent {
  mdlvendor_companyname:any;
  purchaseorderadd !:purchaseorderadd;
  contractpoform : FormGroup | any;
  vendor_list : any[]=[];
  contract_list:any[]=[];
  branch_list: any;
  purchaseorder_list:any[]=[];
  responsedata: any;
  mdlBranchName: any;
  txtshipto: any;
  temptable:any;
  terms_list:any[]=[];
  txtvendordetails: any;
  txtbillto: any;
  vendor_gid_key:any;
  ratecontract_key:any;
  dispatch_mode:any;
  mdlTerms :any;
  response_data:any;
  contractvendor_list:any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: Router, private router: ActivatedRoute) 
  {    this.purchaseorderadd = {} as purchaseorderadd;
  this.contractpoform = new FormGroup({
    vendor_companyname: new FormControl('', Validators.required),
    branch_name: new FormControl('', Validators.required),
    po_date: new FormControl(this.getCurrentDate(), Validators.required),
    expected_date: new FormControl(this.getCurrentDate(), Validators.required),
    vendor_gid: new FormControl(''),
    qty_ordered: new FormControl(''),
    vendor_details:new FormControl(''),
    vendor_name:new FormControl(''),
    address1:new FormControl(''),
    shipping_address:new FormControl(''),
    po_no: new FormControl('',[Validators.required]),
  });
    
}
    ngOnInit(): void {
      const options: Options = {
        dateFormat: 'd-m-Y',    
      };
      flatpickr('.date-picker', options);
      debugger
      const encryptedratecontract_gid = this.router.snapshot.paramMap.get('ratecontract_gid');
      const encryptedVendorGid = this.router.snapshot.paramMap.get('vendor_gid');
      const secretKey = 'storyboarderp';
      this.vendor_gid_key = encryptedVendorGid;
      this.ratecontract_key = encryptedratecontract_gid;
      const vendor_gid = AES.decrypt(this.vendor_gid_key, secretKey).toString(enc.Utf8);
      const ratecontract_gid = AES.decrypt(this.ratecontract_key, secretKey).toString(enc.Utf8);
     this.GetContractPO(vendor_gid);
    this.GetMappingvendor(ratecontract_gid);
       var url = 'PmrTrnPurchaseOrder/GetBranch'
       this.service.get(url).subscribe((result: any) => {
         this.branch_list = result.GetBranch;
         const firstBranch = this.branch_list[0];
         const branchName = firstBranch.branch_gid;
         this.contractpoform.get('branch_name')?.setValue(branchName);
       });

       var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
       this.service.get(api).subscribe((result: any) => {
         this.responsedata = result;
         this.terms_list = this.responsedata.GetTermsandConditions
       });

    }
    getCurrentDate(): string {
      const today = new Date();
      const dd = String(today.getDate()).padStart(2, '0');
      const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
      const yyyy = today.getFullYear();
      return dd + '-' + mm + '-' + yyyy;
    }
    GetContractPO(vendor_gid:any){
      debugger
      let param ={
        vendor_gid :vendor_gid
      }
      this.NgxSpinnerService.show();
     var url = 'PmrTrnPurchaseOrder/GetContractPO'
     this.service.getparams(url,param).subscribe((result: any) => {
       this.responsedata = result;
       this.contract_list = this.responsedata.GetContractPO;
       console.log(this.contract_list )
       this.NgxSpinnerService.hide();
     });
   }
   GetMappingvendor(ratecontract_gid: any) {
    debugger
    var api = 'PmrTrnRateContract/Getcontractvendor'
    this.NgxSpinnerService.show()
    let param = {
      ratecontract_gid: ratecontract_gid,
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      this.response_data = result;
      this.contractvendor_list = this.response_data.contractvendor_list;
      this.contractpoform.get("vendor_name")?.setValue(this.contractvendor_list[0].vendor_companyname);
      const emailId = this.contractvendor_list[0].email_id;
      const contactTelephonenumber = this.contractvendor_list[0].contact_telephonenumber;
      const gstNo = this.contractvendor_list[0].gst_no;
      const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gstNo}`;
      this.contractpoform.get("vendor_details")?.setValue(vendorDetails);
      let address = this.contractvendor_list[0].address.replace(/,,/g, ',');
      address = address.replace(/,/g, '\n');
      this.contractpoform.get("address1")?.setValue(address);

      // this.contractpoform.get("address1")?.setValue(this.contractvendor_list[0].address.replace(/,/g, '\n'));
    });
    this.NgxSpinnerService.hide()
  }
  get vendor_companyname() {
    return this.contractpoform.get('vendor_companyname')!;
  };
  get po_no() {
    return this.contractpoform.get('po_no')!;
  }
  get branch_name() {
    return this.contractpoform.get('branch_name')!;
  }
  GetOnChangeTerms() {
    let termsconditions_gid = this.contractpoform.value.template_name;
    let param = {
      termsconditions_gid: termsconditions_gid
    }
    var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.contractpoform.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.contractpoform.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
    });

    }
onsubmit(){
    debugger
    this.temptable=[];
    let j=0;
    const product = this.contract_list.some(item => item.qty_ordered && item.qty_ordered > 0);
    if (!product) {
      this.ToastrService.warning('Please fill in at least one value in the Product Qty.');
      return;
    }
    for(let i=0;i<this.contract_list.length;i++){
            if(this.contract_list[i].qty_ordered !=null && this.contract_list[i].qty_ordered !=0){
            this.temptable.push(this.contract_list[i]);
            j++;
            }
        } 
    var params = { 
      contractpo_list: this.temptable,
      branch_gid : this.contractpoform.get("branch_name")?.value,
      po_no : this.contractpoform.get("po_no")?.value,
      po_date : this.contractpoform.get("po_date")?.value,
      expected_date : this.contractpoform.get("expected_date")?.value,
      vendor_name : this.contractpoform.get("vendor_name")?.value,
      shipping_address : this.contractpoform.get("shipping_address")?.value,
      po_covernote : this.contractpoform.get("po_covernote")?.value,
      template_content : this.contractpoform.get("template_content")?.value,
      vendor_gid:this.contractvendor_list[0].vendor_gid,
    };
    this.NgxSpinnerService.show();
      var url = 'PmrTrnPurchaseOrder/Postcontractpo';  
      this.service.postparams(url,params).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
        } else {
          this.ToastrService.success(result.message);
          this.route.navigate(['/pmr/PmrTrnPurchaseorderSummary'])
          this.NgxSpinnerService.hide();
        }
      });     
}
}
