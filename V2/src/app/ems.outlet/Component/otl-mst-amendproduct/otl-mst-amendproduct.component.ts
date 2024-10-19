import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

export class product_list {
  campaign_gid:any;
  prodassign:any;
  product_gid: any;
  cost_price: number=0;
}

@Component({
  selector: 'app-otl-mst-amendproduct',
  templateUrl: './otl-mst-amendproduct.component.html',
  styleUrls: ['./otl-mst-amendproduct.component.scss']
})
export class OtlMstAmendproductComponent {
  responsedata: any;
  holidayunemployee_list: any[] = [];
  selection = new SelectionModel<product_list>(true, []);
  leave_name: any;
  leave_code: any;
  branch_gid: any;
  Leaveassign_type: any;
  pick: Array<any> = [];
  products: any[] = [];
  CurObj: product_list = new product_list();
  changepriceform: FormGroup | any;
  cost_price: number=0;

  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private NgxSpinnerService: NgxSpinnerService
  ) { }
  ngOnInit(): void {
    debugger
    
    const branch_gid = this.router.snapshot.paramMap.get('branch_gid1');
    this.branch_gid = branch_gid;
    const secretKey = 'storyboard';
    const deencryptedParam = AES.decrypt(this.branch_gid, secretKey).toString(enc.Utf8);
    this.branch_gid = deencryptedParam
    this.GetProductSummary(this.branch_gid);
    this.changepriceform = new FormGroup({
      cost_price: new FormControl('')
    })
  }

  // summary
  GetProductSummary(branch_gid: any) {
    debugger
    var api = 'OtlMstBranch/GetAmendProductSummary';
    let param  = {branch_gid : branch_gid }
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.products = this.responsedata.GetAmendProduct_list;
      setTimeout(() => {
        $('#products').DataTable();
      }, 1);      
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.products.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.products.forEach((row: product_list) => this.selection.select(row));
  }


  onupdate() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.prodassign = this.pick
    this.CurObj.campaign_gid = this.branch_gid
    if (this.CurObj.prodassign.length === 0) {
      this.ToastrService.warning("Select atleast one branch");
      return;
    }

    debugger
    // let param = {
    //   product_list:this.products,
    //   cost_price : this.changepriceform.value.cost_price,
    //   campaign_gid : this.branch_gid
    // }
    this.NgxSpinnerService.show();
    var url = 'OtlMstBranch/PostChangePrice';
    this.service.postparams(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/outlet/OtlMstPriceManagement'])
        this.NgxSpinnerService.hide();

      }
    });   
    this.selection.clear();
  }
  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }
}
