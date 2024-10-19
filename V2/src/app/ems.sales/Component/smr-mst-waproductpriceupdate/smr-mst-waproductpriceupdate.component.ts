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
export class waproduct_list {
  branch_gid:any;
  waprodassign:any;
  product_gid: any;
  whatsapp_price: number=0;
}

@Component({
  selector: 'app-smr-mst-waproductpriceupdate',
  templateUrl: './smr-mst-waproductpriceupdate.component.html',
  styleUrls: ['./smr-mst-waproductpriceupdate.component.scss']
})
export class SmrMstWaproductpriceupdateComponent {
  branch_gid: any;
  responsedata: any;
  assignedproduct_list: any[] = [];
  selection = new SelectionModel<waproduct_list>(true, []);
  CurObj: waproduct_list = new waproduct_list();
  pick: Array<any> = [];

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
    this.GetProductSummary(deencryptedParam);

  }
  GetProductSummary(deencryptedParam: any) {
    this.NgxSpinnerService.show();
    var api = 'SmrMstWhatsappproductpricemanagement/GetwaassignedproductSummary';
    let param = { branch_gid: deencryptedParam }
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.assignedproduct_list = this.responsedata.assignedproduct_list;
      setTimeout(() => {
        $('#assignedproduct_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });

  }
  
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assignedproduct_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.assignedproduct_list.forEach((row: waproduct_list) => this.selection.select(row));
  }
  onupdate() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.waprodassign = this.pick
    this.CurObj.branch_gid = this.branch_gid
    if (this.CurObj.waprodassign.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Select atleast one Record");
      return;
    }
    this.NgxSpinnerService.show();
    var url = 'SmrMstWhatsappproductpricemanagement/updatewaproductprice';
    this.service.postparams(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);
        this.route.navigate(['/smr/SmrMstWhatsappproductpricemanagement'])
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
