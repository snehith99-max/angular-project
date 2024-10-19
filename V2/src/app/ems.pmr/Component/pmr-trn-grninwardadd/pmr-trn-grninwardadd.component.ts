import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

export class IGrn {
  summary_list: string[] = [];
  addgrn_lists : string []=[];
  qtyreceivedas: string = "";
  qty_delivered: string = "";
  qty_grnadjusted: string = "";
  
}

interface IGrnadd {  
  branch_name: string;
  vendor_companyname: string;
  contactperson_name: string;
  dc_no: string;
  grn_date: string;
  expected_date:string;
  invoice_date: string;
  dc_date: string;
  invoiceref_no: string;
  contact_telephonenumber: string;
  email_id: string;
  grn_remarks: string;
  address: string;
  grn_gid: string;
  qtyreceivedas: string;
  qty_delivered: string;
  qty_grnadjusted: string;
  user_firstname: string;
  user_firstname1: string;
  addgrn_lists: string
}

@Component({
  selector: 'app-pmr-trn-grninwardadd',
  templateUrl: './pmr-trn-grninwardadd.component.html',
  styleUrls: ['./pmr-trn-grninwardadd.component.scss']
})

export class PmrTrnGrninwardaddComponent implements OnInit {
  purchaseorder_gid: any;
  addgrnform!: FormGroup;
  productform!: FormGroup;
  grn_lists: any;
  mdlBranchName:any;
  branch_list: any;
  tableData: any[] = [];
  summary_list: any[] = [];
  addgrn_lists: any[] = [];
  productgroup_list: any[] = [];
  userlist: any[] = [];
  summary_list1: any[] = [];
  file!: File;
  grnaadd!: IGrnadd;
  pick: Array<any> = [];
  invoicegid: any;
  qtyReceivedAs: string = '';
  parameter: any;
  parameterValue1: any;
  purchaseordergid: any;
  assignvisitlist: any[] = [];
  responsedata: any;
  CurObj: IGrn = new IGrn();
  selection = new SelectionModel<IGrn>(true, []);
  IGrn: any;
  purchaseorder_list: any;
  modeofdispatch_list:any[]=[];
  GetASN_list:any[]=[];
  payment_days: any;
  delivery_days: any;
  total_amount: any;
  total_tax: any;
  discount_amount: any;
  addon_amount: any;
  freight_charges: any;
  packing_charges:any;
  roundoff:any;
  insurance_charges:any;
  total_discount_amount:any;
 // buybackorscrap: any;
  grand_total: any;
  txtshipto:any;
  currency_code: any;
  temptable : any []=[]
  mintsoft_flag: string | null = null;
  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private FormBuilder: FormBuilder, private router: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService) {
    this.grnaadd = {} as IGrnadd;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    const purchaseorder_gid = this.router.snapshot.paramMap.get('purchaseorder_gid');
    this.purchaseordergid = purchaseorder_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaseordergid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getaddgrnsummary(deencryptedParam);
    this.Getsummaryaddgrn(deencryptedParam);
    var url = 'ImsTrnDeliveryorderSummary/modeofdispatchdropdown'
    this.service.get(url).subscribe((result:any)=>{
      this.modeofdispatch_list = result.modeofdispatch_list
    });
    var url = 'PmrTrnGrnInward/GetGoodInTypesMintSoft';
    this.service.get(url).subscribe((result: any) => {
      this.GetASN_list = result.GetGoodInType_list;
    })
    // var url = 'PmrTrnPurchaseOrder/GetBranch'
    // this.service.get(url).subscribe((result: any) => {
    //   this.branch_list = result.GetBranch;
    //   const firstBranch = this.branch_list[0];
    //   const branchName = firstBranch.branch_gid;
    //   this.addgrnform.get('branch_name')?.setValue(branchName);
    // });
    this.addgrnform = new FormGroup({
      
      // grn_date: new FormControl(this.getCurrentDate()),
      // branch_name: new FormControl(''),
      // vendor_companyname: new FormControl(''),
      // contactperson_name: new FormControl(''),
      // contact_telephonenumber: new FormControl(''),
      // email_id: new FormControl(''),
      // address: new FormControl(''),
      // vendor_details: new FormControl(''),
     
      // grn_remarks: new FormControl(''),
      // tax_number: new FormControl(''),

      // dc_no: new FormControl('', [Validators.required]),
      // dc_date: new FormControl(this.getCurrentDate()),
      // invoiceref_no: new FormControl(''),
      // invoice_date: new FormControl(this.getCurrentDate()),
      // priority_flag: new FormControl('N', Validators.required),

      // file: new FormControl(''),
      // qtyreceivedas: new FormControl(''),
      // qty_delivered: new FormControl(''),
      // qty_grnadjusted: new FormControl(''),      
      // addgrn_lists: this.FormBuilder.array([]),
      // required: new FormControl('Y', Validators.required),

      grn_gid: new FormControl(''),
      purchaseorder_gid: new FormControl(''),
       user_firstname: new FormControl(''),
       user_firstname1: new FormControl(''),
      branch_name: new FormControl(''),
      dc_no: new FormControl('',Validators.required),
      grn_date: new FormControl(this.getCurrentDate()),
      expected_date:new FormControl(''),
      vendor_companyname: new FormControl(''),
      vendor_details: new FormControl(''),
      address: new FormControl(''),
      modeof_dispatch: new FormControl(''),
      deliverytracking: new FormControl(''),
      no_box: new FormControl(''),
      delivery_note: new FormControl(''),
      qty_delivered: new FormControl(''),
      qtyreceivedas: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      qty_ordered: new FormControl(''),
      product_gid: new FormControl(''),
      qty_received: new FormControl(''),
      received_note: new FormControl(''),
      display_field_name: new FormControl(''),
      goodsintypes_id:new FormControl(''),


    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
   
    return dd + '-' + mm + '-' + yyyy;
  }

  Getaddgrnsummary(purchaseorder_gid: any) {
    debugger;
 
    var url = 'PmrTrnGrn/Getaddgrnsummary'
    this.NgxSpinnerService.show()
    let param = {
      purchaseorder_gid: purchaseorder_gid
    }
    debugger;
    this.service.getparams(url, param).subscribe((result: any) => {
      this.grn_lists = result.grn_lists;
      console.log(this.grn_lists)
       this.addgrnform.get("branch_name")?.setValue(this.grn_lists[0].branch_name);
      this.addgrnform.get("vendor_companyname")?.setValue(this.grn_lists[0].vendor_companyname);
      this.addgrnform.get("contactperson_name")?.setValue(this.grn_lists[0].contactperson_name);
      this.addgrnform.get("dc_no")?.setValue(this.grn_lists[0].dc_no);
      this.addgrnform.get("invoiceref_no")?.setValue(this.grn_lists[0].invoiceref_no);
      // this.addgrnform.get("grn_date")?.setValue(this.grn_lists[0].grn_date);
      this.addgrnform.get("expected_date")?.setValue(this.grn_lists[0].expected_date);
      // this.addgrnform.get("invoice_date")?.setValue(this.grn_lists[0].invoice_date);
      // this.addgrnform.get("dc_date")?.setValue(this.grn_lists[0].dc_date);
      this.addgrnform.get("contact_telephonenumber")?.setValue(this.grn_lists[0].contact_telephonenumber);
      this.addgrnform.get("email_id")?.setValue(this.grn_lists[0].email_id);
      this.addgrnform.get("grn_remarks")?.setValue(this.grn_lists[0].grn_remarks);
      this.addgrnform.get("address")?.setValue(this.grn_lists[0].address);
      this.addgrnform.get("purchaseorder_gid")?.setValue(this.grn_lists[0].purchaseorder_gid);
      this.addgrnform.get("grn_gid")?.setValue(this.grn_lists[0].grn_gid);
      this.addgrnform.get("user_firstname")?.setValue(this.grn_lists[0].user_firstname);
      this.addgrnform.get("user_firstname1")?.setValue(this.grn_lists[0].user_firstname1);
      // const Name = this.grn_lists[0].contactperson_name;
      const emailId = this.grn_lists[0].email_id;
      const contactTelephonenumber = this.grn_lists[0].contact_telephonenumber;
      const gst_no=this.grn_lists[0].tax_number;
      const Date=this.grn_lists[0].purchaseorder_date;
const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gst_no}`;
      this.addgrnform.get("vendor_details")?.setValue(vendorDetails);
      this.mintsoft_flag = this.grn_lists[0].mintsoft_flag;
      this.NgxSpinnerService.hide();
    });
    
  }
  Getsummaryaddgrn(purchaseorder_gid: any) {
    debugger;
    this.NgxSpinnerService.show();
    var url = 'PmrTrnGrn/Getsummaryaddgrn'
    let param = {
      purchaseorder_gid: purchaseorder_gid
    }
    debugger;
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
          this.addgrn_lists = this.responsedata.addgrn_list;
          console.log(this.addgrn_lists)
      this.addgrnform.get("productgroup_name")?.setValue(this.addgrn_lists[0].productgroup_name);
      this.addgrnform.get("product_code")?.setValue(this.addgrn_lists[0].product_code);
      this.addgrnform.get("display_field_name")?.setValue(this.addgrn_lists[0].display_field_name);
      this.addgrnform.get("product_name")?.setValue(this.addgrn_lists[0].product_name);
      this.addgrnform.get("productuom_name")?.setValue(this.addgrn_lists[0].productuom_name);
      this.addgrnform.get("qty_ordered")?.setValue(this.addgrn_lists[0].qty_ordered);
      this.addgrnform.get("qty_delivered")?.setValue(this.addgrn_lists[0].qty_delivered);
      // this.addgrnform.get("qty_received")?.setValue(this.addgrn_lists[0].qty_received);

      for (let i = 0; i < this.addgrn_lists.length; i++) {
        // this.addgrnform.addControl(`qtyreceivedas_${i}`, new FormControl(this.addgrn_lists[i].qtyreceivedas));
        // this.addgrnform.addControl(`qty_delivered_${i}`, new FormControl(this.addgrn_lists[i].qty_delivered));
        // this.addgrnform.addControl(`qty_grnadjusted_${i}`, new FormControl(this.addgrn_lists[i].qty_grnadjusted));
         this.addgrnform.addControl(`qty_received_${i}`, new FormControl(this.addgrn_lists[i].qty_received));
      }
      this.NgxSpinnerService.hide();
    });
  }

  onChange2(event: any) {
    this.file = event.target.files[0];
  }

  get branch_name() {
    return this.addgrnform.get('branch_name')!;
  }
  get vendor_companyname() {
    return this.addgrnform.get('vendor_companyname')!;
  }
  get contactperson_name() {
    return this.addgrnform.get('contactperson_name')!;
  }
  get dc_no() {
    return this.addgrnform.get('dc_no')!;
  }
  get grn_date() {
    return this.addgrnform.get('grn_date')!;
  }
  get expected_date(){
    return this.addgrnform.get('expected_date')!;
  }
  get modeof_dispatch() {
    return this.addgrnform.get('modeof_dispatch')!;
  }
  get deliverytracking() {
    return this.addgrnform.get('deliverytracking')!;
  }
  get no_box() {
    return this.addgrnform.get('no_box')!;
  }
  get contact_telephonenumber() {
    return this.addgrnform.get('contact_telephonenumber')!;
  }
  get email_id() {
    return this.addgrnform.get('email_id')!;
  }
  get grn_remarks() {
    return this.addgrnform.get('grn_remarks')!;
  }
  get address() {
    return this.addgrnform.get('address')!;
  }
  // get purchaseorder_gid() {
  //   return this.addgrnform.get('purchaseorder_gid')!;
  // }
  get grn_gid() {
    return this.addgrnform.get('grn_gid')!;
  }
  get user_firstname() {
    return this.addgrnform.get('user_firstname')!;
  }
  get qtyreceivedas() {
    return this.addgrnform.get('qtyreceivedas')!;
  }
  get user_firstname1() {
    return this.addgrnform.get('user_firstname1')!;
  }
  get qty_delivered() {
    return this.addgrnform.get('qty_delivered')!;
  }
  get qty_grnadjusted() {
    return this.addgrnform.get('qty_grnadjusted')!;
  }
  get dc_noControl() {
    return this.addgrnform.get('dc_no')!;
  }
  get goodsintypes_id() {
    return this.addgrnform.get('goodsintypes_id')!;
  }

  
onsubmit() {
  debugger;
  this.temptable=[];
  let f = 0;
  const qtyreceived = this.addgrn_lists.some(item => item.qty_received && item.qty_received > 0);
      if (!qtyreceived) {
        this.ToastrService.warning('Please fill in at least one value in the Quantity.');
        return;
      }
      for(let i=0;i<this.addgrn_lists.length;i++){
        let qty_received = Number(this.addgrn_lists[i].qty_received)
        if(qty_received !=null && qty_received !=0){
        this.temptable.push(this.addgrn_lists[i]);
        f++;
        }
    } 
// continue
  // for (let i = 0; i < this.addgrn_lists.length; i++) {
  //   if (!this.addgrn_lists[i].qty_received || this.addgrn_lists[i].qty_received === "0" || this.addgrn_lists[i].qty_received === 0) {
  //     f = 1;
  //     break;
  //   }
  // }

  // if (f === 0) {

// upload file


  let formData = new FormData();
  if (this.file != null && this.file != undefined) {

    formData.append("file", this.file, this.file.name);
    formData.append("branch_name",this.addgrnform.value.branch_name);
    formData.append("vendor_companyname",this.addgrnform.value.vendor_companyname);
    formData.append("purchaseorder_gid",this.addgrnform.value.purchaseorder_gid);
    formData.append("contactperson_name",this.addgrnform.value.contactperson_name);
    formData.append("dc_no",this.addgrnform.value.dc_no);
    formData.append("grn_date",this.addgrnform.value.grn_date);
    formData.append("expected_date",this.addgrnform.value.expected_date);
    formData.append("contact_telephonenumber",this.addgrnform.value.contact_telephonenumber);
    formData.append("email_id",this.addgrnform.value.email_id);
    formData.append("modeof_dispatch",this.addgrnform.value.modeof_dispatch);
    formData.append("deliverytracking",this.addgrnform.value.deliverytracking);
    formData.append("address",this.addgrnform.value.address);
    formData.append("grn_gid",this.addgrnform.value.grn_gid);
    formData.append("user_firstname",this.addgrnform.value.user_firstname);
    formData.append("user_firstname1",this.addgrnform.value.user_firstname1);
    formData.append("no_box",this.addgrnform.value.no_box);
    formData.append("received_note",this.addgrnform.value.received_note);
    formData.append("goodsintypes_id",this.addgrnform.value.goodsintypes_id);
    formData.append("summary_list",JSON.stringify(this.temptable));
    var url = 'PmrTrnGrn/PostGrnSubmitUpload'
    this.NgxSpinnerService.show();
    this.service.postparams(url, formData).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.route.navigate(['/pmr/PmrTrnGrninward']);
      }
    });
  }
  else {
    const requestData = {
      branch_name: this.addgrnform.value.branch_name,
      vendor_companyname: this.addgrnform.value.vendor_companyname,
      purchaseorder_gid: this.addgrnform.value.purchaseorder_gid,
      contactperson_name: this.addgrnform.value.contactperson_name,
      dc_no: this.addgrnform.value.dc_no,   
      grn_date: this.addgrnform.value.grn_date,
      expected_date:this.addgrnform.value.expected_date,
      contact_telephonenumber: this.addgrnform.value.contact_telephonenumber,
      email_id: this.addgrnform.value.email_id,
      modeof_dispatch: this.addgrnform.value.modeof_dispatch,
      deliverytracking: this.addgrnform.value.deliverytracking,
      address: this.addgrnform.value.address,
      grn_gid: this.addgrnform.value.grn_gid,
      user_firstname: this.addgrnform.value.user_firstname,
      user_firstname1: this.addgrnform.value.user_firstname1,
      no_box: this.addgrnform.value.no_box,
      received_note: this.addgrnform.value.received_note,
      goodsintypes_id: this.addgrnform.value.goodsintypes_id,
      summary_list: this.temptable,
    };

    const url = 'PmrTrnGrn/PostGrnSubmit';
    this.NgxSpinnerService.show(); 
    this.service.post(url, requestData).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      window.scrollTo({
        top: 0, 
      });

      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/pmr/PmrTrnGrninward']);
      }
    });
  // } else {
  //   this.ToastrService.warning("Kindly fill valid details.");
  // }
  }
}

  event(data: any, i: number) {
    const qty_received = +this.addgrnform.get(`qty_received_${i}`)?.value || 0; 
    const qty_received_as = +data.qtyreceivedas || 0;
    const sum = qty_received + qty_received_as;
    this.addgrnform.get([`qty_delivered_${i}`])?.setValue(sum);
  }
  
     GetPurchaseOrderDetails() {
      debugger
      this.NgxSpinnerService.show();
    var url = 'PmrTrnGrnInward/GetPurchaseOrderDetails'
    const purchaseorder_gid = this.router.snapshot.paramMap.get('purchaseorder_gid');
    this.purchaseordergid = purchaseorder_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaseordergid, secretKey).toString(enc.Utf8);
    let param = {
           purchaseorder_gid : deencryptedParam 
      }
    this.service.getparams(url,param).subscribe((result: any) => {
      // $('#purchaseorder_list').DataTable().destroy();
      this.responsedata = result;
      this.purchaseorder_list = this.responsedata.Getpurchaseorder_list;

      this.payment_days = this.purchaseorder_list[0].payment_days;
      this.delivery_days = this.purchaseorder_list[0].delivery_days;

      this.total_amount = this.purchaseorder_list[0].total_amount;
      this.total_tax = this.purchaseorder_list[0].total_tax;
      this.total_discount_amount = this.purchaseorder_list[0].total_discount_amount;
      this.addon_amount = this.purchaseorder_list[0].addon_amount;
      this.freight_charges = this.purchaseorder_list[0].freight_charges;
      //this.buybackorscrap = this.purchaseorder_list[0].buybackorscrap;
      this.packing_charges = this.purchaseorder_list[0].packing_charges;
      this.roundoff = this.purchaseorder_list[0].roundoff;
      this.insurance_charges = this.purchaseorder_list[0].insurance_charges;
      this.grand_total = this.purchaseorder_list[0].grand_total;
      this.currency_code = this.purchaseorder_list[0].currency_code;

      // setTimeout(() => {
      //   $('#purchaseorder_list').DataTable();
      // }, 1);
      this.NgxSpinnerService.hide();
    })
  }
  qytrecived(index: number, qtyOrdered: number, value: any): void {
    debugger
    let inputValue = value ? parseInt(value, 10) : 0;

    if (inputValue > qtyOrdered) {
      this.addgrnform.addControl(`qty_received_${index}`, new FormControl(qtyOrdered));
     // this.addgrn_lists[index].qty_received = qtyOrdered.toString();
    } 

   
}

}