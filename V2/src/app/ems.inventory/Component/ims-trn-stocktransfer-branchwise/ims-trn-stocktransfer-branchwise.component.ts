import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface Iamend {
  branch_name1: string;
  productgroup_name: string;
  product_code: string;
  product_name: string;
  product_desc: string;
  productuom_name: string;
  stock_balance: string;
  transfer_stock: string;
  location_name: string;
  branch_name: string;
  remarks: string;
  stock_gid: string;
  unit_price:string;
  product_gid: string;
  branch_gid: string;
  productuom_gid:string;

}

@Component({
  selector: 'app-ims-trn-stocktransfer-branchwise',
  templateUrl: './ims-trn-stocktransfer-branchwise.component.html',
  styleUrls: ['./ims-trn-stocktransfer-branchwise.component.scss']
})
export class ImsTrnStocktransferBranchwiseComponent {
  StockForm!: FormGroup;
  amend!: Iamend;
  responsedata: any;
  stock_gid: any;
  ViewVendorregister_list: any;
  mdlamendtype: any;
  branchtransfer_list: any[] = [];
  location_list: any[] = [];
  branch_list: any[] = [];
  mdllocationName: any;
  mdlBranchName: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.amend = {} as Iamend;
  }
  ngOnInit(): void {

    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
      this.StockForm.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

    });
    debugger
    const stock_gid = this.router.snapshot.paramMap.get('stock_gid');
    this.stock_gid = stock_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.stock_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetBranchWiseTransfer(deencryptedParam);

    this.StockForm = new FormGroup({
      branch_name1: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      product_desc: new FormControl(''),
      productuom_name: new FormControl(''),
      stock_balance: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      transfer_stock: new FormControl('', Validators.required),
      location_name: new FormControl('', Validators.required),
      remarks: new FormControl(''),
      stock_gid: new FormControl(''),
      branch_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid:new FormControl(''),
      unit_price:new FormControl(''),

    });


  }
  GetBranchWiseTransfer(stock_gid: any) {
    debugger;

    var url = 'ImsTrnStockTransferSummary/GetBranchWiseTransfer'
    this.NgxSpinnerService.show()
    let param = {
      stock_gid: stock_gid
    }
    debugger;
    this.service.getparams(url, param).subscribe((result: any) => {
      this.branchtransfer_list = result.branchtransfer;
      console.log(this.branchtransfer_list)
      this.StockForm.get("product_name")?.setValue(this.branchtransfer_list[0].product_name);
      this.StockForm.get("product_code")?.setValue(this.branchtransfer_list[0].product_code);
      this.StockForm.get("productuom_name")?.setValue(this.branchtransfer_list[0].productuom_name);
      this.StockForm.get("productgroup_name")?.setValue(this.branchtransfer_list[0].productgroup_name);
      this.StockForm.get("uom_gid")?.setValue(this.branchtransfer_list[0].uom_gid);
      this.StockForm.get("created_date")?.setValue(this.branchtransfer_list[0].created_date);
      this.StockForm.get("stock_gid")?.setValue(this.branchtransfer_list[0].stock_gid);
      this.StockForm.get("product_desc")?.setValue(this.branchtransfer_list[0].product_desc);
      this.StockForm.get("stock_balance")?.setValue(this.branchtransfer_list[0].stock_qty);
      this.StockForm.get("branch_name")?.setValue(this.branchtransfer_list[0].transfer_branch);
      this.StockForm.get("branch_name1")?.setValue(this.branchtransfer_list[0].branch_name);
      this.StockForm.get("location_name")?.setValue(this.branchtransfer_list[0].location_name);
      this.StockForm.get("transfer_stock")?.setValue(this.branchtransfer_list[0].transfer_stock);
      this.StockForm.get("product_gid")?.setValue(this.branchtransfer_list[0].product_gid);
      this.StockForm.get("productuom_gid")?.setValue(this.branchtransfer_list[0].productuom_gid);
      this.StockForm.get("branch_gid")?.setValue(this.branchtransfer_list[0].branch_gid);
      this.StockForm.get("unit_price")?.setValue(this.branchtransfer_list[0].unit_price);
      this.NgxSpinnerService.hide();
    });


  }
  get transfer_stock() {
    return this.StockForm.get('transfer_stock');
  }

  get location_name() {
    return this.StockForm.get('location_name')!;
  }
  get branch_name() {
    return this.StockForm.get('branch_name')!;
  }

  GetOnChangeLocation() {
    debugger;

    let branch_gid = this.StockForm.value.branch_name.branch_gid;
    let param = {
      branch_gid: branch_gid
    }
    var url = 'ImsTrnStockSummary/GetOnChangeLocation';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.location_list = this.responsedata.GetLocation;
      this.StockForm.get("location_name")?.setValue(result.location_list[0].location_name);
      // this.StockForm.get("location_gid")?.setValue(result.location_list[0].location_gid);
    });
  }

  onClearlocation() {
    this.mdllocationName = '';
  }
  public update(): void {
    debugger;
    this.amend = this.StockForm.value;

    if (this.amend.branch_name != null && this.amend.location_name != null && this.amend.transfer_stock != null) {

      var params = {
        product_gid: this.StockForm.value.product_gid,
        branch_name1: this.StockForm.value.branch_name1,
        productgroup_name: this.StockForm.value.productgroup_name,
        product_code: this.StockForm.value.product_code,
        productuom_name: this.StockForm.value.productuom_name,
        unit_price: this.StockForm.value.unit_price,
        product_desc: this.StockForm.value.product_desc,
        stock_balance: this.StockForm.value.stock_balance,
        branch_name:this.StockForm.value.branch_name.branch_name,
        location_name:this.StockForm.value.location_name.location_name,
        location_gid:this.StockForm.value.location_name.location_gid,
        transfer_stock: this.StockForm.value.transfer_stock,
        remarks: this.StockForm.value.remarks,
        stock_gid: this.StockForm.value.stock_gid,
        productuom_gid: this.StockForm.value.productuom_gid,
        branch_gid: this.StockForm.value.branch_name.branch_gid,
      }
      var url2 = 'ImsTrnStockTransferSummary/Poststocktransfer';
      this.NgxSpinnerService.show()
      this.service.post(url2, params).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide()
          this.ToastrService.warning(result.message)

        }
        else {
          this.route.navigate(['/ims/ImsTrnStockTransfer']);
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
  redirecttolist() {
    this.route.navigate(['/ims/ImsTrnStockTransfer']);

  }

}
