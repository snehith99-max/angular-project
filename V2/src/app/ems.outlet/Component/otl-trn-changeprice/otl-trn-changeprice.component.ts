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
  campaign_gid:any;
  produnassign:any;
  product_gid: any;
}

@Component({
  selector: 'app-otl-trn-changeprice',
  templateUrl: './otl-trn-changeprice.component.html',
  styleUrls: ['./otl-trn-changeprice.component.scss']
})
export class OtlTrnChangepriceComponent {

  responsedata:any;
  holidayunemployee_list:any[] = [];
  selection = new SelectionModel<IBranch>(true, []);
  leave_name:any;
  leave_code:any;
  campaign_gid: any;
  Leaveassign_type: any;
  pick:Array<any> = [];
  products: any[] = [];
  CurObj: IBranch = new IBranch();
  changepriceform : FormGroup | any;
  cost_price:any;
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
      this.GetProductSummary();
      const campaign_gid = this.router.snapshot.paramMap.get('campaign_gid');
      this.campaign_gid = campaign_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.campaign_gid, secretKey).toString(enc.Utf8);    
      this.campaign_gid = deencryptedParam
      
        this.changepriceform = new FormGroup({
          cost_price : new FormControl('')
        })
      }

      // summary
      GetProductSummary() {

        var api = 'OtlMstBranch/GetProductSummary';
        this.NgxSpinnerService.show();
        this.service.get(api).subscribe((result: any) => {
          this.responsedata = result;
          this.products = this.responsedata.Prod_list;
          for(let i=0;i<this.products.length;i++){
            this.changepriceform.addControl(`cost_price_${i}`, new FormControl(this.products[i].cost_price));
          }
          
          
          setTimeout(() => {
            $('#products').DataTable();
          }, 1);
          this.NgxSpinnerService.hide();
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
          this.products.forEach((row: IBranch) => this.selection.select(row));
      }


      unassign(){
        debugger;
          // this.pick = this.selection.selected  
          // this.CurObj.produnassign = this.pick
          // this.CurObj.campaign_gid=this.campaign_gid
          //   if (this.CurObj.produnassign.length === 0) {
          //   this.ToastrService.warning("Select atleast one branch");
          //   return;
          // } 
          let param = {
            product_list:this.products,
            cost_price : this.changepriceform.value.cost_price,
            campaign_gid : this.campaign_gid
          }
          debugger
          this.NgxSpinnerService.show();
            var url = 'OtlMstBranch/PostChangePrice';  
            this.service.postparams(url, param).subscribe((result: any) => {
              if (result.status === false) {
                this.ToastrService.warning(result.message);
                
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


