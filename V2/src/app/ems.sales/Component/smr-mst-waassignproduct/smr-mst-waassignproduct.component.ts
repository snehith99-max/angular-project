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
export class IBranch {
  branch_gid:any;
  waprodassign:any;
  product_gid: any;
}
@Component({
  selector: 'app-smr-mst-waassignproduct',
  templateUrl: './smr-mst-waassignproduct.component.html',
  styleUrls: ['./smr-mst-waassignproduct.component.scss']
})
export class SmrMstWaassignproductComponent {
  branch_gid: any;
  responsedata: any;
  CurObj: IBranch = new IBranch();
  selection = new SelectionModel<IBranch>(true, []);
  unassignedproduct_list: any[] = [];
  pick:Array<any> = [];

  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private NgxSpinnerService: NgxSpinnerService
     ){}
  ngOnInit(): void { 
     
    const campaign_gid = this.router.snapshot.paramMap.get('branch_gid');
    this.branch_gid = campaign_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.branch_gid, secretKey).toString(enc.Utf8);    
    this.branch_gid = deencryptedParam
    this.GetWaunassignedproductsummary(deencryptedParam);

          
    }
    GetWaunassignedproductsummary(deencryptedParam : any) {
      debugger
      var api = 'SmrMstWhatsappproductpricemanagement/GetWaunassignedproductsummary';
      this.NgxSpinnerService.show();
      let param = {
        branch_gid:deencryptedParam
    };
      this.service.getparams(api,param).subscribe((result: any) => {
        this.responsedata = result;
        this.unassignedproduct_list = result.unassignedproduct_list;
        setTimeout(() => {
          $('#unassignedproduct_list').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
      });
  
    }
    isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.unassignedproduct_list.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.unassignedproduct_list.forEach((row: IBranch) => this.selection.select(row));
    }

    waproductassign(){
        this.pick = this.selection.selected  
        this.CurObj.waprodassign = this.pick
        this.CurObj.branch_gid=this.branch_gid
          if (this.CurObj.waprodassign.length === 0) {
            window.scrollTo({
              top: 0,
            });
          this.ToastrService.warning("Select atleast one record");
          return;
        } 
         this.NgxSpinnerService.show();
          var url = 'SmrMstWhatsappproductpricemanagement/postwaassignedlist';  
          this.service.post(url, this.CurObj).subscribe((result: any) => {
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
}
