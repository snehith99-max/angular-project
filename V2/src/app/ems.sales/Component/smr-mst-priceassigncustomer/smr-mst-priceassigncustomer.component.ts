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

export class IAssignEmployee {
  pricesegment_gid:any;
  pricesegmentassign:any;
}

@Component({
  selector: 'app-smr-mst-priceassigncustomer',
  templateUrl: './smr-mst-priceassigncustomer.component.html',
  styleUrls: ['./smr-mst-priceassigncustomer.component.scss']
})
export class SmrMstPriceassigncustomerComponent {
  responsedata:any;
  assign_list:any[] = [];
  pricesegmentheader_list:any[] = [];
  selection = new SelectionModel<IAssignEmployee>(true, []);
  leave_name:any;
  leave_code:any;
  deencryptedParam:any
  pricesegment_gid: any;
  Leaveassign_type: any;
  pick:Array<any> = [];
  CurObj: IAssignEmployee = new IAssignEmployee();


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
  
      //// Summary Grid//////
      const pricesegment_gid = this.router.snapshot.paramMap.get('pricesegment_gid');
      this.pricesegment_gid = pricesegment_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.pricesegment_gid, secretKey).toString(enc.Utf8);    
      this.pricesegment_gid = deencryptedParam
      this.deencryptedParam = deencryptedParam;
      let param = {
        pricesegment_gid: deencryptedParam
      }

      
        var url = 'SmrMstPricesegmentSummary/assigncustomer'
        this.NgxSpinnerService.show()
        this.service.getparams(url,param).subscribe((result: any) => {
        this.responsedata = result;
        this.assign_list = this.responsedata.pricesegmentassign;
        this.NgxSpinnerService.hide();
          setTimeout(() => {
            $('#assign_list').DataTable();
            }, );
        
        });  
        this.pricesegmentheader()      
      }

      pricesegmentheader(){
        var url = "SmrMstPricesegmentSummary/assigncustomerheadercount";
        let params = {
          pricesegment_gid: this.deencryptedParam
        }
        this.service.getparams(url,params).subscribe((result:any)=>{
          this.pricesegmentheader_list = result.pricesegmentheader_list
        })
      }

     isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.assign_list.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.assign_list.forEach((row: IAssignEmployee) => this.selection.select(row));
    }

    assign(){
      debugger;
        this.pick = this.selection.selected  
        this.CurObj.pricesegmentassign = this.pick
        this.CurObj.pricesegment_gid=this.pricesegment_gid
          if (this.CurObj.pricesegmentassign.length === 0) {
          this.ToastrService.warning("Select atleast one customer");
          return;
        } 
    
        debugger
        this.NgxSpinnerService.show();
          var url = 'SmrMstPricesegmentSummary/AssignSubmiCustomer';  
          this.NgxSpinnerService.show()
          this.service.post(url, this.CurObj).subscribe((result: any) => {
            if (result.status === false) {
              this.ToastrService.warning(result.message);
              this.NgxSpinnerService.hide();  
              
            } else {
              this.ToastrService.success(result.message);
              this.route.navigate(['/smr/SmrMstPricesegmentSummary'])
              this.NgxSpinnerService.hide();                
            }
          });        
        this.selection.clear();
    
    }


}

