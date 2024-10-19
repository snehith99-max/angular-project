
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
interface Idamaged {
  damaged_type:string;
  branch_name:string;
  productgroup_name:string;
  product_code: string;
  product_name: string;
  display_field: string;
  productuom_name: string;
  stock_balance: string;
  damaged_qty: string;
  remarks: string;
  stock_gid: string;
  product_gid: string;
  branch_gid: string;

}

@Component({
  selector: 'app-ims-trn-add-damagedstock',
  templateUrl: './ims-trn-add-damagedstock.component.html',
  styleUrls: ['./ims-trn-add-damagedstock.component.scss']
})
export class ImsTrnAddDamagedstockComponent {

  reactiveForm!: FormGroup;
  damaged!: Idamaged;
  responsedata:any;
  stock_gid:any;
  ViewVendorregister_list:any;
  mdldamagedtype:any;
  damaged_lists : any[] = [];

  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService,private ToastrService: ToastrService,private route:Router,private router: ActivatedRoute,private NgxSpinnerService :NgxSpinnerService ) {
    this.damaged = {} as Idamaged;
  }
  ngOnInit(): void {

    
    debugger
    const stock_gid = this.router.snapshot.paramMap.get('stock_gid');
    // console.log(termsconditions_gid)
    this.stock_gid = stock_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.stock_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getaddgrnsummary(deencryptedParam);
    // this.GetdamagedDetail(deencryptedParam)
    this.reactiveForm = new FormGroup({  
      damaged_type: new FormControl(''),
      branch_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      display_field: new FormControl(''),
      productuom_name: new FormControl(''),
      stock_balance: new FormControl(''),
      damaged_qty: new FormControl('',Validators.required),
      remarks: new FormControl(''),
      stock_gid: new FormControl(''),
      branch_gid: new FormControl(''),
      product_gid: new FormControl(''),
      
    });
  

  }
  Getaddgrnsummary(stock_gid: any) {
    debugger;
 
    var url = 'ImsTrnStockSummary/GetDamagedStock'
    this.NgxSpinnerService.show()
    let param = {
      stock_gid: stock_gid
    }
    debugger;
    this.service.getparams(url, param).subscribe((result: any) => {
      this.damaged_lists = result.Getdamagedstocklist;
      console.log(this.damaged_lists)
      this.reactiveForm.get("product_name")?.setValue(this.damaged_lists[0].product_name);
      this.reactiveForm.get("product_code")?.setValue(this.damaged_lists[0].product_code);
      this.reactiveForm.get("productuom_name")?.setValue(this.damaged_lists[0].productuom_name);
      this.reactiveForm.get("productgroup_name")?.setValue(this.damaged_lists[0].productgroup_name);
      this.reactiveForm.get("uom_gid")?.setValue(this.damaged_lists[0].uom_gid);
      this.reactiveForm.get("created_date")?.setValue(this.damaged_lists[0].created_date);
      this.reactiveForm.get("stock_gid")?.setValue(this.damaged_lists[0].stock_gid);
      this.reactiveForm.get("display_field")?.setValue(this.damaged_lists[0].display_field);
      this.reactiveForm.get("stock_balance")?.setValue(this.damaged_lists[0].stock_balance);
      this.reactiveForm.get("damaged_qty")?.setValue(this.damaged_lists[0].damaged_qty);
      this.reactiveForm.get("branch_name")?.setValue(this.damaged_lists[0].branch_name);
      this.reactiveForm.get("product_gid")?.setValue(this.damaged_lists[0].product_gid);
      this.reactiveForm.get("branch_gid")?.setValue(this.damaged_lists[0].branch_gid);
      this.NgxSpinnerService.hide();
    });
    
    
  }
  get damaged_type() {
    return this.reactiveForm.get('damaged_type');
  }
   
  get stock_qty() {
    return this.reactiveForm.get('stock_qty')!;
  }
  get damaged_qty() {
    return this.reactiveForm.get('damaged_qty')!;
  }

  public validate(): void {
    debugger
    this.damaged = this.reactiveForm.value;
  
    if (this.damaged.damaged_qty != null && this.damaged.damaged_type != null && this.damaged.branch_name != null && this.damaged.product_name != null  ) {
         
        var params = {    
          damaged_type:this.reactiveForm.value.damaged_type,
          product_gid:this.reactiveForm.value.product_gid,
          branch_name:this.reactiveForm.value.branch_name,
          productgroup_name:this.reactiveForm.value.productgroup_name,
          product_code:this.reactiveForm.value.product_code,
          productuom_name:this.reactiveForm.value.productuom_name,
          display_field:this.reactiveForm.value.display_field,
          stock_balance:this.reactiveForm.value.stock_balance,
          damaged_qty:this.reactiveForm.value.damaged_qty,
          remarks:this.reactiveForm.value.remarks,
          stock_gid:this.reactiveForm.value.stock_gid,
          uom_gid:this.reactiveForm.value.uom_gid,
          branch_gid:this.reactiveForm.value.branch_gid,
         }
        var url2 = 'ImsTrnStockSummary/PostDamagedstock'
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
