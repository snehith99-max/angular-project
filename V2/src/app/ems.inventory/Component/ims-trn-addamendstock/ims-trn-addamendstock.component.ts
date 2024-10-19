import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'; 
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES , enc} from 'crypto-js';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface Iamend {
  amend_type:string;
  branch_name:string;
  productgroup_name:string;
  product_code: string;
  product_name: string;
  display_field: string;
  productuom_name: string;
  stock_balance: string;
  amend_qty: string;
  remarks: string;
  stock_gid: string;
  product_gid: string;
  branch_gid: string;

}

@Component({
  selector: 'app-ims-trn-addamendstock',
  templateUrl: './ims-trn-addamendstock.component.html',
  styleUrls: ['./ims-trn-addamendstock.component.scss']
})
export class ImsTrnAddamendstockComponent {
  reactiveForm!: any;
  amend!: Iamend;
  responsedata:any;
  stock_gid:any;
  ViewVendorregister_list:any;
  mdlamendtype:any;
  amend_lists : any[] = [];
  fb: any;

  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService,private ToastrService: ToastrService,private route:Router,private router: ActivatedRoute,private NgxSpinnerService :NgxSpinnerService ) {
    this.amend = {} as Iamend;
  }
  ngOnInit(): void {

    
    debugger
    const stock_gid = this.router.snapshot.paramMap.get('stock_gid');
    // console.log(termsconditions_gid)
    this.stock_gid = stock_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.stock_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getamendsummary(deencryptedParam);
    // this.GetAmendDetail(deencryptedParam)
    this.reactiveForm = new FormGroup({  
      amend_type: new FormControl('',Validators.required),
      branch_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      display_field: new FormControl(''),
      productuom_name: new FormControl(''),
      stock_balance: new FormControl(''),
      amend_qty: new FormControl('',Validators.required),
      remarks: new FormControl(''),
      stock_gid: new FormControl(''),
      branch_gid: new FormControl(''),
      product_gid: new FormControl(''),
      
    });
  

  }
  Getamendsummary(stock_gid: any) {
    debugger;
 
    var url = 'ImsTrnStockSummary/GetAmendStock'
    this.NgxSpinnerService.show()
    let param = {
      stock_gid: stock_gid
    }
    debugger;
    this.service.getparams(url, param).subscribe((result: any) => {
      this.amend_lists = result.Getamendstocklist;
      console.log(this.amend_lists)
      this.reactiveForm.get("product_name")?.setValue(this.amend_lists[0].product_name);
      this.reactiveForm.get("product_code")?.setValue(this.amend_lists[0].product_code);
      this.reactiveForm.get("productuom_name")?.setValue(this.amend_lists[0].productuom_name);
      this.reactiveForm.get("productgroup_name")?.setValue(this.amend_lists[0].productgroup_name);
      this.reactiveForm.get("uom_gid")?.setValue(this.amend_lists[0].uom_gid);
      this.reactiveForm.get("created_date")?.setValue(this.amend_lists[0].created_date);
      this.reactiveForm.get("stock_gid")?.setValue(this.amend_lists[0].stock_gid);
      this.reactiveForm.get("display_field")?.setValue(this.amend_lists[0].display_field);
      this.reactiveForm.get("stock_balance")?.setValue(this.amend_lists[0].stock_balance);
      this.reactiveForm.get("amend_qty")?.setValue(this.amend_lists[0].amend_qty);
      this.reactiveForm.get("branch_name")?.setValue(this.amend_lists[0].branch_name);
      this.reactiveForm.get("product_gid")?.setValue(this.amend_lists[0].product_gid);
      this.reactiveForm.get("branch_gid")?.setValue(this.amend_lists[0].branch_gid);
      this.NgxSpinnerService.hide();
    });
    
    
  }
  get amend_type() {
    return this.reactiveForm.get('amend_type');
  }
   
  get stock_qty() {
    return this.reactiveForm.get('stock_qty')!;
  }
  get amend_qty() {
    return this.reactiveForm.get('amend_qty')!;
  }

  initForm() {
    this.reactiveForm = this.fb.group({
      amend_type: [ this.reactiveForm.amend_type,  Validators.compose([

          Validators.required,
          ]),
      ],
    });
  }

  public validate(): void {
    debugger
    this.amend = this.reactiveForm.value;
  
    if (this.amend.amend_qty != null&&this.amend.amend_type != null && this.amend.amend_type != null && this.amend.branch_name != null && this.amend.product_name != null  ) {
         
        var params = {    
          amend_type:this.reactiveForm.value.amend_type,
          product_gid:this.reactiveForm.value.product_gid,
          branch_name:this.reactiveForm.value.branch_name,
          productgroup_name:this.reactiveForm.value.productgroup_name,
          product_code:this.reactiveForm.value.product_code,
          productuom_name:this.reactiveForm.value.productuom_name,
          display_field:this.reactiveForm.value.display_field,
          stock_balance:this.reactiveForm.value.stock_balance,
          amend_qty:this.reactiveForm.value.amend_qty,
          remarks:this.reactiveForm.value.remarks,
          stock_gid:this.reactiveForm.value.stock_gid,
          uom_gid:this.reactiveForm.value.uom_gid,
          branch_gid:this.reactiveForm.value.branch_gid,
         }
        var url2 = 'ImsTrnStockSummary/Postamendstock'
        this.NgxSpinnerService.show()
     this.service.post(url2,params).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide()
            this.ToastrService.warning(result.message)
            
          }
          else {
            this.route.navigate(['/ims/ImsTrnStocksummary']);
            this.NgxSpinnerService.hide()
            this.ToastrService.success(result.message)
            
          }
          this.responsedata = result;
        });
      }
    
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      this.NgxSpinnerService.hide()
    }
    
    return;
    
    
  
  
   }
   redirecttolist(){
    this.route.navigate(['/ims/ImsTrnStocksummary']);
  
  }
}
